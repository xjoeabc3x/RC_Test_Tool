using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ServerSetting : ScriptableObject
{
    [SerializeField, Header("[ 配置的名稱 ]")]
    string m_ConfigName;
    [SerializeField, Header("[ DataBase 網址]")]
    string m_DataBase;
    [SerializeField, Header("[ AssetBundle 下載]")]
    string m_AssetBundle = "";
    [SerializeField, Header("[ API key ]")]
    string[] m_apiKeys;
    [SerializeField, Header("[ API 網址]")]
    string[] m_apiURLs;
    Dictionary<string, string> m_dicFullURL = new Dictionary<string, string>();
#if UNITY_EDITOR
    Object m_GoBackObject;
    public Object goBackObject { get { return m_GoBackObject; } }
#endif
    static ServerSetting m_instance;
    public static ServerSetting Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = Resources.Load("ServerSetting") as ServerSetting;
                if (m_instance == null)
                {
                    Debug.Log("***Create new AppSetting***");
                    // If not found, autocreate the asset object.
                    m_instance = CreateInstance<ServerSetting>();
                    #if UNITY_EDITOR
                    string csPath = "";
                    string folder = "";
                    string filePath = "";
                    // 取得AppSetting.cs的path
                    string[] guids = AssetDatabase.FindAssets("ServerSetting");
                    for (int i = 0; i < guids.Length; i++)
                    {
                        csPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                        if (csPath.Contains(".cs"))
                        {
                            folder = Path.Combine(Path.GetDirectoryName(csPath), "Resources");
                            filePath = Path.Combine(folder, "ServerSetting.asset");
                            break;
                        }
                    }
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    AssetDatabase.CreateAsset(m_instance, filePath);
                    AssetDatabase.Refresh();
                    #endif
                }
            }
            return m_instance;
        }
    }
    bool bRead = false;
    public string database
    {
        get
        {
            if (!bRead)
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                string path = Application.persistentDataPath;
                path = Path.Combine(path, "internal_data");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, "url.txt");
                if (File.Exists(path))
                {
                    StreamReader streamReader = File.OpenText(path);
                    m_DataBase = streamReader.ReadToEnd();
                }
                bRead = true;
#endif
            }
            return m_DataBase;
        }
#if UNITY_EDITOR
        set
        {
            m_DataBase = value;
        }
#endif
    }
    public string assetbundle
    {
        get
        {
            return m_AssetBundle;
        }
#if UNITY_EDITOR
        set
        {
            m_AssetBundle = value;
        }
#endif
    }
    public string configName
    {
        get
        {
            return m_ConfigName;
        }
#if UNITY_EDITOR
        set
        {
            m_ConfigName = value;
        }
#endif
    }
    public string GetAPI(string key)
    {
        string url = "";
        if(!m_dicFullURL.TryGetValue(key, out url))
        {
            if (TryGetAPIURL(key, out url))
            {
                url = Path.Combine(database, url);
                m_dicFullURL.Add(key, url);
            }
        }
        return url;
    }
    bool TryGetAPIURL(string key, out string url)
    {
        url = "";
        for(int i = 0; i < m_apiKeys.Length; i++)
        {
            if(key.Equals(m_apiKeys[i]))
            {
                url = m_apiURLs[i];
                return true;
            }
        }
        return false;
    }
    #if UNITY_EDITOR
    [MenuItem("Setting/Server")]
    static void MenuSelected()
    {
        Selection.activeObject = Instance;
    }
    public static void ExternalSelected(Object goBackObject)
    {
        Instance.m_GoBackObject = goBackObject;
        MenuSelected();
    }
    public void SetAPI(string[] keys, string[] urls)
    {
        m_apiKeys = keys;
        m_apiURLs = urls;
    }
    public void SetConfigName(string configName)
    {
        m_ConfigName = configName;
    }
    #endif
}
