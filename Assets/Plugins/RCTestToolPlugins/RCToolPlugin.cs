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
        public string rssi = "";
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

    #region [�ƥ�ŧi]
    /// <summary>
    /// �j�M�ƥ�
    /// </summary>
    public delegate void EventType_NewDevice(string address, string name, string rssi);
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
    /// <summary>
    /// DFU���A
    /// </summary>
    public delegate void EventType_DFUStatus(string address, string status);
    public static event EventType_DFUStatus onReceiveDFUStatus;
    /// <summary>
    /// DFU��s�i��
    /// </summary>
    public delegate void EventType_DFUProgress(string address, string percent);
    public static EventType_DFUProgress onReceiveDFUProgress;
    /// <summary>
    /// DFU���~�T��
    /// </summary>
    public delegate void EventType_DFUError(string address, string errorcode, string errortype, string msg);
    public static EventType_DFUError onReceiveDFUError;
    /// <summary>
    /// L2��s�T��
    /// </summary>
    public delegate void EventType_L2Msg(string address, string msg);
    public static EventType_L2Msg onReceiveL2Msg;
    /// <summary>
    /// L2��s���~
    /// </summary>
    public delegate void EventType_L2Error(string address, string errormsg);
    public static EventType_L2Error onReceiveL2Error;
    #endregion

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
    private float senddelay = 0.1f;
    private void Update()
    {
        SendDataWithDelay();
    }
    /// <summary>
    /// �C0.1��e�@�����O,��callback�Y�e�U�@��
    /// </summary>
    private void SendDataWithDelay()
    {
        if (WaitToSend.Count > 0)
        {
            if (senddelay > 0)
            {
                senddelay -= Time.deltaTime * 1;
            }
            else if (senddelay <= 0)
            {
                SendInfo info = WaitToSend.Dequeue();
                Debug.Log("OnUnity SendDataWithDelay :" + info.data);
                _SendData(info.Address, info.data, info.Key);
                senddelay = 0.1f;
            }
        }
    }

    #region [APIs for Unity call]
    /// <summary>
    /// �I�sPlugin����Destroy����
    /// </summary>
    public static void Destroy()
    {
        Debug.Log("On Unity Destroy...");
        _Destroy();
    }
    /// <summary>
    /// �}�lDFU��s
    /// </summary>
    /// <param name="address">macAddress</param>
    /// <param name="filePath">�ɦW�t���ɦW</param>
    public static void StartDFU(string address, string filePath)
    {
        Debug.Log(string.Format("On Unity StartDFU({0}, {1})...", address, filePath));
        _StartDFU(address, filePath);
    }

    public static void AbortDFU()
    {
        Debug.Log("On Unity AbortDFU...");
        _AbortDFU();
    }

    public static void StartL2(string address, string type, string filePath)
    {
        if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(filePath))
            return;
        _StartL2(address, type, filePath);
    }

    public static void AbortL2()
    {
        _AbortL2();
    }
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

    #endregion

    #region [APIs : Plugin callback to Unity]
    /// <summary>
    /// ������s�˸m
    /// </summary>
    public void OnReceiveDevice(string str) //address|name|rssi
    {
        Debug.Log("OnReceiveDevice:" + str);
        string[] info = str.Split('|');
        onRceiveDevice(info[0], info[1], info[2]);
        if (!Devices_Dic.ContainsKey(info[0]))
        {
            BLEDevice newDevice = new BLEDevice();
            newDevice.Address = info[0];
            newDevice.DeviceName = info[1];
            newDevice.rssi = info[2];
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
        if (Devices_Dic.ContainsKey(info[0]))
        {
            Devices_Dic[info[0]].Connected = (info[1] == "Connected") ? true : false;
            Debug.Log("Devices_Dic[info[0]].Connected :" + Devices_Dic[info[0]].Connected);
        }
        onDeviceStatusChanged(info[0], info[1]);
    }
    /// <summary>
    /// ������˸m�^�ǰT��(�[�K)
    /// </summary>
    public void OnReceiveEncodeRawData(string str) //address|byte0,byte1,...,byteN
    {
        Debug.Log("OnReceiveEncodeRawData:" + str);
        string[] info = str.Split('|');
        onReceiveEncodeRawData(info[0], info[1]);
    }
    /// <summary>
    /// ������˸m�^�ǰT��(�ѱK)
    /// </summary>
    public void OnReceiveDecodeRawData(string str) //address|byte0,byte1,...,byteN
    {
        Debug.Log("OnReceiveDecodeRawData:" + str);
        string[] info = str.Split('|');
        onReceiveDecodeRawData(info[0], info[1]);
        //IsFCcallback(str);
    }

    private bool IsFCcallback(string input)
    {
        if (input.Split(',')[0] == "FC")
        {
            senddelay = 0;
            return true;
        }
        return false;
    }
    /// <summary>
    /// DFU��s���A
    /// </summary>
    /// <param name="str">address|status</param>
    public void DFUStatus(string str)
    {
        Debug.Log("On Unity DFUStatus :" + str);
        if (string.IsNullOrEmpty(str) || str.Split('|').Length < 2)
            return;
        string[] info = str.Split('|');
        onReceiveDFUStatus(info[0], info[1]);
    }
    /// <summary>
    /// DFU��s�i�צʤ���
    /// </summary>
    /// <param name="str">address|percent(int)</param>
    public void DFUProgress(string str)
    {
        Debug.Log("On Unity DFUProgress :" + str);
        if (string.IsNullOrEmpty(str) || str.Split('|').Length < 2)
            return;
        string[] info = str.Split('|');
        onReceiveDFUProgress(info[0], info[1]);
    }
    /// <summary>
    /// DFU��s���~�T��
    /// </summary>
    /// <param name="str">address|errorcode(int)|errortype(int)|msg(string)</param>
    public void DFUError(string str)
    {
        Debug.Log("On Unity DFUProgress :" + str);
        if (string.IsNullOrEmpty(str) || str.Split('|').Length < 4)
            return;
        string[] info = str.Split('|');
        onReceiveDFUError(info[0], info[1], info[2], info[3]);
    }

    public void L2_UpdateMsg(string str)
    {
        Debug.Log("On Unity  L2_UpdateMsg:" + str);
        if (string.IsNullOrEmpty(str) || str.Split('|').Length < 2)
            return;
        string[] info = str.Split('|');
        onReceiveL2Msg(info[0], info[1]);
    }

    public void L2_UpdateError(string str)
    {
        Debug.Log("On Unity  L2_UpdateError:" + str);
        if (string.IsNullOrEmpty(str) || str.Split('|').Length < 2)
            return;
        string[] info = str.Split('|');
        onReceiveL2Error(info[0], info[1]);
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

    private static void _Destroy()
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("Destroy");
        }
    }

    private static void _StartDFU(string address, string filepath)
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("StartDFU", address, filepath);
        }
    }

    private static void _AbortDFU()
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("AbortDFU");
        }
    }

    private static void _StartL2(string address, string func, string filePath)
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("StartL2", address, func, filePath);
        }
    }

    private static void _AbortL2()
    {
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass mjc = new AndroidJavaClass("com.giant.RCTestTool.BluetoothLeService.UnityPlugins"))
        {
            mjc.CallStatic("AbortL2");
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
    // �I�sPlugin����Destroy����
    private static void _Destroy(){}
    // �}�l�s��DFU��s
    private static void _StartDFU(string address, string filepath){}

    private static void _StartL2(string address, string type, string filePath){}

    private static void _AbortL2(){}
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
