using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWebRequest 
{
    System.Action<ServerCommunicate.EState, ColdWebRequest> onStateChanged;
    public string key { private set; get; }
    public bool autoRemove { private set; get; }
    LitJson.JsonData m_Content;
    public LitJson.JsonData content
    {
        set
        {
            m_Content = value;
            isDone = true;
        }
        get
        {
            return m_Content;
        }
    }
    string m_Error;
    public string error
    {
        set
        {
            m_Error = value;
            isDone = true;
        }
        get
        {
            return m_Error;
        }
    }
    public bool isDone { private set; get; }
    public ColdWebRequest(string key, bool autoRemove, System.Action<ServerCommunicate.EState, ColdWebRequest> onStateChanged)
    {
        this.key = key;
        this.autoRemove = autoRemove;
        this.onStateChanged = onStateChanged;
        isDone = false;
    }
    public void ChangeState(ServerCommunicate.EState state)
    {
        onStateChanged?.Invoke(state, this);
    }
    public ColdWebRequest RegisterActionOnStateChanged(System.Action<ServerCommunicate.EState, ColdWebRequest> action)
    {
        onStateChanged -= action;
        onStateChanged += action;
        return this;
    }
    public ColdWebRequest UnRegisterActionOnStateChanged(System.Action<ServerCommunicate.EState, ColdWebRequest> action)
    {
        onStateChanged -= action;
        return this;
    }
}
