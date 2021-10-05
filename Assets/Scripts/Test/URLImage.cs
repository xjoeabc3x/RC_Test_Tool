using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class URLImage : MonoBehaviour
{
    //https://images.giant-bicycles.com/b_white,c_pad,h_400,q_80/zrakybzapdsh0uevyoep/MY19-Fathom-E+-3-29er_Color-A.jpg

    [SerializeField]
    RawImage YourRawImage;

    void Start()
    {
        //StartCoroutine(DownloadImage("https://images.giant-bicycles.com/b_white,c_pad,h_400,q_80/zrakybzapdsh0uevyoep/MY19-Fathom-E+-3-29er_Color-A.jpg"));
        StartCoroutine(IETest());
    }

    IEnumerator IETest()
    {
        //UnityWebRequest request = new UnityWebRequest("https://middleware.giant-hpb.com/bike-informations/E2EA2701");
        var request = UnityWebRequest.Get("https://middleware.giant-hpb.com/bike-informations/E2EA2701");
        request.timeout = 10;
        //yield return request.SendWebRequest();
        yield return request.Send();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("-- ServerCommunicate Call Error : " + request.error);
        }
        else
        {
            // callback
            Debug.Log(request.downloadHandler.text);
        }
        request.Dispose();
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            YourRawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
    }
}
