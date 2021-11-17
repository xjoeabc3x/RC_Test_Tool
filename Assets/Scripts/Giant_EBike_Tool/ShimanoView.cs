using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class ShimanoView : MonoBehaviour
{
    [SerializeField, Header("[Device選擇]")]
    Dropdown DeviceDropDown;
    [SerializeField, Header("[Index輸入]")]
    InputField IndexInput;
    [SerializeField, Header("[SG回應]")]
    Text Callback_Text;

    private void OnEnable()
    {
        HomeManager.RegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
    }

    private void ParseCallBack_onReceiveDecodeParsedData(string callback)
    {
        if (!string.IsNullOrEmpty(callback))
        {
            //Device,Moduel,Major,Minor,SubMinor
            string Key = callback.Split('|')[1];
            string Value = callback.Split('|')[2];
            string[] datas = Value.Split(',');
            Callback_Text.text = string.Format("<color=yellow>Callback : </color>\nDevice :{0}\n", datas[0]);
            Callback_Text.text += string.Format("Moduel :{0}\nMajor :{1}\nMinor :{2}\nSub-Minor :{3}", datas[1], datas[2], datas[3], datas[4]);
        }
    }

    private void OnDisable()
    {
        HomeManager.UnRegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
    }

    public void Send_DF()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        if (!CheckIndexInput())
            return;

        byte[] sendbyte = new byte[2];
        switch (DeviceDropDown.value)
        {
            case 0:
                sendbyte[0] = 0x01;
                break;
            case 1:
                sendbyte[0] = 0x02;
                break;
            case 2:
                sendbyte[0] = 0x03;
                break;
            default:
                sendbyte[0] = 0x01;
                break;
        }
        sendbyte[1] = (byte)(int.Parse(IndexInput.text));
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "DF", sendbyte, null);
    }

    private bool CheckIndexInput()
    {
        if (string.IsNullOrEmpty(IndexInput.text))
        {
            Toast.Instance.ShowToast("Index input is empty.");
            return false;
        }
        if (!int.TryParse(IndexInput.text, out int result))
        {
            Toast.Instance.ShowToast("Index input error.");
            return false;
        }
        if (result > 255 || result < 0)
        {
            Toast.Instance.ShowToast("Index input out of range.");
            return false;
        }
        return true;
    }
}
