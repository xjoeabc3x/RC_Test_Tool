using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using UnityEngine.Android;

public class FileTest : MonoBehaviour
{
    [SerializeField]
    Text FileList;

    private void Awake()
    {
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
    }

    public void FileSearch()
    {
        //DirectoryInfo di = new DirectoryInfo("/storage/emulated/0/FW");
        DirectoryInfo di = new DirectoryInfo("/storage/emulated/0");
        foreach (var fi in di.GetDirectories())
        {
            FileList.text += fi.FullName + "\n";
        }
    }

    public void FileSearch_File(bool IsExteranl)
    {
        byte[] bbb = GetStreamBytes();

        //dpevowxxapp_20200908000.bin
        Debug.Log(Application.persistentDataPath);
        FileList.text += Application.persistentDataPath + "|\n";
        //string line;
        //StreamReader theReader = new StreamReader(Path.Combine(Application.persistentDataPath, "dpevowxxapp_20200908000.bin"), Encoding.Default);
        //line = theReader.ReadToEnd();
        //theReader.Close();
        //Debug.Log(line.ToString());

        //TextAsset asset = Resources.Load("dpevowxxapp_20200908000") as TextAsset;
        //Stream s = new MemoryStream(asset.bytes);
        //BinaryReader br = new BinaryReader(s);
        //Debug.Log(br.ToString());
        if (IsExteranl)
        {
            DirectoryInfo di = new DirectoryInfo("/storage/emulated/0/FW/New EVO");
            FileList.text += di.GetFiles().Length + "|\n";
            foreach (var fi in di.GetFiles())
            {
                FileList.text += fi.Name + "|\n";
                FileList.text += fi.FullName + "|\n";
            }
        }
        else
        {
            DirectoryInfo di = new DirectoryInfo(Application.streamingAssetsPath);
            FileList.text += di.GetFiles().Length + "|\n";
            foreach (var fi in di.GetFiles())
            {
                FileList.text += fi.Name + "|\n";
                FileList.text += fi.FullName + "|\n";
            }
        }
    }

    public byte[] GetStreamBytes()
    {
        try
        {
            FileStream stream = new FileInfo(Path.Combine(Application.streamingAssetsPath, "dpevowxxapp_20200908000.bin")).OpenRead();
            byte[] buffer = new byte[stream.Length];
            //從流中讀取位元組塊並將該資料寫入給定緩衝區buffer中
            stream.Read(buffer, 0, System.Convert.ToInt32(stream.Length));  
            stream.Close();
            stream.Dispose();
            return buffer;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
        return null;
    }

    public void ClearMSG()
    {
        FileList.text = "";
    }
}
