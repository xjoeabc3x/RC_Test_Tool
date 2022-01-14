using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class TuningView : MonoBehaviour
{
    public static TuningView Instance;
    [SerializeField, Header("[五個段位]")]
    List<TuningType> tuningType = new List<TuningType>();

    private Dictionary<string, string> callback_dic = new Dictionary<string, string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        HomeManager.RegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1A_tuning", null, null);
        Init();
    }

    private void ParseCallBack_onReceiveDecodeParsedData(string callback)
    {
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
        if (Key == "2D")
        {
            Toast.Instance.ShowToast("[2D] Set Finished." + Value);
        }
        if (Key == "2C")
        {
            SetCurrentState();
        }
    }

    private void OnDisable()
    {
        HomeManager.UnRegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1A_normal", null, null);
        ResetView();
    }

    private void Init()
    {
        //判斷不可Tuning DU
        ChoosedDeviceManager.BikeDetail bikeDetail = ChoosedDeviceManager.GetBikeDetail(ChoosedDeviceManager.DeviceAddress);
        if (bikeDetail == null)
        {
            Toast.Instance.ShowToast("Need to Get Bike Detail first.");
            ChoosedDeviceManager.Instance.SetBackButtonState(true);
            ChoosedDeviceManager.Instance.CloseChildPages();
            return;
        }
        string DUType = bikeDetail.DUType;
        string RCType = bikeDetail.RCType;
        if (string.IsNullOrEmpty(DUType) || DUType == "null" || string.IsNullOrEmpty(RCType) || RCType == "null")
        {
            Toast.Instance.ShowToast("Need to Get Bike Detail first.");
            ChoosedDeviceManager.Instance.SetBackButtonState(true);
            ChoosedDeviceManager.Instance.CloseChildPages();
            return;
        }
        if (NotTuningDU(DUType))
        {
            Toast.Instance.ShowToast("Not supported DU.");
            ChoosedDeviceManager.Instance.SetBackButtonState(true);
            ChoosedDeviceManager.Instance.CloseChildPages();
            return;
        }

        if (NotTuningRC(RCType))
        {
            Toast.Instance.ShowToast("Not supported RC.");
            ChoosedDeviceManager.Instance.SetBackButtonState(true);
            ChoosedDeviceManager.Instance.CloseChildPages();
            return;
        }
        //微調按鈕文字
        SetTuningButtonText(DUType);
        //讀取[2C]
        //同步狀態
        StartCoroutine("Read2C_sync");
    }

    IEnumerator Read2C_sync()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "2C", null, null);
        Loading.Instance.ShowLoading(0.6f);
    }
    //解析[2C]狀態然後同步到UI
    private void SetCurrentState()
    {
        if (callback_dic.ContainsKey("2C"))
        {
            //power,sport,active,tour,eco
            string[] data = callback_dic["2C"].Split(',');
            //power
            for (int i = 0; i < tuningType.Count; i++)
            {
                switch (data[i])
                {
                    case "1": //大
                        tuningType[i].ButtonChoosed(2);
                        break;
                    case "2": //中
                        tuningType[i].ButtonChoosed(1);
                        break;
                    case "3": //小
                        tuningType[i].ButtonChoosed(0);
                        break;
                }
            }
        }
    }

    public void SetTuning(bool isDefault)
    {
        StartCoroutine("_SetTuning", isDefault);
    }

    IEnumerator _SetTuning(bool isDefault)
    {
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "1A_tuning", null, null);
        Loading.Instance.ShowLoading(1.8f);
        yield return new WaitForSecondsRealtime(0.6f);
        if (isDefault)
        {
            CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "2D", null, null);
        }
        else
        {
            List<string> datas = new List<string>();
            for (int i = 0; i < tuningType.Count; i++)
            {
                switch (tuningType[i].GetCurrentChoose())
                {
                    case 0: //小
                        datas.Add("3");
                        break;
                    case 1: //中
                        datas.Add("2");
                        break;
                    case 2: //大
                        datas.Add("1");
                        break;
                    default:
                        datas.Add("0");
                        break;
                }
            }
            string[] setdata = new string[3] { datas[1] + datas[0], datas[3] + datas[2], "0" + datas[4] };
            CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "2D", null, setdata);
        }
        yield return new WaitForSecondsRealtime(0.6f);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "2C", null, null);
    }

    /// <summary>
    /// 不可Tuning的DU
    /// </summary>
    /// <returns></returns>
    public static bool NotTuningDU(string DUType)
    {
        switch (DUType)
        {
            case "DU1 CAN-BIC PW-SE":
                return true;
            case "DU5 JPN-BIC PW-SE":
                return true;
            case "DU8 JPN-PCB PW-X2":
                return true;
            case "DU9":
                return true;
            case "DU12 JPN-SCB":
                return true;
            case "DU13":
                return true;
            case "DU14":
                return true;
            case "DU18 G370":
                return true;
            case "DU19":
                return true;
            default:
                return false;
        }
    }

    public static bool NotTuningRC(string RCType)
    {
        switch (RCType)
        {
            case "RideControl EVO JPN":
                return true;
            case "RideControl ONE JPN":
                return true;
            default:
                return false;
        }
    }

    private void SetTuningButtonText(string DUType)
    {
        string[] str = GetTuningButtonText(DUType);
        for (int i = 0; i < 5; i++)
        {
            tuningType[i].SetTuningButtonText(str[3 * i], str[(3 * i) + 1], str[(3 * i) + 2]);
        }
    }

    private string[] GetTuningButtonText(string DUType)
    {
        switch (DUType)
        {
            case "DU2 PCB-BIC PW-X":
                return new string[15] { "300", "350", "360", 
                    "200", "250", "300", 
                    "175", "200", "250", 
                    "125", "150", "175", 
                    "50", "75", "100" };
            case "DU3 CAN-BIC2 PW-SE":
                return new string[15] { "250", "300", "350", 
                    "175", "200", "250", "125", "150", "175", "75", "100", "125", "50", "75", "-" };
            case "DU4 Comfort-BIC":
                return new string[15] { "250", "300", "-", 
                    "150", "175", "200", 
                    "100", "125", "150", 
                    "75", "100", "-", 
                    "50", "75", "-" };
            case "DU4 Comfort-BIC PW-TE":
                return new string[15] { "250", "300", "-", 
                    "150", "175", "200", 
                    "100", "125", "150", 
                    "75", "100", "-", 
                    "50", "75", "-" };
            case "DU6 Comfort-BIC2 PW ST":
                return new string[15] { "250", "300", "350", 
                    "175", "200", "250", 
                    "125", "150", "175", 
                    "75", "100", "125", 
                    "50", "75", "-" };
            case "DU7 PCB-BIC2 PW-X2":
                return new string[15] { "300", "350", "360", 
                    "200", "250", "300", "175", "200", "250", "125", "150", "175", "50", "75", "100" };
            case "DU10 ICB-BIC standard":
                return new string[15] { "250", "300", "-", 
                    "150", "175", "200", 
                    "100", "125", "150", 
                    "75", "100", "-", 
                    "50", "75", "-" };
            case "DU11 ICB-BIC coaster":
                return new string[15] { "250", "300", "-", 
                    "150", "175", "200", 
                    "100", "125", "150", 
                    "75", "100", "-", 
                    "50", "75", "-" };
            case "DU15 PCB BIC3":
                return new string[15] { "300", "350", "400", 
                    "200", "250", "300", 
                    "150", "175", "200", 
                    "100", "125", "150", 
                    "50", "75", "100" };
            case "DU16 EP8":
                return new string[15] { "9", "10", "11", 
                    "7", "8", "9", 
                    "5", "6", "7", 
                    "3", "4", "5", 
                    "1", "2", "3" };
            case "DU20":
                return new string[15] { "250", "300", "350",
                    "175", "200", "250",
                    "125", "150", "175",
                    "75", "100", "125",
                    "50", "75", "-" };
            default:
                return new string[15] { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-" };
        }
    }

    private void ResetView()
    {
        for (int i = 0; i < tuningType.Count; i++)
        {
            tuningType[i].ResetButtons();
        }
    }
}
