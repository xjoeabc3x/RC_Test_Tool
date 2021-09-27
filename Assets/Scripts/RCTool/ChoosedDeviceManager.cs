using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class ChoosedDeviceManager : MonoBehaviour
{
    private static ChoosedDeviceManager Instance = null;

    [SerializeField, Header("[���D��r]")]
    Text TittleText;
    [SerializeField, Header("���D��r�C��(�s�u)")]
    Color connectedColor;
    [SerializeField, Header("���D��r�C��(�_�u)")]
    Color disconnectedColor;
    [SerializeField, Header("[����Callback]")]
    Text CallbackText;
    [SerializeField, Header("[�޲z���l�e��]")]
    List<GameObject> ChildPages = new List<GameObject>();
    [SerializeField, Header("[�\�����s]")]
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
    //�ҥή�
    private void OnEnable()
    {
        RCToolPlugin.onReceiveEncodeRawData += RCToolPlugin_onReceiveRawData;
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged += RCToolPlugin_onDeviceStatusChanged;
        //AppManager.AndroidBackButton += AppManager_AndroidBackButton;
        SetTittleText(DeviceTittle);
        SetTittleTextColor();
    }
    //���ή�
    private void OnDisable()
    {
        RCToolPlugin.onReceiveEncodeRawData -= RCToolPlugin_onReceiveRawData;
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged -= RCToolPlugin_onDeviceStatusChanged;
        //AppManager.AndroidBackButton -= AppManager_AndroidBackButton;
        SetTittleText("");
    }
    //���쪺callback(Encode)
    private void RCToolPlugin_onReceiveRawData(string address, string data)
    {
        //Debug.Log("OnUnityReceiveRawData :" + address + "|||" + data);
        string input = string.Format("\n[{0}]{1} :{2}", DateTime.Now, address, data);
        CallbackText.text += input;
        
    }
    //���쪺callback(Decode)
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
    //��^���s
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
    /// �]�m���D��r
    /// </summary>
    public void SetTittleText(string content)
    {
        TittleText.text = content;
    }
    /// <summary>
    /// �s�u/���u�ɳ]�m���D�C��
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

    #region [���s�I�s��]
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
