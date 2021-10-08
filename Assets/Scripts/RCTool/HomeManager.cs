using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    #region [宣告區]
    public static HomeManager Instance;
    [SerializeField, Header("[按鈕Prefab]")]
    GameObject ButtonPrefab;
    [SerializeField, Header("[按鈕擺放位置]")]
    RectTransform ButtonPos;

    Dictionary<string, GameObject> ButtonDic = new Dictionary<string, GameObject>();
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(this);
        }
        //註冊接收事件
        RCToolPlugin.onRceiveDevice += RCToolPlugin_onRceiveDevice;
        RCToolPlugin.onDeviceStatusChanged += RCToolPlugin_onDeviceStatusChanged;
    }

    #region --Functions--

    public void ButtonClicked(string address)
    {
        ChoosedDeviceManager.DeviceTittle = RCToolPlugin.GetDeviceName(address) + "|" + address;
        ChoosedDeviceManager.DeviceAddress = address;
        AppManager.Instance.SetPage("ChoosedDevice");
    }

    #endregion

    #region --Events--
    private void RCToolPlugin_onRceiveDevice(string address, string name, string rssi)
    {
        if (!ButtonDic.ContainsKey(address))
        {
            var NewButton = Instantiate(ButtonPrefab, ButtonPos);
            NewButton.GetComponent<DeviceButton>().SetButtonInfo(name + "|" + address + "|" + rssi, address);
            ButtonDic.Add(address, NewButton);
        }
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
    #endregion
    #region --Buttons--
    public void StartScan()
    {
        ClearButton();
        RCToolPlugin.StartScan();
    }

    private void ClearButton()
    {
        foreach (KeyValuePair<string, GameObject> obj in ButtonDic)
        {
            Destroy(obj.Value);
        }
        ButtonDic.Clear();
    }

    public void StopScan()
    {
        RCToolPlugin.StopScan();
    }
    #endregion
}
