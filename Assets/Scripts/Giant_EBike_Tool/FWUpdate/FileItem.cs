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
        tg.group = GetTG();
    }

    private void OnDisable()
    {
        tg.onValueChanged.RemoveListener(ToggleChanged);
    }

    private void ToggleChanged(bool isOn)
    {
        if (isOn)
        {
            if (DFUView.Instance != null)
                DFUView.Instance.CurrentFile = DFUView.Instance.gameObject.activeInHierarchy ? FullFileName : DFUView.Instance.CurrentFile;
            if (L2View.Instance != null)
                L2View.Instance.CurrentFile = L2View.Instance.gameObject.activeInHierarchy ? FullFileName : L2View.Instance.CurrentFile;
            if (L1View.Instance != null)
                L1View.Instance.CurrentFile = L1View.Instance.gameObject.activeInHierarchy ? FullFileName : L1View.Instance.CurrentFile;
        }
    }

    private ToggleGroup GetTG()
    {
        if (DFUView.Instance != null && DFUView.Instance.gameObject.activeInHierarchy)
            return DFUView.Instance.toggleGroup;
        if (L2View.Instance != null && L2View.Instance.gameObject.activeInHierarchy)
            return L2View.Instance.toggleGroup;
        if (L1View.Instance != null && L1View.Instance.gameObject.activeInHierarchy)
            return L1View.Instance.toggleGroup;
        return null;
    }

    public void SetTittle(string Name)
    {
        FileName.text = Name;
    }
}
