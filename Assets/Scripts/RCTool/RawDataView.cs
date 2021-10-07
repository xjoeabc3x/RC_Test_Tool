using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawDataView : MonoBehaviour
{
    public static RawDataView Instance = null;
    [SerializeField, Header("[CallbackText Prefab]")]
    GameObject CallbackText_prefab;
    [SerializeField, Header("[CallbackText¦Cªí¦ì¸m]")]
    RectTransform CallbackList_pos;
    private List<GameObject> CallbackObj_List = new List<GameObject>();
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
    }

}
