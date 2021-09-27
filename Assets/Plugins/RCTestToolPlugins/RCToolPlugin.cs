using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;
using System;
/// <summary>
/// 1.��l���ŪުA��
/// 2.���y/����y�Ū޸˸m
/// 3.�s��/�_�}�Ū޸˸m
/// 4.�e�X���O���Ū޸˸m
/// 5.�����Ūަ^�ǰT��
/// </summary>
public class RCToolPlugin : MonoBehaviour
{
    /// <summary>
    /// �Ť��˸m
    /// </summary>
    public class BLEDevice
    {
        public string Address = "";
        public string DeviceName = "";
        public bool Connected = false;
    }

    /// <summary>
    /// ���ݰe�X�����Oclass
    /// </summary>
    public class SendInfo
    {
        public string Address = "";
        public string data = "";
        public string Key = "";
    }

    /// <summary>
    /// ���ݰe�X�����OQueue
    /// </summary>
    private static Queue<SendInfo> WaitToSend = new Queue<SendInfo>();

    public static RCToolPlugin Instance;
    /// <summary>
    /// �j�M�쪺�˸m�C��, Key:address
    /// </summary>
    public static Dictionary<string, BLEDevice> Devices_Dic = new Dictionary<string, BLEDevice>();
    /// <summary>
    /// �j�M�ƥ�
    /// </summary>
    public delegate void EventType_NewDevice(string address, string name);
    public static event EventType_NewDevice onRceiveDevice;
    /// <summary>
    /// �˸m�s�����A�ƥ�
    /// </summary>
    public delegate void EventType_DeviceStatusChanged(string address, string status);
    public static event EventType_DeviceStatusChanged onDeviceStatusChanged;
    /// <summary>
    /// �����˸m�T���ƥ�(�[�K)
    /// </summary>
    public delegate void EventType_ReceiveEncodeRawData(string address, string data);
    public static event EventType_ReceiveEncodeRawData onReceiveEncodeRawData;
    /// <summary>
    /// �����˸m�T���ƥ�(�ѱK)
    /// </summary>
    public delegate void EventType_ReceiveDecodeRawData(string address, string data);
    public static event EventType_ReceiveDecodeRawData onReceiveDecodeRawData;

    //public delegate void EventType_ReadySendData(bool ready);
    //public static event EventType_ReadySendData onReadeySendData;

    //��l��
    private void Awake()
	{
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != Instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        _Init();
    }
    //���O����
    private float senddelay = 0.5f;
    private void Update()
    {
        SendDataWithDelay();
    }
    /// <summary>
    /// �C0.5��e�@�����O
    /// </summary>
    private void SendDataWithDelay()
    {
        if (WaitToSend.Count > 0)
        {
            if (senddelay > 0)
            {
                senddelay -= Time.deltaTime;
            }
            else if (senddelay < 0)
            {
                SendInfo info = WaitToSend.Dequeue();
                _SendData(info.Address, info.data, info.Key);
                senddelay = 0.5f;
            }
        }
    }

