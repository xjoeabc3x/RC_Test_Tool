using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosedDeviceManager : MonoBehaviour
{
    public class BikeDetail
    {
        //RC/SG type
        public string RCType = "null";
        //RC/SG UI fw
        public string RCFW = "null";
        //RC/SG UI hw
        public string RCHW = "null";
        //ctg1
        public string ctg1 = "null";
        //ctg2
        public string ctg2 = "null";
        //DU type
        public string DUType = "null";
        //DU fw name
        public string DUFW_MD = "null";
        //DU hw name
        public string DUHW_MD = "null";
        //DU fw version
        public string DUFWver = "null";
        //DU rcid
        public string RCID = "null";
        //DU SN
        public string SN = "null";
        //BBSS
        public string BBSS = "null";

        //UIBle_Ver
        public string UIBle_Ver = "null";

        //Bike name
        public string frameNumber = "null";
        //DU odo
        public string odo = "null";
        //DU tut
        public string tut = "null";
        //距上次回廠時間
        public string lstc = "null";
        //距上次回廠距離
        public string fstc = "null";
        //DU,BATT,SBATT,RMO_1,RMO_2,DSP,S_FD,S_RD,S_SWitchShifter
        public string exist_DU = "null";
        public string exist_BATT = "null";
        public string exist_SBATT = "null";
        public string exist_RMO1 = "null";
        public string exist_RMO2 = "null";
        public string exist_DSP = "null";
        public string exist_SFD = "null";
        public string exist_SRD = "null";
        public string exist_SSWS = "null";
        //remote1 type,fw,hw
        public string rmo1Type = "null";
        public string rmo1FW = "null";
        public string rmo1HW = "null";
        //remote2 type,fw,hw
        public string rmo2Type = "null";
        public string rmo2FW = "null";
        public string rmo2HW = "null";
        //display type,fw,hw
        public string dspType = "null";
        public string dspFW = "null";
        public string dspHW = "null";
        //主電池Cell版本,fw,sn
        public string minor = "null";
        public string major = "null";
        public string BATTSN = "null";
        //主(+副)電池容量,主電池壽命,前次充飽容量 rsoc(%),eplife(%),fcc(Wh)
        public string rsoc = "null";
        public string eplife = "null";
        public string fcc = "null";
        //主電池充電循環次數,充電次數,大電流放電比例100% ccy,cchg,hrd
        public string ccy = "null";
        public string cchg = "null";
        public string hrd = "null";
        //副電池容量,副電池壽命,前次充飽容量 rsoc(%),eplife(%),fcc(Wh)
        public string sub_rsoc = "null";
        public string sub_eplife = "null";
        public string sub_fcc = "null";
        //副電池Cell版本,fw,sn minor,major,SN
        public string sub_minor = "null";
        public string sub_major = "null";
        public string sub_BATTSN = "null";
        //副電池充電循環次數,充電次數,大電流放電比例100% ccy,cchg,hrd
        public string sub_ccy = "null";
        public string sub_cchg = "null";
        public string sub_hrd = "null";
        //Ring是否存在以及按鈕狀態 left,right,left1,left2,left3,right1,right2,right3
        public string left = "null";
        public string right = "null";
        public string left1 = "null";
        public string left2 = "null";
        public string left3 = "null";
        public string right1 = "null";
        public string right2 = "null";
        public string right3 = "null";
    }

    public static ChoosedDeviceManager Instance = null;
    private static Dictionary<string, BikeDetail> BikeDetail_Dic = new Dictionary<string, BikeDetail>();
    private RawDataView rawDataView;
    [SerializeField, Header("[標題文字]")]
    Text TittleText;
    [SerializeField, Header("標題文字顏色(連線)")]
    Color connectedColor;
    [SerializeField, Header("標題文字顏色(斷線)")]
    Color disconnectedColor;
    [SerializeField, Header("[收到Callback]")]
    Text CallbackText;
    [SerializeField, Header("[車子詳細資訊]")]
    Text BikeDetailText;
    [SerializeField, Header("[管理的子畫面]")]
    List<GameObject> ChildPages = new List<GameObject>();
    [SerializeField, Header("[功能表按鈕]")]
    GameObject ToDoListButton;
    [SerializeField, Header("[自動讀取開關]")]
    Toggle AutoBikeDetail;
    //標題
    public static string DeviceTittle = "";
    //所選裝置位址
    public static string DeviceAddress = "";
    //暫存所有的回應
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
        //抓rawDataView頁面
        rawDataView = GetComponentInChildren<RawDataView>(true);
    }
    //啟用時
    private void OnEnable()
    {
        RCToolPlugin.onReceiveEncodeRawData += RCToolPlugin_onReceiveEncodeRawData;
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged += RCToolPlugin_onDeviceStatusChanged;
        HomeManager.RegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
        HomeManager.RegistEncodeEvent(ParseCallBack_onReceiveEncodeParsedData);
        SetTittleText(DeviceTittle);
        SetTittleTextColor();
        ShowBikeDetail();
    }
    /// <summary>
    /// 收到已解密的
    /// </summary>
    private void ParseCallBack_onReceiveDecodeParsedData(string callback)
    {
        Debug.Log("After parse callback :" + callback);
        if (!string.IsNullOrEmpty(callback))
        {
            AddNewCallback(callback);
        }
    }
    /// <summary>
    /// 收到未解密(或明碼)的
    /// </summary>
    /// <param name="callback"></param>
    private void ParseCallBack_onReceiveEncodeParsedData(string callback)
    {
        //{23},[BLEver]會送到這裡
        Debug.Log("After parse callback (ParseCallBack_onReceiveEncodeParsedData):" + callback);
        if (!string.IsNullOrEmpty(callback))
        {
            AddNewCallback(callback);
        }
    }

    //停用時
    private void OnDisable()
    {
        RCToolPlugin.onReceiveEncodeRawData -= RCToolPlugin_onReceiveEncodeRawData;
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged -= RCToolPlugin_onDeviceStatusChanged;
        HomeManager.UnRegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
        HomeManager.UnRegistEncodeEvent(ParseCallBack_onReceiveEncodeParsedData);
        SetTittleText("");
        BikeDetailText.text = "";
    }
    //收到的callback(Encode)
    private void RCToolPlugin_onReceiveEncodeRawData(string address, string data)
    {
        rawDataView.AddEncodeCallback(address, data);
    }
    //收到的callback(Decode)
    private void RCToolPlugin_onReceiveDecodeRawData(string address, string data)
    {
        rawDataView.AddDecodeCallback(address, data);
    }

    private void RCToolPlugin_onDeviceStatusChanged(string address, string status)
    {
        switch (status)
        {
            case "Connected":
                SetTittleText(RCToolPlugin.GetDeviceName(address) + "|" + address);
                if (AutoBikeDetail.isOn)
                    RequestBikeDetail();
                break;
            default:
                Debug.Log(address + " status :" + status);
                break;
        }
    }
    //返回按鈕
    public void AppManager_AndroidBackButton()
    {
        AppManager.Instance.SetPage("Home");
    }
    private void Update()
    {
        SetTittleTextColor();
        //SetToDoListButton();
    }
    /// <summary>
    /// 設置標題文字
    /// </summary>
    public void SetTittleText(string content)
    {
        TittleText.text = content;
    }
    /// <summary>
    /// 連線/離線時設置標題顏色
    /// </summary>
    private void SetTittleTextColor()
    {
        if (RCToolPlugin.IsConnected(DeviceAddress))
            TittleText.color = connectedColor;
        else
            TittleText.color = disconnectedColor;
    }

    private void SetToDoListButton()
    {
        ToDoListButton.SetActive(RCToolPlugin.IsConnected(DeviceAddress));
    }
    #region [車子資訊流程]
    public void RequestBikeDetail()
    {
        if (!RCToolPlugin.IsConnected(DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        StartCoroutine("_RequestBikeDetail");
    }
    private IEnumerator _RequestBikeDetail()
    {
        Loading.Instance.ShowLoading(3.5f);
        callback_dic.Clear();
        BikeDetailText.text = "";
        yield return new WaitForSeconds(1.5f);
        string[] cmdList = { "02", "05", "UIBLE_Version", "09", "32", "12", "0A", "0C", "D4", "D1", "D2", "D3", "0D", "13", "0E", "37", "38", "39", "DD" };
        for (int i = 0; i < cmdList.Length; i++)
        {
            CommandManager.SendCMD(DeviceAddress, cmdList[i], null, null);
        }
        yield return new WaitForSeconds(2.0f);
        Loading.Instance.HideLoading();
    }
    /// <summary>
    /// 新增收到的回應到暫存
    /// </summary>
    private void AddNewCallback(string input)
    {
        if (input.Split('|').Length < 3)
            return;
        string Key = input.Split('|')[1];
        string Value = input.Split('|')[2];
        if (!Value.EndsWith("wait"))
        {
            if (!callback_dic.ContainsKey(Key))
                callback_dic.Add(Key, Value);
            else if (callback_dic.ContainsKey(Key))
                callback_dic[Key] = Value;
            ParseBikeDetail();
            ShowBikeDetail();
        }

    }
    /// <summary>
    /// 解析字典裡暫存的回應
    /// </summary>
    public void ParseBikeDetail()
    {
        try
        {
            BikeDetail info = new BikeDetail();
            string str = "";
            if (callback_dic.TryGetValue("05", out str))
            {
                //RCType,UI_FirmwareVersion,UI_HardwareVersion,EV_ctg1,EV_ctg2
                string[] data = str.Split(',');
                info.RCType = data[0];
                info.RCFW = data[1];
                info.RCHW = data[2];
                info.ctg1 = data[3];
                info.ctg2 = data[4];
            }
            if (callback_dic.TryGetValue("UIBle_Ver", out str))
            {
                //R3/R4/R7/R9
                info.UIBle_Ver = str;
            }
            if (callback_dic.TryGetValue("09", out str))
            {
                //DUType(DU7), DU_FWname(0PB), DU_HWname(X0PB), DUFWVersion(202107030), RCID/evymc(50810), SN(202101)
                string[] data = str.Split(',');
                info.DUType = data[0];
                info.DUFW_MD = data[1];
                info.DUHW_MD = data[2];
                info.DUFWver = data[3];
                info.RCID = data[4];
                info.SN = data[5];
            }
            if (callback_dic.TryGetValue("32", out str))
            {
                //車架號碼
                info.frameNumber = str;
            }
            if (callback_dic.TryGetValue("12", out str))
            {
                //odo,tut
                string[] data = str.Split(',');
                info.odo = data[0];
                info.tut = data[1];
            }
            if (callback_dic.TryGetValue("0A", out str))
            {
                //stct,lstc,fstc,baac,paac,caac,naac,taac,eaac
                string[] data = str.Split(',');
                info.lstc = data[1];
                info.fstc = data[2];
            }
            if (callback_dic.TryGetValue("0C", out str))
            {
                //ErrorCode,BBSSfw
                string[] data = str.Split(',');
                info.BBSS = data[1];
            }
            if (callback_dic.TryGetValue("D4", out str))
            {
                //DU, BATT, SBATT, RMO_1, RMO_2, DSP, S_FD, S_RD, S_SWitchShifter
                string[] data = str.Split(',');
                info.exist_DU = data[0];
                info.exist_BATT = data[1];
                info.exist_SBATT = data[2];
                info.exist_RMO1 = data[3];
                info.exist_RMO2 = data[4];
                info.exist_DSP = data[5];
                info.exist_SFD = data[6];
                info.exist_SRD = data[7];
                info.exist_SSWS = data[8];
            }
            if (callback_dic.TryGetValue("D1", out str))
            {
                //remote_type,fw_ver,hw_ver
                string[] data = str.Split(',');
                info.rmo1Type = data[0];
                info.rmo1FW = data[1];
                info.rmo1HW = data[2];
            }
            if (callback_dic.TryGetValue("D2", out str))
            {
                //remote_type,fw_ver,hw_ver
                string[] data = str.Split(',');
                info.rmo2Type = data[0];
                info.rmo2FW = data[1];
                info.rmo2HW = data[2];
            }
            if (callback_dic.TryGetValue("D3", out str))
            {
                //remote_type,fw_ver,hw_ver
                string[] data = str.Split(',');
                info.dspType = data[0];
                info.dspFW = data[1];
                info.dspHW = data[2];
            }
            if (callback_dic.TryGetValue("0D", out str))
            {
                //minor,major,SN
                string[] data = str.Split(',');
                info.minor = data[0];
                info.major = data[1];
                info.BATTSN = data[2];
            }
            if (callback_dic.TryGetValue("13", out str))
            {
                //rsoc(%),eplife(%),fcc(Wh)
                string[] data = str.Split(',');
                info.rsoc = data[0];
                info.eplife = data[1];
                info.fcc = data[2];
            }
            if (callback_dic.TryGetValue("0E", out str))
            {
                //ccy,cchg,hrd
                string[] data = str.Split(',');
                info.ccy = data[0];
                info.cchg = data[1];
                info.hrd = data[2];
            }
            if (callback_dic.TryGetValue("37", out str))
            {
                //rsoc(%),eplife(%),fcc(Wh)
                string[] data = str.Split(',');
                info.sub_rsoc = data[0];
                info.sub_eplife = data[1];
                info.sub_fcc = data[2];
            }
            if (callback_dic.TryGetValue("38", out str))
            {
                //minor,major,SN
                string[] data = str.Split(',');
                info.sub_minor = data[0];
                info.sub_major = data[1];
                info.sub_BATTSN = data[2];
            }
            if (callback_dic.TryGetValue("39", out str))
            {
                //ccy,cchg,hrd
                string[] data = str.Split(',');
                info.sub_ccy = data[0];
                info.sub_cchg = data[1];
                info.sub_hrd = data[2];
            }
            if (callback_dic.TryGetValue("DD", out str))
            {
                //left,right,left1,left2,left3,right1,right2,right3
                string[] data = str.Split(',');
                info.left = data[0];
                info.right = data[1];
                info.left1 = data[2];
                info.left2 = data[3];
                info.left3 = data[4];
                info.right1 = data[5];
                info.right2 = data[6];
                info.right3 = data[7];
            }
            if (!BikeDetail_Dic.ContainsKey(DeviceAddress))
            {
                BikeDetail_Dic.Add(DeviceAddress, info);
            }
            else
            {
                BikeDetail_Dic[DeviceAddress] = info;
            }
        }
        catch (Exception e)
        {
            Debug.Log("ParseBikeDetail() Error!!!" + e.Message);
        }
    }
    /// <summary>
    /// 顯示解析後的資訊在UI上
    /// </summary>
    public void ShowBikeDetail()
    {
        try
        {
            if (!BikeDetail_Dic.ContainsKey(DeviceAddress))
                return;
            BikeDetail info = BikeDetail_Dic[DeviceAddress];
            string str = "";
            str += string.Format("<color=yellow>[05]</color>RC Type :{0}\nRC firmware version :{1}\nRC hardware version :{2}\nctg1 :{3}, ctg2 :{4}", info.RCType, info.RCFW, info.RCHW, info.ctg1, info.ctg2);
            str += string.Format("\n\n<color=yellow>[FB,11,00]</color>UIBle version :{0}", info.UIBle_Ver);
            str += string.Format("\n\n<color=yellow>[09]</color>DU Type :{0}(RCID :{4})\nDU firmware name :{1}({3})\nDU hardware name :{2}\nDU SN number :{5}", info.DUType, info.DUFW_MD, info.DUHW_MD, info.DUFWver, info.RCID, info.SN);
            str += string.Format("\n\n<color=yellow>[0C]</color>BBSS firmware version :{0}", info.BBSS);
            str += string.Format("\n\n<color=yellow>[32]</color>Frame Number :{0}", info.frameNumber);
            str += string.Format("\n\n<color=yellow>[12]</color>DU ODO :{0}\nTotal Ride Time :{1}", info.odo, info.tut);
            str += string.Format("\n\n<color=yellow>[0A]</color>Last Service Time :{0}\nLast Service Distance :{1}", info.lstc, info.fstc);
            str += string.Format("\n\n<color=yellow>[D4]</color>0:Not Exist, 1:Exist\nDU :{0}\nMain EnergyPak :{1}\nSub EnergyPak :{2}\nRemote-1 :{3}\nRemote-2 :{4}\nDisplay :{5}\nShimano Front-Derailleur :{6}\nShimano Rear-Derailleur :{7}\nShimano Switch/Shifter :{8}"
                , info.exist_DU, info.exist_BATT, info.exist_SBATT, info.exist_RMO1, info.exist_RMO2, info.exist_DSP, info.exist_SFD, info.exist_SRD, info.exist_SSWS);
            str += string.Format("\n\n<color=yellow>[D1]</color>Remote-1 type :{0}\nRemote-1 firmware version :{1}\nRemote-1 hardware version :{2}", info.rmo1Type, info.rmo1FW, info.rmo1HW);
            str += string.Format("\n\n<color=yellow>[D2]</color>Remote-2 type :{0}\nRemote-2 firmware version :{1}\nRemote-2 hardware version :{2}", info.rmo2Type, info.rmo2FW, info.rmo2HW);
            str += string.Format("\n\n<color=yellow>[D3]</color>Display type :{0}\nDisplay firmware version :{1}\nDisplay hardware version :{2}", info.dspType, info.dspFW, info.dspHW);
            str += string.Format("\n\n<color=yellow>[0D]</color>Main EnergyPak Cell version :{0}\nMain EnergyPak firmware version :{1}\nMain EnergyPak SN number :{2}", info.minor, info.major, info.BATTSN);
            str += string.Format("\n\n<color=yellow>[13]</color>Total Electricity :{0}\nEnergyPak Health :{1}\nLast full charge :{2}", info.rsoc, info.eplife, info.fcc);
            str += string.Format("\n\n<color=yellow>[0E]</color>Main EnergyPak charge cycle :{0}\nCharge Times :{1}\nLarge current discharge ratio :{2}", info.ccy, info.cchg, info.hrd);
            str += string.Format("\n\n<color=yellow>[37]</color>Sub EnergyPak Electricity :{0}\nSub EnergyPak Health :{1}\nLast full charge :{2}", info.sub_rsoc, info.sub_eplife, info.sub_fcc);
            str += string.Format("\n\n<color=yellow>[38]</color>Sub EnergyPak Cell version :{0}\nSub EnergyPak firmware version :{1}\nSub EnergyPak SN number :{2}", info.sub_minor, info.sub_major, info.sub_BATTSN);
            str += string.Format("\n\n<color=yellow>[39]</color>Sub EnergyPak charge cycle :{0}\nCharge Times :{1}\nLarge current discharge ratio :{2}", info.sub_ccy, info.sub_cchg, info.sub_hrd);
            str += string.Format("\n\n<color=yellow>[DD]</color>Left Ring :{0}, Right Ring :{1}\nLeft1 :{2}\nLeft2 :{3}\nLeft3 :{4}\nRight1 :{5}\nRight2 :{6}\nRight3 :{7}",
                info.left, info.right, info.left1, info.left2, info.left3, info.right1, info.right2, info.right3);
            BikeDetailText.text = str;
        }
        catch {
            Debug.Log("ShowBikeDetail() Error!!!");
        }
    }
    /// <summary>
    /// 獲取要存在Log的文字
    /// </summary>
    public static string GetBikeDetail_Log(string address)
    {
        try
        {
            if (!BikeDetail_Dic.ContainsKey(address))
                return "";
            BikeDetail info = BikeDetail_Dic[address];
            string str = "";
            str += string.Format("[05]RC類型 :{0}\nRC韌體版本 :{1}\nRC硬體版本 :{2}\nctg1 :{3}, ctg2 :{4}", info.RCType, info.RCFW, info.RCHW, info.ctg1, info.ctg2);
            str += string.Format("\n\n[FB,11,00]藍牙版本 :{0}", info.UIBle_Ver);
            str += string.Format("\n\n[09]DU型號 :{0}(RCID :{4})\nDU韌體名稱 :{1}({3})\nDU硬體名稱 :{2}\nDU生產流水號 :{5}", info.DUType, info.DUFW_MD, info.DUHW_MD, info.DUFWver, info.RCID, info.SN);
            str += string.Format("\n\n[0C]BBSS韌體版本 :{0}", info.BBSS);
            str += string.Format("\n\n[32]車架號碼 :{0}", info.frameNumber);
            str += string.Format("\n\n[12]馬達總里程 :{0}\n總騎乘時間 :{1}", info.odo, info.tut);
            str += string.Format("\n\n[0A]距離上次回廠時間 :{0}\n距離上次回廠距離 :{1}", info.lstc, info.fstc);
            str += string.Format("\n\n[D4]0:不存在, 1:存在\n馬達 :{0}\n主電池 :{1}\n副電池 :{2}\nRemote-1 :{3}\nRemote-2 :{4}\n螢幕 :{5}\nShimano Front-Derailleur :{6}\nShimano Rear-Derailleur :{7}\nShimano Switch/Shifter :{8}"
                , info.exist_DU, info.exist_BATT, info.exist_SBATT, info.exist_RMO1, info.exist_RMO2, info.exist_DSP, info.exist_SFD, info.exist_SRD, info.exist_SSWS);
            str += string.Format("\n\n[D1]Remote-1 型號 :{0}\nRemote-1韌體版本 :{1}\nRemote-1硬體版本 :{2}", info.rmo1Type, info.rmo1FW, info.rmo1HW);
            str += string.Format("\n\n[D2]Remote-2 型號 :{0}\nRemote-2韌體版本 :{1}\nRemote-2硬體版本 :{2}", info.rmo2Type, info.rmo2FW, info.rmo2HW);
            str += string.Format("\n\n[D3]Display 型號 :{0}\nDisplay韌體版本 :{1}\nDisplay硬體版本 :{2}", info.dspType, info.dspFW, info.dspHW);
            str += string.Format("\n\n[0D]主電池Cell版本 :{0}\n主電池韌體版本 :{1}\n主電池生產流水號 :{2}", info.minor, info.major, info.BATTSN);
            str += string.Format("\n\n[13]總電量 :{0}\n電池健康度 :{1}\n前次充飽容量 :{2}", info.rsoc, info.eplife, info.fcc);
            str += string.Format("\n\n[0E]主電池充電循環次數 :{0}\n充電次數 :{1}\n大電流放電比例 :{2}", info.ccy, info.cchg, info.hrd);
            str += string.Format("\n\n[37]副電池容量 :{0}\n副電池壽命 :{1}\n前次充飽容量 :{2}", info.sub_rsoc, info.sub_eplife, info.sub_fcc);
            str += string.Format("\n\n[38]副電池Cell版本 :{0}\n副電池韌體版本 :{1}\n副電池生產流水號 :{2}", info.sub_minor, info.sub_major, info.sub_BATTSN);
            str += string.Format("\n\n[39]副電池充電循環次數 :{0}\n充電次數 :{1}\n大電流放電比例 :{2}", info.sub_ccy, info.sub_cchg, info.sub_hrd);
            str += string.Format("\n\n[DD]Left Ring :{0}, Right Ring :{1}\nLeft1 :{2}\nLeft2 :{3}\nLeft3 :{4}\nRight1 :{5}\nRight2 :{6}\nRight3 :{7}",
                info.left, info.right, info.left1, info.left2, info.left3, info.right1, info.right2, info.right3);
            return str;
        }
        catch
        {
            Debug.Log("GetBikeDetail_Log() Error!!!");
        }
        return "";
    }
    /// <summary>
    /// 獲取要存在RideRecord的文字
    /// </summary>
    public static string GetBikeDetail_RideRecord(string address)
    {

        if (!BikeDetail_Dic.ContainsKey(address))
            return "";
        BikeDetail info = BikeDetail_Dic[address];
        string str = "";
        str += string.Format("[05]RC類型 :{0}\r\nRC韌體版本 :{1}\r\nRC硬體版本 :{2}\r\nctg1 :{3}, ctg2 :{4}", info.RCType, info.RCFW, info.RCHW, info.ctg1, info.ctg2);
        str += string.Format("\r\n\r\n[FB 11 00]藍牙版本 :{0}", info.UIBle_Ver);
        str += string.Format("\r\n\r\n[09]DU型號 :{0}(RCID :{4})\r\nDU韌體名稱 :{1}({3})\r\nDU硬體名稱 :{2}\r\nDU生產流水號 :{5}", info.DUType, info.DUFW_MD, info.DUHW_MD, info.DUFWver, info.RCID, info.SN);
        str += string.Format("\r\n\r\n[0C]BBSS韌體版本 :{0}", info.BBSS);
        str += string.Format("\r\n\r\n[32]車架號碼 :{0}", info.frameNumber);
        str += string.Format("\r\n\r\n[12]馬達總里程 :{0}\r\n總騎乘時間 :{1}", info.odo, info.tut);
        str += string.Format("\r\n\r\n[0A]距離上次回廠時間 :{0}\r\n距離上次回廠距離 :{1}", info.lstc, info.fstc);
        str += string.Format("\r\n\r\n[D4]0:不存在, 1:存在\r\n馬達 :{0}\r\n主電池 :{1}\r\n副電池 :{2}\r\nRemote-1 :{3}\r\nRemote-2 :{4}\r\n螢幕 :{5}\r\nShimano Front-Derailleur :{6}\r\nShimano Rear-Derailleur :{7}\r\nShimano Switch/Shifter :{8}"
            , info.exist_DU, info.exist_BATT, info.exist_SBATT, info.exist_RMO1, info.exist_RMO2, info.exist_DSP, info.exist_SFD, info.exist_SRD, info.exist_SSWS);
        str += string.Format("\r\n\r\n[D1]Remote-1 型號 :{0}\r\nRemote-1韌體版本 :{1}\r\nRemote-1硬體版本 :{2}", info.rmo1Type, info.rmo1FW, info.rmo1HW);
        str += string.Format("\r\n\r\n[D2]Remote-2 型號 :{0}\r\nRemote-2韌體版本 :{1}\r\nRemote-2硬體版本 :{2}", info.rmo2Type, info.rmo2FW, info.rmo2HW);
        str += string.Format("\r\n\r\n[D3]Display 型號 :{0}\r\nDisplay韌體版本 :{1}\r\nDisplay硬體版本 :{2}", info.dspType, info.dspFW, info.dspHW);
        str += string.Format("\r\n\r\n[0D]主電池Cell版本 :{0}\r\n主電池韌體版本 :{1}\r\n主電池生產流水號 :{2}", info.minor, info.major, info.BATTSN);
        str += string.Format("\r\n\r\n[13]總電量 :{0}\r\n電池健康度 :{1}\r\n前次充飽容量 :{2}", info.rsoc, info.eplife, info.fcc);
        str += string.Format("\r\n\r\n[0E]主電池充電循環次數 :{0}\r\n充電次數 :{1}\r\n大電流放電比例 :{2}", info.ccy, info.cchg, info.hrd);
        str += string.Format("\r\n\r\n[37]副電池容量 :{0}\r\n副電池壽命 :{1}\r\n前次充飽容量 :{2}", info.sub_rsoc, info.sub_eplife, info.sub_fcc);
        str += string.Format("\r\n\r\n[38]副電池Cell版本 :{0}\r\n副電池韌體版本 :{1}\r\n副電池生產流水號 :{2}", info.sub_minor, info.sub_major, info.sub_BATTSN);
        str += string.Format("\r\n\r\n[39]副電池充電循環次數 :{0}\r\n充電次數 :{1}\r\n大電流放電比例 :{2}", info.sub_ccy, info.sub_cchg, info.sub_hrd);
        str += string.Format("\r\n\r\n[DD]Left Ring :{0}, Right Ring :{1}\r\nLeft1 :{2}\r\nLeft2 :{3}\r\nLeft3 :{4}\r\nRight1 :{5}\r\nRight2 :{6}\r\nRight3 :{7}",
            info.left, info.right, info.left1, info.left2, info.left3, info.right1, info.right2, info.right3);
        return str;
    }
    #endregion

    #region [按鈕呼叫的]

    public void SendCMD(string cmdNum)
    {
        if (!RCToolPlugin.IsConnected(DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        CommandManager.SendCMD(DeviceAddress, cmdNum, null, null);
    }

    public void Connect()
    {
        RCToolPlugin.ConnectDevice(DeviceAddress);
    }

    public void Disconnect()
    {
        RCToolPlugin.DisconnectDevice(DeviceAddress);
    }

    public void CloseChildPages()
    {
        for (int i = 0; i < ChildPages.Count; i++)
        {
            ChildPages[i].SetActive(false);
        }
    }

    public static BikeDetail GetBikeDetail(string address)
    {
        if (BikeDetail_Dic.ContainsKey(address))
            return BikeDetail_Dic[address];
        return null;
    }
    #endregion
}
