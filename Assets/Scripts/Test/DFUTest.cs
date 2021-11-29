using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.IO.Compression;
using System;
using UnityEngine.Networking;
using Internals;

public class DFUTest : MonoBehaviour
{
    [SerializeField]
    Text FilePath;
    [SerializeField]
    Text callback;
    [SerializeField]
    InputField customInput;

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
        callback.text = string.Format("add:{0}, Ecode:{1}, Etype:{2}, MSG:{3}", address, errorcode, errortype, msg);
    }

    private void RCToolPlugin_onReceiveDFUProgress(string address, string percent)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveDFUProgress :add:{0}, per:{1}", address, percent));
        callback.text = string.Format("add:{0}, per:{1}", address, percent);
    }

    private void RCToolPlugin_onReceiveDFUStatus(string address, string status)
    {
        Debug.Log(string.Format("RCToolPlugin_onReceiveDFUStatus :add:{0}, status:{1}", address, status));
        callback.text = string.Format("add:{0}, status:{1}", address, status);
    }

    public void GetFilePath(string filename)
    {
        byte[] bbb = GetStreamBytes(filename);
        FilePath.text += string.Format("bbb :{0}\nPath :{1}", bbb.Length, Path.Combine(Application.persistentDataPath, filename));
    }

    private byte[] GetStreamBytes(string filename)
    {
        FileStream stream = new FileInfo(Path.Combine(Application.persistentDataPath, filename)).OpenRead();

        //WWW theReader = new WWW(Path.Combine(Application.streamingAssetsPath, filename));
        //byte[] bb = theReader.bytes;

        StartCoroutine("TestFileCatch");

        byte[] buffer = new byte[stream.Length];
        //從流中讀取位元組塊並將該資料寫入給定緩衝區buffer中
        stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
        stream.Close();
        stream.Dispose();
        return buffer;
    }

    private IEnumerator TestFileCatch()
    {
        //WWW theReader = new WWW(Path.Combine(Application.streamingAssetsPath, "sg10yxxxapp_20210823001.zip"));
        UnityWebRequest rrr = new UnityWebRequest(Path.Combine(Application.streamingAssetsPath, "sg10yxxxapp_20210823001.zip"));
        rrr.downloadHandler = new DownloadHandlerBuffer();
        rrr.timeout = 5;
        yield return rrr.SendWebRequest();
        byte[] bb = rrr.downloadHandler.data;
        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "Test.zip"), bb);
        FilePath.text += string.Format("bb :{0}\n", bb.Length);
        rrr.Dispose();
        customInput.text = Path.Combine(Application.streamingAssetsPath, "sg10yxxxapp_20210823001.zip");
    }

    public void CopyFolder()
    {
        using (Unzip unzip = new Unzip(Path.Combine(Application.persistentDataPath, "FW.zip")))
        {
            unzip.ExtractToDirectory(Application.persistentDataPath);
        }
        StartCoroutine("_CopyFolder");
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();

        // If the destination directory doesn't exist, create it.       
        Directory.CreateDirectory(destDirName);

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
            }
        }
    }

    private IEnumerator _CopyFolder()
    {
        UnityWebRequest rrr = new UnityWebRequest(new Uri("https://uat-files.giant-hpb.com/public/firmwares/sg10yxxxapp_20211018000.zip"));
        rrr.downloadHandler = new DownloadHandlerBuffer();
        rrr.timeout = 5;
        yield return rrr.SendWebRequest();
        byte[] bb = rrr.downloadHandler.data;
        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "sg10yxxxapp_20211018000.zip"), bb);
        rrr.Dispose();

        //DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath);
        //foreach (var fi in di.GetFiles("*" + "sg10yxxxapp_" + "*"))
        //{
        //    UnityWebRequest request = new UnityWebRequest(Path.Combine(Application.streamingAssetsPath, fi.Name));
        //    request.downloadHandler = new DownloadHandlerBuffer();
        //    request.timeout = 5;
        //    yield return request.SendWebRequest();
        //    byte[] datas = request.downloadHandler.data;
        //    File.WriteAllBytes(Path.Combine(Application.persistentDataPath, fi.Name), datas);
        //    request.Dispose();
        //}
    }

    public void StartDFU()
    {
        if (string.IsNullOrEmpty(ChoosedDeviceManager.DeviceAddress))
            return;
        RCToolPlugin.StartDFU(ChoosedDeviceManager.DeviceAddress, Path.Combine(Application.persistentDataPath, "sg10yxxxapp_20210823001.zip"));
        //RCToolPlugin.StartDFU(ChoosedDeviceManager.DeviceAddress, Path.Combine(Application.streamingAssetsPath, "sg10yxxxapp_20210823001.zip"));
    }

    public void StartCustom()
    {
        RCToolPlugin.StartDFU(ChoosedDeviceManager.DeviceAddress, customInput.text);
    }

    public void AbortDFU()
    {
        RCToolPlugin.AbortDFU();
    }

}