    #region [APIs for Unity call]
    /// <summary>
    /// �}�l���y
    /// </summary>
    public static void StartScan()
    {
        Debug.Log("On Unity StartScan...");
        _StartScan();
        Devices_Dic.Clear();
    }
    /// <summary>
    /// ����y
    /// </summary>
    public static void StopScan()
    {
        Debug.Log("On Unity StopScan...");
        _StopScan();
    }
    /// <summary>
    /// �s���˸m
    /// </summary>
    public static void ConnectDevice(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            Debug.Log("address IsNullOrEmpty.");
            return;
        }
        if (IsConnected(address))
        {
            Debug.Log("Device already connected.");
            return;
        }
        Debug.Log("On Unity ConnectDevice:" + address);
        _Connect(address);
    }
    /// <summary>
    /// �_�}�˸m
    /// </summary>
    public static void DisconnectDevice(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            Debug.Log("address IsNullOrEmpty.");
            return;
        }
        Debug.Log("On Unity DisconnectDevice:" + address);
        _Disconnect(address);
    }
    /// <summary>
    /// �e��Ƶ��˸m
    /// </summary>
    public static void SendData(string address, byte[] data, string key_num)
    {
        if (!IsConnected(address))
        {
            Debug.Log("Is Not Connected.");
            return;
        }
        if (string.IsNullOrEmpty(address))
        {
            Debug.Log("address IsNullOrEmpty.");
            return;
        }
        if (data == null || data.Length <= 0)
        {
            Debug.Log("data IsNullOrEmpty.");
            return;
        }
        Debug.Log("On Unity SendData to:" + address);
        SendInfo info = new SendInfo();
        info.Address = address;
        info.data = ByteArr_To_ByteString(data);
        info.Key = key_num;
        WaitToSend.Enqueue(info);
        //_SendData(address, ByteArr_To_ByteString(data), key_num);
    }
    /// <summary>
    /// �e��Ƶ��˸m(data�n�H�r���Ϲj)
    /// </summary>
    public static void SendData(string address, string data, string key_num)
    {
        if (!IsConnected(address))
        {
            Debug.Log("Is Not Connected.");
            return;
        }
        if (string.IsNullOrEmpty(address))
        {
            Debug.Log("address IsNullOrEmpty.");
            return;
        }
        if (string.IsNullOrEmpty(data))
        {
            Debug.Log("data IsNullOrEmpty.");
            return;
        }
        Debug.Log("On Unity SendData to:" + address);
        SendInfo info = new SendInfo();
        info.Address = address;
        info.data = data;
        info.Key = key_num;
        WaitToSend.Enqueue(info);
        //_SendData(address, data, key_num);
    }
    /// <summary>
    /// �P�_�O�_�s�u
    /// </summary>
    public static bool IsConnected(string address)
    {
        if (!Devices_Dic.ContainsKey(address))
        {
            //Debug.Log("IsConnected == false");
            return false;
        }
        //Debug.Log("IsConnected == true");
        return Devices_Dic[address].Connected;

    }
    /// <summary>
    /// ���o�˸m�W��(�L�צ��L�s�u)
    /// </summary>
    public static string GetDeviceName(string address)
    {
        if (string.IsNullOrEmpty(address) || !Devices_Dic.ContainsKey(address))
        {
            Debug.Log("This address is not exist.");
            return null;
        }
        else
        {
            return Devices_Dic[address].DeviceName;
        }
        return "";
    }

    //public static byte[] Encode(byte[] input, byte key_num)
    //{
    //    return _Encode(input, key_num);
    //}

    #endregion

    #region [APIs : Plugin callback to Unity]
    /// <summary>
    /// ������s�˸m
    /// </summary>
    public void OnReceiveDevice(string str) //address|name
    {
        Debug.Log("OnReceiveDevice:" + str);
        string[] info = str.Split('|');
        onRceiveDevice(info[0], info[1]);
        if (!Devices_Dic.ContainsKey(info[0]))
        {
            BLEDevice newDevice = new BLEDevice();
            newDevice.Address = info[0];
            newDevice.DeviceName = info[1];
            Devices_Dic.Add(info[0], newDevice);
        }
    }
    /// <summary>
    /// ������˸m�s�����A�ܤ�
    /// </summary>
    public void DeviceStateChange(string str) //address|Disconnect/Connecting/Connected/Disconnecting
    {
        Debug.Log("DeviceStateChange:" + str);
        string[] info = str.Split('|');
        onDeviceStatusChanged(info[0], info[1]);
        if (Devices_Dic.ContainsKey(info[0]))
        {
            Devices_Dic[info[0]].Connected = (info[1] == "Connected") ? true : false;
            Debug.Log("Devices_Dic[info[0]].Connected :" + Devices_Dic[info[0]].Connected);
        }
    }
    /// <summary>
    /// ������˸m�^�ǰT��(�[�K)
    /// </summary>
    public void OnReceiveEncodeRawData(string str) //address|byte0,byte1,...,byteN
    {
        Debug.Log("OnReceiveEncodeRawData:" + str);
        string[] info = str.Split('|');
        onReceiveEncodeRawData(info[0], info[1]);
        //onReadeySendData(true);
    }
    /// <summary>
    /// ������˸m�^�ǰT��(�ѱK)
    /// </summary>
    public void OnReceiveDecodeRawData(string str) //address|byte0,byte1,...,byteN
    {
        Debug.Log("OnReceiveDecodeRawData:" + str);
        string[] info = str.Split('|');
        onReceiveDecodeRawData(info[0], info[1]);
    }

    #endregion

    #region [APIs : Unity call Plugin]

#if UNITY_ANDROID
    private static void _Init()
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("Init");
        }
    }

    private static void _StartScan()
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("StartScan");
        }
    }

    private static void _StopScan()
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("StopScan");
        }
    }

    private static void _Connect(string address)
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("Connect", address);
        }
    }

    private static void _Disconnect(string address)
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("Disconnect", address);
        }
    }

    private static void _SendData(string address, string data, string key_num)
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("SendData", address, data, key_num);
        }
    }

    private static void _checkPermission()
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("checkPermission");
        }
    }

    //private static byte[] _Encode(string input, string key_num)
    //{
    //    AndroidJNI.AttachCurrentThread();
    //    using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
    //    {
    //        string output = mjc.CallStatic<string>("Encode", input, key_num);
    //        return ByteString_To_ByteArr(output);
    //    }
    //}

#elif UNITY_EDITOR
    // ��l���Ť�
    private static void _Init(){}
    // �}�l���y
    private static void _StartScan(){}
    // ����y
    private static void _StopScan(){}
    // �s���˸m
    private static void _Connect(string address){}
    // �_�}�˸m
    private static void _Disconnect(string address){}
    // �e��Ƶ��˸m
    private static void _SendData(string address, string data, string key_num){}

    private static void _checkPermission(){}
    //// ��ƥ[�K(����)
    //private static byte[] _Encode(string input, string key_num){}
#endif
    #endregion

    #region [General Function]
    /// <summary>
    /// ��byte array�ର�r��(0x5A -> 5A), �H�r���Ϲj
    /// </summary>
    public static string ByteArr_To_ByteString(byte[] bytes)
    {
        if (bytes == null || bytes.Length <= 0)
        {
            Debug.LogError("bytes is null or Empty.");
            return null;
        }
        string str = "";
        for (int i = 0; i < bytes.Length; i++)
        {
            if (i == 0)
            {
                str = bytes[0].ToString("X");
            }
            else
            {
                str = string.Format("{0},{1:00}", str, bytes[i].ToString("X"));
            }
        }
        return str;
    }
    /// <summary>
    /// �ഫ�H�r�����Ϲj��byteString�ܦ�byte[]
    /// </summary>
    public static byte[] ByteString_To_ByteArr(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            Debug.Log("str IsNullOrEmpty.");
            return null;
        }
        string[] data = str.Split(',');
        byte[] bytes = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            bytes[i] = (byte)int.Parse(data[i], System.Globalization.NumberStyles.HexNumber);
        }
        return bytes;
    }

    #endregion
}
