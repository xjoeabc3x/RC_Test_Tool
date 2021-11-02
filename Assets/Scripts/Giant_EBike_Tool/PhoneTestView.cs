using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneTestView : MonoBehaviour
{
    [SerializeField, Header("[主數據顯示]")]
    Dropdown MainInfo;

    public void SendNotification(string type)
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device not connected.");
            return;
        }
        byte[] sendbyte = new byte[2] { 0x01, 0x01 };
        switch (type)
        {
            case "phone":
                sendbyte[0] = 0x04;
                break;
            case "sms":
                sendbyte[0] = 0x02;
                break;
            case "mail":
                sendbyte[0] = 0x01;
                break;
            default:
                Debug.LogError("Something wrong.");
                return;
        }
        switch (MainInfo.value)
        {
            case 0:
                sendbyte[1] = 0x08;
                break;
            case 1:
                sendbyte[1] = 0x04;
                break;
            case 2:
                sendbyte[1] = 0x02;
                break;
            case 3:
                sendbyte[1] = 0x01;
                break;
        }
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1E", sendbyte, null);
    }
}
