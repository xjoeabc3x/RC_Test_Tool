using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
            case "04":
                RCToolPlugin.SendData(address, cmd_04(_setdata), "01");
                break;
            case "05":
                RCToolPlugin.SendData(address, cmd_05(), "01");
                break;
            case "06":
                RCToolPlugin.SendData(address, cmd_06(), "01");
                break;
            case "07":
                RCToolPlugin.SendData(address, cmd_07(), "01");
                break;
            case "08":
                RCToolPlugin.SendData(address, cmd_08(), "01");
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
            case "0F":
                RCToolPlugin.SendData(address, cmd_0F(), "01");
                break;
            case "10":
                RCToolPlugin.SendData(address, cmd_10(), "01");
                break;
            case "11":
                RCToolPlugin.SendData(address, cmd_11(), "01");
                break;
            case "12":
                RCToolPlugin.SendData(address, cmd_12(), "01");
                break;
            case "13":
                RCToolPlugin.SendData(address, cmd_13(), "01");
                break;
            case "15":
                RCToolPlugin.SendData(address, cmd_15(), "01");
                break;
            case "16":
                RCToolPlugin.SendData(address, cmd_16(), "01");
                break;
            case "17":
                RCToolPlugin.SendData(address, cmd_17(), "01");
                break;
            case "19":
                RCToolPlugin.SendData(address, cmd_19(), "01");
                break;
            case "1A_normal":
                RCToolPlugin.SendData(address, cmd_1A_normal(), "01");
                break;
            case "1A_update":
                RCToolPlugin.SendData(address, cmd_1A_update(), "01");
                break;
            case "1A_tuning":
                RCToolPlugin.SendData(address, cmd_1A_tuning(), "01");
                break;
            case "1A_service":
                RCToolPlugin.SendData(address, cmd_1A_service(), "01");
                break;
            case "1A_navigation":
                RCToolPlugin.SendData(address, cmd_1A_navigation(), "01");
                break;
            case "1A_workout":
                RCToolPlugin.SendData(address, cmd_1A_workout(), "01");
                break;
            case "1D":
                RCToolPlugin.SendData(address, cmd_1D(_setdata), "01");
                break;
            case "1E":
                RCToolPlugin.SendData(address, cmd_1E(setdata), "01");
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
            case "28":
                RCToolPlugin.SendData(address, cmd_28(), "01");
                break;
            case "2B":
                RCToolPlugin.SendData(address, cmd_2B(_setdata), "01");
                break;
            case "2C":
                RCToolPlugin.SendData(address, cmd_2C(), "01");
                break;
            case "2D":
                RCToolPlugin.SendData(address, cmd_2D(_setdata), "01");
                break;
            case "2E":
                RCToolPlugin.SendData(address, cmd_2E_part1(_setdata), "01");
                RCToolPlugin.SendData(address, cmd_2E_part2(_setdata), "01");
                break;
            case "30":
                RCToolPlugin.SendData(address, cmd_30(), "01");
                break;
            case "31":
                RCToolPlugin.SendData(address, cmd_31(_setdata), "01");
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
            case "35":
                RCToolPlugin.SendData(address, cmd_35(), "01");
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
            case "3A":
                RCToolPlugin.SendData(address, cmd_3A(), "01");
                break;
            case "3B":
                RCToolPlugin.SendData(address, cmd_3B(), "01");
                break;
            case "40":
                RCToolPlugin.SendData(address, cmd_40(setdata), "01");
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
            case "B1":
                RCToolPlugin.SendData(address, cmd_B1(), "01");
                break;
            case "B2":
                RCToolPlugin.SendData(address, cmd_B2(setdata), "01");
                break;
            case "B3":
                RCToolPlugin.SendData(address, cmd_B3(), "01");
                break;
            case "B4":
                RCToolPlugin.SendData(address, cmd_B4(setdata), "01");
                break;
            case "C0":
                RCToolPlugin.SendData(address, cmd_C0(), "01");
                break;
            case "C1":
                RCToolPlugin.SendData(address, cmd_C1(setdata), "01");
                break;
            case "C2":
                RCToolPlugin.SendData(address, cmd_C2(), "01");
                break;
            case "C3":
                RCToolPlugin.SendData(address, cmd_C3(_setdata), "01");
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
            case "D5":
                RCToolPlugin.SendData(address, cmd_D5(), "01");
                break;
            case "D6":
                RCToolPlugin.SendData(address, cmd_D6(setdata), "01");
                break;
            case "D7":
                RCToolPlugin.SendData(address, cmd_D7(), "01");
                break;
            case "D8":
                RCToolPlugin.SendData(address, cmd_D8(setdata), "01");
                break;
            case "D9":
                RCToolPlugin.SendData(address, cmd_D9(), "01");
                break;
            case "DA":
                RCToolPlugin.SendData(address, cmd_DA(_setdata), "01");
                break;
            case "DB":
                RCToolPlugin.SendData(address, cmd_DB(), "01");
                break;
            case "DC":
                RCToolPlugin.SendData(address, cmd_DC(setdata), "01");
                break;
            case "DD":
                RCToolPlugin.SendData(address, cmd_DD(), "01");
                break;
            case "DE":
                RCToolPlugin.SendData(address, cmd_DE(_setdata), "01");
                break;
            case "DF":
                RCToolPlugin.SendData(address, cmd_DF(setdata), "01");
                break;
            case "E0":
                RCToolPlugin.SendData(address, cmd_E0(_setdata), "01");
                break;
            case "UIBLE_Version":
                RCToolPlugin.SendData(address, UIBle_Ver(), "00");
                break;
            default:
                Debug.Log("No such command key...");
                break;
        }
    }

    public static void SendCustomCMD(string address, string CMD_Key, string[] _setdata)
    {
        if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(CMD_Key) || !int.TryParse(CMD_Key, NumberStyles.HexNumber, new CultureInfo("en-US"), out int i))
            return;
        RCToolPlugin.SendData(address, cmd_custom(CMD_Key, _setdata), "01");
    }

    #region [指令集]

    private static string cmd_custom(string header, string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", header, "01", "00", "00", "08", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
        if (data != null)
        {
            send_byte[3] = data.Length.ToString("X2");
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

    private static string cmd_04(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "04", "0E", "19", "86", "08", "1C", "03", "00", "00", "00", "00", "00", "45", "14", "00", "00", "01", "00" };
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

    private static byte[] cmd_05()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x05, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_06()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x06, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_07()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x07, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_08()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x08, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
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

    private static byte[] cmd_0F()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x0F, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_10()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_11()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x11, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
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

    private static byte[] cmd_15()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x15, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_16()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x16, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_17()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x17, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
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

    private static byte[] cmd_1A_update()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x1A, 0x01, 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_1A_service()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x1A, 0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_1A_navigation()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x1A, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_1A_workout()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x1A, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static string cmd_1D(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "1D", "08", "01", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
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

    private static byte[] cmd_1E(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x1E, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
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

    private static byte[] cmd_28()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x28, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static string cmd_2B(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "2B", "0E", "00", "00", "10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
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

    private static string cmd_2E_part1(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "2E", "13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
        if (data != null)
        {
            for (int i = 0; i < 14; i++)
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

    private static string cmd_2E_part2(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "2E", "13", "01", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
        if (data != null)
        {
            for (int i = 0; i < 5; i++)
            {
                send_byte[i + 4] = data[i + 14];
            }
        }
        string result = send_byte[0];
        for (int i = 1; i < send_byte.Length; i++)
        {
            result = string.Format("{0},{1}", result, send_byte[i]);
        }
        return result;
    }

    private static byte[] cmd_30()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x30, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static string cmd_31(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "31", "04", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
        if (data != null)
        {
            for (int i = 0; i < 5; i++)
            {
                send_byte[i + 4] = data[i + 14];
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

    private static byte[] cmd_33_part1(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x33, 0x0E, 0x45, 0x41, 0x43, 0x54, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < 14; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        return send_byte;
    }

    private static byte[] cmd_33_part2(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x33, 0x0E, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < 4; i++)
            {
                send_byte[i + 4] = data[i + 14];
            }
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

    private static byte[] cmd_35()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x35, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
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

    private static byte[] cmd_3A()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x3A, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_3B()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x3B, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_40(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0x40, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
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

    private static byte[] cmd_B1()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xB1, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_B2(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xB2, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        return send_byte;
    }

    private static byte[] cmd_B3()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xB3, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_B4(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xB4, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        return send_byte;
    }

    private static byte[] cmd_C0()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xC0, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_C1(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xC1, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        return send_byte;
    }

    private static byte[] cmd_C2()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xC2, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static string cmd_C3(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "C3", "02", "19", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
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

    private static byte[] cmd_D5()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xD5, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_D6(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xD6, 0x0C, 0x12, 0x1B, 0x1C, 0x12, 0x11, 0x10, 0x12, 0x13, 0x20, 0x12, 0x14, 0x1B, 0x00, 0x00, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        return send_byte;
    }

    private static byte[] cmd_D7()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xD7, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_D8(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xD8, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
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

    private static byte[] cmd_DB()
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xDB, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        return send_byte;
    }

    private static byte[] cmd_DC(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xDC, 0x02, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        return send_byte;
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

    private static byte[] cmd_DF(byte[] data)
    {
        byte[] send_byte = new byte[20] { 0xFB, 0x21, 0xDF, 0x02, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00 };
        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                send_byte[i + 4] = data[i];
            }
        }
        return send_byte;
    }

    private static string cmd_E0(string[] data)
    {
        string[] send_byte = new string[20] { "FB", "21", "E0", "08", "01", "01", "00", "00", "0", "0", "0", "00", "00", "00", "00", "00", "00", "00", "01", "00" };
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
