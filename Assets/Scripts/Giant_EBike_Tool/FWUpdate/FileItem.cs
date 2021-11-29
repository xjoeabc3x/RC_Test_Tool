using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileItem : MonoBehaviour
{
    [SerializeField, Header("[��ܪ��ɦW]")]
    Text FileName;
    [Header("�����|�ɦW")]
    public string FullFileName;
    [SerializeField, Header("[�襤]")]
    Toggle tg;

    private void OnEnable()
    {
        tg.onValueChanged.AddListener(ToggleChanged);
        tg.group = DFUView.Instance.toggleGroup;
    }

    private void OnDisable()
    {
        tg.onValueChanged.RemoveListener(ToggleChanged);
    }

    private void ToggleChanged(bool isOn)
    {
        if (isOn)
        {
            DFUView.Instance.CurrentFile = FullFileName;
        }
    }

    public void SetTittle(string Name)
    {
        FileName.text = Name;
    }
}
