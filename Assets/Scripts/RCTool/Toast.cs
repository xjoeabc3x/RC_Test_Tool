using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    public static Toast Instance = null;
    [SerializeField, Header("[Toast¾B¸n]")]
    GameObject ToastMask;
    [SerializeField, Header("[°T®§¤å¦r]")]
    Text InfoText;

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
        if (ToastMask.activeInHierarchy)
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

    public void ShowToast(string content)
    {
        delay = 2f;
        InfoText.text = content;
        ToastMask.SetActive(false);
        ToastMask.SetActive(true);
    }

    public void HideLoading()
    {
        InfoText.text = "-";
        ToastMask.SetActive(false);
    }
}
