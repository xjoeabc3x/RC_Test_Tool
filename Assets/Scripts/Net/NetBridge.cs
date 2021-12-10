using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetBridge : MonoSingletonExtend<NetBridge>
{
    Dictionary<string, ColdWebRequest> dicColdWebRequest = new Dictionary<string, ColdWebRequest>();
    Dictionary<string, LitJson.JsonData> dicContent = new Dictionary<string, LitJson.JsonData>();
    string mUserAgent;
    public string userAgent
    {
        get
        {
            if (string.IsNullOrEmpty(mUserAgent))
            {
                /*
                 * "AppNum/Version (SystemInfo.operatingSystem; SystemInfo.deviceModel)"
                 * AppNum = app類型( 0-教學版, 1-個人版 )
                 * Version = 版本
                 * operatingSystem = OS版本
                 * deviceModel = 裝置
                 */
                mUserAgent = string.Format("0/{0} {1}; {2}", AppSetting.Instance.ver, SystemInfo.operatingSystem, SystemInfo.deviceModel);
                mUserAgent = mUserAgent.Replace("(", "&#40");
                mUserAgent = mUserAgent.Replace(")", "&#41");
            }
            return mUserAgent;
        }
    }
    /// <summary>
    /// token(登入後取得，跟server拿資料用的)
    /// </summary>
    /// <value>The token.</value>
    string mToken = "";
    public string token
    {
        set
        {
            mToken = value;
            // 設定token
            ServerCommunicate.Instance.RegisterHeader("Authorization", "Bearer " + mToken);
            //
            //LocalDataManager.Instance.SaveToken(value);
        }
        get
        {
            return mToken;
        }
    }
    public bool autoDelRequest = true;
    public ColdWebRequest CallServer( string keyURL, Dictionary<string, string> data, System.Action<ServerCommunicate.EState, ColdWebRequest> action)
    {
        Debug.Log(string.Format("<color=red>Call </color> <color=blue>({0})</color> - {1}", keyURL, ServerSetting.Instance.GetAPI(keyURL)));
        try
        {
            ColdWebRequest request = new ColdWebRequest(keyURL, autoDelRequest, action);
            if (dicColdWebRequest.ContainsKey(keyURL))
                dicColdWebRequest[keyURL] = request;
            else 
                dicColdWebRequest.Add(keyURL, request);
            ServerCommunicate.Instance.RegisterHeader("app-info", userAgent);
            ServerCommunicate.Instance.Call(keyURL, ServerSetting.Instance.GetAPI(keyURL), data, OnReceive);
            return request;
        }
        catch (UnityException ue)
        {
            Debug.LogException(ue);
            return null;
        }
    }
    void OnReceive(string id, ServerCommunicate.EState state, string content)
    {
        Debug.Log(string.Format("NetBridge Receive [ <color=blue>{0}</color> ({1}) ]: {2}", id, state, content));
        ColdWebRequest request;
        if (!dicColdWebRequest.TryGetValue(id, out request))
        {
            Debug.Log("Can't find ColdWebRequest");
            return;
        }
        switch (state)
        {
            case ServerCommunicate.EState.Start:
                break;
            case ServerCommunicate.EState.Fail:
                request.error = content;
                if (request.autoRemove)
                    dicColdWebRequest.Remove(id);
                break;
            case ServerCommunicate.EState.Success:
                try
                {
                    LitJson.JsonData jd = LitJson.JsonMapper.ToObject(content);
                    if (int.Parse(jd["status"].ToString()) == 1)
                    {
                        if (jd.Keys.Contains("token"))
                            token = jd["token"].ToString();
                        request.content = jd;
                    }
                    else
                    {
                        request.error = jd["code"].ToString();
                        Debug.Log("-- NetBridge : status 0, code : " + request.error);
                    }
                }
                catch
                {
                    request.error = content;
                }
                if (request.autoRemove)
                    dicColdWebRequest.Remove(id);
                break;
        }
        request.ChangeState(state);
    }
    List<string> m_ListRemoveKey = new List<string>();
    public void RemoveDoneRequest()
    {
        m_ListRemoveKey.Clear();
        foreach (KeyValuePair<string, ColdWebRequest> kv in dicColdWebRequest)
        {
            if (!kv.Value.isDone)
                continue;
            m_ListRemoveKey.Add(kv.Key);
        }
        for (int i = 0; i < m_ListRemoveKey.Count; i++)
            dicColdWebRequest.Remove(m_ListRemoveKey[i]);
    }
    public void ClearRequests()
    {
        dicColdWebRequest.Clear();
        ServerCommunicate.Instance.AbortAll();
    }
    public IEnumerable<ColdWebRequest> GetColdWebRequests()
    {
        foreach (KeyValuePair<string, ColdWebRequest> kv in dicColdWebRequest)
            yield return kv.Value;
    }
    public LitJson.JsonData GetContent(string id) 
    {
        return dicContent.ContainsKey(id) ? dicContent[id] : null;
    }
}
