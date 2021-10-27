using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class NaviView : MonoBehaviour
{
    [SerializeField, Header("[����switch]")]
    Toggle switcher;
    [SerializeField, Header("[1D_Buttons]")]
    GameObject buttons_1D;
    [SerializeField, Header("[E0_Buttons]")]
    GameObject buttons_E0;
    [SerializeField, Header("[�U���I�Z��]")]
    InputField NextAct_input;
    [SerializeField, Header("[�ت��a�Z��]")]
    InputField GoalDis_input;
    [SerializeField, Header("[�ت��a�ɶ�]")]
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
        Toast.Instance.ShowToast("�I�u��");
    }

    public void Set_1D_1(string str)
    {
        Toast.Instance.ShowToast("�I�u��");
    }

    public void Set_E0(string str)
    {
        Toast.Instance.ShowToast("�I�u��");
    }
}
