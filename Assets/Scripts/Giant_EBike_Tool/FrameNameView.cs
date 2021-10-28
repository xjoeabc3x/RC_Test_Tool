using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class FrameNameView : MonoBehaviour
{
    [SerializeField, Header("[¿é¤J®Ø]")]
    InputField inputText;

    private void OnEnable()
    {
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
    }

    private void RCToolPlugin_onReceiveDecodeRawData(string address, string data)
    {
        string callback = ParseCallBack.CallbackInfo(address, data);
        if (!string.IsNullOrEmpty(callback))
        {
            AddNewCallback(callback);
        }
    }

    private void AddNewCallback(string input)
    {
        string Key = input.Split('|')[1];
        string Value = input.Split('|')[2];
        if (Key == "33")
        {
            Toast.Instance.ShowToast("[33] Set Finished." + Value);
        }
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
    }

    public void SetFrameNumber()
    {
        byte[] datas = new byte[] { 0x45, 0x41, 0x43, 0x54, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
        if (!string.IsNullOrEmpty(inputText.text))
        {
            datas = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            byte[] input = System.Text.Encoding.ASCII.GetBytes(inputText.text);
            for (int i = 0; i < input.Length; i++)
            {
                datas[i] = input[i];
            }
        }
        //[33]
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "33", datas, null);
    }
}
