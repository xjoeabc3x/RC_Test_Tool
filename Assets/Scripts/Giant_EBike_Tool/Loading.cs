using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public static Loading Instance = null;
    [SerializeField, Header("[Loading¾B¸n]")]
    GameObject LoadingMask;
    [SerializeField, Header("[Invoke Button]")]
    GameObject InvokeButton;
    [SerializeField, Header("[Invoke Button Text]")]
    Text InvokeButton_Text;

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
    private float delay = 10f;
    private void Update()
    {
        if (LoadingMask.activeInHierarchy)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime * 1;
            }
            else if (delay <= 0)
            {
                HideLoading();
                delay = 10f;
            }
        }
    }

    public void ShowLoading()
    {
        delay = 100000f;
        LoadingMask.SetActive(true);
    }

    public Action onClickButton;

    public void ShowLoading(Action ButtonEvent, string ButtonText)
    {
        delay = 100000f;
        LoadingMask.SetActive(true);
        onClickButton = ButtonEvent;
        InvokeButton_Text.text = (string.IsNullOrEmpty(ButtonText)) ? "" : ButtonText;
        InvokeButton.SetActive(true);
    }

    public void OnClickButton()
    {
        onClickButton?.Invoke();
        onClickButton = null;
        HideLoading();
    }

    public void ShowLoading(float TimeInSec)
    {
        delay = TimeInSec;
        LoadingMask.SetActive(true);
    }

    public void HideLoading()
    {
        LoadingMask.SetActive(false);
        InvokeButton.SetActive(false);
        InvokeButton_Text.text = "";
    }
}
