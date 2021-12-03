using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FWUpdateView : MonoBehaviour
{
    [SerializeField, Header("[DFU]")]
    GameObject dfuview;
    [SerializeField, Header("[L2]")]
    GameObject l2view;
    [SerializeField, Header("[L1]")]
    GameObject l1view;

    public void ResetView()
    {
        dfuview.SetActive(false);
        l2view.SetActive(false);
        l1view.SetActive(false);
    }
}
