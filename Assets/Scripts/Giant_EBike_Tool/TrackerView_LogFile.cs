using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TrackerView_LogFile : MonoBehaviour
{
    [SerializeField, Header("[���P�B�M��]")]
    Text UnsyncList_text;
    [SerializeField, Header("[Item Prefab]")]
    GameObject itemPrefab;
    [SerializeField, Header("[FileList Pos]")]
    Transform FileListPos;

    private void OnEnable()
    {
        RCToolPlugin.onReceiveTrackerMsg += OnReceiveTrackerMsg;
        RCToolPlugin.onReceiveTrackerError += OnReceiveTrackerError;
    }

    private void OnReceiveTrackerMsg(string address, string msg)
    {
        Debug.Log(string.Format("OnReceiveTrackerMsg :add:{0}, msg:{1}", address, msg));
        Toast.Instance.ShowToast(string.Format("address:{0}, msg:{1}", address, msg));
        if (msg.ToLower().Contains("complete"))
        {
            Loading.Instance.HideLoading();
        }
    }

    private void OnReceiveTrackerError(string address, string errorMsg)
    {
        Debug.Log(string.Format("OnReceiveTrackerError :add:{0}, msg:{1}", address, errorMsg));
        Toast.Instance.ShowToast(string.Format("address:{0}, ErrorMsg:{1}", address, errorMsg));
        if (errorMsg.ToLower().Contains("fail"))
        {
            Loading.Instance.HideLoading();
        }
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveTrackerMsg -= OnReceiveTrackerMsg;
        RCToolPlugin.onReceiveTrackerError -= OnReceiveTrackerError;
    }

    //���o���P�B�M��
    public void GetLogFile()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        RCToolPlugin.GetTrackerUnsyncFiles(ChoosedDeviceManager.DeviceAddress, 0, 3);
        Loading.Instance.ShowLoading(AbortGetLogFile, "Cancel");
    }

    private void AbortGetLogFile()
    {
        RCToolPlugin.AbortGetTrackerUnsyncFiles();
    }

    //�}�l�v�@�n�D�ɮ�
    public void StartGetUnsycFile()
    {
        
    }
    //�����Ƨ����U�ɮײM��
    public void RefreshList()
    {
        GetFileItems("LogFile_Tracker");
        Toast.Instance.ShowToast("Refreshed.");
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
            var newFileItem = Instantiate(itemPrefab, FileListPos);
            Text title = newFileItem.GetComponentInChildren<Text>(true);
            title.text = fi.Name;
        }
    }

    //���ܥ��P�B�M�檺Flag :0x00, 0x01
    public void ModifyFileFlag(int status)
    {
        
    }
    //�R����Ƨ����ɮ�
    public void DeleteFiles()
    {
        
    }
}
