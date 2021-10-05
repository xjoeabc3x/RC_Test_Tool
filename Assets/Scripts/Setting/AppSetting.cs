using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
public class AppSetting : ScriptableObject
{
    [SerializeField] string mVer;
    public string ver { get { return mVer; } }
    [SerializeField] string mAppName;
    public string appName { get { return mAppName; } }
    [SerializeField] SystemLanguage mLanguage = SystemLanguage.ChineseSimplified;
    public SystemLanguage language { get { return mLanguage; } }
    public string folderLanguage { get { return GetLanguage(language); } }
    [SerializeField] bool m_MultipleConnect = true;
    public bool multipleConnect { get { return m_MultipleConnect; } }
	[SerializeField] bool m_EnableLog = true;
	public bool EnableLog { set { m_EnableLog = value; } get { return m_EnableLog; } }
    [SerializeField] string m_EditorAssetLoadPath;
    public string editorAssetLoadPath { get { return m_EditorAssetLoadPath; } }
#if UNITY_EDITOR && UNITY_ANDROID
    [SerializeField] string m_keystorePath;
    [SerializeField] string m_keystorePass;
    [SerializeField] string m_keyaliasName;
    [SerializeField] string m_keyaliasPass;
    [SerializeField] string m_PackageName;
#endif
    public string fileName 
    {
        get
        {
            #if UNITY_ANDROID
            return string.Concat("RC_", mVer.Replace(".", "-"), ".apk");
            #endif
            return "";
        }
    }

    static AppSetting mInstance;
    public static AppSetting Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = Resources.Load("AppSetting") as AppSetting;
                if (mInstance == null)
                {
                    Debug.Log("***Create new AppSetting***");
                    // If not found, autocreate the asset object.
                    mInstance = CreateInstance<AppSetting>();
#if UNITY_EDITOR
                    string csPath = "";
                    string folder = "";
                    string filePath = "";
                    // 取得AppSetting.cs的path
                    string[] guids = AssetDatabase.FindAssets("AppSetting");
                    for (int i = 0; i < guids.Length; i++)
                    {
                        csPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                        if (csPath.Contains(".cs"))
                        {
                            folder = Path.Combine(Path.GetDirectoryName(csPath), "Resources");
                            filePath = Path.Combine(folder, "AppSetting.asset");
                            break;
                        }
                    }
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);
                    AssetDatabase.CreateAsset(mInstance, filePath);
                    AssetDatabase.Refresh();
#endif
                }
            }
            return mInstance;
        }
    }

    public static string GetLanguage(SystemLanguage sl, int count = 0)
    {
        switch (sl)
        {
            case SystemLanguage.English:
                return "en-US";
            case SystemLanguage.ChineseSimplified:
                return "zh-Hans";
            case SystemLanguage.ChineseTraditional:
                return "zh-Hant";
            case SystemLanguage.Unknown:
                if (count >= 1)
                    return "zh-Hans";
                return GetLanguage(Application.systemLanguage, 1);
            default:
                return "zh-Hans";
        }

    }

#if UNITY_EDITOR
    [MenuItem("Setting/App")]
    static void AppSettingSelected()
    {
        Selection.activeObject = Instance;
    }
#endif
}
