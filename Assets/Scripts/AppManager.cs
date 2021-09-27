using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using ParseRCCallback;

public class AppManager : MonoSingletonExtend<AppManager>
{
    public delegate void Event_AndroidBackButton();
    public static event Event_AndroidBackButton AndroidBackButton;

    private static string[] askPermissions = new string[]
    { 
        Permission.CoarseLocation,
        Permission.FineLocation,
        Permission.ExternalStorageWrite,
    };

    protected override void OnAwake()
    {
        base.OnAwake();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1080, 1920, false);
        Permission.RequestUserPermissions(askPermissions);
        InitPages();
        //Debug.Log(ParseCallBack.CallbackInfo("test", "FC,21,32,12,61,62,63,64,65,66,67,68,69,6A,6B,6C,6D,6E,0E,93"));
        //Debug.Log(ParseCallBack.CallbackInfo("test", "FC,21,32,12,6F,70,71,72,00,00,00,00,00,00,00,00,00,00,04,3D"));
        //Debug.Log(string.Format("{0}", (int)((((decimal)83) / 255)*100)));
    }

    //private void Update()
    //{
    //    //監聽Android back button
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        AndroidBackButton();
    //    }
    //}

    #region --頁面切換管理--
    [SerializeField, Header("[管理的頁面]")]
    List<GameObject> ManagedPages_List = new List<GameObject>();
    Dictionary<string, GameObject> ManagedPages_Dic = new Dictionary<string, GameObject>();

    private void InitPages()
    {
        for (int i = 0; i < ManagedPages_List.Count; i++)
        {
            RegistPage(ManagedPages_List[i]);
        }
    }

    /// <summary>
    /// 註冊頁面
    /// </summary>
    public bool RegistPage(GameObject obj)
    {
        if (!ManagedPages_Dic.ContainsKey(obj.name))
        {
            ManagedPages_Dic.Add(obj.name, obj);
            Debug.Log("RegistPage :" + obj.name);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 取消註冊頁面
    /// </summary>
    public bool UnRegistPage(GameObject obj)
    {
        if (ManagedPages_Dic.ContainsKey(obj.name))
        {
            ManagedPages_Dic.Remove(obj.name);
            Debug.Log("UnRegistPage :" + obj.name);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 切換頁面
    /// </summary>
    public void SetPage(string pageName)
    {
        CloseAllPages();
        if (ManagedPages_Dic.ContainsKey(pageName))
        {
            ManagedPages_Dic[pageName].SetActive(true);
        }
    }
    /// <summary>
    /// 關閉所有頁面
    /// </summary>
    private void CloseAllPages()
    {
        foreach (KeyValuePair<string, GameObject> obj in ManagedPages_Dic)
        {
            obj.Value.SetActive(false);
        }
    }
    #endregion
}
