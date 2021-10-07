using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKey : MonoBehaviour
{
    [SerializeField, Header("[����Key]")]
    public string Key = "";

    private void Start()
    {
        if (string.IsNullOrEmpty(Key))
            Key = name;
    }
}
