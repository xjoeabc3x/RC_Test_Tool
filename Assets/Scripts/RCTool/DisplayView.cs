using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ParseRCCallback;

/// <summary>
/// NewEVO 顯示數據
/// </summary>
public enum EDisplayInfo
{
    Clock = 0,
    Calories = 1,
    Elevation = 2,
    PhoneBattery = 3,
    Estimated_time_of_arrival = 4,
    Estimated_Distance_of_arrival = 5,
    Trip_Time = 6,
    Trip_Distance = 7,
    Speed = 8,
    AVG_Speed = 9,
    Cadence = 10,
    AVG_Cadence = 11,
    Power = 12,
    AVG_Power = 13,
    Battery_level = 14,
    Battery_main = 15,
    Battery_sub = 16,
    Remain_Range = 17,
    ODO = 18,
    Service_interval = 19,
    Assist_mode = 20,
    Heart_rate = 21,
    Max_Speed = 22,
    Max_Power = 23,
    Max_Cadence = 24
}
/// <summary>
/// 語言
/// </summary>
public enum ELanguage
{
    English = 0,
    French = 1,
    German = 2,
    Dutch = 3,
    Spanish = 4,
    Italian = 5,
    Traditional_Chinese = 6,
    Simplified_Chinese = 7,
    Japanese = 8,
    Korean = 9
}
/// <summary>
/// 開機畫面
/// </summary>
public enum EBrandType
{
    Giant = 0,
    Liv = 1,
    Momentum = 2
}

public class DisplayView : MonoBehaviour
{
    [SerializeField, Header("[主數據]")]
    Dropdown mainInfo;
    [SerializeField, Header("[左數據]")]
    Dropdown leftInfo;
    [SerializeField, Header("[右數據]")]
    Dropdown rightInfo;
    [SerializeField, Header("[第幾頁]")]
    int currentPage = 1;
    [SerializeField, Header("[最多幾頁]")]
    int maxPage;
    [SerializeField, Header("[第幾頁文字]")]
    Text currentPage_text;
    [SerializeField, Header("[語言]")]
    Dropdown language;
    [SerializeField, Header("[開機畫面]")]
    Dropdown launchScreen;

    private Dictionary<string, string> callback_dic = new Dictionary<string, string>();
    private EVOSet evoset = null;

    private void OnEnable()
    {
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
        Init();
    }

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
        if (Key == "D6")
        {
            Toast.Instance.ShowToast("[D6] Set Finished." + Value);
        }
        if (Key == "D8")
        {
            Toast.Instance.ShowToast("[D8] Set Finished." + Value);
        }
        if (Key == "D5")
        {
            //
        }
        if (Key == "D7")
        {
            //
        }
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        DisplayReset();
    }

    private void Init()
    {
        evoset = new EVOSet();
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "D5", null, null);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "D7", null, null);
    }

    private void DisplayReset()
    {
        evoset = null;
        currentPage = 1;
        currentPage_text.text = currentPage.ToString();
    }

    #region [Button]

    private void SetPage(int num)
    {
        currentPage += num;
        //overflow
        if (currentPage > maxPage)
        {
            currentPage = 1;
        }
        //minerflow
        else if (currentPage < 1)
        {
            currentPage = maxPage;
        }
        currentPage_text.text = currentPage.ToString();
    }

    public void NextPage()
    {
        SetPage(1);
    }

    public void PreviousPage()
    {
        SetPage(-1);
    }

    #endregion
}

public class EVOSet
{
    //第一頁主數據
    public int Main1 = 0;
    public int Left1 = 0;
    public int Right1 = 0;
    public int Main2 = 0;
    public int Left2 = 0;
    public int Right2 = 0;
    public int Main3 = 0;
    public int Left3 = 0;
    public int Right3 = 0;
    public int Main4 = 0;
    public int Left4 = 0;
    public int Right4 = 0;
    //語言
    public int Language = 0;
    //開機畫面
    public int LaunchScreen = 0;
}
