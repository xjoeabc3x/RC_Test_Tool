using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    public static void SendCMD(string address, string CMD_Key, byte[] setdata, string[] _setdata)
    {
        switch (CMD_Key)
        {//"02", "05", "09", "32", "12", "0A", "D4", "D1", "D2", "D3", "0D", "13", "0E", "37", "38", "39"
            case "01":
                RCToolPlugin.SendData(address, cmd_01(), "01");
                break;
            case "02":
                RCToolPlugin.SendData(address, cmd_02(), "01");
                break;
            case "03":
                RCToolPlugin.SendData(address, cmd_03(), "01");
                break;
            case "05":
                RCToolPlugin.SendData(address, cmd_05(), "01");
                break;
            case "09":
                RCToolPlugin.SendData(address, cmd_09(), "01");
                break;
            case "0A":
                RCToolPlugin.SendData(address, cmd_0A(), "01");
                break;
            case "0B":
                RCToolPlugin.SendData(address, cmd_0B(), "01");
                break;
            case "0C":
                RCToolPlugin.SendData(address, cmd_0C(), "01");
                break;
            case "0D":
                RCToolPlugin.SendData(address, cmd_0D(), "01");
                break;
            case "0E":
                RCToolPlugin.SendData(address, cmd_0E(), "01");
                break;
            case "12":
                RCToolPlugin.SendData(address, cmd_12(), "01");
                break;
            case "13":
                RCToolPlugin.SendData(address, cmd_13(), "01");
                break;
            case "16":
                RCToolPlugin.SendData(address, cmd_16(), "01");
                break;
            case "19":
                RCToolPlugin.SendData(address, cmd_19(), "01");
                break;
            case "1A_normal":
                RCToolPlugin.SendData(address, cmd_1A_normal(), "01");
                break;
            case "1A_tuning":
                RCToolPlugin.SendData(address, cmd_1A_tuning(), "01");
                break;
            case "20":
                RCToolPlugin.SendData(address, cmd_20(), "01");
                break;
            case "21":
                RCToolPlugin.SendData(address, cmd_21(), "01");
                break;
            case "23ON":
                RCToolPlugin.SendData(address, cmd_23ON(), "00");
                break;
            case "23OFF":
                RCToolPlugin.SendData(address, cmd_23OFF(), "00");
                break;
            case "2C":
                RCToolPlugin.SendData(address, cmd_2C(), "01");
                break;
            case "2D":
                RCToolPlugin.SendData(address, cmd_2D(_setdata), "01");
                break;
            case "32":
                RCToolPlugin.SendData(address, cmd_32(), "01");
                break;
            case "33":
                RCToolPlugin.SendData(address, cmd_33_part1(setdata), "01");
                RCToolPlugin.SendData(address, cmd_33_part2(setdata), "01");
                break;
            case "34,04":
                RCToolPlugin.SendData(address, cmd_34_04(), "01");
                break;
            case "34,03":
                RCToolPlugin.SendData(address, cmd_34_03(), "01");
                break;
            case "37":
                RCToolPlugin.SendData(address, cmd_37(), "01");
                break;
            case "38":
                RCToolPlugin.SendData(address, cmd_38(), "01");
                break;
            case "39":
                RCToolPlugin.SendData(address, cmd_39(), "01");
                break;
            case "90":
                RCToolPlugin.SendData(address, cmd_90(), "01");
                break;
            case "91":
                RCToolPlugin.SendData(address, cmd_91(), "01");
                break;
            case "A0":
                RCToolPlugin.SendData(address, cmd_A0(), "01");
                break;
            case "A1":
                RCToolPlugin.SendData(address, cmd_A1(), "04");
                break;
            case "D1":
                RCToolPlugin.SendData(address, cmd_D1(), "01");
                break;
            case "D2":
                RCToolPlugin.SendData(address, cmd_D2(), "01");
                break;
            case "D3":
                RCToolPlugin.SendData(address, cmd_D3(), "01");
                break;
            case "D4":
                RCToolPlugin.SendData(address, cmd_D4(), "01");
                break;
            case "D9":
                RCToolPlugin.SendData(address, cmd_D9(), "01");
                break;
            case "DA":
                RCToolPlugin.SendData(address, cmd_DA(_setdata), "01");
                break;
            case "DD":
                RCToolPlugin.SendData(address, cmd_DD(), "01");
                break;
            case "DE":
                RCToolPlugin.SendData(address, cmd_DE(_setdata), "01");
                break;
            case "UIBLE_Version":
                RCToolPlugin.SendData(address, UIBle_Ver(), "00");
                break;
            default:
                Debug.Log("No such command key...");
                break;
        }
    }
    #region [指令集]
    /// <summary>
    /// Service Platform連線
    /// </summary>
    private static byte[] cmd_01()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }
    /// <summary>
    /// App藍牙連線
    /// </summary>
    private static byte[] cmd_02()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_03()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x03, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_05()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x05, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_09()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x09, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_0A()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x0A, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_0B()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x0B, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_0C()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x0C, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_0D()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x0D, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_0E()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x0E, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_12()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x12, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_13()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x13, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_16()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x16, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_19()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x19, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_1A_normal()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x1A, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_1A_tuning()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x1A, 0x01, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }
    /// <summary>
    /// Service Platform離線
    /// </summary>
    private static byte[] cmd_20()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x20, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }
    /// <summary>
    /// App藍牙離線
    /// </summary>
    private static byte[] cmd_21()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x21, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_23ON()
    {
        byte[] send_byte = new byte[4] { 0xFB, 0x22, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_23OFF()
    {
        byte[] send_byte = new byte[4] { 0xFB, 0x22, 0x00, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_2C()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x2C, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static string cmd_2D(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "2D", "03", "00", "00", "10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        string result = send_byte[0];
        for (int i = 1; i < send_byte.Length; i++)
        {
            result = string.Format("{0},{1}", result, send_byte[i]);
        }
        return result;
    }

    private static byte[] cmd_32()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x32, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_33_part1(byte[] input)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x33, 0x0E, 0x45, 0x41, 0x43, 0x54, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x00 };
        for (int i = 0; i < 14; i++)
        {
            send_byte[i + 4] = input[i];
        }
        return send_byte;
    }

    private static byte[] cmd_33_part2(byte[] input)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x33, 0x0E, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x00 };
        for (int i = 0; i < 4; i++)
        {
            send_byte[i + 4] = input[i + 14];
        }
        return send_byte;
    }

    private static byte[] cmd_34_03()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x34, 0x01, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_34_04()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x34, 0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_37()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x37, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_38()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x38, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_39()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x39, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_90()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x90, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_91()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x91, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_A0()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xA0, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_A1()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xA1, 0x04, 0x01, (byte)(500/256), (byte)(500%256), 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_D1()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xD1, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_D2()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xD2, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_D3()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xD3, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_D4()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xD4, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_D9()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xD9, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static string cmd_DA(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "DA", "08", "01", "01", "04", "07", "01", "64", "0A", "01", "00", "00", "00", "00", "00", "00", "01", "00" };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        string result = send_byte[0];
        for (int i = 1; i < send_byte.Length; i++)
        {
            result = string.Format("{0},{1}", result, send_byte[i]);
        }
        return result;
    }

    private static byte[] cmd_DD()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xDD, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static string cmd_DE(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "DE", "07", "04", "01", "02", "03", "04", "05", "06", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        string result = send_byte[0];
        for (int i = 1; i < send_byte.Length; i++)
        {
            result = string.Format("{0},{1}", result, send_byte[i]);
        }
        return result;
    }
    /// <summary>
    /// 藍牙韌體版本
    /// </summary>
    private static byte[] UIBle_Ver()
    {
        byte[] send_byte = new byte[3] { 0xFB, 0x11, 0x00 };
        return send_byte;
    }

    private static void Cmd_02()
    {
        //BitArray bar = new BitArray(new byte[3] { 0x00,0x00,0x00});

        //byte[] aes_data = new byte[20] { 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        //byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        //aes_data[0] = 0x02;
        //aes_data[1] = 0x01;
        //aes_data = RCToolPlugin.Encode(aes_data, 0x01);
        //for (int i = 0; i < 16; i++)
        //    send_byte[2 + i] = aes_data[i];
        //send_byte[18] = 0x01;
        //send_byte[19] = AES.creat_crc(send_byte, 19);
        //Debug.Log("On Unity encode:" + RCToolPlugin.ByteArr_To_ByteString(send_byte));
    }
    #endregion
}
