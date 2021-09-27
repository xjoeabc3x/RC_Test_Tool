using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using UnityEngine;

namespace ParseRCCallback
{
    public static class ParseCallBack
    {
        //package counter超過14byte計數(還剩下幾個package要接收)
        private static int packagecounter = 0;

        /// <summary>
        /// 解析RC/SG回傳訊息,!!!Notice:一定要先Decode!!!
        /// </summary>
        /// <param name="datas">Ex[05]:FC,21,05,0A,37,08,15,01,45,14,00,00,00,00,00,00,00,00,0A,97</param>
        /// <returns>各指令相對應結果</returns>
        public static string CallbackInfo(string address, string datas)
        {
            string[] data = datas.Split(',');
            if (data[0] == "FC")
            {
                if (data.Length >= 20)
                {
                    packagecounter = PackageCompute(data[3]);
                    switch (data[2])
                    {//{ "05", "09", "32", "30", "D4", "D1", "D2", "D3", "0D", "13", "0E", "37", "38", "39" }
                        case "05":
                            return address + "|05|" + Parse_05(datas);
                        case "09":
                            return address + "|09|" + Parse_09(datas);
                        case "0D":
                            return address + "|0D|" + Parse_0D(datas);
                        case "0E":
                            return address + "|0E|" + Parse_0E(datas);
                        case "13":
                            return address + "|13|" + Parse_13(datas);
                        case "30":
                            return address + "|30|" + Parse_30(datas);
                        case "32":
                            return address + "|32|" + Parse_32(datas);
                        case "37":
                            return address + "|37|" + Parse_37(datas);
                        case "38":
                            return address + "|38|" + Parse_38(datas);
                        case "39":
                            return address + "|39|" + Parse_39(datas);
                        case "D1":
                            return address + "|D1|" + Parse_D1(datas);
                        case "D2":
                            return address + "|D2|" + Parse_D2(datas);
                        case "D3":
                            return address + "|D3|" + Parse_D3(datas);
                        case "D4":
                            return address + "|D4|" + Parse_D4(datas);
                        default:
                            Debug.Log("Unknown TD.");
                            return null;
                    }
                }
                else
                {
                    switch (data[1])
                    {
                        case "":
                            break;
                        default:
                            Debug.Log("Unknown callback.");
                            break;
                    }
                }
            }
            return null;
        }

        #region [Parse]

        //Callback暫存
        public static string[] DataCache = null;

