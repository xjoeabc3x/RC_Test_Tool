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

        //RCToolPlugin_onReceiveDecodeRawData("test", "FC,21,DB,10,0A,11,13,0A,0E,0A,00,00,00,00,00,00,00,00,0E,F1");
        //RCToolPlugin_onReceiveDecodeRawData("test", "FC,21,DB,10,00,00,00,00,00,00,00,00,00,00,00,00,00,00,02,1C");
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
            List<string> log = new List<string>();
            log.Add(content);
            DeviceLogDic.Add(address, log);
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

                DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

                using (StreamWriter file = new StreamWriter(path))
                {
                    file.WriteLine(str);
                }

                //clear after save
                DeviceLogDic[address].Clear();
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
