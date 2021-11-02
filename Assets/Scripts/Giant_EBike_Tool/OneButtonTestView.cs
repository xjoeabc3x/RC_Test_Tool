using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class OneButtonTestView : MonoBehaviour
{
    [SerializeField, Header("[Bike Detail]")]
    Toggle BikeDetail;
    [SerializeField, Header("[Tuning]")]
    Toggle Tuning;
    [SerializeField, Header("[Display]")]
    Toggle Display;
    [SerializeField, Header("[Ring]")]
    Toggle Ring;
    [SerializeField, Header("[OnOff]")]
    Toggle OnOff;
    [SerializeField, Header("[SpeedLimit]")]
    Toggle SpeedLimit;
    [SerializeField, Header("[測試結果畫面]")]
    GameObject ResultView;
    [SerializeField, Header("[測試結果Text]")]
    Text ResultText;

    private Dictionary<string, List<string>> cmds = new Dictionary<string, List<string>>();
    private Dictionary<string, string> callbacks = new Dictionary<string, string>();

    private void OnEnable()
    {
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveDecodeRawData;
    }

    private void RCToolPlugin_onReceiveDecodeRawData(string address, string data)
    {
        string callback = ParseCallBack.CallbackInfo(address, data);
        string Key = callback.Split('|')[1];
        string Value = callback.Split('|')[2];
        if (!callbacks.ContainsKey(Key))
            callbacks.Add(Key, Value);
        else
            callbacks[Key] = Value;
    }

    private void OnDisable()
    {
        RCToolPlugin.onReceiveDecodeRawData -= RCToolPlugin_onReceiveDecodeRawData;
        //ResetView();
    }

    public void StartTest()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device not connected.");
            return;
        }
        if (!BikeDetail.isOn && !Tuning.isOn && !Display.isOn && !Ring.isOn && !OnOff.isOn && !SpeedLimit.isOn)
        {
            Toast.Instance.ShowToast("At least select one item.");
            return;
        }
        ResetView();
        float delay = BikeDetail.isOn ? 9f : 0;
        delay += Tuning.isOn ? 2f : 0;
        delay += Display.isOn ? 2f : 0;
        delay += Ring.isOn ? 1f : 0;
        delay += OnOff.isOn ? 1f : 0;
        delay += SpeedLimit.isOn ? 1f : 0;
        if (BikeDetail.isOn)
        {
            cmds.Add("Bike Detail", new List<string>() { "02", "05", "09", "32", "12", "0A", "0C", "D4", "D1", "D2", "D3", "0D", "13", "0E", "37", "38", "39", "DD" });
        }
        if (Tuning.isOn)
        {
            cmds.Add("Tuning", new List<string>() { "1A_tuning", "2C", "2D", "1A_normal" });
        }
        if (Display.isOn)
        {
            cmds.Add("Display", new List<string>() { "D5", "D6", "D7", "D8" });
        }
        if (Ring.isOn)
        {
            cmds.Add("Ring", new List<string>() { "DD", "DE" });
        }
        if (OnOff.isOn)
        {
            cmds.Add("OnOff", new List<string>() { "D9", "DA" });
        }
        if (SpeedLimit.isOn)
        {
            cmds.Add("SpeedLimit", new List<string>() { "C0", "C1" });
        }
        StartCoroutine("DoTest", delay);
    }

    private IEnumerator DoTest(float delay)
    {
        foreach (KeyValuePair<string, List<string>> item in cmds)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, item.Value[i], null, null);
            }
        }
        Loading.Instance.ShowLoading(delay + 2);
        Toast.Instance.ShowToast("Wait for about :" + (int)delay + " sec");
        yield return new WaitForSeconds(delay + 2);
        ShowResult();
    }

    private void ShowResult()
    {
        ResultView.SetActive(true);
        foreach (KeyValuePair<string, List<string>> item in cmds)
        {
            ResultText.text += item.Key + "\n";
            for (int i = 0; i < item.Value.Count; i++)
            {
                if (callbacks.ContainsKey(item.Value[i]))
                {
                    ResultText.text += string.Format("[{0}] : OK\n", item.Value[i]);
                }
                else if (item.Value[i].StartsWith("1A"))
                {
                    if (callbacks.ContainsKey("1A"))
                    {
                        ResultText.text += string.Format("[{0}] : OK\n", item.Value[i]);
                    }
                    else
                    {
                        ResultText.text += string.Format("[{0}] : No response\n", item.Value[i]);
                    }
                }
                else
                {
                    ResultText.text += string.Format("[{0}] : No response\n", item.Value[i]);
                }
            }
        }
    }

    private void ResetView()
    {
        cmds.Clear();
        callbacks.Clear();
        ResultView.SetActive(false);
        ResultText.text = "";
    }
}
