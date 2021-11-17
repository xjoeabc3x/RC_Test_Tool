using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public static Loading Instance = null;
    [SerializeField, Header("[Loading¾B¸n]")]
    GameObject LoadingMask;
    //[SerializeField, Header("[°T®§¤å¦r]")]
    //Text InfoText;

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

    public void ShowLoading(float TimeInSec)
    {
        delay = TimeInSec;
        LoadingMask.SetActive(true);
        //StopCoroutine("_ShowLoading");
        //StartCoroutine("_ShowLoading", TimeInSec);
    }

    //private IEnumerator _ShowLoading(float TimeInSec)
    //{
    //    yield return new WaitForSecondsRealtime(TimeInSec);
    //    HideLoading();
    //}

    public void HideLoading()
    {
        LoadingMask.SetActive(false);
        //InfoText.text = "";
    }
}
