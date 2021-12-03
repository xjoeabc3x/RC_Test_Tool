using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class L2View : MonoBehaviour
{
    public static L2View Instance = null;

    private string CurrentType;
    public string CurrentFile;

    [SerializeField, Header("[]")]
    Dropdown TypeChoose;
    [SerializeField, Header("[]")]
    Dropdown MCUChoose;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this);
            return;
        }
    }

    private void OnEnable()
    {
        RCToolPlugin.onReceiveL2Msg += RCToolPlugin_onReceiveL2Msg;
        RCToolPlugin.onReceiveL2Error += RCToolPlugin_onReceiveL2Error;
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveL2Msg -= RCToolPlugin_onReceiveL2Msg;
        RCToolPlugin.onReceiveL2Error -= RCToolPlugin_onReceiveL2Error;
    }

    private void RCToolPlugin_onReceiveL2Msg(string address, string msg)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveL2Msg :add:{0}, msg:{1}", address, msg));
        Toast.Instance.ShowToast(string.Format("address:{0}, msg:{1}", address, msg));
        if (msg.ToLower().Contains("complete"))
        {
            Loading.Instance.HideLoading();
        }
    }

    private void RCToolPlugin_onReceiveL2Error(string address, string msg)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveL2Error :add:{0}, msg:{1}", address, msg));
        Toast.Instance.ShowToast(string.Format("address:{0}, msg:{1}", address, msg));
        if (msg.ToLower().Contains("fail"))
        {
            RCToolPlugin.AbortL2();
            Loading.Instance.HideLoading();
        }
    }

    public void StartL2_Test(string filename)
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        RCToolPlugin.StartL2(ChoosedDeviceManager.DeviceAddress, "12", Path.Combine(Application.persistentDataPath, filename));
        Loading.Instance.ShowLoading();
    }

    public void StartL2()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        if (string.IsNullOrEmpty(ChoosedDeviceManager.DeviceAddress) || string.IsNullOrEmpty(CurrentFile) || string.IsNullOrEmpty(CurrentType))
            return;
        RCToolPlugin.StartL2(ChoosedDeviceManager.DeviceAddress, CurrentType, CurrentFile);
        Loading.Instance.ShowLoading();
    }
}
