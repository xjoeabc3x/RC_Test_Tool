using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;
using System;

public class EnergyPakView : MonoBehaviour
{
    [SerializeField, Header("[SG¦^À³]")]
    Text Callback_Text;

    private void OnEnable()
    {
        HomeManager.RegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
    }

    private void OnDisable()
    {
        HomeManager.UnRegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
    }

    private void ParseCallBack_onReceiveDecodeParsedData(string callback)
    {
        if (!string.IsNullOrEmpty(callback))
        {
            string Key = callback.Split('|')[1];
            string Value = callback.Split('|')[2];
            if (Key == "DB" && !Value.EndsWith("wait"))
            {
                string[] datas = Value.Split(',');
                //Panasonic
                //PF/GA,EPFW,EPSN(yyyymmddxxxxx),vcmax,vcmin,tmp1,tmp2
                if (datas.Length > 1)
                {
                    Callback_Text.text = string.Format("<color=yellow>Panasonic : </color>\nCell Version :{0}\n", datas[0]);
                    Callback_Text.text += string.Format("EnergyPak Firmware Version :{0}\n", datas[1]);
                    Callback_Text.text += string.Format("EnergyPak SN number(yyyymmddxxxxx) :{0}\n", datas[2]);
                    Callback_Text.text += string.Format("Max Cell Voltage :{0} mV\n", datas[3]);
                    Callback_Text.text += string.Format("Min Cell Voltage :{0} mV\n", datas[4]);
                    Callback_Text.text += string.Format("Cell 1 Temp :{0} ¢XC\n", datas[5]);
                    Callback_Text.text += string.Format("Cell 2 Temp :{0} ¢XC", datas[6]);
                }
                //Trend
                else
                {
                    Callback_Text.text = "<color=yellow>TREND POWER ID : </color>\n" + Value;
                }
            }
        }
    }

    public void Send_DB()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        Callback_Text.text = "<color=yellow>Callback...</color>";
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "DB", null, null);
    }
}
