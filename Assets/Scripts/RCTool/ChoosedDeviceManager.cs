using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class ChoosedDeviceManager : MonoBehaviour
{
    private static ChoosedDeviceManager Instance = null;

    [SerializeField, Header("[標題文字]")]
    Text TittleText;
    [SerializeField, Header("標題文字顏色(連線)")]
    Color connectedColor;
    [SerializeField, Header("標題文字顏色(斷線)")]
    Color disconnectedColor;
    [SerializeField, Header("[收到Callback]")]
    Text CallbackText;
    [SerializeField, Header("[管理的子畫面]")]
    List<GameObject> ChildPages = new List<GameObject>();
    [SerializeField, Header("[功能表按鈕]")]
    GameObject ToDoListButton;
    //
    public static string DeviceTittle = "";
    //
    public static string DeviceAddress = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

    }
    //啟用時
    private void OnEnable()
    {
        RCToolPlugin.onReceiveEncodeRawData += RCToolPlugin_onReceiveRawData;
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged += RCToolPlugin_onDeviceStatusChanged;
        //AppManager.AndroidBackButton += AppManager_AndroidBackButton;
        SetTittleText(DeviceTittle);
        SetTittleTextColor();
    }
    //停用時
    private void OnDisable()
    {
        RCToolPlugin.onReceiveEncodeRawData -= RCToolPlugin_onReceiveRawData;
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged -= RCToolPlugin_onDeviceStatusChanged;
        //AppManager.AndroidBackButton -= AppManager_AndroidBackButton;
        SetTittleText("");
    }
    //收到的callback(Encode)
    private void RCToolPlugin_onReceiveRawData(string address, string data)
    {
        //Debug.Log("OnUnityReceiveRawData :" + address + "|||" + data);
        string input = string.Format("\n[{0}]{1} :{2}", DateTime.Now, address, data);
        CallbackText.text += input;
        
    }
    //收到的callback(Decode)
    private void RCToolPlugin_onReceiveDecodeRawData(string address, string data)
    {
        string input = string.Format("\n<color=red>[{0}]{1} :{2}</color>", DateTime.Now, address, data);
        CallbackText.text += input;
        ParseCallBack.CallbackInfo(address, data);
    }

    private void RCToolPlugin_onDeviceStatusChanged(string address, string status)
    {
        switch (status)
        {
            case "Connected":
                SetTittleText(RCToolPlugin.GetDeviceName(address) + "|" + address);
                CommandManager.SendCMD(address, "02", null);
                break;
            default:
                Debug.Log(address + " status :" + status);
                break;
        }
    }
    //返回按鈕
    public void AppManager_AndroidBackButton()
    {
        AppManager.Instance.SetPage("Home");
    }

    private void Update()
    {
        SetTittleTextColor();
        SetToDoListButton();
    }
    /// <summary>
    /// 設置標題文字
    /// </summary>
    public void SetTittleText(string content)
    {
        TittleText.text = content;
    }
    /// <summary>
    /// 連線/離線時設置標題顏色
    /// </summary>
    private void SetTittleTextColor()
    {
        if (RCToolPlugin.IsConnected(DeviceAddress))
            TittleText.color = connectedColor;
        else
            TittleText.color = disconnectedColor;
    }

    private void SetToDoListButton()
    {
        ToDoListButton.SetActive(RCToolPlugin.IsConnected(DeviceAddress));
    }

    public void RequestBikeDetail()
    {
        string[] cmdList = new string[] { "05", "09", "32", "30", "D4", "D1", "D2", "D3", "0D", "13", "0E", "37", "38", "39" };
    }

    #region [按鈕呼叫的]
    public void SendCMD(string cmdNum)
    {
        if (!RCToolPlugin.IsConnected(DeviceAddress))
        {
            Debug.Log("Device is not Connected.");
            return;
        }
        CommandManager.SendCMD(DeviceAddress, cmdNum, null);
    }

    public void Connect()
    {
        RCToolPlugin.ConnectDevice(DeviceAddress);
    }

    public void Disconnect()
    {
        RCToolPlugin.DisconnectDevice(DeviceAddress);
    }
    #endregion
}
