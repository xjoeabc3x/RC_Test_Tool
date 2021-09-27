using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceButton : MonoBehaviour
{
    [SerializeField, Header("[Tittle]")]
    Text ButtonTittle;
    [SerializeField, Header("[���sImage]")]
    Image ButtonBG;
    [SerializeField, Header("[�s�u�C��]")]
    Color ConnectedColor;
    [SerializeField, Header("[���u�C��]")]
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
