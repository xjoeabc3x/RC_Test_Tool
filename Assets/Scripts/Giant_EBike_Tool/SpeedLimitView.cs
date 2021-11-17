using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class SpeedLimitView : MonoBehaviour
{
    [SerializeField, Header("[限速開關]")]
    Toggle SpeedLimit_toggle;
    [SerializeField, Header("[單位切換]")]
    Toggle Unit_toggle;

    private void OnEnable()
    {
        HomeManager.RegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "C0", null, null);
    }

    private void ParseCallBack_onReceiveDecodeParsedData(string callback)
    {
        if (!string.IsNullOrEmpty(callback))
        {
            string Key = callback.Split('|')[1];
            string Value = callback.Split('|')[2];
            if (Key == "C0")
            {
                Sync_C0(Value);
            }
            if (Key == "C1")
            {
                Toast.Instance.ShowToast("[C1] Set Finished :" + Value);
            }
        }
    }

    private void Sync_C0(string data)
    {
        SpeedLimit_toggle.isOn = (data.Split(',')[0] == "0")? true : false;
        Unit_toggle.isOn = (data.Split(',')[1] == "0") ? true : false;
    }

    private void OnDisable()
    {
        HomeManager.UnRegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
    }

    public void Set_C1()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device not connected.");
            return;
        }
        byte splm = (byte)((SpeedLimit_toggle.isOn) ? 0x02 : 0x00);
        byte mkph = (byte)((Unit_toggle.isOn) ? 0x01 : 0x00);
        byte[] senddata = new byte[] { (byte)(splm ^ mkph) };
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "C1", senddata, null);
    }
}
