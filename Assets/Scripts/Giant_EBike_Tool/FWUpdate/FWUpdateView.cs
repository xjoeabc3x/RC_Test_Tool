using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FWUpdateView : MonoBehaviour
{
    [SerializeField, Header("[DFU]")]
    GameObject dfuview;

    public void ResetView()
    {
        dfuview.SetActive(false);
    }
}
