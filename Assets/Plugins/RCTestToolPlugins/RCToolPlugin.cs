using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// 1.初始化藍芽服務
/// 2.掃描/停止掃描藍芽裝置
/// 3.連接/斷開藍芽裝置
/// 4.送出指令給藍芽裝置
/// 5.接收藍芽回傳訊息
/// </summary>
public class RCToolPlugin : MonoBehaviour
{
    /// <summary>
    /// 藍牙裝置
    /// </summary>
    public class BLEDevice
    {
        public string Address = "";
        public string DeviceName = "";
        public string rssi = "";
        public bool Connected = false;
    }
    /// <summary>
    /// 等待送出的指令class
    /// </summary>
    public class SendInfo
    {
        public string Address = "";
        public string data = "";
        public string Key = "";
    }
    /// <summary>
    /// 等待送出的指令Queue
    /// </summary>
    private static Queue<SendInfo> WaitToSend = new Queue<SendInfo>();

    public static RCToolPlugin Instance;
    /// <summary>
    /// 搜尋到的裝置列表, Key:address
    /// </summary>
    public static Dictionary<string, BLEDevice> Devices_Dic = new Dictionary<string, BLEDevice>();
    /// <summary>
    /// 搜尋事件
    /// </summary>
    public delegate void EventType_NewDevice(string address, string name, string rssi);
    public static event EventType_NewDevice onRceiveDevice;
    /// <summary>
    /// 裝置連接狀態事件
    /// </summary>
    public delegate void EventType_DeviceStatusChanged(string address, string status);
    public static event EventType_DeviceStatusChanged onDeviceStatusChanged;
    /// <summary>
    /// 接收裝置訊息事件(加密)
    /// </summary>
    public delegate void EventType_ReceiveEncodeRawData(string address, string data);
    public static event EventType_ReceiveEncodeRawData onReceiveEncodeRawData;
    /// <summary>
    /// 接收裝置訊息事件(解密)
    /// </summary>
    public delegate void EventType_ReceiveDecodeRawData(string address, string data);
    public static event EventType_ReceiveDecodeRawData onReceiveDecodeRawData;

    //初始化
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
    //指令延遲
    private float senddelay = 0.05f;
    private void Update()
    {
        SendDataWithDelay();
    }
    /// <summary>
    /// 每0.5秒送一次指令,有callback即送下一個
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
                senddelay = 0.05f;
            }
        }
    }

    #region [APIs for Unity call]
    /// <summary>
    /// 開始掃描
    /// </summary>
    public static void StartScan()
    {
        Debug.Log("On Unity StartScan...");
        _StartScan();
        Devices_Dic.Clear();
    }
    /// <summary>
    /// 停止掃描
    /// </summary>
    public static void StopScan()
    {
        Debug.Log("On Unity StopScan...");
        _StopScan();
    }
    /// <summary>
    /// 連接裝置
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
    /// 斷開裝置
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
    /// 送資料給裝置
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
    /// 送資料給裝置(data要以逗號區隔)
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
    /// 判斷是否連線
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
    /// 取得裝置名稱(無論有無連線)
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
    /// 接收到新裝置
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
    /// 接收到裝置連接狀態變化
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
    /// 接收到裝置回傳訊息(加密)
    /// </summary>
    public void OnReceiveEncodeRawData(string str) //address|byte0,byte1,...,byteN
    {
        Debug.Log("OnReceiveEncodeRawData:" + str);
        string[] info = str.Split('|');
        onReceiveEncodeRawData(info[0], info[1]);
        IsFCcallback(str);
    }
    /// <summary>
    /// 接收到裝置回傳訊息(解密)
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
    // 初始化藍牙
    private static void _Init(){}
    // 開始掃描
    private static void _StartScan(){}
    // 停止掃描
    private static void _StopScan(){}
    // 連接裝置
    private static void _Connect(string address){}
    // 斷開裝置
    private static void _Disconnect(string address){}
    // 送資料給裝置
    private static void _SendData(string address, string data, string key_num){}

    private static void _checkPermission(){}
    //// 資料加密(停用)
    //private static byte[] _Encode(string input, string key_num){}
#endif
    #endregion

    #region [General Function]
    /// <summary>
    /// 把byte array轉為字串(0x5A -> 5A), 以逗號區隔
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
    /// 轉換以逗號為區隔的byteString變成byte[]
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
