using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TrackerView : MonoBehaviour
{
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
            AbortGeteSimProfile();
            AbortWriteIotProfile();
            Loading.Instance.HideLoading();
        }
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveTrackerMsg -= OnReceiveTrackerMsg;
        RCToolPlugin.onReceiveTrackerError -= OnReceiveTrackerError;
    }

    public void GeteSimProfile()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        RCToolPlugin.GetTrackerFile(ChoosedDeviceManager.DeviceAddress, Path.Combine(Application.persistentDataPath, "eSimProfile/eSimProfile.json"), 
            0, 2, 0);
        Loading.Instance.ShowLoading(AbortGeteSimProfile, "Cancel");
    }

    private void AbortGeteSimProfile()
    {
        RCToolPlugin.AbortGetTrackerFile();
    }

    public void RefreshList()
    {
        GetFileItems("eSimProfile");
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

    public void WriteIotProfile()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        RCToolPlugin.WriteTrackerFile(ChoosedDeviceManager.DeviceAddress, Path.Combine(Application.persistentDataPath, "IoTProfile/IotProfile_Test.json"), 
            0, 1);
        Loading.Instance.ShowLoading(AbortWriteIotProfile, "Cancel");
    }

    private void AbortWriteIotProfile()
    {
        RCToolPlugin.AbortWriteTrackerFile();
    }

}
