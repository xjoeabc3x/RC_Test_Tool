using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TuningType : MonoBehaviour
{
    [SerializeField, Header("[�L�ի��s]")]
    List<GameObject> tuningButtons = new List<GameObject>();
    [SerializeField, Header("[�L�ի��s��r]")]
    List<Text> tuningButtons_text = new List<Text>();
    [SerializeField, Header("[�L�ի��sImage]")]
    List<Image> tuningButtons_image = new List<Image>();
    [SerializeField, Header("[�襤���C��]")]
    Color SelectedColor;
    [SerializeField, Header("[���襤���C��]")]
    Color UnselectedColor;
    [SerializeField, Header("[��e�襤�����s]")]
    int currentChoose = 0;

    public void SetTuningButtonActive(int id, bool state)
    {
        tuningButtons[id].SetActive(state);
    }

    public void SetTuningButtonText(string btn1, string btn2, string btn3)
    {
        tuningButtons_text[0].text = btn1;
        tuningButtons_text[1].text = btn2;
        tuningButtons_text[2].text = btn3;
        if (btn1 == "-")
            SetTuningButtonActive(0, false);
        if (btn2 == "-")
            SetTuningButtonActive(1, false);
        if (btn3 == "-")
            SetTuningButtonActive(2, false);
    }

    public void ButtonChoosed(int buttonID)
    {
        for (int i = 0; i < tuningButtons_image.Count; i++)
        {
            tuningButtons_image[i].color = UnselectedColor;
            tuningButtons_image[i].raycastTarget = true;
        }
        tuningButtons_image[buttonID].color = SelectedColor;
        tuningButtons_image[buttonID].raycastTarget = false;
        currentChoose = buttonID;
    }

    public int GetCurrentChoose()
    {
        return currentChoose;
    }

    public void ResetButtons()
    {
        currentChoose = 0;
        SetTuningButtonText("-", "-", "-");
        for (int i = 0; i < tuningButtons_image.Count; i++)
        {
            tuningButtons_image[i].color = UnselectedColor;
            tuningButtons_image[i].raycastTarget = true;
            SetTuningButtonActive(i, true);
        }
    }
}
