using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;
using System;
using System.Globalization;
using Setting;

public class RingView : MonoBehaviour
{
    [SerializeField, Header("[按鈕位置]")]
    private List<Transform> ButtonPos = new List<Transform>();
    [SerializeField, Header("[按鈕本人]")]
    private List<Transform> Buttons = new List<Transform>();
    [SerializeField, Header("[按鈕啟用顏色]")]
    private Color EnableColor;
    [SerializeField, Header("[按鈕停用顏色]")]
    private Color DisableColor;

    private Dictionary<string, string> callback_dic = new Dictionary<string, string>();

    private void OnEnable()
    {
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
        GetRing();
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        ResetRingView();
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

    private void GetRing()
    {
        StartCoroutine("_GetRing");
    }

    private IEnumerator _GetRing()
    {
        Loading.Instance.ShowLoading(1f);
        //[DD]
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "DD", null, null);
        yield return new WaitForSeconds(1f);
        ParseCallback();
        ChoosedDeviceManager.Instance.ParseBikeDetail();
        ChoosedDeviceManager.Instance.ShowBikeDetail();
        InitRingState();
    }

    private void ParseCallback()
    {
        if (callback_dic.ContainsKey("DD"))
        {
            //left, right, left1, left2, left3, right1, right2, right3
            string[] data = callback_dic["DD"].Split(',');
        }
    }

    private void InitRingState()
    {
        //馬達限制
        ChoosedDeviceManager.BikeDetail bikeInfo = ChoosedDeviceManager.GetBikeDetail(ChoosedDeviceManager.DeviceAddress);
        if (bikeInfo == null)
            return;
        switch (bikeInfo.DUType)
        {
            case "DU5 JPN-BIC PW-SE":
                Buttons[4].gameObject.SetActive(false);
                Buttons[5].gameObject.SetActive(false);
                break;
            case "DU8 JPN-PCB PW-X2":
                Buttons[4].gameObject.SetActive(false);
                Buttons[5].gameObject.SetActive(false);
                break;
            case "DU12 JPN-SCB":
                Buttons[4].gameObject.SetActive(false);
                Buttons[5].gameObject.SetActive(false);
                break;
            default:
                break;
        }
        //左右Ring存在狀況

        //Get[DD]後按鈕丟到相對應位置上
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

    public void ResetRingView()
    {
        //回復按鈕位置,parent,setActive
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].gameObject.SetActive(true);
            Position pos = Buttons[i].GetComponent<Position>();
            if (pos != null)
            {
                pos.Reset();
                pos.ResetOrgParent();
            }
        }
        //左右Ring圖片,collider
        SetButtonPosState(-1, true);
    }

    private void SetButtonPosState(int number, bool state)
    {
        if (number == -1)
        {
            for (int i = 0; i < ButtonPos.Count; i++)
            {
                ButtonPos[i].GetComponent<Image>().color = EnableColor;
                ButtonPos[i].GetComponent<BoxCollider>().enabled = true;
            }
            return;
        }
        ButtonPos[number].GetComponent<BoxCollider>().enabled = state;
        ButtonPos[number].GetComponent<Image>().color = (state) ? EnableColor : DisableColor;
    }

}