        /// <summary>
        /// 解析[05]RC/SG,UI韌體版本,UI硬體版本,ctg1,ctg2
        /// </summary>
        /// <param name="input"></param>
        /// <returns>RCType,UI_FirmwareVersion,UI_HardwareVersion,EV_ctg1,EV_ctg2</returns>
        private static string Parse_05(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            string[] datas = GetAES(input);
            string rc_type = GetRCType(BitChoose_string(datas[0], 0, 2) + BitChoose_string(datas[4], 0, 2));
            string ui_fw_ver = "2" + ByteToInt_string(datas[2], 3) + ByteToInt_string(datas[1], 2) + BitChoose_int(datas[0], 3, 7).ToString() + ByteToInt_string(datas[3], 3);
            string ui_hw_ver = string.Format("2{0}{1:00}{2:00000}", ByteToInt_string(datas[5], 3), BitChoose_int(datas[4], 4, 7), CombineByteToInt(datas[7] + "," + datas[6]));
            string ev_ctg1 = ByteToInt_string(datas[8], 3);
            string ev_ctg2 = ByteToInt_string(datas[9], 3);
            return string.Format("{0},{1},{2},{3},{4}", rc_type, ui_fw_ver, ui_hw_ver, ev_ctg1, ev_ctg2);
        }
        /// <summary>
        /// 解析[09]馬達型號,馬達韌體版本,馬達硬體版本,生產流水號,RCID
        /// </summary>
        /// <param name="input"></param>
        /// <param name="package"></param>
        /// <returns>DUType(DU7),DU_FWversion(0PB),DU_HWversion(X0PB),SN,RCID/evymc(50810)</returns>
        private static string Parse_09(string input)
        {
            string DUType = "";
            string DUFW = "";
            string DUHW = "";
            string SN = "";
            string evymc = "";
            switch (packagecounter)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //判斷馬達
                    //解析
                    DUFW = GetASCIItoString(DataCache, 0, 2);
                    DUHW = GetASCIItoString(DataCache, 8, 11);
                    SN = GetYamahaSN(DataCache[4], DataCache[5], DataCache[6], DataCache[7]);
                    evymc = GetRCID(DataCache);
                    switch (DUFW)
                    {
                        case "0SC":
                            DUType = "DU1";
                            break;
                        case "0PA":
                            DUType = "DU2";
                            break;
                        case "0SE":
                            DUType = "DU3";
                            break;
                        case "1RA":
                            DUType = "DU4";
                            break;
                        case "1RB":
                            //DU4,6
                            DUType = DU4orDU6(DataCache);
                            break;
                        case "1RC":
                            //DU4,6
                            DUType = DU4orDU6(DataCache);
                            break;
                        case "0SG":
                            DUType = "DU5";
                            break;
                        case "0PB":
                            DUType = "DU7";
                            break;
                        case "0PD":
                            DUType = "DU7";
                            break;
                        case "0PC":
                            DUType = "DU8";
                            break;
                        case "CAN":
                            DUType = "DU9";
                            break;
                        case "2SA":
                            DUType = "DU10";
                            break;
                        case "2SB":
                            DUType = "DU11";
                            break;
                        case "1RD":
                            DUType = "DU12";
                            break;
                        case "GR1":
                            DUType = "DU13";
                            break;
                        case "GR2":
                            DUType = "DU14";
                            break;
                        case "2YA":
                            DUType = "DU15";
                            break;
                        case "GR3":
                            DUType = "DU18";
                            break;
                        case "2SE":
                            DUType = "DU119";
                            break;
                        case "EP8": //Shimano
                            DUFW = "EP800";
                            DUType = "DU16";
                            SN = GetShimanoSN(DataCache[5], DataCache[6], DataCache[7]);
                            break;
                        default:
                            Debug.Log("Unknow DU_Firmware.");
                            break;
                    }
                    //清空暫存
                    DataCache = null;
                    //packagecounter歸零
                    packagecounter = 0;
                    return string.Format("{0},{1},{2},{3},{4}", DUType, DUFW, DUHW, SN, evymc);
                //第一包
                case 2:
                    //作暫存
                    DataCache = GetAES(input);
                    return "09_wait";
                default:
                    Debug.Log("[09] parse Error." + packagecounter);
                    return null;
            }
        }
        /// <summary>
        /// 解析[0D]電池Cell版本韌體版本生產流水號
        /// </summary>
        /// <param name="input"></param>
        /// <returns>minor,major,SN</returns>
        private static string Parse_0D(string input)
        {
            string[] aes = GetAES(input);
            string minor = "";
            switch (aes[0])
            {
                case "00":
                    minor = "PF";
                    break;
                case "01":
                    minor = "GA";
                    break;
                default:
                    minor = "Reserved";
                    break;
            }
            string major = ByteToInt_string(aes[1], 3);
            string SN = string.Format("2{0}{1}{2}{3:00000}", ByteToInt_string(aes[2], 3), ByteToInt_string(aes[3], 2), ByteToInt_string(aes[4], 2)
                , CombineByteToInt(aes[6] + "," + aes[5]));
            return string.Format("{0},{1},{2}", minor, major, SN);
        }
        /// <summary>
        /// 解析[0E]充電循環次數,充電次數,大電流放電比例100%
        /// </summary>
        /// <param name="input"></param>
        /// <returns>ccy,cchg,hrd</returns>
        private static string Parse_0E(string input)
        {
            string[] aes = GetAES(input);
            string ccy = CombineByteToInt(aes[1] + "," + aes[0]).ToString();
            string cchg = CombineByteToInt(aes[3] + "," + aes[2]).ToString();
            string hrd = string.Format("{0}%", (int)((((decimal)(int.Parse(ByteToInt_string(aes[4], 0))))/255)*100));
            return string.Format("{0},{1},{2}", ccy, cchg, hrd);
        }

