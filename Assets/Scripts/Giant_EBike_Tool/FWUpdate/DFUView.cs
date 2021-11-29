using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DFUView : MonoBehaviour
{
    public static DFUView Instance = null;

    public string CurrentFile;

    [SerializeField, Header("[檔案清單位置]")]
    Transform FileListPos;
    [SerializeField, Header("[檔案Prefab]")]
    GameObject FileItemPrefab;
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
        RCToolPlugin.onReceiveDFUStatus += RCToolPlugin_onReceiveDFUStatus;
        RCToolPlugin.onReceiveDFUProgress += RCToolPlugin_onReceiveDFUProgress;
        RCToolPlugin.onReceiveDFUError += RCToolPlugin_onReceiveDFUError;
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveDFUStatus -= RCToolPlugin_onReceiveDFUStatus;
        RCToolPlugin.onReceiveDFUProgress -= RCToolPlugin_onReceiveDFUProgress;
        RCToolPlugin.onReceiveDFUError -= RCToolPlugin_onReceiveDFUError;
    }

    private void RCToolPlugin_onReceiveDFUError(string address, string errorcode, string errortype, string msg)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveDFUError :add:{0}, Ecode:{1}, Etype:{2}, MSG:{3}", address, errorcode, errortype, msg));
        Toast.Instance.ShowToast(string.Format("add:{0}, Ecode:{1}, Etype:{2}, MSG:{3}", address, errorcode, errortype, msg));
        Loading.Instance.HideLoading();
    }

    private void RCToolPlugin_onReceiveDFUProgress(string address, string percent)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveDFUProgress :add:{0}, per:{1}", address, percent));
        Toast.Instance.ShowToast(string.Format("add:{0}, per:{1}", address, percent));
    }

    private void RCToolPlugin_onReceiveDFUStatus(string address, string status)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveDFUStatus :add:{0}, status:{1}", address, status));
        Toast.Instance.ShowToast(string.Format("add:{0}, status:{1}", address, status));
        if (status.ToLower().Contains("complete"))
        {
            Loading.Instance.HideLoading();
        }
    }

    public void GetFileItems(string folderPath)
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

    public void StartDFU()
    {
        if (string.IsNullOrEmpty(ChoosedDeviceManager.DeviceAddress) || string.IsNullOrEmpty(CurrentFile))
            return;
        RCToolPlugin.StartDFU(ChoosedDeviceManager.DeviceAddress, CurrentFile);
        Loading.Instance.ShowLoading();
    }

}
