using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParseRCCallback;

public class DiagnoseView : MonoBehaviour
{
    [SerializeField, Header("[RC]")]
    Text RC_result;
    [SerializeField, Header("[DU]")]
    Text DU_result;
    [SerializeField, Header("[EP]")]
    Text EP_result;

    private void OnEnable()
    {
        HomeManager.RegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
    }

    private void ParseCallBack_onReceiveDecodeParsedData(string callback)
    {
        if (!string.IsNullOrEmpty(callback))
        {
            string Key = callback.Split('|')[1];
            string Value = callback.Split('|')[2];
            if (Key == "15")
            {
                Parse_15(Value);
                Handheld.Vibrate();
            }
            if (Key == "16")
            {
                Parse_16(Value);
                Handheld.Vibrate();
            }
            if (Key == "17")
            {
                Parse_17(Value);
                Handheld.Vibrate();
            }
            //Handheld.Vibrate();
        }
    }

    private void OnDisable()
    {
        HomeManager.UnRegistDecodeEvent(ParseCallBack_onReceiveDecodeParsedData);
    }

    public void StartDiagnose()
    {
        if (!RCToolPlugin.IsConnected(ChoosedDeviceManager.DeviceAddress))
        {
            Toast.Instance.ShowToast("Device is not connected.");
            return;
        }
        RC_result.text = "Ride Control[15] : null";
        DU_result.text = "Drive Unit[16] : null";
        EP_result.text = "Battery[17] : null";
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "15", null, null);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "16", null, null);
        CommandManager.SendCMD(ChoosedDeviceManager.DeviceAddress, "17", null, null);
        Loading.Instance.ShowLoading(2.0f);
    }

    private void Parse_15(string input)
    {
        RC_result.text = (input == "20") ? string.Format("Ride Control[15] : OK({0})", input) : string.Format("Ride Control Error[15] : 0x{0}", input);
    }

    private void Parse_16(string input)
    {
        DU_result.text = (input == "60") ? string.Format("Drive Unit[16] : OK({0})", input) : string.Format("Drive Unit Error[16] : 0x{0}", input);
    }

    private void Parse_17(string input)
    {
        EP_result.text = (input == "C0") ? string.Format("Battery[17] : OK({0})", input) : string.Format("Battery Error[17] : 0x{0}", input);
    }

}