        private static string Parse_13(string input)
        {
            return null;
        }

        private static string Parse_30(string input)
        {
            return null;
        }
        /// <summary>
        /// 解析[32]車架號碼ASCII to String
        /// </summary>
        /// <param name="input"></param>
        /// <returns>車架號碼</returns>
        private static string Parse_32(string input)
        {
            switch (packagecounter)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //解析
                    string result = "";
                    for (int i = 0; i < DataCache.Length; i++)
                    {
                        result += (char)(int.Parse(DataCache[i], NumberStyles.HexNumber));
                    }
                    //清空暫存
                    DataCache = null;
                    //packagecounter歸零
                    packagecounter = 0;
                    return result;
                //第一包
                case 2:
                    //作暫存
                    DataCache = GetAES(input);
                    return "32_wait";
                default:
                    return null;
            }
        }

        private static string Parse_37(string input)
        {
            return null;
        }

        private static string Parse_38(string input)
        {
            return null;
        }

        private static string Parse_39(string input)
        {
            return null;
        }
        /// <summary>
        /// 解析[D1]Remote-1版本
        /// </summary>
        /// <param name="input"></param>
        /// <returns>remote_type,fw_ver,hw_ver,pic_icon,pic_navi,pic_number,pic_eu_language,pic_tra_chinese,pic_sim_chinese,pic_jpn,pic_korea</returns>
        private static string Parse_D1(string input)
        {
            switch (packagecounter)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //解析
                    string remote_type = GetRemoteType(DataCache[4]);
                    string fw_ver = "";
                    string icon = ByteToInt_string(DataCache[9], 3);
                    string navi = ByteToInt_string(DataCache[10], 3);
                    string number = ByteToInt_string(DataCache[11], 3);
                    string eu_lan = ByteToInt_string(DataCache[12], 3);
                    string tra_chi = ByteToInt_string(DataCache[13], 3);
                    string sim_chi = ByteToInt_string(DataCache[14], 3);
                    string jpn = ByteToInt_string(DataCache[15], 3);
                    string korea = ByteToInt_string(DataCache[16], 3);
                    //fw_ver
                    string fwv = string.Format("2{0}{1}{2:00}{3}", ByteToInt_string(DataCache[2], 3), ByteToInt_string(DataCache[1], 2)
                        , BitChoose_int(DataCache[0], 3, 7), ByteToInt_string(DataCache[3], 3));
                    switch (remote_type)
                    {
                        case "Remote-CT":
                            fw_ver = string.Format("rmctywxxapp_{0}.bin", fwv);
                            break;
                        case "Remote-Sport":
                            fw_ver = string.Format("rmsptwxxapp_{0}.bin", fwv);
                            break;
                        case "Remote 2 in 1":
                            fw_ver = string.Format("rm2in1wxxapp_{0}.bin", fwv);
                            break;
                    }
                    //hw_ver
                    string hw_ver = string.Format("2{0}{1:00}{2:00000}", ByteToInt_string(DataCache[6], 3), BitChoose_int(DataCache[5], 4, 7)
                        , CombineByteToInt(DataCache[8] + "," + DataCache[7]));
                    //清空暫存
                    DataCache = null;
                    //packagecounter歸零
                    packagecounter = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", remote_type, fw_ver, hw_ver, icon, navi, number, eu_lan, tra_chi, sim_chi, jpn, korea);
                //第一包
                case 2:
                    //作暫存
                    DataCache = GetAES(input);
                    return "D1_wait";
                default:
                    return null;
            }
        }
        /// <summary>
        /// 解析[D2]Remote-2版本
        /// </summary>
        /// <param name="input"></param>
        /// <returns>remote_type,fw_ver,hw_ver,pic_icon,pic_navi,pic_number,pic_eu_language,pic_tra_chinese,pic_sim_chinese,pic_jpn,pic_korea</returns>
        private static string Parse_D2(string input)
        {
            switch (packagecounter)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //解析
                    string remote_type = GetRemoteType(DataCache[4]);
                    string fw_ver = "";
                    string icon = ByteToInt_string(DataCache[9], 3);
                    string navi = ByteToInt_string(DataCache[10], 3);
                    string number = ByteToInt_string(DataCache[11], 3);
                    string eu_lan = ByteToInt_string(DataCache[12], 3);
                    string tra_chi = ByteToInt_string(DataCache[13], 3);
                    string sim_chi = ByteToInt_string(DataCache[14], 3);
                    string jpn = ByteToInt_string(DataCache[15], 3);
                    string korea = ByteToInt_string(DataCache[16], 3);
                    //fw_ver
                    string fwv = string.Format("2{0}{1}{2:00}{3}", ByteToInt_string(DataCache[2], 3), ByteToInt_string(DataCache[1], 2)
                        , BitChoose_int(DataCache[0], 3, 7), ByteToInt_string(DataCache[3], 3));
                    switch (remote_type)
                    {
                        case "Remote ON/OFF":
                            fw_ver = string.Format("rmnfswxxdapp_{0}.bin", fwv);
                            break;
                    }
                    //hw_ver
                    string hw_ver = string.Format("2{0}{1:00}{2:00000}", ByteToInt_string(DataCache[6], 3), BitChoose_int(DataCache[5], 4, 7)
                        , CombineByteToInt(DataCache[8] + "," + DataCache[7]));
                    //清空暫存
                    DataCache = null;
                    //packagecounter歸零
                    packagecounter = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", remote_type, fw_ver, hw_ver, icon, navi, number, eu_lan, tra_chi, sim_chi, jpn, korea);
                //第一包
                case 2:
                    //作暫存
                    DataCache = GetAES(input);
                    return "D2_wait";
                default:
                    return null;
            }
        }
        /// <summary>
        /// 解析[D3]Display版本
        /// </summary>
        /// <param name="input">display_type,fw_ver,hw_ver,pic_icon,pic_navi,pic_number,pic_eu_language,pic_tra_chinese,pic_sim_chinese,pic_jpn,pic_korea</param>
        /// <returns></returns>
        private static string Parse_D3(string input)
        {
            switch (packagecounter)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //解析
                    string display_type = GetRemoteType(DataCache[4]);
                    string fw_ver = "";
                    string icon = ByteToInt_string(DataCache[9], 3);
                    string navi = ByteToInt_string(DataCache[10], 3);
                    string number = ByteToInt_string(DataCache[11], 3);
                    string eu_lan = ByteToInt_string(DataCache[12], 3);
                    string tra_chi = ByteToInt_string(DataCache[13], 3);
                    string sim_chi = ByteToInt_string(DataCache[14], 3);
                    string jpn = ByteToInt_string(DataCache[15], 3);
                    string korea = ByteToInt_string(DataCache[16], 3);
                    //fw_ver
                    string fwv = string.Format("2{0}{1}{2:00}{3}", ByteToInt_string(DataCache[2], 3), ByteToInt_string(DataCache[1], 2)
                        , BitChoose_int(DataCache[0], 3, 7), ByteToInt_string(DataCache[3], 3));
                    switch (display_type)
                    {
                        case "New Evo":
                            fw_ver = string.Format("dpevowxxdapp_{0}.bin", fwv);
                            break;
                    }
                    //hw_ver
                    string hw_ver = string.Format("2{0}{1:00}{2:00000}", ByteToInt_string(DataCache[6], 3), BitChoose_int(DataCache[5], 4, 7)
                        , CombineByteToInt(DataCache[8] + "," + DataCache[7]));
                    //清空暫存
                    DataCache = null;
                    //packagecounter歸零
                    packagecounter = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", display_type, fw_ver, hw_ver, icon, navi, number, eu_lan, tra_chi, sim_chi, jpn, korea);
                //第一包
                case 2:
                    //作暫存
                    DataCache = GetAES(input);
                    return "D3_wait";
                default:
                    return null;
            }
        }
        /// <summary>
        /// 解析[D4]SG相關配件是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns>DU,BATT,SBATT,RMO_1,RMO_2,DSP,S_FD,S_RD,S_SWitchShifter(int number)</returns>
        private static string Parse_D4(string input)
        {
            string[] aes = GetAES(input);
            string byte0 = BitChoose_string(aes[0], 0, 7);
            int byte1 = Convert.ToInt32(BitChoose_string(aes[1], 4, 7), 2);
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", byte0[7].ToString(), byte0[6].ToString()
                , byte0[5].ToString(), byte0[4].ToString(), byte0[3].ToString()
                , byte0[2].ToString(), byte0[1].ToString(), byte0[0].ToString()
                , byte1);
        }
        #endregion

        #region [Tools]
        /// <summary>
        /// 組合byte回傳int
        /// </summary>
        public static int CombineByteToInt(byte[] input)
        {
            if (input.Length <= 0)
                return -1;
            return CombineByteToInt(ByteArr_To_ByteString(input));
        }
        /// <summary>
        /// 組合byteString[]回傳int
        /// </summary>
        public static int CombineByteToInt(string[] input, int startIdx, int endIdx)
        {
            if (startIdx < 0 || startIdx > 7 || startIdx < endIdx || endIdx < 0 || endIdx > 7)
                return -1;
            int lenth = endIdx - startIdx + 1;
            string[] data = new string[lenth];
            for (int i = 0; i < lenth; i++)
            {
                data[i] = input[i + startIdx];
            }
            return CombineByteToInt(StringArr_To_String(data));
        }
        /// <summary>
        /// 以逗號為區隔的String組合byte回傳int
        /// </summary>
        public static int CombineByteToInt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return -1;
            //input.Replace(",", "");
            string[] cache = input.Split(',');
            if (cache.Length <= 1)
                return int.Parse(input, NumberStyles.HexNumber);
            string result = "";
            for (int i = 0; i < cache.Length; i++)
            {
                result += cache[i];
            }
            return int.Parse(result, NumberStyles.HexNumber);
        }
        /// <summary>
        /// 組合byte回傳int字串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="show">要顯示幾個位數</param>
        /// <returns></returns>
        public static string ByteToInt_string(string input, int show)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            //input.Replace(",", "");
            string[] cache = input.Split(',');
            if (cache.Length <= 1)
            {
                switch (show)
                {
                    case 1:
                        return string.Format("{0:0}", int.Parse(input, NumberStyles.HexNumber));
                    case 2:
                        return string.Format("{0:00}", int.Parse(input, NumberStyles.HexNumber));
                    case 3:
                        return string.Format("{0:000}", int.Parse(input, NumberStyles.HexNumber));
                    case 4:
                        return string.Format("{0:0000}", int.Parse(input, NumberStyles.HexNumber));
                    default:
                        return string.Format("{0}", int.Parse(input, NumberStyles.HexNumber));
                }
            }
            string result = "";
            for (int i = 0; i < cache.Length; i++)
            {
                result += cache[i];
            }
            switch (show)
            {
                case 2:
                    return string.Format("{0:00}", int.Parse(result, NumberStyles.HexNumber));
                case 3:
                    return string.Format("{0:000}", int.Parse(result, NumberStyles.HexNumber));
                case 4:
                    return string.Format("{0:0000}", int.Parse(result, NumberStyles.HexNumber));
                default:
                    return string.Format("{0:0}", int.Parse(input, NumberStyles.HexNumber));
            }
        }
        /// <summary>
        /// 把byte array轉為字串(0x5A -> 5A), 以逗號區隔
        /// </summary>
        public static string ByteArr_To_ByteString(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                Debug.LogError("bytes is null or Empty.");
                return null;
            }
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i == 0)
                {
                    str = bytes[0].ToString("X");
                }
                else
                {
                    str = string.Format("{0},{1:00}", str, bytes[i].ToString("X"));
                }
            }
            return str;
        }
        /// <summary>
        /// 轉換以逗號為區隔的byteString變成byte[]
        /// </summary>
        public static byte[] ByteString_To_ByteArr(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                Debug.Log("str IsNullOrEmpty.");
                return null;
            }
            string[] data = str.Split(',');
            byte[] bytes = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                bytes[i] = (byte)int.Parse(data[i], NumberStyles.HexNumber);
            }
            return bytes;
        }
        /// <summary>
        /// 轉換string[]變成以逗號為區隔的string
        /// </summary>
        public static string StringArr_To_String(string[] str)
        {
            if (str.Length <= 0)
            {
                Debug.Log("str IsNullOrEmpty.");
                return null;
            }
            string result = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (i == 0)
                    result = str[i];
                else
                    result += "," + str[i];
            }
            return result;
        }
        /// <summary>
        /// 提取所選範圍的bit(index = 0~7),TD文件Bit7對應到code中應為startIdx = 0
        /// </summary>
        public static string BitChoose_string(string input, int startIdx, int endIdx)
        {
            if (startIdx < 0 || startIdx > 7 || startIdx > endIdx || endIdx < 0 || endIdx > 7)
            {
                return null;
            }

            string s = Convert.ToString(int.Parse(input, NumberStyles.HexNumber), 2);
            if (s.Length < 8)
            {
                int c = 8 - s.Length;
                for (int j = 0; j < c; j++)
                {
                    s = string.Format("0{0}", s);
                }
            }
            string result = "";
            for (int i = startIdx; i <= endIdx; i++)
            {
                result += int.Parse(s[i].ToString());
            }
            return result;
        }
        /// <summary>
        /// 提取所選範圍的bit(index = 0~7),TD文件Bit7對應到code中應為startIdx = 0
        /// </summary>
        public static int BitChoose_int(string input, int startIdx, int endIdx)
        {
            if (startIdx < 0 || startIdx > 7 || startIdx > endIdx || endIdx < 0 || endIdx > 7)
            {
                return -1;
            }

            string s = Convert.ToString(int.Parse(input, NumberStyles.HexNumber), 2);
            if (s.Length < 8)
            {
                int c = 8 - s.Length;
                for (int j = 0; j < c; j++)
                {
                    s = string.Format("0{0}", s);
                }
            }
            string result = "";
            for (int i = startIdx; i <= endIdx; i++)
            {
                result += int.Parse(s[i].ToString());
            }
            return Convert.ToInt32(result, 2);
        }
        /// <summary>
        /// 判斷RCType
        /// </summary>
        public static string GetRCType(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;
            switch (code)
            {
                case "000000":
                    return "RideControl Charge";
                case "000001":
                    return "RideControl Charge S5";
                case "000010":
                    return "RideControl EVO JPN";
                case "000011":
                    return "RideControl ONE BLE";
                case "000100":
                    return "RideControl EVO";
                case "000101":
                    return "RideControl EVO Pro";
                case "000110":
                    return "RideControl EVO S5";
                case "000111":
                    return "RideControl EVO 45";
                case "001000":
                    return "RideControl ONE ANT+";
                case "001001":
                    return "RideControl ONE JPN";
                case "001010":
                    return "Smart Gateway 10Y";
                case "001011":
                    return "Smart Gateway 10S";
                case "001100":
                    return "Smart Gateway 10B";
                case "001101":
                    return "RideControl ONE SGP";
                case "001110":
                    return "RideControl ONE CHN";
                default:
                    return "Unrecognized type.";
            }
        }
        /// <summary>
        /// 提取有效數值(14個byte)
        /// </summary>
        public static string[] GetAES(string input)
        {
            string[] datas = input.Split(',');
            string[] aes = new string[14];
            for (int i = 0; i < aes.Length; i++)
            {
                aes[i] = datas[i + 4];
            }
            return aes;
        }
        /// <summary>
        /// 計算這個指令有幾個包
        /// </summary>
        /// <param name="hexstring">16進制字串</param>
        /// <returns></returns>
        private static int PackageCompute(string hexstring)
        {
            if (packagecounter <= 0)
            {
                if (string.IsNullOrEmpty(hexstring))
                    return 0;
                //byte to int
                int total = int.Parse(hexstring, NumberStyles.HexNumber);
                int result = total / 14;
                if (total % 14 != 0 && result != 0)
                    result += 1;
                return result;
            }
            if (packagecounter > 0)
                return (packagecounter -= 1);
            return 0;
        }
        /// <summary>
        /// 把Hex字串轉為ASCII字串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIdx"></param>
        /// <param name="endIdx"></param>
        /// <returns></returns>
        public static string GetASCIItoString(string[] input, int startIdx, int endIdx)
        {
            if (input.Length <= 0 || endIdx < startIdx || startIdx < 0 || endIdx < 0 || startIdx >= input.Length || endIdx >= input.Length)
                return null;
            List<byte> data = new List<byte>();
            for (int i = startIdx; i <= endIdx; i++)
            {
                data.Add((byte)(int.Parse(input[i], NumberStyles.HexNumber)));
            }
            return Encoding.ASCII.GetString(data.ToArray());
        }
        /// <summary>
        /// 輸入字串轉為byte array
        /// </summary>
        /// <param name="input"></param>
        public static byte[] GetStringToASCIIbyte(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            return Encoding.ASCII.GetBytes(input);
        }
        /// <summary>
        /// 判斷是DU4還DU6
        /// </summary>
        /// <param name="input">[09]callback(Yamaha)</param>
        /// <returns></returns>
        private static string DU4orDU6(string[] input)
        {
            string RCID = GetRCID(input);
            switch (RCID[RCID.Length - 2].ToString())
            {
                case "3":
                    return "DU4";
                case "5":
                    return "DU6";
            }
            return null;
        }
        /// <summary>
        /// 取得RCID
        /// </summary>
        private static string GetRCID(string[] input)
        {
            if (input.Length <= 0)
                return null;
            return int.Parse(input[14] + input[13], NumberStyles.HexNumber).ToString();
        }

        public static string GetYamahaSN(string hexYY, string hexMM, string hexDD, string sn)
        {
            
            return string.Format("20{0}{1}{2}{3}", hexYY, hexMM, hexDD, (char)int.Parse(sn, NumberStyles.HexNumber));
        }

        public static string GetShimanoSN(string svf1, string svf2, string svf3)
        {
            int s1 = int.Parse(svf1, NumberStyles.HexNumber);
            int s2 = int.Parse(svf2, NumberStyles.HexNumber);
            int s3 = int.Parse(svf3, NumberStyles.HexNumber);

            return string.Format("{0}.{1}.{2}", s1, s2, s3);
        }

        public static string[] AddNewDatas(string[] ori, string[] newdatas)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < ori.Length; i++)
            {
                result.Add(ori[i]);
            }
            for (int i = 0; i < newdatas.Length; i++)
            {
                result.Add(newdatas[i]);
            }
            return result.ToArray();
        }

        private static string GetRemoteType(string input)
        {
            switch (input)
            {
                case "03":
                    return "Remote-CT";
                case "04":
                    return "New Evo";
                case "05":
                    return "Remote-Sport";
                case "06":
                    return "Remote 2 in 1";
                case "07":
                    return "Remote ON/OFF";
                default:
                    return "Unknow remote type.";
            }
        }
        #endregion
    }
}
