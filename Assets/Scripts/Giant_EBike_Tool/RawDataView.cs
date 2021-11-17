using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class RawDataView : MonoBehaviour
{
    public static RawDataView Instance = null;
    [SerializeField, Header("[CallbackText Prefab]")]
    GameObject CallbackText_prefab;
    [SerializeField, Header("[CallbackText列表位置]")]
    RectTransform CallbackList_pos;
    private List<GameObject> CallbackObj_List = new List<GameObject>();
    [SerializeField, Header("[指令代號]")]
    InputField CMD_Input;
    [SerializeField, Header("[指令內容]")]
    InputField CMD_Internal_Input;
    [SerializeField, Header("[自訂義指令畫面]")]
    GameObject CustomCMD;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    public void ClearMSG()
    {
        foreach (GameObject obj in CallbackObj_List)
        {
            Destroy(obj);
        }
        CallbackObj_List.Clear();
    }

    public void AddEncodeCallback(string address, string data)
    {
        string input = string.Format("<color=black>[{0}] :{1}</color>", DateTime.Now, data);
        var newtext = Instantiate(CallbackText_prefab, CallbackList_pos);
        Text CallbackText = newtext.GetComponent<Text>();
        CallbackText.text = string.Format("{0}", input);
        newtext.transform.SetAsFirstSibling();
        CallbackObj_List.Add(newtext);
    }

    public void AddDecodeCallback(string address, string data)
    {
        string input = string.Format("<color=yellow>[{0}] :{1}</color>", DateTime.Now, data);
        var newtext = Instantiate(CallbackText_prefab, CallbackList_pos);
        Text CallbackText = newtext.GetComponent<Text>();
        CallbackText.text = string.Format("{0}", input);
        newtext.transform.SetAsFirstSibling();
        CallbackObj_List.Add(newtext);
        HomeManager.AddNewLog(address, string.Format("\n[{0}] :{1}", DateTime.Now, data));
    }

    public void SendCustomCMD()
    {
        if (!CheckHeader() || !CheckInternal())
            return;
        string[] setdata = CMD_Internal_Input.text.Split(',');
        if (string.IsNullOrEmpty(CMD_Internal_Input.text) || setdata.Length < 1)
            setdata = null;
        CommandManager.SendCustomCMD(ChoosedDeviceManager.DeviceAddress, CMD_Input.text, setdata);
        CustomCMD.SetActive(false);
    }

    private bool CheckHeader()
    {
        if (!int.TryParse(CMD_Input.text, NumberStyles.HexNumber, new CultureInfo("en-US"), out int result))
        {
            Toast.Instance.ShowToast("CMD Input error.");
            return false;
        }
        if (result < 0 || result > 255)
        {
            Toast.Instance.ShowToast("CMD Input error.");
            return false;
        }
        return true;
    }

    private bool CheckInternal()
    {
        if (!string.IsNullOrEmpty(CMD_Internal_Input.text))
        {
            string[] datas = CMD_Internal_Input.text.Split(',');
            for (int i = 0; i < datas.Length; i++)
            {
                if (!int.TryParse(datas[i], NumberStyles.HexNumber, new CultureInfo("en-US"), out int result))
                {
                    Toast.Instance.ShowToast("Internal input error.");
                    return false;
                }
                if (result < 0 || result > 255)
                {
                    Toast.Instance.ShowToast("Internal input error.");
                    return false;
                }
            }
        }
        return true;
    }

    public void ToUpper(InputField input)
    {
        input.text = input.text.ToUpper();
    }
}
