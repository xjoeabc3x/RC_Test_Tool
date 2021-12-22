using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class L1View : MonoBehaviour
{
    public static L1View Instance = null;

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
        RCToolPlugin.onReceiveL1Msg += RCToolPlugin_onReceiveL1Msg;
        RCToolPlugin.onReceiveL1Error += RCToolPlugin_onReceiveL1Error;
        SearchFile();
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveL1Msg -= RCToolPlugin_onReceiveL1Msg;
        RCToolPlugin.onReceiveL1Error -= RCToolPlugin_onReceiveL1Error;
        CurrentFile = "";
        CurrentType = "";
    }

    private void RCToolPlugin_onReceiveL1Msg(string address, string msg)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveL1Msg :add:{0}, msg:{1}", address, msg));
        Toast.Instance.ShowToast(string.Format("address:{0}, msg:{1}", address, msg));
        if (msg.ToLower().Contains("complete"))
        {
            Loading.Instance.HideLoading();
        }
    }

    private void RCToolPlugin_onReceiveL1Error(string address, string msg)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveL1Error :add:{0}, msg:{1}", address, msg));
        Toast.Instance.ShowToast(string.Format("address:{0}, msg:{1}", address, msg));
        if (msg.ToLower().Contains("fail"))
        {
            RCToolPlugin.AbortL1();
            Loading.Instance.HideLoading();
        }
    }

    public void StartL1()
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
        RCToolPlugin.StartL1(ChoosedDeviceManager.DeviceAddress, CurrentType, CurrentFile);
        Loading.Instance.ShowLoading(_AbortL1, "Cancel");
    }

    private void _AbortL1()
    {
        RCToolPlugin.AbortL1();
        Loading.Instance.HideLoading();
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
                        GetFileItems("FW/Charge_S5/L1");
                        break;
                    case 1: //EVO jpn
                        GetFileItems("FW/EVO_JPN/L1");
                        break;
                    case 2: //EVO
                        GetFileItems("FW/EVO/L1");
                        break;
                    case 3: //EVO Pro
                        GetFileItems("FW/EVO_Pro/L1");
                        break;
                    case 4: //EVO S5
                        GetFileItems("FW/EVO_S5/L1");
                        break;
                    case 5: //EVO 45
                        GetFileItems("FW/EVO_45/L1");
                        break;
                    case 6: //One ble
                        GetFileItems("FW/ONE_BLE/L1");
                        break;
                }
                break;
            case 1:
                CurrentType = "F0";
                GetFileItems("FW/UI_BLE/R3");
                break;
            case 2:
                CurrentType = "F0";
                GetFileItems("FW/UI_BLE/R4");
                break;
        }
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
