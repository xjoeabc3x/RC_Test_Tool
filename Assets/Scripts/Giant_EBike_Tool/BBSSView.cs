using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class BBSSView : MonoBehaviour
{
    [SerializeField, Header("[ºÏ³q¶q]")]
    Text Magnetic_Flux;

    private void OnEnable()
    {
        HomeManager.RegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
    }

    private void ParseCallBack_onReceiveDecodeParsedData(string callback)
    {
        if (!string.IsNullOrEmpty(callback))
        {
            string Key = callback.Split('|')[1];
            string Value = callback.Split('|')[2];
            if (Key == "40")
            {
                Parse_40(Value);
            }
        }
    }

    private void OnDisable()
    {
        HomeManager.UnRegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "20", null, null);
    }

    private void Parse_40(string datas)
    {
        string[] data = datas.Split(',');
        string result = string.Format("<color=yellow>[40]Callback</color>\n\nAck(1:success, 0:fail) : {0}\nMax Magnetic Flux : {1} Wb\nMin Magnetic Flux : {2} Wb", data[0], data[1], data[2]);
        Magnetic_Flux.text = result;
    }

    public void Send_40(bool onoff)
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        //if (onoff)
        //{
        //    CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "01", null, null);
        //    CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "40", new byte[] { 0x01 }, null);
        //}
        //else
        //    CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "40", new byte[] { 0x00 }, null);

        StartCoroutine("_Getting");
    }

    private IEnumerator _Getting()
    {
        Magnetic_Flux.text = string.Format("<color=yellow>[40]Callback</color>");
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "01", null, null);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "40", new byte[] { 0x01 }, null);
        for (int i = 10; i > 1; i--)
        {
            Magnetic_Flux.text = string.Format("<color=yellow>[40]Callback</color>\n\nWaitng...({0})", i);
            yield return new WaitForSeconds(1.0f);
        }
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "40", new byte[] { 0x00 }, null);
    }

    public void Cancel()
    {
        StopCoroutine("_Getting");
        Magnetic_Flux.text = "<color=yellow>[40]Callback</color>";
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "20", null, null);
    }
}
