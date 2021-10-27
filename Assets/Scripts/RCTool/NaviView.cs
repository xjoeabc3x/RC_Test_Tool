using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class NaviView : MonoBehaviour
{
    [SerializeField, Header("[切換switch]")]
    Toggle switcher;
    [SerializeField, Header("[1D_Buttons]")]
    GameObject buttons_1D;
    [SerializeField, Header("[E0_Buttons]")]
    GameObject buttons_E0;
    [SerializeField, Header("[下個點距離]")]
    InputField NextAct_input;
    [SerializeField, Header("[目的地距離]")]
    InputField GoalDis_input;
    [SerializeField, Header("[目的地時間]")]
    InputField GoalTime_input;

    private void OnEnable()
    {
        RCToolPlugin.onDeviceStatusChanged += RCToolPlugin_onDeviceStatusChanged;
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1A_navigation", null, null);
        //1A navi
        //buttons
        SetButtons();
    }

    private void RCToolPlugin_onDeviceStatusChanged(string address, string status)
    {
        switch (status)
        {
            case "Connected":
                CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1A_navigation", null, null);
                break;
            default:
                Debug.Log(address + " status :" + status);
                break;
        }
    }

    private void OnDisable()
    {
        RCToolPlugin.onDeviceStatusChanged -= RCToolPlugin_onDeviceStatusChanged;
        //1A normal
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1A_normal", null, null);
    }

    public void SetButtons()
    {
        buttons_1D.SetActive(!switcher.isOn);
        buttons_E0.SetActive(switcher.isOn);
    }

    public void Set_1D_0(string str)
    {
        Toast.Instance.ShowToast("施工中");
    }

    public void Set_1D_1(string str)
    {
        Toast.Instance.ShowToast("施工中");
    }

    public void Set_E0(string str)
    {
        Toast.Instance.ShowToast("施工中");
    }
}
