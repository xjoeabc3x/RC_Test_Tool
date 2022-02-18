using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class RideRecordView : MonoBehaviour
{
    [SerializeField, Header("[取樣間隔]")]
    MySlider RecordInterval;
    [SerializeField, Header("[取樣間隔_Unit]")]
    Text RecordInterval_Unit;
    [SerializeField, Header("[速度]")]
    Slider Speed;
    [SerializeField, Header("[速度_Unit]")]
    Text Speed_Unit;
    [SerializeField, Header("[扭力]")]
    Slider Trq;
    [SerializeField, Header("[扭力_Unit]")]
    Text Trq_Unit;
    [SerializeField, Header("[踏頻]")]
    Slider Cde;
    [SerializeField, Header("[踏頻_Unit]")]
    Text Cde_Unit;
    [SerializeField, Header("[助力電流]")]
    Slider Acur;
    [SerializeField, Header("[助力電流_Unit]")]
    Text Acur_Unit;
    [SerializeField, Header("[旅程距離]")]
    Slider Trid;
    [SerializeField, Header("[旅程距離_Unit]")]
    Text Trid_Unit;
    [SerializeField, Header("[旅程時間]")]
    Slider Trit;
    [SerializeField, Header("[旅程時間_Unit]")]
    Text Trit_Unit;
    [SerializeField, Header("[功率]")]
    Slider Hpw;
    [SerializeField, Header("[功率_Unit]")]
    Text Hpw_Unit;
    [SerializeField, Header("[電量]")]
    Slider Battery;
    [SerializeField, Header("[電量_Unit]")]
    Text Battery_Unit;
    [SerializeField, Header("[錯誤碼]")]
    Text Ecode;
    [SerializeField, Header("[當前助力剩餘距離]")]
    Slider Carr;
    [SerializeField, Header("[當前助力剩餘距離_Unit]")]
    Text Carr_Unit;
    [SerializeField, Header("[助力模式]")]
    Text Mode;
    [SerializeField, Header("[馬達總里程]")]
    Slider ODO;
    [SerializeField, Header("[馬達總里程_Unit]")]
    Text ODO_Unit;

    private void OnEnable()
    {
        HomeManager.RegistEncodeEvent(OnReceiveEncodeParse);
        RecordInterval.onPointUpEvent += RecordInterval_onPointUpEvent;
        RecordInterval.value = AppManager.RECInterval;
        UpdateIntervalText();
    }

    private void RecordInterval_onPointUpEvent()
    {
        AppManager.RECInterval = (int)RecordInterval.value;
        UpdateIntervalText();
    }

    public void UpdateIntervalText()
    {
        RecordInterval_Unit.text = RecordInterval.value + " sec";
    }

    private void OnDisable()
    {
        HomeManager.UnRegistEncodeEvent(OnReceiveEncodeParse);
        RecordInterval.onPointUpEvent -= RecordInterval_onPointUpEvent;
    }

    private void OnReceiveEncodeParse(string callback) //address|key|value
    {
        string address = callback.Split('|')[0];
        string Key = callback.Split('|')[1];
        string Value = callback.Split('|')[2];
        if (address.CompareTo(ChoosedDeviceManager.DeviceAddress) == 0 && Key == "23")
        {
            UpdateUI(Value);
        }
    }

    public void StartRecord()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "23ON", null, null);
        ResetUI();
    }

    public void StopRecord()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "23OFF", null, null);
        HomeManager.SaveRideRecord(ChoosedDeviceManager.DeviceAddress);
    }

    private void ResetUI()
    {
        Speed.value = 0;
        Speed_Unit.text = string.Format("km/hr");
        Trq.value = 0;
        Trq_Unit.text = string.Format("Nm");
        Cde.value = 0;
        Cde_Unit.text = string.Format("rpm");
        Acur.value = 0;
        Acur_Unit.text = string.Format("A");
        Trid.value = 0;
        Trid_Unit.text = string.Format("km");
        Trit.value = 0;
        Trit_Unit.text = string.Format("min");
        Hpw.value = 0;
        Hpw_Unit.text = string.Format("W");
        Battery.value = 0;
        Battery_Unit.text = string.Format("%");
        Ecode.text = "Ecode :";
        Carr.value = 0;
        Carr_Unit.text = string.Format("km");
        Mode.text = "Assist Mode(SG) :";
        ODO.value = 0;
        ODO_Unit.text = string.Format("km");
    }

    private void UpdateUI(string input)
    {
        string[] data = input.Split(',');
        switch (data.Length)
        {
            case 2: //[ecode,carr]
                Ecode.text = "Ecode : 0x" + data[0];
                Carr.value = int.Parse(data[1]);
                Carr_Unit.text = string.Format("{0} km", int.Parse(data[1]));
                break;
            case 4: //[ecode,carr,cur_ast,odo]
                Ecode.text = "Ecode : 0x" + data[0];
                Carr.value = int.Parse(data[1]);
                Carr_Unit.text = string.Format("{0} km", int.Parse(data[1]));
                Mode.text = "Assist Mode(SG) : " + data[2];
                ODO.value = int.Parse(data[3]);
                ODO_Unit.text = string.Format("{0} km", int.Parse(data[3]));
                break;
            case 8: //[spd,trq,cde,acur,trid,trit,hpw,rsoc]
                Speed.value = float.Parse(data[0]);
                Speed_Unit.text = string.Format("{0:0.0} km/hr", float.Parse(data[0]));
                Trq.value = float.Parse(data[1]);
                Trq_Unit.text = string.Format("{0:0.0} Nm", float.Parse(data[1]));
                Cde.value = float.Parse(data[2]);
                Cde_Unit.text = string.Format("{0:0.0} rpm", float.Parse(data[2]));
                Acur.value = float.Parse(data[3]);
                Acur_Unit.text = string.Format("{0:0.00} A", float.Parse(data[3]));
                Trid.value = float.Parse(data[4]);
                Trid_Unit.text = string.Format("{0:0.0} km", float.Parse(data[4]));
                Trit.value = int.Parse(data[5]);
                Trit_Unit.text = string.Format("{0} min", int.Parse(data[5]));
                Hpw.value = float.Parse(data[6]);
                Hpw_Unit.text = string.Format("{0:0.0} W", float.Parse(data[6]));
                Battery.value = int.Parse(data[7]);
                Battery_Unit.text = string.Format("{0} %", int.Parse(data[7]));
                break;
        }
    }
}
