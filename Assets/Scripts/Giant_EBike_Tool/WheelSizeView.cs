using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;
using System;

public class WheelSizeView : MonoBehaviour
{
    [SerializeField, Header("[½ü®|¿é¤J]")]
    InputField wheelsize_input;
    [SerializeField, Header("[­­³t¿é¤J]")]
    InputField speedLimit_input;

    private string[] cache;

    private void OnEnable()
    {
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged += RCToolPlugin_onDeviceStatusChanged;
        Get03();
    }

    private void RCToolPlugin_onDeviceStatusChanged(string address, string status)
    {
        switch (status)
        {
            case "Connected":
                Get03();
                break;
            default:
                Debug.Log(address + " status :" + status);
                break;
        }
    }

    private void RCToolPlugin_onReceiveDecodeRawData(string address, string data)
    {
        string callback = ParseCallBack.CallbackInfo(address, data);
        if (!string.IsNullOrEmpty(callback))
        {
            string Key = callback.Split('|')[1];
            string Value = callback.Split('|')[2];
            if (Key == "03")
            {
                cache = ParseCallBack.GetAES(data);
                Sync03(Value);
            }
            if (Key == "04")
            {
                Toast.Instance.ShowToast("[04] Set Finished :" + Value);
            }
        }
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged -= RCToolPlugin_onDeviceStatusChanged;
        cache = null;
    }

    private void Get03()
    {
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "03", null, null);
    }
    private void Sync03(string input)
    {
        speedLimit_input.text = input.Split(',')[0];
        wheelsize_input.text = input.Split(',')[1];
    }

    public void Save04()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device not connected");
            return;
        }
        else
        {
            if (string.IsNullOrEmpty(speedLimit_input.text))
            {
                speedLimit_input.text = "0";
            }
            if (string.IsNullOrEmpty(wheelsize_input.text))
            {
                wheelsize_input.text = "0";
            }

            if (int.Parse(speedLimit_input.text) > 255 || int.Parse(speedLimit_input.text) < 0)
            {
                Toast.Instance.ShowToast("Speed Limit Input Error.");
                return;
            }
            if (int.Parse(wheelsize_input.text) > 65535 || int.Parse(wheelsize_input.text) < 0)
            {
                Toast.Instance.ShowToast("Wheel Size Input Error.");
                return;
            }

            string spdlim = int.Parse(speedLimit_input.text).ToString("X2");
            string ccfr = int.Parse(wheelsize_input.text).ToString("X4"); //000B
            string ccfr_h = ccfr[0].ToString() + ccfr[1].ToString();
            string ccfr_l = ccfr[2].ToString() + ccfr[3].ToString();
            cache[0] = spdlim;
            cache[1] = ccfr_l;
            cache[2] = ccfr_h;
            CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "04", null, cache);
        }
    }
}
