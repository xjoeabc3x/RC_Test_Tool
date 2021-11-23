using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class DFUTest : MonoBehaviour
{
    [SerializeField]
    Text FilePath;
    [SerializeField]
    Text callback;

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
        FilePath.text = Path.Combine(Application.streamingAssetsPath, filename);
    }

    public void StartDFU()
    {
        if (string.IsNullOrEmpty(ChoosedDeviceManager.DeviceAddress) || string.IsNullOrEmpty(FilePath.text))
        {
            return;
        }
        RCToolPlugin.StartDFU(ChoosedDeviceManager.DeviceAddress, FilePath.text);
    }

}
