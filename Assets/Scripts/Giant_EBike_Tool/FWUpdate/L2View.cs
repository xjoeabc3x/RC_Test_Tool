using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class L2View : MonoBehaviour
{
    public static L2View Instance = null;

    private string CurrentType;
    public string CurrentFile;

    [SerializeField, Header("[檔案清單位置]")]
    Transform FileListPos;
    [SerializeField, Header("[檔案Prefab]")]
    GameObject FileItemPrefab;
    [SerializeField, Header("[更新的目標Function]")]
    Dropdown TypeChoose;
    [SerializeField, Header("[MCU目標]")]
    Dropdown MCUChoose;
    [Header("[ToggleGroup]")]
    public ToggleGroup toggleGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this);
            return;
        }
    }

    private void OnEnable()
    {
        RCToolPlugin.onReceiveL2Msg += RCToolPlugin_onReceiveL2Msg;
        RCToolPlugin.onReceiveL2Error += RCToolPlugin_onReceiveL2Error;
        SearchFile();
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveL2Msg -= RCToolPlugin_onReceiveL2Msg;
        RCToolPlugin.onReceiveL2Error -= RCToolPlugin_onReceiveL2Error;
        CurrentFile = "";
        CurrentType = "";
    }

    public void SearchFile()
    {
        MCUChoose.gameObject.SetActive(false);
        CurrentFile = "";
        CurrentType = "";
        switch (TypeChoose.value)
        {
            case 0: //MCU
                CurrentType = "01";
                MCUChoose.gameObject.SetActive(true);
                switch (MCUChoose.value)
                {
                    case 0: //Charge S5
                        GetFileItems("FW/Charge_S5");
                        break;
                    case 1: //EVO jpn
                        GetFileItems("FW/EVO_JPN");
                        break;
                    case 2: //One
                        GetFileItems("FW/ONE");
                        break;
                    case 3: //EVO
                        GetFileItems("FW/EVO");
                        break;
                    case 4: //EVO Pro
                        GetFileItems("FW/EVO_Pro");
                        break;
                    case 5: //EVO S5
                        GetFileItems("FW/EVO_S5");
                        break;
                    case 6: //EVO 45
                        GetFileItems("FW/EVO_45");
                        break;
                    case 7: //One ant
                        GetFileItems("FW/ONE_ANT");
                        break;
                    case 8: //One ble
                        GetFileItems("FW/ONE_BLE");
                        break;
                }
                break;
            case 1: //CAN
                CurrentType = "04";
                GetFileItems("FW/CAN");
                break;
            case 2: //BBSS
                CurrentType = "06";
                GetFileItems("FW/BBSS");
                break;
            case 3: //CT
                CurrentType = "08";
                GetFileItems("FW/Remote_CT");
                break;
            case 4: //NewEVO
                CurrentType = "0A";
                GetFileItems("FW/New_EVO");
                break;
            case 5: //Sport
                CurrentType = "0D";
                GetFileItems("FW/Remote_Sport");
                break;
            case 6: //2in1
                CurrentType = "0F";
                GetFileItems("FW/Remote_2in1");
                break;
            case 7: //OnOff
                CurrentType = "12";
                GetFileItems("FW/Remote_OnOff");
                break;
        }
    }

    private void RCToolPlugin_onReceiveL2Msg(string address, string msg)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveL2Msg :add:{0}, msg:{1}", address, msg));
        Toast.Instance.ShowToast(string.Format("address:{0}, msg:{1}", address, msg));
        if (msg.ToLower().Contains("complete"))
        {
            Loading.Instance.HideLoading();
        }
    }

    private void RCToolPlugin_onReceiveL2Error(string address, string msg)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveL2Error :add:{0}, msg:{1}", address, msg));
        Toast.Instance.ShowToast(string.Format("address:{0}, msg:{1}", address, msg));
        if (msg.ToLower().Contains("fail"))
        {
            RCToolPlugin.AbortL2();
            Loading.Instance.HideLoading();
        }
    }

    public void StartL2()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        if (string.IsNullOrEmpty(ChoosedDeviceManager.DeviceAddress) || string.IsNullOrEmpty(CurrentFile) || string.IsNullOrEmpty(CurrentType))
        {
            Toast.Instance.ShowToast("Something go wrong.");
            return;
        }
        RCToolPlugin.StartL2(ChoosedDeviceManager.DeviceAddress, CurrentType, CurrentFile);
        Loading.Instance.ShowLoading(_AbortL2, "Cancel");
    }

    private void _AbortL2()
    {
        RCToolPlugin.AbortL2();
        Loading.Instance.HideLoading();
    }

    private void GetFileItems(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
            return;
        string path = Path.Combine(Application.persistentDataPath, folderPath);
        DirectoryInfo di = new DirectoryInfo(path);
        for (int i = FileListPos.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(FileListPos.GetChild(0).gameObject);
        }
        foreach (FileInfo fi in di.GetFiles())
        {
            var newFileItem = Instantiate(FileItemPrefab, FileListPos);
            FileItem fileitem = newFileItem.GetComponent<FileItem>();
            fileitem.FullFileName = fi.FullName;
            fileitem.SetTittle(fi.Name);
        }
    }
}
