using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using ParseRCCallback;
using UnityEngine.Events;
using System.Globalization;

public class HomeManager : MonoBehaviour
{
    #region [宣告區]
    public static HomeManager Instance;
    [SerializeField, Header("[按鈕Prefab]")]
    GameObject ButtonPrefab;
    [SerializeField, Header("[按鈕擺放位置]")]
    RectTransform ButtonPos;

    Dictionary<string, GameObject> ButtonDic = new Dictionary<string, GameObject>();
    private static Dictionary<string, List<string>> DeviceLogDic = new Dictionary<string, List<string>>();
    private static Dictionary<string, List<string>> RideRecordDic = new Dictionary<string, List<string>>();

    //public delegate void EventType_ReceiveDecodeParsedData(string callback);
    //public static event EventType_ReceiveDecodeParsedData onReceiveDecodeParsedData;

    //public delegate void EventType_ReceiveEncodeParsedData(string callback);
    //public static event EventType_ReceiveEncodeParsedData onReceiveEncodeParsedData;

    private static UnityEvent<string> onReceiveDecodeParsedData = new UnityEvent<string>();
    private static UnityEvent<string> onReceiveEncodeParsedData = new UnityEvent<string>();

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(this);
        }
        //註冊接收事件
        RCToolPlugin.onRceiveDevice += RCToolPlugin_onRceiveDevice;
        RCToolPlugin.onDeviceStatusChanged += RCToolPlugin_onDeviceStatusChanged;
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onReceiveEncodeRawData += RCToolPlugin_onReceiveEncodeRawData;
        RegistEncodeEvent(ParseCallBack_onReceiveEncodeParsedData);
        //Debug.Log(string.Format("\r\n[{0}]", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff")));
        //RCToolPlugin_onReceiveDecodeRawData("test", "FC,21,DB,10,0A,11,13,0A,0E,0A,00,00,00,00,00,00,00,00,0E,F1");
        //RCToolPlugin_onReceiveDecodeRawData("test", "FC,21,DB,10,00,00,00,00,00,00,00,00,00,00,00,00,00,00,02,1C");
    }

    private void ParseCallBack_onReceiveEncodeParsedData(string input)
    {
        Debug.Log("HomeManager.ParseCallBack_onReceiveEncodeParsedData :" + input);
        string address = input.Split('|')[0];
        string Key = input.Split('|')[1];
        string Value = input.Split('|')[2];
        if (Key == "23")
        {
            AddNewRideRecord(address, Value);
        }
    }

    private void RCToolPlugin_onReceiveEncodeRawData(string address, string data)
    {
        string callback = ParseCallBack.Parse_Encode(address, data);
        if (callback != null)
            onReceiveEncodeParsedData.Invoke(callback);
    }

    private void RCToolPlugin_onReceiveDecodeRawData(string address, string data)
    {
        string callback = ParseCallBack.Parse_Decode(address, data);
        if (callback != null)
            onReceiveDecodeParsedData.Invoke(callback);
    }
    #region [註冊事件]
    /// <summary>
    /// 註冊要接收解碼解析後的事件
    /// </summary>
    public static void RegistDecodeEvent(UnityAction<string> function)
    {
        onReceiveDecodeParsedData.AddListener(function);
    }
    /// <summary>
    /// 註銷要接收解碼解析後的事件
    /// </summary>
    public static void UnRegistDecodeEvent(UnityAction<string> function)
    {
        onReceiveDecodeParsedData.RemoveListener(function);
    }
    /// <summary>
    /// 註冊要接收明碼解析後的事件
    /// </summary>
    public static void RegistEncodeEvent(UnityAction<string> function)
    {
        onReceiveEncodeParsedData.AddListener(function);
    }
    /// <summary>
    /// 註銷要接收明碼解析後的事件
    /// </summary>
    public static void UnRegistEncodeEvent(UnityAction<string> function)
    {
        onReceiveEncodeParsedData.RemoveListener(function);
    }
    #endregion
    #region --Functions--

    public void ButtonClicked(string address)
    {
        ChoosedDeviceManager.DeviceTittle = RCToolPlugin.GetDeviceName(address) + "|" + address;
        ChoosedDeviceManager.DeviceAddress = address;
        AppManager.Instance.SetPage("ChoosedDevice");
    }

    public static void AddNewLog(string address, string content)
    {
        if (DeviceLogDic.ContainsKey(address))
        {
            DeviceLogDic[address].Add(content);
        }
        else
        {
            //List<string> log = new List<string>();
            //log.Add(content);
            DeviceLogDic.Add(address, new List<string>() { content });
        }
    }

    public static void AddNewRideRecord(string address, string content)
    {
        Debug.Log("AddNewRideRecord :" + address + "|" + content);
        //add time
        //parse time,speed[], trq[], cde[], acur[], trid[], trit[], hpw[], rsoc[], ecode[], carr[], curast[], odo[]
        //string.Format("\n[{0}] :{1}", DateTime.Now, data)
        string[] datas = content.Split(',');
        string result = string.Format("\r\n[{0}]", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff"));
        switch (datas.Length)
        {
            case 2: //5 ecode,carr
                result += string.Format(",,,,,,,,,{0},{1},,", datas[0], datas[1]);
                break;
            case 4: //8
                result += string.Format(",,,,,,,,,{0},{1},{2},{3}", datas[0], datas[1], datas[2], datas[3]);
                break;
            case 8: //17
                result += string.Format(",{0},{1},{2},{3},{4},{5},{6},{7},,,,", datas[0], datas[1], datas[2], datas[3], datas[4], datas[5], datas[6], datas[7]);
                break;
            default:
                break;
        }
        if (RideRecordDic.ContainsKey(address))
        {
            Debug.Log("AddNewRideRecord 123456");
            RideRecordDic[address].Add(result);
        }
        else
        {
            Debug.Log("AddNewRideRecord 1234567890");
            RideRecordDic.Add(address, new List<string>() { result });
        }
    }

    private static void SaveLog(string address)
    {
        if (DeviceLogDic.ContainsKey(address))
        {
            if (DeviceLogDic[address].Count > 0)
            {
                //save Logs
                string str = ChoosedDeviceManager.GetBikeDetail_Log(address);
                string date = DateTime.Now.ToString().Replace('/', '_');
                string path = Path.Combine(Application.persistentDataPath, string.Format("{0}-{1}.txt", address.Replace(':', '_'), date));
                //File.Create(path);
                for (int i = 0; i < DeviceLogDic[address].Count; i++)
                {
                    str += DeviceLogDic[address][i];
                }

                //DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

                using (StreamWriter file = new StreamWriter(path))
                {
                    file.WriteLine(str);
                }

                //clear after save
                DeviceLogDic[address].Clear();
            }
        }
    }

    public static void SaveRideRecord(string address)
    {
        if (RideRecordDic.ContainsKey(address))
        {
            if (RideRecordDic[address].Count > 0)
            {
                //save Logs
                string str = ChoosedDeviceManager.GetBikeDetail_RideRecord(address);
                //time,speed[], trq[], cde[], acur[], trid[], trit[], hpw[], rsoc[], ecode[], carr[], curast[], odo[]
                str += "\r\n\r\nTime(ms),Speed(km/hr),Trq(Nm),Cadance(RPM),Motor Current(A),Trid(km),Trit(min),Human Power(W),Battery(%),Ecode,carr(km),Current Assist,DU ODO(km)";
                string date = DateTime.Now.ToString().Replace('/', '_');
                string path = Path.Combine(Application.persistentDataPath, string.Format("REC_{0}-{1}.csv", address.Replace(':', '_'), date));
                //File.Create(path);
                for (int i = 0; i < RideRecordDic[address].Count; i++)
                {
                    str += RideRecordDic[address][i];
                }
                //DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

                using (StreamWriter file = new StreamWriter(path))
                {
                    file.WriteLine(str);
                }
                //clear after save
                RideRecordDic[address].Clear();
                Toast.Instance.ShowToast("Ride Record saved.");
            }
        }
    }

    public void OpenFile()
    {
        //Application.OpenURL(Path.Combine(Application.streamingAssetsPath, filename));
        Application.OpenURL("https://drive.google.com/file/d/1RKV6En1W4Iokgh-PW02O160OgSZWXops/view?usp=sharing");
    }
    #endregion

    #region --Events--
    private void RCToolPlugin_onRceiveDevice(string address, string name, string rssi)
    {
        if (!ButtonDic.ContainsKey(address))
        {
            var NewButton = Instantiate(ButtonPrefab, ButtonPos);
            NewButton.GetComponent<DeviceButton>().SetButtonInfo(name + "|" + address + "|" + rssi, address);
            ButtonDic.Add(address, NewButton);
        }
    }

    private void RCToolPlugin_onDeviceStatusChanged(string address, string status)
    {
        if (ButtonDic.ContainsKey(address))
        {
            Toast.Instance.ShowToast(string.Format("[{0}] connection status change :{1}", address, status));
            DeviceButton deviceButton = ButtonDic[address].GetComponent<DeviceButton>();
            switch (status)
            {
                case "Connected":
                    deviceButton.SetConnectStatus(true);
                    break;
                default:
                    deviceButton.SetConnectStatus(false);
                    //斷線存Log
                    SaveLog(address);
                    SaveRideRecord(address);
                    break;
            }
        }
    }
    #endregion
    #region --Buttons--
    public void StartScan()
    {
        ClearButton();
        RCToolPlugin.StartScan();
    }

    private void ClearButton()
    {
        foreach (KeyValuePair<string, GameObject> obj in ButtonDic)
        {
            Destroy(obj.Value);
        }
        ButtonDic.Clear();
    }

    public void StopScan()
    {
        RCToolPlugin.StopScan();
    }
    #endregion
}
