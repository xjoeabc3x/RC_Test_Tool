using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
public class ServerCommunicate : MonoSingletonExtend<ServerCommunicate>
{
    public enum EState
    {
        Wait,
        Start,
        Success,
        Fail
    }
    class CallInfo
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        public string id;
        /// <summary>
        /// 網址
        /// </summary>
        public string url;
        /// <summary>
        /// 資料
        /// </summary>
        public Dictionary<string, string> data;
        /// <summary>
        /// 回呼 < 識別碼, 狀態, 資料 >
        /// </summary>
        public System.Action<string, EState, string> callback;
    }
    Dictionary<string, string> header = new Dictionary<string, string>();
    Queue<CallInfo> queueCall = new Queue<CallInfo>();
    Coroutine coroutineSend;
    UnityWebRequest m_Request;
    public void RegisterHeader(string key, string value) {
        if (header.ContainsKey(key))
            header[key] = value;
        else
            header.Add(key, value);
    }
    public void AbortAll()
    {
        queueCall.Clear();
        if (m_Request != null)
            m_Request.Abort();
    }
    public void Call(string id, string url, Dictionary<string, string> data, System.Action<string, EState, string> callback) 
    {
        // 沒有傳輸及暫存，直接處理
        if(coroutineSend == null && queueCall.Count == 0) 
        {
            coroutineSend = StartCoroutine(Send(id, url, data, callback));
        }
        else 
        {
            // 先暫存
            CallInfo info = new CallInfo();
            info.id = id;
            info.url = url;
            info.data = data;
            info.callback = callback;
            queueCall.Enqueue(info);
            // 沒有傳輸就處理暫存
            if(coroutineSend == null) {
                NextCall();
            }
        }
    }
    void NextCall() 
    {
        if (queueCall.Count > 0)
        {
            CallInfo info = queueCall.Dequeue();
            coroutineSend = StartCoroutine(Send(info.id, info.url, info.data, info.callback));
        }
        else
            coroutineSend = null;
    }

    IEnumerator Send(string id, string url, Dictionary<string, string> data, System.Action<string, EState, string> callback)
    {
        WWWForm form = new WWWForm();
        if(data != null) 
        {
            foreach (KeyValuePair<string, string> kv in data) 
            {
                form.AddField(kv.Key, kv.Value);
            }   
        }
        m_Request = UnityWebRequest.Post(url, form);
        m_Request.timeout = 10;
        if(header.Count > 0) 
        {
            foreach (KeyValuePair<string, string> kv in header)
                m_Request.SetRequestHeader(kv.Key, kv.Value);
        }
        // 通知開始
        callback?.Invoke(id, EState.Start, "");
        yield return m_Request.SendWebRequest();
        if (m_Request.isNetworkError)
        {
            Debug.LogError("-- ServerCommunicate Call Error : " + m_Request.error);
            if (callback != null)
                callback(id, EState.Fail, m_Request.error);
            
        }
        else
        {
            // callback
            if (callback != null)
                callback(id, EState.Success, m_Request.downloadHandler.text);
        }
        m_Request.Dispose();
        m_Request = null;
        coroutineSend = null;
        // call next
        NextCall();
    }
}
