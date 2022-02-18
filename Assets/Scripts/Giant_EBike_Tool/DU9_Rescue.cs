using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DU9_Rescue : MonoBehaviour
{
    private void OnEnable()
    {
        HomeManager.RegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
        RCToolPlugin.onReceiveL2Msg += RCToolPlugin_onReceiveL2Msg;
        RCToolPlugin.onReceiveL2Error += RCToolPlugin_onReceiveL2Error;
    }

    private void OnDisable()
    {
        HomeManager.UnRegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
        RCToolPlugin.onReceiveL2Msg -= RCToolPlugin_onReceiveL2Msg;
        RCToolPlugin.onReceiveL2Error -= RCToolPlugin_onReceiveL2Error;
    }

    private void ParseCallBack_onReceiveDecodeParsedData(string callback)
    {
        if (!string.IsNullOrEmpty(callback))
        {
            
        }
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

    public void StartCANRescue()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
    }
}
