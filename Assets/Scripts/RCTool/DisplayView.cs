using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Globalization;
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
        mainInfo.onValueChanged.AddListener(MainInfoSet);
        leftInfo.onValueChanged.AddListener(LeftInfoSet);
        rightInfo.onValueChanged.AddListener(RightInfoSet);
        language.onValueChanged.AddListener(LanguageSet);
        launchScreen.onValueChanged.AddListener(LaunchScreenSet);
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
            ParseD5_sync(Value);
        }
        if (Key == "D7")
        {
            ParseD7_sync(Value);
        }
    }

    private void ParseD5_sync(string str)
    {
        //page1_main,page1_left,page1_right,page2_main,page2_left,page2_right,page3_main,page3_left,page3_right,page4_main,page4_left,page4_right
        string[] data = str.Split(',');
        evoset.Main1 = GetD5_type(data[0]);
        evoset.Left1 = GetD5_type(data[1]);
        evoset.Right1 = GetD5_type(data[2]);

        evoset.Main2 = GetD5_type(data[3]);
        evoset.Left2 = GetD5_type(data[4]);
        evoset.Right2 = GetD5_type(data[5]);

        evoset.Main3 = GetD5_type(data[6]);
        evoset.Left3 = GetD5_type(data[7]);
        evoset.Right3 = GetD5_type(data[8]);

        evoset.Main4 = GetD5_type(data[9]);
        evoset.Left4 = GetD5_type(data[10]);
        evoset.Right4 = GetD5_type(data[11]);
        D5_Sync();
    }

    private void D5_Sync()
    {
        if (evoset != null)
        {
            SetPage();
        }
    }

    private int GetD5_type(string str)
    {
        switch (str)
        {
            case "01":
                return 0;
            case "02":
                return 1;
            case "03":
                return 2;
            case "04":
                return 3;
            case "05":
                return 4;
            case "06":
                return 5;
            case "10":
                return 6;
            case "11":
                return 7;
            case "12":
                return 8;
            case "13":
                return 9;
            case "14":
                return 10;
            case "15":
                return 11;
            case "16":
                return 12;
            case "17":
                return 13;
            case "18":
                return 14;
            case "19":
                return 15;
            case "1A":
                return 16;
            case "1B":
                return 17;
            case "1C":
                return 18;
            case "1D":
                return 19;
            case "1E":
                return 20;
            case "1F":
                return 21;
            case "20":
                return 22;
            case "21":
                return 23;
            case "22":
                return 24;
            default:
                return 0;
        }
    }

    private byte GetD5_type(int num)
    {
        switch (num)
        {
            case 0:
                return 0x01;
            case 1:
                return 0x02;
            case 2:
                return 0x03;
            case 3:
                return 0x04;
            case 4:
                return 0x05;
            case 5:
                return 0x06;
            case 6:
                return 0x10;
            case 7:
                return 0x11;
            case 8:
                return 0x12;
            case 9:
                return 0x13;
            case 10:
                return 0x14;
            case 11:
                return 0x15;
            case 12:
                return 0x16;
            case 13:
                return 0x17;
            case 14:
                return 0x18;
            case 15:
                return 0x19;
            case 16:
                return 0x1A;
            case 17:
                return 0x1B;
            case 18:
                return 0x1C;
            case 19:
                return 0x1D;
            case 20:
                return 0x1E;
            case 21:
                return 0x1F;
            case 22:
                return 0x20;
            case 23:
                return 0x21;
            case 24:
                return 0x22;
            default:
                return 0x00;
        }
    }

    private void ParseD7_sync(string str)
    {
        //language("00" English),brand_type("00" Giant)
        string[] data = str.Split(',');
        evoset.Language = int.Parse(data[0], NumberStyles.HexNumber);
        evoset.LaunchScreen = int.Parse(data[1], NumberStyles.HexNumber);
        //Sync
        language.value = evoset.Language;
        launchScreen.value = evoset.LaunchScreen;
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        mainInfo.onValueChanged.RemoveListener(MainInfoSet);
        leftInfo.onValueChanged.RemoveListener(LeftInfoSet);
        rightInfo.onValueChanged.RemoveListener(RightInfoSet);
        language.onValueChanged.RemoveListener(LanguageSet);
        launchScreen.onValueChanged.RemoveListener(LaunchScreenSet);
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

    private void SetCurrent(int num)
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
        SetPage();
    }

    private void SetPage()
    {
        switch (currentPage)
        {
            case 1:
                mainInfo.value = evoset.Main1;
                leftInfo.value = evoset.Left1;
                rightInfo.value = evoset.Right1;
                break;
            case 2:
                mainInfo.value = evoset.Main2;
                leftInfo.value = evoset.Left2;
                rightInfo.value = evoset.Right2;
                break;
            case 3:
                mainInfo.value = evoset.Main3;
                leftInfo.value = evoset.Left3;
                rightInfo.value = evoset.Right3;
                break;
            case 4:
                mainInfo.value = evoset.Main4;
                leftInfo.value = evoset.Left4;
                rightInfo.value = evoset.Right4;
                break;
            default:
                mainInfo.value = 0;
                leftInfo.value = 0;
                rightInfo.value = 0;
                break;
        }
    }

    public void NextPage()
    {
        SetCurrent(1);
    }

    public void PreviousPage()
    {
        SetCurrent(-1);
    }

    public void SetDefault()
    {
        evoset.Main1 = 8;
        evoset.Left1 = 17;
        evoset.Right1 = 18;
        evoset.Main2 = 8;
        evoset.Left2 = 7;
        evoset.Right2 = 6;
        evoset.Main3 = 8;
        evoset.Left3 = 9;
        evoset.Right3 = 22;
        evoset.Main4 = 8;
        evoset.Left4 = 17;
        evoset.Right4 = 10;
        SetPage();
    }

    public void SetEVO()
    {
        byte[] senddata = new byte[12] { GetD5_type(evoset.Main1), GetD5_type(evoset.Left1), GetD5_type(evoset.Right1),
        GetD5_type(evoset.Main2), GetD5_type(evoset.Left2), GetD5_type(evoset.Right2),
        GetD5_type(evoset.Main3), GetD5_type(evoset.Left3), GetD5_type(evoset.Right3),
        GetD5_type(evoset.Main4), GetD5_type(evoset.Left4), GetD5_type(evoset.Right4)};
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "D6", senddata, null);
        byte[] senddata02 = new byte[2] { (byte)evoset.Language, (byte)evoset.LaunchScreen };
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "D8", senddata02, null);
        //CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "D5", null, null);
        //CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "D7", null, null);
    }
    
    #endregion

    #region [DropDownEvent]

    private void MainInfoSet(int id)
    {
        switch (currentPage)
        {
            case 1:
                evoset.Main1 = id;
                break;
            case 2:
                evoset.Main2 = id;
                break;
            case 3:
                evoset.Main3 = id;
                break;
            case 4:
                evoset.Main4 = id;
                break;
        }
    }

    private void LeftInfoSet(int id)
    {
        switch (currentPage)
        {
            case 1:
                evoset.Main1 = id;
                break;
            case 2:
                evoset.Main2 = id;
                break;
            case 3:
                evoset.Main3 = id;
                break;
            case 4:
                evoset.Main4 = id;
                break;
        }
    }

    private void RightInfoSet(int id)
    {
        switch (currentPage)
        {
            case 1:
                evoset.Main1 = id;
                break;
            case 2:
                evoset.Main2 = id;
                break;
            case 3:
                evoset.Main3 = id;
                break;
            case 4:
                evoset.Main4 = id;
                break;
        }
    }

    private void LanguageSet(int id)
    {
        evoset.Language = id;
    }

    private void LaunchScreenSet(int id)
    {
        evoset.LaunchScreen = id;
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
