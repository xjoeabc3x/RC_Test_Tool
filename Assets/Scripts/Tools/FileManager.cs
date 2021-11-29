using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : MonoSingletonExtend<FileManager>
{
    private List<IEnumerator> starters = new List<IEnumerator>();

    public void CopyFile_StreamingToPersist(string[] sourcePath, string[] destDir)
    {
        if (sourcePath == null || sourcePath.Length < 1)
            return;
        StartCoroutine(_CopyFile_StreamingToPersist(sourcePath, destDir));
    }

    private IEnumerator _CopyFile_StreamingToPersist(string[] sourcePath, string[] destDir)
    {
        foreach (string de in destDir)
        {
            if (string.IsNullOrEmpty(de))
                continue;
            if (!Directory.Exists(de))
            {
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, de));
            }
        }

        foreach (string fi in sourcePath)
        {
            if (string.IsNullOrEmpty(fi))
                continue;
            UnityWebRequest request = new UnityWebRequest(Path.Combine(Application.streamingAssetsPath, fi));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = 5;
            yield return request.SendWebRequest();
            byte[] datas = request.downloadHandler.data;
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, fi), datas);
            request.Dispose();
        }
    }
}
