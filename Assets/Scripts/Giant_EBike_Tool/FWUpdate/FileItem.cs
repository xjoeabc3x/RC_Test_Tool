using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileItem : MonoBehaviour
{
    [SerializeField, Header("[顯示的檔名]")]
    Text FileName;
    [Header("全路徑檔名")]
    public string FullFileName;
    [SerializeField, Header("[選中]")]
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
