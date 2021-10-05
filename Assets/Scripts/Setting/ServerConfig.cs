/*
 * Server 設定檔，可設定不同的配置檔，方便切換
 * 只在 Editor 時有作用
 */
 #if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerConfig : ScriptableObject
{
    public string database;
    public string assetbundle;
    public List<string> apiKeys = new List<string>();
    public List<string> apiUrls = new List<string>();
}
#endif
