using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceButton : MonoBehaviour
{
    [SerializeField, Header("[Tittle]")]
    Text ButtonTittle;
    [SerializeField, Header("[按鈕Image]")]
    Image ButtonBG;
    [SerializeField, Header("[連線顏色]")]
    Color ConnectedColor;
    [SerializeField, Header("[離線顏色]")]
    Color DisonnectColor;

    private string Address;

    public void SetButtonInfo(string tittle, string address)
    {
        ButtonTittle.text = tittle;
        Address = address;
    }

    public void SetConnectStatus(bool connected)
    {
        if (connected)
        {
            ButtonBG.color = ConnectedColor;
        }
        else
        {
            ButtonBG.color = DisonnectColor;
        }
    }

    public void ButtonClicked()
    {
        HomeManager.Instance.ButtonClicked(Address);
    }
}
