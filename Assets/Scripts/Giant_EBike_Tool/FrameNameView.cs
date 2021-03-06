using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class FrameNameView : MonoBehaviour
{
    [SerializeField, Header("[???J??]")]
    InputField inputText;

    private void OnEnable()
    {
        HomeManager.RegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
    }

    private void ParseCallBack_onReceiveDecodeParsedData(string callback)
    {
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
        HomeManager.UnRegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
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
