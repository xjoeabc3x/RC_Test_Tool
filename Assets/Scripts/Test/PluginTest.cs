using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PluginTest : MonoBehaviour
{
    public static string ConnectedAddress = "";

    [SerializeField, Header("[Button Prefab]")]
    GameObject ButtonPrefab;
    [SerializeField, Header("[Button Pos]")]
    RectTransform ButtonPos;
    [SerializeField, Header("[Received Raw Data]")]
    Text EventText;

    Dictionary<string, GameObject> ButtonDic = new Dictionary<string, GameObject>();

    private void Awake()
    {
        RCToolPlugin.onRceiveDevice += RCToolPlugin_onRceiveDevice;
        RCToolPlugin.onDeviceStatusChanged += RCToolPlugin_onDeviceStatusChanged;
        RCToolPlugin.onReceiveEncodeRawData += RCToolPlugin_onReceiveRawData;
        RCToolPlugin.onReceiveDecodeRawData += RCToolPlugin_onReceiveRawData;
    }

    private void RCToolPlugin_onReceiveRawData(string address, string data)
    {
        EventText.text += address + "|" + data + "\n";
    }

    private void RCToolPlugin_onDeviceStatusChanged(string address, string status)
    {
        if (ButtonDic.ContainsKey(address))
        {
            DeviceButton deviceButton = ButtonDic[address].GetComponent<DeviceButton>();
            switch (status)
            {
                case "Connected":
                    deviceButton.SetConnectStatus(true);
                    break;
                default:
                    deviceButton.SetConnectStatus(false);
                    break;
            }
        }
    }

    private void RCToolPlugin_onRceiveDevice(string address, string name)
    {
        if (!ButtonDic.ContainsKey(address))
        {
            var NewButton = Instantiate(ButtonPrefab, ButtonPos);
            NewButton.GetComponent<DeviceButton>().SetButtonInfo(address + "|" + name, address);
            ButtonDic.Add(address, NewButton);
        }
    }

    public void Scan()
    {
        ClearButton();
        RCToolPlugin.StartScan();
    }

    public void ClearMessageText()
    {
        EventText.text = "";
    }

    private void ClearButton()
    {
        foreach (KeyValuePair<string, GameObject> obj in ButtonDic)
        {
            Destroy(obj.Value);
        }
        ButtonDic.Clear();
    }

    public void Send_02()
    {
        //if (string.IsNullOrEmpty(ConnectedAddress))
        //{
        //    return;
        //}

        CommandManager.SendCMD(ConnectedAddress, "02", null, null);
    }
}
