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
        if (bikeInfo == null || Buttons.Count <= 0 || ButtonPos.Count <= 0 || !callback_dic.ContainsKey("DD"))
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
                Buttons[4].gameObject.SetActive(true);
                Buttons[5].gameObject.SetActive(true);
                break;
        }
        //左右Ring存在狀況  left,right,left1,left2,left3,right1,right2,right3
        string[] data = callback_dic["DD"].Split(',');
        switch (data[0]) //left
        {
            case "0":
                SetButtonPosState(0, false);
                SetButtonPosState(1, false);
                SetButtonPosState(2, false);
                break;
            case "1":
                SetButtonPosState(0, true);
                SetButtonPosState(1, true);
                SetButtonPosState(2, true);
                break;
            default:
                SetButtonPosState(0, false);
                SetButtonPosState(1, false);
                SetButtonPosState(2, false);
                break;
        }
        switch (data[1]) //right
        {
            case "0":
                SetButtonPosState(3, false);
                SetButtonPosState(4, false);
                SetButtonPosState(5, false);
                break;
            case "1":
                SetButtonPosState(3, true);
                SetButtonPosState(4, true);
                SetButtonPosState(5, true);
                break;
            default:
                SetButtonPosState(3, false);
                SetButtonPosState(4, false);
                SetButtonPosState(5, false);
                break;
        }
        //Get[DD]後按鈕丟到相對應位置上
        for (int i = 2; i < data.Length; i++)
        {
            switch (data[i])
            {
                case "Up":
                    Buttons[0].SetParent(ButtonPos[i - 2]);
                    Buttons[0].GetComponent<RectTransform>().localPosition = Vector3.zero;
                    break;
                case "Down":
                    Buttons[1].SetParent(ButtonPos[i - 2]);
                    Buttons[1].GetComponent<RectTransform>().localPosition = Vector3.zero;
                    break;
                case "Info":
                    Buttons[2].SetParent(ButtonPos[i - 2]);
                    Buttons[2].GetComponent<RectTransform>().localPosition = Vector3.zero;
                    break;
                case "Light":
                    Buttons[3].SetParent(ButtonPos[i - 2]);
                    Buttons[3].GetComponent<RectTransform>().localPosition = Vector3.zero;
                    break;
                case "Walk assist":
                    Buttons[4].SetParent(ButtonPos[i - 2]);
                    Buttons[4].GetComponent<RectTransform>().localPosition = Vector3.zero;
                    break;
                case "Smart assist":
                    Buttons[5].SetParent(ButtonPos[i - 2]);
                    Buttons[5].GetComponent<RectTransform>().localPosition = Vector3.zero;
                    break;
                default:
                    break;
            }
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
        if (Key == "DE")
        {
            Toast.Instance.ShowToast("[DE] Set Finished.");
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
                //ButtonPos[i].GetComponent<ColliderElement>().enabled = true;
            }
            return;
        }
        //ButtonPos[number].GetComponent<ColliderElement>().enabled = state;
        ButtonPos[number].GetComponent<Image>().color = (state) ? EnableColor : DisableColor;
    }

    public void CheckButtonPosState()
    {
        for (int i = 0; i < ButtonPos.Count; i++)
        {
            if (ButtonPos[i].childCount >= 2)
            {
                Position pos = ButtonPos[i].GetChild(0).GetComponent<Position>();
                pos.Reset();
                pos.ResetOrgParent();
            }
        }
    }

    public void SetRing()
    {
        for (int i = 0; i < ButtonPos.Count; i++)
        {
            if (ButtonPos[i].childCount <= 0)
            {
                Toast.Instance.ShowToast("Empty not allowed.");
                return;
            }
        }
        StartCoroutine("_SetRing");
    }

    private IEnumerator _SetRing()
    {
        Loading.Instance.ShowLoading(1f);
        List<string> setData = new List<string>();
        //left only:0x02
        //right only:0x01
        //both:0x04
        if (callback_dic.ContainsKey("DD"))
        {
            Debug.Log("_SetRing :" + callback_dic["DD"]);
            string[] data = callback_dic["DD"].Split(',');
            if (data[0] != "0" || data[1] != "0")
            {
                if (data[0] == "1" && data[1] == "0")
                {
                    setData.Add("02");
                }
                else if (data[0] == "0" && data[1] == "1")
                {
                    setData.Add("01");
                }
                else if (data[0] == "1" && data[1] == "1")
                {
                    setData.Add("03");
                }
                //check each buttonpos
                //if no child, set 0x00
                for (int i = 0; i < ButtonPos.Count; i++)
                {
                    if (ButtonPos[i].childCount > 0)
                    {
                        ItemKey key = ButtonPos[i].GetChild(0).GetComponent<ItemKey>();
                        if (key != null)
                        {
                            switch (key.Key)
                            {
                                case "Up":
                                    setData.Add("01");
                                    break;
                                case "Down":
                                    setData.Add("02");
                                    break;
                                case "Info":
                                    setData.Add("03");
                                    break;
                                case "Light":
                                    setData.Add("04");
                                    break;
                                case "WalkAssist":
                                    setData.Add("05");
                                    break;
                                case "SmartAssist":
                                    setData.Add("06");
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        setData.Add("00");
                    }
                }
                //[DE]
                CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "DE", null, setData.ToArray());
                yield return new WaitForSeconds(1f);
            }
        }
    }

}
