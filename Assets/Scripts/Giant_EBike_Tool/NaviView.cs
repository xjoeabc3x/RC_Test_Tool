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
        if (!InputValueOK())
            return;
        string[] dis = GetNextDis();
        string[] desd = GetGoalDis();
        string[] dest = GetGoalTime();
        string[] senddata = new string[] { str, "00", dis[0], dis[1], desd[0], desd[1], dest[0], dest[1] };
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1D", null, senddata);
    }

    public void Set_1D_1(string str)
    {
        if (!InputValueOK())
            return;
        string[] dis = GetNextDis();
        string[] desd = GetGoalDis();
        string[] dest = GetGoalTime();
        string[] senddata = new string[] { "00", str, dis[0], dis[1], desd[0], desd[1], dest[0], dest[1] };
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1D", null, senddata);
    }

    public void Set_E0(string str)
    {
        if (!InputValueOK())
            return;
        string[] dis = GetNextDis();
        string[] desd = GetGoalDis();
        string[] dest = GetGoalTime();
        string[] senddata = new string[] { str, str, dis[0], dis[1], desd[0], desd[1], dest[0], dest[1] };
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "E0", null, senddata);
    }

    private bool InputValueOK()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return false;
        }
        if (string.IsNullOrEmpty(NextAct_input.text) || int.Parse(NextAct_input.text) > 65535 || int.Parse(NextAct_input.text) < 0)
        {
            Toast.Instance.ShowToast("Distance from next point input Error.");
            return false;
        }
        if (string.IsNullOrEmpty(GoalDis_input.text) || float.Parse(GoalDis_input.text) > 6553.5f || float.Parse(GoalDis_input.text) < 0)
        {
            Toast.Instance.ShowToast("Distance from destination input Error.");
            return false;
        }
        if (string.IsNullOrEmpty(GoalTime_input.text) || int.Parse(GoalTime_input.text) > 65535 || int.Parse(GoalTime_input.text) < 0)
        {
             Toast.Instance.ShowToast("Time to destination input Error.");
            return false;
        }
        return true;
    }

    private string[] GetNextDis()
    {
        string dis = int.Parse(NextAct_input.text).ToString("X4"); //000B
        string dis_h = dis[0].ToString() + dis[1].ToString();
        string dis_l = dis[2].ToString() + dis[3].ToString();
        return new string[2] { dis_l, dis_h };
    }

    private string[] GetGoalDis()
    {
        string desd = ((int)(float.Parse(GoalDis_input.text) * 10)).ToString("X4"); //000B
        string desd_h = desd[0].ToString() + desd[1].ToString();
        string desd_l = desd[2].ToString() + desd[3].ToString();
        return new string[2] { desd_l, desd_h }; ;
    }

    private string[] GetGoalTime()
    {
        string dest = int.Parse(GoalTime_input.text).ToString("X4"); //000B
        string dest_h = dest[0].ToString() + dest[1].ToString();
        string dest_l = dest[2].ToString() + dest[3].ToString();
        return new string[2] { dest_l, dest_h };
    }
}
