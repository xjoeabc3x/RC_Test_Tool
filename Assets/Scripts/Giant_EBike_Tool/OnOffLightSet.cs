using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;
using System;
using System.Globalization;

public class OnOffLightSet : MonoBehaviour
{
    [SerializeField, Header("[白天晚上]")]
    Toggle DayNightSwitch;
    [SerializeField, Header("[滑桿]")]
    MySlider LightSlider;
    [SerializeField, Header("[滑桿數值]")]
    Text LightValue;

    private Dictionary<string, string> callback_dic = new Dictionary<string, string>();

    private void Awake()
    {
        LightSlider.onValueChanged.AddListener(OnSliderValueChange);
        DayNightSwitch.onValueChanged.AddListener(OnSwitchValueChange);
    }

    private void OnEnable()
    {
        RCToolPlugin.onReceiveEncodeRawData += RCToolPlugin_onReceiveRawData;
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
        LightSlider.onPointUpEvent += LightSlider_onPointUpEvent;

        GetLight(DayNightSwitch.isOn);
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveEncodeRawData -= RCToolPlugin_onReceiveRawData;
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        LightSlider.onPointUpEvent -= LightSlider_onPointUpEvent;
    }

    //收到的callback(Encode)
    private void RCToolPlugin_onReceiveRawData(string address, string data)
    {

    }
    //收到的callback(Decode)
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
        if (!Value.EndsWith("wait"))
        {
            if (!callback_dic.ContainsKey(Key))
                callback_dic.Add(Key, Value);
            else if (callback_dic.ContainsKey(Key))
                callback_dic[Key] = Value;
        }
    }
    string current_low = "01";
    string current_high = "01";
    private void ParseCallback()
    {
        if (callback_dic.ContainsKey("D9"))
        {
            //EVOLevel, EVOLevel1_value, EVOLevel2_value, EVOLevel3_value, ONOFFLevel, ONOFFHigh_Value, ONOFFLow_Value, Light_Level
            string[] data = callback_dic["D9"].Split(',');
            current_high = int.Parse(data[5]).ToString("X2");
            current_low = int.Parse(data[6]).ToString("X2");
            if (DayNightSwitch.isOn)//night
            {
                LightSlider.value = int.Parse(data[6]);
            }
            else //day
            {
                LightSlider.value = int.Parse(data[5]);
            }
        }
    }

    public void OnSwitchValueChange(bool state)
    {
        GetLight(state);
    }

    public void OnSliderValueChange(float value)
    {
        LightValue.text = ((int)value).ToString();
    }

    private void LightSlider_onPointUpEvent()
    {
        SetLight();
    }

    private void GetLight(bool state)
    {
        //false=day, true=night
        StartCoroutine("_GetLight", state);
    }

    private IEnumerator _GetLight(bool state)
    {
        Loading.Instance.ShowLoading(1f);
        //[D9]
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "D9", null, null);
        yield return new WaitForSeconds(1f);
        ParseCallback();
        SetLight();
    }

    private void SetLight()
    {
        StartCoroutine("_SetLight");
    }

    private IEnumerator _SetLight()
    {
        Loading.Instance.ShowLoading(1f);
        string[] send_byte = new string[8] { "01", "01", "04", "07", "00", current_high, current_low, "00" };
        string lightvalue = ((int)LightSlider.value).ToString("X2");
        if (DayNightSwitch.isOn) //night
        {
            send_byte[7] = "01";
            send_byte[6] = lightvalue;
        }
        else //day
        {
            send_byte[7] = "00";
            send_byte[5] = lightvalue;
        }

        //[DA]
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "DA", null, send_byte);
        yield return new WaitForSeconds(1f);
    }

}
