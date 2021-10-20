using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

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

        //Bike name
        public string frameNumber = "null";
        //DU odo
        public string odo = "null";
        //DU tut
        public string tut = "null";
        //�Z�W���^�t�ɶ�
        public string lstc = "null";
        //�Z�W���^�t�Z��
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
        //�D�q��Cell����,fw,sn
        public string minor = "null";
        public string major = "null";
        public string BATTSN = "null";
        //�D(+��)�q���e�q,�D�q���ةR,�e���R���e�q rsoc(%),eplife(%),fcc(Wh)
        public string rsoc = "null";
        public string eplife = "null";
        public string fcc = "null";
        //�D�q���R�q�`������,�R�q����,�j�q�y��q���100% ccy,cchg,hrd
        public string ccy = "null";
        public string cchg = "null";
        public string hrd = "null";
        //�ƹq���e�q,�ƹq���ةR,�e���R���e�q rsoc(%),eplife(%),fcc(Wh)
        public string sub_rsoc = "null";
        public string sub_eplife = "null";
        public string sub_fcc = "null";
        //�ƹq��Cell����,fw,sn minor,major,SN
        public string sub_minor = "null";
        public string sub_major = "null";
        public string sub_BATTSN = "null";
        //�ƹq���R�q�`������,�R�q����,�j�q�y��q���100% ccy,cchg,hrd
        public string sub_ccy = "null";
        public string sub_cchg = "null";
        public string sub_hrd = "null";
        //Ring�O�_�s�b�H�Ϋ��s���A left,right,left1,left2,left3,right1,right2,right3
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
    [SerializeField, Header("[���D��r]")]
    Text TittleText;
    [SerializeField, Header("���D��r�C��(�s�u)")]
    Color connectedColor;
    [SerializeField, Header("���D��r�C��(�_�u)")]
    Color disconnectedColor;
    [SerializeField, Header("[����Callback]")]
    Text CallbackText;
    [SerializeField, Header("[���l�ԲӸ�T]")]
    Text BikeDetailText;
    [SerializeField, Header("[�޲z���l�e��]")]
    List<GameObject> ChildPages = new List<GameObject>();
    [SerializeField, Header("[�\�����s]")]
    GameObject ToDoListButton;
    //���D
    public static string DeviceTittle = "";
    //�ҿ�˸m��}
    public static string DeviceAddress = "";

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
        rawDataView = GetComponentInChildren<RawDataView>(true);
    }
    //�ҥή�
    private void OnEnable()
    {
        RCToolPlugin.onReceiveEncodeRawData += RCToolPlugin_onReceiveRawData;
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged += RCToolPlugin_onDeviceStatusChanged;
        //AppManager.AndroidBackButton += AppManager_AndroidBackButton;
        SetTittleText(DeviceTittle);
        SetTittleTextColor();
        ShowBikeDetail();
    }
    //���ή�
    private void OnDisable()
    {
        RCToolPlugin.onReceiveEncodeRawData -= RCToolPlugin_onReceiveRawData;
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        RCToolPlugin.onDeviceStatusChanged -= RCToolPlugin_onDeviceStatusChanged;
        //AppManager.AndroidBackButton -= AppManager_AndroidBackButton;
        SetTittleText("");
        BikeDetailText.text = "";
    }
    //���쪺callback(Encode)
    private void RCToolPlugin_onReceiveRawData(string address, string data)
    {
        rawDataView.AddEncodeCallback(address, data);
        //{23}�|�e��o��
    }
    //���쪺callback(Decode)
    private void RCToolPlugin_onReceiveDecodeRawData(string address, string data)
    {
        rawDataView.AddDecodeCallback(address, data);
        string callback = ParseCallBack.CallbackInfo(address, data);
        Debug.Log("After parse callback :" + callback);
        if (!string.IsNullOrEmpty(callback))
        {
            AddNewCallback(callback);
        }
    }

    private void RCToolPlugin_onDeviceStatusChanged(string address, string status)
    {
        switch (status)
        {
            case "Connected":
                SetTittleText(RCToolPlugin.GetDeviceName(address) + "|" + address);
                RequestBikeDetail();
                break;
            default:
                Debug.Log(address + " status :" + status);
                break;
        }
    }
    //��^���s
    public void AppManager_AndroidBackButton()
    {
        AppManager.Instance.SetPage("Home");
    }
    private void Update()
    {
        SetTittleTextColor();
        SetToDoListButton();
    }
    /// <summary>
    /// �]�m���D��r
    /// </summary>
    public void SetTittleText(string content)
    {
        TittleText.text = content;
    }
    /// <summary>
    /// �s�u/���u�ɳ]�m���D�C��
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
    #region [���l��T�y�{]
    public void RequestBikeDetail()
    {
        StartCoroutine("_RequestBikeDetail");
    }
    private IEnumerator _RequestBikeDetail()
    {
        Loading.Instance.ShowLoading(12.6f);
        callback_dic.Clear();
        BikeDetailText.text = "";
        yield return new WaitForSeconds(1.0f);
        string[] cmdList = { "02", "05", "09", "32", "12", "0A", "0C", "D4", "D1", "D2", "D3", "0D", "13", "0E", "37", "38", "39", "DD" };
        for (int i = 0; i < cmdList.Length; i++)
        {
            CommandManager.SendCMD(DeviceAddress, cmdList[i], null, null);
        }
        yield return new WaitForSeconds(10.6f);
        ParseBikeDetail();
        yield return new WaitForSeconds(1.6f);
        ShowBikeDetail();
        Loading.Instance.HideLoading();
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
                //���[���X
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
                //DU,BATT,SBATT,RMO_1,RMO_2,DSP,S_FD,S_RD,S_SWitchShifter
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
        catch
        {
            Debug.Log("ParseBikeDetail() Error!!!");
        }
    }
    public void ShowBikeDetail()
    {
        try
        {
            if (!BikeDetail_Dic.ContainsKey(DeviceAddress))
                return;
            BikeDetail info = BikeDetail_Dic[DeviceAddress];
            string str = "";
            str += string.Format("<color=yellow>[05]</color>RC���� :{0}\nRC���骩�� :{1}\nRC�w�骩�� :{2}\nctg1 :{3}, ctg2 :{4}", info.RCType, info.RCFW, info.RCHW, info.ctg1, info.ctg2);
            str += string.Format("\n\n<color=yellow>[09]</color>DU���� :{0}(RCID :{4})\nDU����W�� :{1}({3})\nDU�w��W�� :{2}\nDU�Ͳ��y���� :{5}", info.DUType, info.DUFW_MD, info.DUHW_MD, info.DUFWver, info.RCID, info.SN);
            str += string.Format("\n\n<color=yellow>[0C]</color>BBSS���骩�� :{0}", info.BBSS);
            str += string.Format("\n\n<color=yellow>[32]</color>���[���X :{0}", info.frameNumber);
            str += string.Format("\n\n<color=yellow>[12]</color>���F�`���{ :{0}\n�`�M���ɶ� :{1}", info.odo, info.tut);
            str += string.Format("\n\n<color=yellow>[0A]</color>�Z���W���^�t�ɶ� :{0}\n�Z���W���^�t�Z�� :{1}", info.lstc, info.fstc);
            str += string.Format("\n\n<color=yellow>[D4]</color>0:���s�b, 1:�s�b\n���F :{0}\n�D�q�� :{1}\n�ƹq�� :{2}\nRemote-1 :{3}\nRemote-2 :{4}\n�ù� :{5}\nShimano Front-Derailleur :{6}\nShimano Rear-Derailleur :{7}\nShimano Switch/Shifter :{8}"
                , info.exist_DU, info.exist_BATT, info.exist_SBATT, info.exist_RMO1, info.exist_RMO2, info.exist_DSP, info.exist_SFD, info.exist_SRD, info.exist_SSWS);
            str += string.Format("\n\n<color=yellow>[D1]</color>Remote-1 ���� :{0}\nRemote-1���骩�� :{1}\nRemote-1�w�骩�� :{2}", info.rmo1Type, info.rmo1FW, info.rmo1HW);
            str += string.Format("\n\n<color=yellow>[D2]</color>Remote-2 ���� :{0}\nRemote-2���骩�� :{1}\nRemote-2�w�骩�� :{2}", info.rmo2Type, info.rmo2FW, info.rmo2HW);
            str += string.Format("\n\n<color=yellow>[D3]</color>Display ���� :{0}\nDisplay���骩�� :{1}\nDisplay�w�骩�� :{2}", info.dspType, info.dspFW, info.dspHW);
            str += string.Format("\n\n<color=yellow>[0D]</color>�D�q��Cell���� :{0}\n�D�q�����骩�� :{1}\n�D�q���Ͳ��y���� :{2}", info.minor, info.major, info.BATTSN);
            str += string.Format("\n\n<color=yellow>[13]</color>�`�q�q :{0}\n�q�����d�� :{1}\n�e���R���e�q :{2}", info.rsoc, info.eplife, info.fcc);
            str += string.Format("\n\n<color=yellow>[0E]</color>�D�q���R�q�`������ :{0}\n�R�q���� :{1}\n�j�q�y��q��� :{2}", info.ccy, info.cchg, info.hrd);
            str += string.Format("\n\n<color=yellow>[37]</color>�ƹq���e�q :{0}\n�ƹq���ةR :{1}\n�e���R���e�q :{2}", info.sub_rsoc, info.sub_eplife, info.sub_fcc);
            str += string.Format("\n\n<color=yellow>[38]</color>�ƹq��Cell���� :{0}\n�ƹq�����骩�� :{1}\n�ƹq���Ͳ��y���� :{2}", info.sub_minor, info.sub_major, info.sub_BATTSN);
            str += string.Format("\n\n<color=yellow>[39]</color>�ƹq���R�q�`������ :{0}\n�R�q���� :{1}\n�j�q�y��q��� :{2}", info.sub_ccy, info.sub_cchg, info.sub_hrd);
            str += string.Format("\n\n<color=yellow>[DD]</color>Left Ring :{0}, Right Ring :{1}\nLeft1 :{2}\nLeft2 :{3}\nLeft3 :{4}\nRight1 :{5}\nRight2 :{6}\nRight3 :{7}",
                info.left, info.right, info.left1, info.left2, info.left3, info.right1, info.right2, info.right3);
            BikeDetailText.text = str;
        }
        catch {
            Debug.Log("ShowBikeDetail() Error!!!");
        }
    }

    public static string GetBikeDetail_Log(string address)
    {
        try
        {
            if (!BikeDetail_Dic.ContainsKey(address))
                return "";
            BikeDetail info = BikeDetail_Dic[address];
            string str = "";
            str += string.Format("[05]RC���� :{0}\nRC���骩�� :{1}\nRC�w�骩�� :{2}\nctg1 :{3}, ctg2 :{4}", info.RCType, info.RCFW, info.RCHW, info.ctg1, info.ctg2);
            str += string.Format("\n\n[09]DU���� :{0}(RCID :{4})\nDU����W�� :{1}({3})\nDU�w��W�� :{2}\nDU�Ͳ��y���� :{5}", info.DUType, info.DUFW_MD, info.DUHW_MD, info.DUFWver, info.RCID, info.SN);
            str += string.Format("\n\n[0C]BBSS���骩�� :{0}", info.BBSS);
            str += string.Format("\n\n[32]���[���X :{0}", info.frameNumber);
            str += string.Format("\n\n[12]���F�`���{ :{0}\n�`�M���ɶ� :{1}", info.odo, info.tut);
            str += string.Format("\n\n[0A]�Z���W���^�t�ɶ� :{0}\n�Z���W���^�t�Z�� :{1}", info.lstc, info.fstc);
            str += string.Format("\n\n[D4]0:���s�b, 1:�s�b\n���F :{0}\n�D�q�� :{1}\n�ƹq�� :{2}\nRemote-1 :{3}\nRemote-2 :{4}\n�ù� :{5}\nShimano Front-Derailleur :{6}\nShimano Rear-Derailleur :{7}\nShimano Switch/Shifter :{8}"
                , info.exist_DU, info.exist_BATT, info.exist_SBATT, info.exist_RMO1, info.exist_RMO2, info.exist_DSP, info.exist_SFD, info.exist_SRD, info.exist_SSWS);
            str += string.Format("\n\n[D1]Remote-1 ���� :{0}\nRemote-1���骩�� :{1}\nRemote-1�w�骩�� :{2}", info.rmo1Type, info.rmo1FW, info.rmo1HW);
            str += string.Format("\n\n[D2]Remote-2 ���� :{0}\nRemote-2���骩�� :{1}\nRemote-2�w�骩�� :{2}", info.rmo2Type, info.rmo2FW, info.rmo2HW);
            str += string.Format("\n\n[D3]Display ���� :{0}\nDisplay���骩�� :{1}\nDisplay�w�骩�� :{2}", info.dspType, info.dspFW, info.dspHW);
            str += string.Format("\n\n[0D]�D�q��Cell���� :{0}\n�D�q�����骩�� :{1}\n�D�q���Ͳ��y���� :{2}", info.minor, info.major, info.BATTSN);
            str += string.Format("\n\n[13]�`�q�q :{0}\n�q�����d�� :{1}\n�e���R���e�q :{2}", info.rsoc, info.eplife, info.fcc);
            str += string.Format("\n\n[0E]�D�q���R�q�`������ :{0}\n�R�q���� :{1}\n�j�q�y��q��� :{2}", info.ccy, info.cchg, info.hrd);
            str += string.Format("\n\n[37]�ƹq���e�q :{0}\n�ƹq���ةR :{1}\n�e���R���e�q :{2}", info.sub_rsoc, info.sub_eplife, info.sub_fcc);
            str += string.Format("\n\n[38]�ƹq��Cell���� :{0}\n�ƹq�����骩�� :{1}\n�ƹq���Ͳ��y���� :{2}", info.sub_minor, info.sub_major, info.sub_BATTSN);
            str += string.Format("\n\n[39]�ƹq���R�q�`������ :{0}\n�R�q���� :{1}\n�j�q�y��q��� :{2}", info.sub_ccy, info.sub_cchg, info.sub_hrd);
            str += string.Format("\n\n[DD]Left Ring :{0}, Right Ring :{1}\nLeft1 :{2}\nLeft2 :{3}\nLeft3 :{4}\nRight1 :{5}\nRight2 :{6}\nRight3 :{7}",
                info.left, info.right, info.left1, info.left2, info.left3, info.right1, info.right2, info.right3);
            return str;
        }
        catch
        {
            Debug.Log("ShowBikeDetail() Error!!!");
        }
        return "";
    }
    #endregion

    #region [���s�I�s��]
    public void SendCMD(string cmdNum)
    {
        if (!RCToolPlugin.IsConnected(DeviceAddress))
        {
            Debug.Log("Device is not Connected.");
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
