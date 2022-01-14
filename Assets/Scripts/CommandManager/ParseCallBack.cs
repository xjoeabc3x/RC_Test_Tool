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
        //private static int packagecounter = 0;
        private class DevicePakCount
        {
            //09
            public int i09 = 0;
            //0A
            public int i0A = 0;
            //32
            public int i32 = 0;
            //D1
            public int iD1 = 0;
            //D2
            public int iD2 = 0;
            //D3
            public int iD3 = 0;
            //DB
            public int iDB = 0;
        }

        public class DeviceDataCache
        {
            public string[] s09 = new string[] { };
            public string[] s0A = new string[] { };
            public string[] s32 = new string[] { };
            public string[] sD1 = new string[] { };
            public string[] sD2 = new string[] { };
            public string[] sD3 = new string[] { };
            public string[] sDB = new string[] { };
        }

        private static Dictionary<string, DevicePakCount> packagecounter = new Dictionary<string, DevicePakCount>();

        public static string Parse_Decode(string address, string datas)
        {
            string callback = CallbackInfo(address, datas);
            if (!string.IsNullOrEmpty(callback) && !callback.EndsWith("wait") && callback.Split('|').Length > 2)
            {
                Debug.Log("Parse_Decode :" + callback);
                return callback;
            }
            return null;
        }

        public static string Parse_Encode(string address, string datas)
        {
            string callback = CallbackInfo_Raw(address, datas);
            if (!string.IsNullOrEmpty(callback) && !callback.EndsWith("wait") && callback.Split('|').Length > 2)
            {
                Debug.Log("Parse_Encode :" + callback);
                return callback;
            }
            return null;
        }

        /// <summary>
        /// 解析RC/SG回傳訊息,!!!Notice:一定要先Decode!!!
        /// </summary>
        /// <param name="datas">Ex[05]:FC,21,05,0A,37,08,15,01,45,14,00,00,00,00,00,00,00,00,0A,97</param>
        /// <returns>各指令相對應結果</returns>
        private static string CallbackInfo(string address, string datas)
        {
            CheckDataCache(address);
            //Debug.Log(string.Format("OnUnity CallbackInfo :{0}|||{1}", address, datas));
            string[] data = datas.Split(',');
            if (data[0] == "FC")
            {
                if (data.Length >= 20 && data[1] == "21")
                {
                    packagecounter[address] = PackageCompute(address, data[2], data[3]);
                    switch (((byte)int.Parse(data[2], NumberStyles.HexNumber)) & 0xFF)
                    {//{ "02", "05", "09", "32", "12", "0A", "D4", "D1", "D2", "D3", "0D", "13", "0E", "37", "38", "39", "DD" }
                        case 0x01:
                            return address + "|01|" + Parse_01(datas);
                        case 0x02:
                            return address + "|02|" + Parse_02(datas);
                        case 0x03:
                            return address + "|03|" + Parse_03(datas);
                        case 0x04:
                            return address + "|04|" + Parse_04(datas);
                        case 0x05:
                            return address + "|05|" + Parse_05(datas);
                        case 0x09:
                            return address + "|09|" + Parse_09(address, datas);
                        case 0x0A:
                            return address + "|0A|" + Parse_0A(address, datas);
                        case 0x0C:
                            return address + "|0C|" + Parse_0C(datas);
                        case 0x0D:
                            return address + "|0D|" + Parse_0D(datas);
                        case 0x0E:
                            return address + "|0E|" + Parse_0E(datas);
                        case 0x12:
                            return address + "|12|" + Parse_12(datas);
                        case 0x13:
                            return address + "|13|" + Parse_13(datas);
                        case 0x15:
                            return address + "|15|" + Parse_15(datas);
                        case 0x16:
                            return address + "|16|" + Parse_16(datas);
                        case 0x17:
                            return address + "|17|" + Parse_17(datas);
                        case 0x1A:
                            return address + "|1A|" + Parse_1A(datas);
                        case 0x2C:
                            return address + "|2C|" + Parse_2C(datas);
                        case 0x2D:
                            return address + "|2D|" + Parse_2D(datas);
                        case 0x30:
                            return address + "|30|" + Parse_30(datas);
                        case 0x32:
                            return address + "|32|" + Parse_32(address, datas);
                        case 0x33:
                            return address + "|33|" + Parse_33(datas);
                        case 0x37:
                            return address + "|37|" + Parse_37(datas);
                        case 0x38:
                            return address + "|38|" + Parse_38(datas);
                        case 0x39:
                            return address + "|39|" + Parse_39(datas);
                        case 0x40:
                            return address + "|40|" + Parse_40(datas);
                        case 0xC0:
                            return address + "|C0|" + Parse_C0(datas);
                        case 0xC1:
                            return address + "|C1|" + Parse_C1(datas);
                        case 0xD1:
                            return address + "|D1|" + Parse_D1(address, datas);
                        case 0xD2:
                            return address + "|D2|" + Parse_D2(address, datas);
                        case 0xD3:
                            return address + "|D3|" + Parse_D3(address, datas);
                        case 0xD4:
                            return address + "|D4|" + Parse_D4(datas);
                        case 0xD5:
                            return address + "|D5|" + Parse_D5(datas);
                        case 0xD6:
                            return address + "|D6|" + Parse_D6(datas);
                        case 0xD7:
                            return address + "|D7|" + Parse_D7(datas);
                        case 0xD8:
                            return address + "|D8|" + Parse_D8(datas);
                        case 0xD9:
                            return address + "|D9|" + Parse_D9(datas);
                        case 0xDA:
                            return address + "|DA|" + Parse_DA(datas);
                        case 0xDB:
                            return address + "|DB|" + Parse_DB(address, datas);
                        case 0xDD:
                            return address + "|DD|" + Parse_DD(datas);
                        case 0xDE:
                            return address + "|DE|" + Parse_DE(datas);
                        case 0xDF:
                            return address + "|DF|" + Parse_DF(datas);
                        default:
                            Debug.Log("Unknown TD :" + datas);
                            return "Unknown TD :" + datas;
                    }
                }
                else if (data[1] == "11") //FC,11,0C,08,10,03,FA
                {
                    //String str = "2"+String.format("%03d", BLE_RX[4]&0xFF)+String.format("%02d", BLE_RX[3]&0xFF)+String.format("%02d ", BLE_RX[2]&0xFF)+String.format("%03d", BLE_RX[5]&0xFF);
                    return address + "|UIBle_Ver|" + Parse_UIBle_Ver(datas);
                }
                else
                {
                    switch (((byte)int.Parse(data[2], NumberStyles.HexNumber)) & 0xFF)
                    {
                        case 0x22:
                            return address + "|23|23ONOFF";
                        default:
                            Debug.Log("Unknown callback :" + datas);
                            return "Unknown callback :" + datas;
                    }
                }
            }
            else if (data[0] == "FB")
            {
                Debug.Log("Send success :" + datas);
                return "";
            }
            Debug.Log("Unknown callback :" + datas);
            return "Unknown callback :" + datas;
        }
        /// <summary>
        /// 解析明碼
        /// </summary>
        /// <returns>各指令相對應結果</returns>
        private static string CallbackInfo_Raw(string address, string datas)
        {
            //Debug.Log(string.Format("OnUnity CallbackInfo :{0}|||{1}", address, datas));
            string[] data = datas.Split(',');
            if (data[0] == "FC")
            {
                if (data.Length >= 20 && data[1] == "23") //{23} 22byte/25byte
                {
                    return address + "|23|" + Parse_23(datas);
                }
                else if (data[1] == "11") //FC,11,0C,08,10,03,FA
                {
                    //String str = "2"+String.format("%03d", BLE_RX[4]&0xFF)+String.format("%02d", BLE_RX[3]&0xFF)+String.format("%02d ", BLE_RX[2]&0xFF)+String.format("%03d", BLE_RX[5]&0xFF);
                    return address + "|UIBle_Ver|" + Parse_UIBle_Ver(datas);
                }
                else
                {
                    switch (((byte)int.Parse(data[2], NumberStyles.HexNumber)) & 0xFF)
                    {
                        case 0x22:
                            return address + "|23|23ONOFF";
                        default:
                            Debug.Log("Unknown callback :" + datas);
                            return "Unknown callback :" + datas;
                    }
                }
            }
            else if (data[0] == "FB")
            {
                Debug.Log("Send success :" + datas);
                return "";
            }
            Debug.Log("Unknown callback :" + datas);
            return "Unknown callback :" + datas;
        }

        #region [Parse]

        //Callback暫存
        //public static string[] DataCache = null;
        private static Dictionary<string, DeviceDataCache> DataCache = new Dictionary<string, DeviceDataCache>();
        /// <summary>
        /// [01]Service Platform連線訊息
        /// </summary>
        /// <param name="input"></param>
        /// <returns>00:Fail, 01:success</returns>
        private static string Parse_01(string input)
        {
            string[] aes = GetAES(input);
            return aes[0];
        }
        /// <summary>
        /// [02]藍牙連線訊息
        /// </summary>
        /// <param name="input"></param>
        /// <returns>00:Fail, 01:success</returns>
        private static string Parse_02(string input)
        {
            string[] aes = GetAES(input);
            return aes[0];
        }
        /// <summary>
        /// 解析[03]限速,輪徑(其他參數暫時不使用)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>SpeedLimit(0~255)km/hr,輪周長(0~65535)mm</returns>
        private static string Parse_03(string input)
        {
            string[] aes = GetAES(input);
            int spdlim = int.Parse(aes[0], NumberStyles.HexNumber);
            int ccfr = int.Parse(aes[2] + aes[1], NumberStyles.HexNumber);
            return string.Format("{0},{1}", spdlim, ccfr);
        }
        /// <summary>
        /// 解析[04]設置是否成功
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_04(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// 解析[05]RC/SG,UI韌體版本,UI硬體版本,ctg1,ctg2
        /// </summary>
        /// <param name="input"></param>
        /// <returns>RCType,UI_FirmwareVersion,UI_HardwareVersion,EV_ctg1,EV_ctg2</returns>
        private static string Parse_05(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            string[] datas = GetAES(input);
            string rc_type = GetRCType(BitChoose_string(datas[0], 0, 2) + BitChoose_string(datas[4], 0, 2));
            //string ui_fw_ver = "2" + ByteToInt_string(datas[2], 3) + ByteToInt_string(datas[1], 2) + BitChoose_int(datas[0], 3, 7).ToString() + ByteToInt_string(datas[3], 3);
            string ui_fw_ver = string.Format("2{0}{1}{2:00}{3}", ByteToInt_string(datas[2], 3), ByteToInt_string(datas[1], 2), BitChoose_int(datas[0], 3, 7), ByteToInt_string(datas[3], 3));
            string ui_hw_ver = string.Format("2{0}{1:00}{2:00000}", ByteToInt_string(datas[5], 3), BitChoose_int(datas[4], 4, 7), CombineByteToInt(datas[7] + "," + datas[6]));
            string ev_ctg1 = ByteToInt_string(datas[8], 3);
            string ev_ctg2 = ByteToInt_string(datas[9], 3);
            return string.Format("{0},{1},{2},{3},{4}", rc_type, ui_fw_ver, ui_hw_ver, ev_ctg1, ev_ctg2);
        }
        /// <summary>
        /// 解析[09]馬達型號,馬達韌體名稱,馬達硬體名稱,韌體版本,RCID,生產流水號
        /// </summary>
        /// <param name="input"></param>
        /// <param name="package"></param>
        /// <returns>DUType(DU7),DU_FWname(0PB),DU_HWname(X0PB),DUFWVersion(202107030),RCID/evymc(50810),SN(202101)</returns>
        private static string Parse_09(string address, string input)
        {
            string DUType = "";
            string DUFW_MD = "";
            string DUHW_MD = "";
            string DUFW = "";
            string evymc = "";
            string SN = "";
            switch (packagecounter[address].i09)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache[address].s09 = AddNewDatas(DataCache[address].s09, GetAES(input));
                    //判斷馬達
                    //解析
                    DUFW_MD = GetASCIItoString(DataCache[address].s09, 0, 2);
                    DUHW_MD = GetASCIItoString(DataCache[address].s09, 8, 11);
                    DUFW = GetYamahaFWver(DataCache[address].s09[4], DataCache[address].s09[5], DataCache[address].s09[6], DataCache[address].s09[7]);
                    evymc = GetRCID(DataCache[address].s09);
                    SN = int.Parse(DataCache[address].s09[17] + DataCache[address].s09[16] + DataCache[address].s09[15], NumberStyles.HexNumber).ToString();
                    switch (DUFW_MD.ToUpper())
                    {
                        case "0SC":
                            DUType = "DU1 CAN-BIC PW-SE";
                            break;
                        case "0PA":
                            DUType = "DU2 PCB-BIC PW-X";
                            break;
                        case "0SE":
                            DUType = "DU3 CAN-BIC2 PW-SE";
                            break;
                        case "1RA":
                            DUType = "DU4 Comfort-BIC";
                            break;
                        case "1RB":
                            //DU4,6
                            DUType = DU4orDU6(DataCache[address].s09);
                            break;
                        case "1RC":
                            //DU4,6
                            DUType = DU4orDU6(DataCache[address].s09);
                            break;
                        case "0SG":
                            DUType = "DU5 JPN-BIC PW-SE";
                            break;
                        case "0PB":
                            DUType = "DU7 PCB-BIC2 PW-X2";
                            break;
                        case "0PD":
                            DUType = "DU7 PCB-BIC2 PW-X2";
                            break;
                        case "0PC":
                            DUType = "DU8 JPN-PCB PW-X2";
                            break;
                        case "CAN":
                            DUType = "DU9";
                            break;
                        case "2SA":
                            DUType = "DU10 ICB-BIC standard";
                            break;
                        case "2SB":
                            DUType = "DU11 ICB-BIC coaster";
                            break;
                        case "1RD":
                            DUType = "DU12 JPN-SCB";
                            break;
                        case "GR1":
                            DUType = "DU13";
                            break;
                        case "GR2":
                            DUType = "DU14";
                            break;
                        case "2YA":
                            DUType = "DU15 PCB BIC3";
                            break;
                        case "GR3":
                            DUType = "DU18 G370";
                            break;
                        case "2SE":
                            DUType = "DU19";
                            break;
                        case "EP8": //Shimano
                            DUFW_MD = "EP800";
                            DUType = "DU16 EP8";
                            DUFW = GetShimanoFWver(DataCache[address].s09[5], DataCache[address].s09[6], DataCache[address].s09[7]);
                            break;
                        case "3MA":
                            DUType = "DU20";
                            break;
                        default:
                            Debug.Log("Unknow DU_Firmware :" + DUFW_MD);
                            break;
                    }
                    //清空暫存
                    DataCache[address].s09 = new string[] { };
                    //packagecounter歸零
                    packagecounter[address].i09 = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5}", DUType, DUFW_MD, DUHW_MD, DUFW, evymc, SN);
                //第一包
                case 2:
                    //作暫存
                    DataCache[address].s09 = GetAES(input);
                    return "09_wait";
                default:
                    Debug.Log("[09] parse Error." + packagecounter[address].i09);
                    return "";
            }
        }
        /// <summary>
        /// 解析[0A]馬達資訊stct代表Service tool連線的次數
        /// lstc代表距離上一次Service tool連線隔多久
        /// fstc代表距離上一次Service tool連線隔多遠
        /// baac代表Boost助力模式的平均電流
        /// paac代表Power助力模式的平均電流
        /// caac代表Climb助力模式的平均電流
        /// naac代表Normal助力模式的平均電流
        /// </summary>
        /// <param name="input"></param>
        /// <returns>stct,lstc,fstc,baac,paac,caac,naac,taac,eaac</returns>
        private static string Parse_0A(string address, string input)
        {
            switch (packagecounter[address].i0A)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache[address].s0A = AddNewDatas(DataCache[address].s0A, GetAES(input));
                    //解析
                    string stct = CombineByteToInt(DataCache[address].s0A[1] + "," + DataCache[address].s0A[0]).ToString();
                    string lstc = CombineByteToInt(DataCache[address].s0A[3] + "," + DataCache[address].s0A[2]).ToString() + "hr";
                    string fstc = CombineByteToInt(DataCache[address].s0A[5] + "," + DataCache[address].s0A[4]).ToString() + "km";
                    string baac = string.Format("{0:0.00}A", ((decimal)CombineByteToInt(DataCache[address].s0A[7] + "," + DataCache[address].s0A[6])) / 10);
                    string paac = string.Format("{0:0.00}A", ((decimal)CombineByteToInt(DataCache[address].s0A[9] + "," + DataCache[address].s0A[8])) / 10);
                    string caac = string.Format("{0:0.00}A", ((decimal)CombineByteToInt(DataCache[address].s0A[11] + "," + DataCache[address].s0A[10])) / 10);
                    string naac = string.Format("{0:0.00}A", ((decimal)CombineByteToInt(DataCache[address].s0A[13] + "," + DataCache[address].s0A[12])) / 10);
                    string taac = string.Format("{0:0.00}A", ((decimal)CombineByteToInt(DataCache[address].s0A[15] + "," + DataCache[address].s0A[14])) / 10);
                    string eaac = string.Format("{0:0.00}A", ((decimal)CombineByteToInt(DataCache[address].s0A[17] + "," + DataCache[address].s0A[16])) / 10);
                    //清空暫存
                    DataCache[address].s0A = new string[] { };
                    //packagecounter歸零
                    packagecounter[address].i0A = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", stct, lstc, fstc, baac, paac, caac, naac, taac, eaac);
                //第一包
                case 2:
                    //作暫存
                    DataCache[address].s0A = GetAES(input);
                    return "0A_wait";
                default:
                    Debug.Log("[0A] parse Error." + packagecounter[address].i0A);
                    return "";
            }
        }
        /// <summary>
        /// 解析[0C]ErrorCode,BBSS firmware version(目前只解析BBSS韌體版本)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>ErrorCode,BBSSfw</returns>
        private static string Parse_0C(string input)
        {
            string[] aes = GetAES(input);
            int dd = BitChoose_int(aes[10], 3, 7);
            int mm = int.Parse(aes[11], NumberStyles.HexNumber);
            int yy = int.Parse(aes[12], NumberStyles.HexNumber);
            int sn = int.Parse(aes[13], NumberStyles.HexNumber);
            return string.Format("ErrorCode,2{0:000}{1:00}{2:00}{3:000}", yy, mm, dd, sn);
        }
        /// <summary>
        /// 解析[0D]主電池Cell版本韌體版本生產流水號
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
        /// 解析[0E]主電池充電循環次數,充電次數,大電流放電比例100%
        /// </summary>
        /// <param name="input"></param>
        /// <returns>ccy,cchg,hrd</returns>
        private static string Parse_0E(string input)
        {
            string[] aes = GetAES(input);
            string ccy = CombineByteToInt(aes[1] + "," + aes[0]).ToString();
            string cchg = CombineByteToInt(aes[3] + "," + aes[2]).ToString();
            string hrd = string.Format("{0}%", ByteToInt_string(aes[4], 0));
            return string.Format("{0},{1},{2}", ccy, cchg, hrd);
        }
        /// <summary>
        /// 解析[12]取得馬達ODO,總使用時間
        /// </summary>
        /// <param name="input"></param>
        /// <returns>odo,tut</returns>
        private static string Parse_12(string input)
        {
            string[] aes = GetAES(input);
            string odo = CombineByteToInt(aes[1] + "," + aes[0]).ToString() + "km";
            string tut = CombineByteToInt(aes[3] + "," + aes[2]).ToString() + "hr";
            return string.Format("{0},{1}", odo, tut);
        }
        /// <summary>
        /// 解析[13]主(+副)電池容量,主電池壽命,前次充飽容量
        /// </summary>
        /// <param name="input"></param>
        /// <returns>rsoc(%),eplife(%),fcc(Wh)</returns>
        private static string Parse_13(string input)
        {
            string[] aes = GetAES(input);
            string rsoc = string.Format("{0}%", ByteToInt_string(aes[0], 0));
            string eplife = string.Format("{0}%", ByteToInt_string(aes[1], 0));
            string fcc = string.Format("{0:0.0}Wh", (decimal)CombineByteToInt(aes[3] + "," + aes[2])/10);
            return string.Format("{0},{1},{2}", rsoc, eplife, fcc);
        }
        /// <summary>
        /// 解析[15]RC error code檢測
        /// </summary>
        /// <param name="input"></param>
        /// <returns>OK(0x20)/error code(0x21...)</returns>
        private static string Parse_15(string input)
        {
            string[] aes = GetAES(input);
            return aes[0];
        }
        /// <summary>
        /// 解析[16]error code檢測(Sensor, DU, EP, E-shifting)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>OK(0x60)/error code(0x31...)</returns>
        private static string Parse_16(string input)
        {
            string[] aes = GetAES(input);
            return aes[0];
        }
        /// <summary>
        /// 解析[17]EP error code檢測
        /// </summary>
        /// <param name="input"></param>
        /// <returns>OK(0xC0)/error code(0xC1...)</returns>
        private static string Parse_17(string input)
        {
            string[] aes = GetAES(input);
            return aes[0];
        }
        /// <summary>
        /// 解析[1A]設置是否成功
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_1A(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// 解析{23}
        /// </summary>
        /// <param name="input"></param>
        /// |0.0 , 0.0 , 0.0 , 0.00 , 3.8 , 8.0 , 0.0 , 62
        /// <returns>[spd,trq,cde,acur,trid,trit,hpw,rsoc] or [ecode,carr] or [ecode,carr,cur_ast,odo]</returns>
        private static string Parse_23(string input)
        {
            string[] datas = Get23Raw(input);
            string spd = ""; //當前車速 km/hr
            string trq = ""; //當前踩踏力矩 Nm
            string cde = ""; //當前踏頻 rpm
            string acur = ""; //當前助力電流 A
            string trid = ""; //當次騎乘距離 km
            string trit = ""; //當次騎乘時間 min
            string hpw = ""; //當前踩踏功率 W
            string rsoc = ""; //EP電量 %

            string ecode = ""; //錯誤碼 0xFF
            string carr = ""; //當前助力剩餘里程 km

            string cur_ast = ""; //當前助力模式 Off/ECO/Tour/Active/Sport/Power/Auto
            string odo = ""; //馬達總里程 km

            switch (datas.Length)
            {
                case 5:
                    ecode = datas[2];
                    carr = string.Format("{0}", int.Parse(datas[4] + datas[3], NumberStyles.HexNumber));
                    return string.Format("{0},{1}", ecode, carr);
                case 8:
                    ecode = datas[2];
                    carr = string.Format("{0}", int.Parse(datas[4] + datas[3], NumberStyles.HexNumber));
                    switch (int.Parse(datas[5], NumberStyles.HexNumber))
                    {
                        case 0:
                            cur_ast = "OFF";
                            break;
                        case 1:
                            cur_ast = "ECO";
                            break;
                        case 2:
                            cur_ast = "Tour";
                            break;
                        case 3:
                            cur_ast = "Active";
                            break;
                        case 4:
                            cur_ast = "Sport";
                            break;
                        case 5:
                            cur_ast = "Power";
                            break;
                        case 6:
                            cur_ast = "Auto";
                            break;
                        default:
                            cur_ast = "Unknow Error.";
                            break;
                    }
                    odo = string.Format("{0}", int.Parse(datas[7] + datas[6], NumberStyles.HexNumber));
                    return string.Format("{0},{1},{2},{3}", ecode, carr, cur_ast, odo);
                case 17:
                    spd = string.Format("{0:0.0}", ((decimal)(int.Parse(datas[3] + datas[2], NumberStyles.HexNumber))) / 10);
                    trq = string.Format("{0:0.0}", ((decimal)(int.Parse(datas[5] + datas[4], NumberStyles.HexNumber))) / 10);
                    cde = string.Format("{0:0.0}", ((decimal)(int.Parse(datas[7] + datas[6], NumberStyles.HexNumber))) / 10);
                    acur = string.Format("{0:0.00}", ((decimal)(int.Parse(datas[9] + datas[8], NumberStyles.HexNumber))) / 100);
                    trid = string.Format("{0:0.0}", ((decimal)(int.Parse(datas[11] + datas[10], NumberStyles.HexNumber))) / 10);
                    trit = string.Format("{0:0}", int.Parse(datas[13] + datas[12], NumberStyles.HexNumber));
                    hpw = string.Format("{0:0.0}", ((decimal)(int.Parse(datas[15] + datas[14], NumberStyles.HexNumber))) / 10);
                    rsoc = string.Format("{0}", int.Parse(datas[16], NumberStyles.HexNumber));
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", spd, trq, cde, acur, trid, trit, hpw, rsoc);
            }
            return "Unknow Error.";
        }
        /// <summary>
        /// 解析[2C]馬達段數
        /// </summary>
        /// <param name="input"></param>
        /// <returns>power,sport,active,tour,eco</returns>
        private static string Parse_2C(string input)
        {
            string[] aes = GetAES(input);
            string asmo1 = aes[0][1].ToString();
            string asmo2 = aes[0][0].ToString();
            string asmo3 = aes[1][1].ToString();
            string asmo4 = aes[1][0].ToString();
            string asmo5 = aes[2][1].ToString();
            return string.Format("{0},{1},{2},{3},{4}", asmo1, asmo2, asmo3, asmo4, asmo5);
        }
        /// <summary>
        /// 解析[2D]設置是否成功
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_2D(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// 解析[30]總里程,上次回廠
        /// </summary>
        /// <param name="input"></param>
        /// <returns>dodo,serin</returns>
        private static string Parse_30(string input)
        {
            string[] aes = GetAES(input);
            string dodo = string.Format("{0}km", CombineByteToInt(aes[1] + "," + aes[0]));
            string serin = string.Format("{0}km", CombineByteToInt(aes[3] + "," + aes[2]));
            return string.Format("{0},{1}", dodo, serin);
        }
        /// <summary>
        /// 解析[32]車架號碼ASCII to String
        /// </summary>
        /// <param name="input"></param>
        /// <returns>車架號碼</returns>
        private static string Parse_32(string address, string input)
        {
            switch (packagecounter[address].i32)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache[address].s32 = AddNewDatas(DataCache[address].s32, GetAES(input));
                    //解析
                    string result = "";
                    for (int i = 0; i < 18; i++)
                    {
                        result += (char)(int.Parse(DataCache[address].s32[i], NumberStyles.HexNumber));
                    }
                    //清空暫存
                    DataCache[address].s32 = new string[] { };
                    //packagecounter歸零
                    packagecounter[address].i32 = 0;
                    return result;
                //第一包
                case 2:
                    //作暫存
                    DataCache[address].s32 = GetAES(input);
                    return "32_wait";
                default:
                    return "";
            }
        }
        /// <summary>
        /// 解析[33]設置車架號碼是否成功
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_33(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// 解析[37]副電池容量,副電池壽命,前次充飽容量
        /// </summary>
        /// <param name="input"></param>
        /// <returns>rsoc(%),eplife(%),fcc(Wh)</returns>
        private static string Parse_37(string input)
        {
            string[] aes = GetAES(input);
            string rsoc = string.Format("{0}%", ByteToInt_string(aes[0], 0));
            string eplife = string.Format("{0}%", ByteToInt_string(aes[1], 0));
            string fcc = string.Format("{0:0.0}Wh", (decimal)CombineByteToInt(aes[3] + "," + aes[2]) / 10);
            return string.Format("{0},{1},{2}", rsoc, eplife, fcc);
        }
        /// <summary>
        /// 解析[38]副電池Cell版本韌體版本生產流水號
        /// </summary>
        /// <param name="input"></param>
        /// <returns>minor,major,SN</returns>
        private static string Parse_38(string input)
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
        /// 解析[39]副電池充電循環次數,充電次數,大電流放電比例100%
        /// </summary>
        /// <param name="input"></param>
        /// <returns>ccy,cchg,hrd</returns>
        private static string Parse_39(string input)
        {
            string[] aes = GetAES(input);
            string ccy = CombineByteToInt(aes[1] + "," + aes[0]).ToString();
            string cchg = CombineByteToInt(aes[3] + "," + aes[2]).ToString();
            string hrd = string.Format("{0}%", ByteToInt_string(aes[4], 0));
            return string.Format("{0},{1},{2}", ccy, cchg, hrd);
        }
        /// <summary>
        /// 解析[40]BBSS磁通量(Wb)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>成功,最大值,最小值</returns>
        private static string Parse_40(string input)
        {
            string[] aes = GetAES(input);
            int success = int.Parse(aes[0], NumberStyles.HexNumber);
            int max = int.Parse(aes[2] + aes[1], NumberStyles.HexNumber);
            if (max > 32767)
            {
                max -= 65536;
            }
            int min = int.Parse(aes[4] + aes[3], NumberStyles.HexNumber);
            if (min > 32767)
            {
                min -= 65536;
            }
            return string.Format("{0},{1},{2}", success, max, min);
        }
        /// <summary>
        /// 解析[C0]有無限速, 單位
        /// </summary>
        /// <param name="input"></param>
        /// <returns>speedLimit(0/1),kph/mph(0/1)</returns>
        private static string Parse_C0(string input)
        {
            string[] aes = GetAES(input);
            return string.Format("{0},{1}", BitChoose_string(aes[0], 6, 6), BitChoose_string(aes[0], 7, 7));
        }
        /// <summary>
        /// 解析[C1]設置是否成功
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_C1(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// 解析[D1]Remote-1版本
        /// </summary>
        /// <param name="input"></param>
        /// <returns>remote_type,fw_ver,hw_ver,pic_icon,pic_navi,pic_number,pic_eu_language,pic_tra_chinese,pic_sim_chinese,pic_jpn,pic_korea</returns>
        private static string Parse_D1(string address, string input)
        {
            switch (packagecounter[address].iD1)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache[address].sD1 = AddNewDatas(DataCache[address].sD1, GetAES(input));
                    //解析
                    string remote_type = GetRemoteType(DataCache[address].sD1[4]);
                    string fw_ver = "";
                    string icon = ByteToInt_string(DataCache[address].sD1[9], 3);
                    string navi = ByteToInt_string(DataCache[address].sD1[10], 3);
                    string number = ByteToInt_string(DataCache[address].sD1[11], 3);
                    string eu_lan = ByteToInt_string(DataCache[address].sD1[12], 3);
                    string tra_chi = ByteToInt_string(DataCache[address].sD1[13], 3);
                    string sim_chi = ByteToInt_string(DataCache[address].sD1[14], 3);
                    string jpn = ByteToInt_string(DataCache[address].sD1[15], 3);
                    string korea = ByteToInt_string(DataCache[address].sD1[16], 3);
                    //fw_ver
                    string fwv = string.Format("2{0}{1}{2:00}{3}", ByteToInt_string(DataCache[address].sD1[2], 3), ByteToInt_string(DataCache[address].sD1[1], 2)
                        , BitChoose_int(DataCache[address].sD1[0], 3, 7), ByteToInt_string(DataCache[address].sD1[3], 3));
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
                    string hw_ver = string.Format("2{0}{1:00}{2:00000}", ByteToInt_string(DataCache[address].sD1[6], 3), BitChoose_int(DataCache[address].sD1[5], 4, 7)
                        , CombineByteToInt(DataCache[address].sD1[8] + "," + DataCache[address].sD1[7]));
                    //清空暫存
                    DataCache[address].sD1 = new string[] { };
                    //packagecounter歸零
                    packagecounter[address].iD1 = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", remote_type, fw_ver, hw_ver, icon, navi, number, eu_lan, tra_chi, sim_chi, jpn, korea);
                //第一包
                case 2:
                    //作暫存
                    DataCache[address].sD1 = GetAES(input);
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
        private static string Parse_D2(string address, string input)
        {
            switch (packagecounter[address].iD2)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache[address].sD2 = AddNewDatas(DataCache[address].sD2, GetAES(input));
                    //解析
                    string remote_type = GetRemoteType(DataCache[address].sD2[4]);
                    string fw_ver = "";
                    string icon = ByteToInt_string(DataCache[address].sD2[9], 3);
                    string navi = ByteToInt_string(DataCache[address].sD2[10], 3);
                    string number = ByteToInt_string(DataCache[address].sD2[11], 3);
                    string eu_lan = ByteToInt_string(DataCache[address].sD2[12], 3);
                    string tra_chi = ByteToInt_string(DataCache[address].sD2[13], 3);
                    string sim_chi = ByteToInt_string(DataCache[address].sD2[14], 3);
                    string jpn = ByteToInt_string(DataCache[address].sD2[15], 3);
                    string korea = ByteToInt_string(DataCache[address].sD2[16], 3);
                    //fw_ver
                    string fwv = string.Format("2{0}{1}{2:00}{3}", ByteToInt_string(DataCache[address].sD2[2], 3), ByteToInt_string(DataCache[address].sD2[1], 2)
                        , BitChoose_int(DataCache[address].sD2[0], 3, 7), ByteToInt_string(DataCache[address].sD2[3], 3));
                    switch (remote_type)
                    {
                        case "Remote ON/OFF":
                            fw_ver = string.Format("rmnfswxxdapp_{0}.bin", fwv);
                            break;
                    }
                    //hw_ver
                    string hw_ver = string.Format("2{0}{1:00}{2:00000}", ByteToInt_string(DataCache[address].sD2[6], 3), BitChoose_int(DataCache[address].sD2[5], 4, 7)
                        , CombineByteToInt(DataCache[address].sD2[8] + "," + DataCache[address].sD2[7]));
                    //清空暫存
                    DataCache[address].sD2 = new string[] { };
                    //packagecounter歸零
                    packagecounter[address].iD2 = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", remote_type, fw_ver, hw_ver, icon, navi, number, eu_lan, tra_chi, sim_chi, jpn, korea);
                //第一包
                case 2:
                    //作暫存
                    DataCache[address].sD2 = GetAES(input);
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
        private static string Parse_D3(string address, string input)
        {
            switch (packagecounter[address].iD3)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache[address].sD3 = AddNewDatas(DataCache[address].sD3, GetAES(input));
                    //解析
                    string display_type = GetRemoteType(DataCache[address].sD3[4]);
                    string fw_ver = "";
                    string icon = ByteToInt_string(DataCache[address].sD3[9], 3);
                    string navi = ByteToInt_string(DataCache[address].sD3[10], 3);
                    string number = ByteToInt_string(DataCache[address].sD3[11], 3);
                    string eu_lan = ByteToInt_string(DataCache[address].sD3[12], 3);
                    string tra_chi = ByteToInt_string(DataCache[address].sD3[13], 3);
                    string sim_chi = ByteToInt_string(DataCache[address].sD3[14], 3);
                    string jpn = ByteToInt_string(DataCache[address].sD3[15], 3);
                    string korea = ByteToInt_string(DataCache[address].sD3[16], 3);
                    //fw_ver
                    string fwv = string.Format("2{0}{1}{2:00}{3}", ByteToInt_string(DataCache[address].sD3[2], 3), ByteToInt_string(DataCache[address].sD3[1], 2)
                        , BitChoose_int(DataCache[address].sD3[0], 3, 7), ByteToInt_string(DataCache[address].sD3[3], 3));
                    switch (display_type)
                    {
                        case "New Evo":
                            fw_ver = string.Format("dpevowxxdapp_{0}.bin", fwv);
                            break;
                    }
                    //hw_ver
                    string hw_ver = string.Format("2{0}{1:00}{2:00000}", ByteToInt_string(DataCache[address].sD3[6], 3), BitChoose_int(DataCache[address].sD3[5], 4, 7)
                        , CombineByteToInt(DataCache[address].sD3[8] + "," + DataCache[address].sD3[7]));
                    //清空暫存
                    DataCache[address].sD3 = new string[] { };
                    //packagecounter歸零
                    packagecounter[address].iD3 = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", display_type, fw_ver, hw_ver, icon, navi, number, eu_lan, tra_chi, sim_chi, jpn, korea);
                //第一包
                case 2:
                    //作暫存
                    DataCache[address].sD3 = GetAES(input);
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
        /// <summary>
        /// 解析[D5]New EVO顯示數值Layout
        /// 0x01 Clock (from App)
        /// 0x02 Calories(from App)
        /// 0x03 Elevation(from App)
        /// 0x04 Phone Battery(from App)
        /// 0x05 Estimated time of arrival(from App)
        /// 0x06 Estimated Distance of arrival(from App)
        /// 0x00,0x07~0x0F Rvd
        /// 0x10 Trip Time (from SG)
        /// 0x11 Trip distance(from SG)
        /// 0x12 Speed(from SG)
        /// 0x13 AVG Speed(from SG)
        /// 0x14 Cadence(from SG)
        /// 0x15 AVG Cadence(from SG)
        /// 0x16 Power(from SG)
        /// 0x17 AVG power(from SG)
        /// 0x18 Battery-level(from SG)
        /// 0x19 Battery-main(from SG)
        /// 0x1A Battery-sub(from SG)
        /// 0x1B Remain Range(from SG)
        /// 0x1C ODO(from SG)
        /// 0x1D Service interval(from SG)
        /// 0x1E Assist mode(from SG)
        /// 0x1F Heart rate(from SG)
        /// 0x20 Max Speed(from SG)
        /// 0x21 Max Power(from SG)
        /// 0x22 Max Cadence(from SG)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>page1_main,page1_left,page1_right,page2_main,page2_left,page2_right,page3_main,page3_left,page3_right,page4_main,page4_left,page4_right</returns>
        private static string Parse_D5(string input)
        {
            string[] aes = GetAES(input);
            string result = aes[0];
            for (int i = 1; i < 12; i++)
            {
                result = string.Format("{0},{1}", result, aes[i]);
            }
            return result;
        }
        /// <summary>
        /// 解析[D6]設置是否成功
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_D6(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// 解析[D7]語言, 開機畫面
        /// </summary>
        /// <param name="input"></param>
        /// <returns>language("00" English),brand_type("00" Giant)</returns>
        private static string Parse_D7(string input)
        {
            string[] aes = GetAES(input);
            return string.Format("{0},{1}", aes[0], aes[1]); ;
        }
        /// <summary>
        /// 解析[D8]設置是否成功
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_D8(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// 解析[D9]EVO背光Level,EVO背光數值,ONOFF背光Level,ONOFF背光數值,車燈開關狀態
        /// </summary>
        /// <param name="input"></param>
        /// <returns>EVOLevel,EVOLevel1_value,EVOLevel2_value,EVOLevel3_value,ONOFFLevel,ONOFFHigh_Value,ONOFFLow_Value,Light_Level</returns>
        private static string Parse_D9(string input)
        {
            string[] aes = GetAES(input);
            string evo_level = int.Parse(aes[0], NumberStyles.HexNumber).ToString();
            string evo_level1 = int.Parse(aes[1], NumberStyles.HexNumber).ToString();
            string evo_level2 = int.Parse(aes[2], NumberStyles.HexNumber).ToString();
            string evo_level3 = int.Parse(aes[3], NumberStyles.HexNumber).ToString();
            string onoff_level = int.Parse(aes[4], NumberStyles.HexNumber).ToString();
            string onoff_high = int.Parse(aes[5], NumberStyles.HexNumber).ToString();
            string onoff_low = int.Parse(aes[6], NumberStyles.HexNumber).ToString();
            string light = int.Parse(aes[7], NumberStyles.HexNumber).ToString();
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", evo_level, evo_level1, evo_level2, evo_level3, onoff_level, onoff_high, onoff_low, light);
        }
        /// <summary>
        /// 解析[DA]設置是否成功
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_DA(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// 解析[DB]電池版本,韌體版本,生產序號,最大電壓,最小電壓,溫度1,溫度2
        /// </summary>
        /// <param name="input"></param>
        /// <returns>PF/GA,EPFW,EPSN(yyyymmddxxxxx),vcmax,vcmin,tmp1,tmp2</returns>
        private static string Parse_DB(string address, string input)
        {
            switch (packagecounter[address].iDB)
            {
                //最後一包
                case 1:
                    //添加新的
                    DataCache[address].sDB = AddNewDatas(DataCache[address].sDB, GetAES(input));
                    //解析
                    //Byte 0 = 0x42(ASCII: “B”) 且Byte 1 = 0x53 (ASCII: “S”)時，代表為嘉普的電池(TREND POWER)，其餘皆為松下電池(Panasonic)
                    string EPVer = "";
                    string EPFW = "";
                    string EPSN = "";
                    string MaxV = "";
                    string MinV = "";
                    int Tmp1 = 0;
                    int Tmp2 = 0;
                    if (DataCache[address].sDB[0] == "42" && DataCache[address].sDB[1] == "53")
                    {
                        return GetASCIItoString(DataCache[address].sDB, 0, 15); //SN id
                    }
                    else
                    {
                        switch (int.Parse(DataCache[address].sDB[0], NumberStyles.HexNumber))
                        {
                            case 0:
                                EPVer = "PF";
                                break;
                            case 1:
                                EPVer = "GA";
                                break;
                            default:
                                EPVer = "Reserved";
                                break;
                        }
                        EPFW = int.Parse(DataCache[address].sDB[1], NumberStyles.HexNumber).ToString();
                        EPSN = string.Format("2{0:000}{1:00}{2:00}{3:00000}", int.Parse(DataCache[address].sDB[2], NumberStyles.HexNumber),
                            int.Parse(DataCache[address].sDB[3], NumberStyles.HexNumber), int.Parse(DataCache[address].sDB[4], NumberStyles.HexNumber), 
                            int.Parse(DataCache[address].sDB[6] + DataCache[address].sDB[5], NumberStyles.HexNumber));
                        MaxV = int.Parse(DataCache[address].sDB[8] + DataCache[address].sDB[7], NumberStyles.HexNumber).ToString();
                        MinV = int.Parse(DataCache[address].sDB[10] + DataCache[address].sDB[9], NumberStyles.HexNumber).ToString();
                        Tmp1 = int.Parse(DataCache[address].sDB[11], NumberStyles.HexNumber);
                        if (Tmp1 > 127)
                            Tmp1 -= 256;
                        Tmp2 = int.Parse(DataCache[address].sDB[12], NumberStyles.HexNumber);
                        if (Tmp2 > 127)
                            Tmp2 -= 256;
                    }
                    
                    //清空暫存
                    DataCache[address].sDB = new string[] { };
                    //packagecounter歸零
                    packagecounter[address].iDB = 0;

                    //EPVer,PF/GA,EPFW,EPSN(yyyymmddxxxxx),vcmax,vcmin,tmp1,tmp2
                    return string.Format("{0},{1},{2},{3},{4},{5},{6}", EPVer, EPFW, EPSN, MaxV, MinV, Tmp1, Tmp2);
                //第一包
                case 2:
                    //作暫存
                    DataCache[address].sDB = GetAES(input);
                    return "DB_wait";
                default:
                    Debug.Log("[DB] parse Error." + packagecounter[address]);
                    return "";
            }
        }
        /// <summary>
        /// 解析[DD]Ring是否存在以及按鈕狀態
        /// </summary>
        /// <param name="input"></param>
        /// <returns>left,right,left1,left2,left3,right1,right2,right3</returns>
        private static string Parse_DD(string input)
        {
            string[] aes = GetAES(input);
            string byte0 = BitChoose_string(aes[0], 6, 7);
            string left1 = GetRingButton(aes[1]);
            string left2 = GetRingButton(aes[2]);
            string left3 = GetRingButton(aes[3]);
            string right1 = GetRingButton(aes[4]);
            string right2 = GetRingButton(aes[5]);
            string right3 = GetRingButton(aes[6]);
            return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", byte0[0].ToString(), byte0[1].ToString()
                , left1, left2, left3, right1, right2, right3);
        }
        /// <summary>
        /// 解析[DE]設置是否成功
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_DE(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// 解析[DF]Shimano馬達相關
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Unit,Index,Model,Version</returns>
        private static string Parse_DF(string input)
        {
            string[] aes = GetAES(input);
            string Device = "";
            //string Index = "";
            string Model = "";
            string Major = "";
            string Minor = "";
            string SubMinor = "";
            switch (int.Parse(aes[0], NumberStyles.HexNumber))
            {
                case 1:
                    Device = "Shimano Front-Derailleur";
                    break;
                case 2:
                    Device = "Shimano Rear-Derailleur";
                    break;
                case 3:
                    Device = "Shimano Switch/Shifter";
                    break;
                default:
                    Device = "Rvd";
                    break;
            }

            if (aes[3] == "04")
            {
                switch (aes[2])
                {
                    case "09":
                        Model = "RD-M9050";
                        break;
                    case "10":
                        Model = "RD-M8050";
                        break;
                    case "16":
                        Model = "RD-RX815/817";
                        break;
                    case "14":
                        Model = "RD-R8050/RX805";
                        break;
                    case "20":
                        Model = "MU-UR500";
                        break;
                    case "06":
                        Model = "MU-S705";
                        break;
                    case "FF":
                        Model = "Invalid";
                        break;
                    default:
                        Model = "Unknow";
                        break;
                }
            }
            else if (aes[3] == "01")
            {
                
            }
            else if (aes[3] == "FF")
            {
                Model = "Invalid";
            }

            switch (int.Parse(aes[4][0].ToString(), NumberStyles.HexNumber))
            {
                case 15:
                    Major = "Invalid";
                    break;
                default:
                    Major = int.Parse(aes[4][0].ToString(), NumberStyles.HexNumber).ToString();
                    break;
            }

            switch (int.Parse(aes[4][1].ToString(), NumberStyles.HexNumber))
            {
                case 15:
                    Minor = "Invalid";
                    break;
                default:
                    Minor = int.Parse(aes[4][1].ToString(), NumberStyles.HexNumber).ToString();
                    break;
            }

            switch (int.Parse(aes[5], NumberStyles.HexNumber))
            {
                case 255:
                    SubMinor = "Invalid";
                    break;
                default:
                    SubMinor = int.Parse(aes[5], NumberStyles.HexNumber).ToString();
                    break;
            }

            return string.Format("{0},{1},{2}.{3}.{4}", Device, Model, Major, Minor, SubMinor);
        }
        /// <summary>
        /// 解析[UIBle_Ver]
        /// </summary>
        /// <param name="input">FC,11,0C,08,10,03,FA</param>
        /// <returns>R3/R4/R7/R9</returns>
        private static string Parse_UIBle_Ver(string input)
        {
            string[] datas = input.Split(',');
            switch (string.Format("2{0:000}{1:00}{2:00}{3:000}", int.Parse(datas[4], NumberStyles.HexNumber), int.Parse(datas[3], NumberStyles.HexNumber), int.Parse(datas[2], NumberStyles.HexNumber), int.Parse(datas[5], NumberStyles.HexNumber)))
            {
                case "20160812003":
                    return "R3";
                case "20180321000":
                    return "R4";
                case "20180321004":
                    return "R4";
                case "20180830007":
                    return "R7";
                case "20190826009":
                    return "R9";
                default:
                    return "R9";
            }
        }
        #endregion

        #region [Tools]

        private static void CheckDataCache(string address)
        {
            if (DataCache == null)
            {
                DataCache = new Dictionary<string, DeviceDataCache>();
            }
            if (!DataCache.ContainsKey(address))
            {
                DataCache.Add(address, new DeviceDataCache());
            }
        }
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

        public static string ByteArr_To_ByteString(string[] bytes)
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
                    str = bytes[0];
                }
                else
                {
                    str = string.Format("{0},{1}", str, bytes[i]);
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
        /// 提取{23}有效數值(17或5或8) ex:[FC,23,11,00,00,00,00,00,00,00,00,00,26,00,08,00,00,00,42,A2]
        ///                          ex:[FC,23,08,01,00,72,00,06,C8,01,00,00,00,00,00,00,00,00,00,6B]
        /// </summary>
        /// <param name="input"></param>
        /// <returns>[08,01,00,72,00,06,C8,01]</returns>
        public static string[] Get23Raw(string input)
        {
            string[] datas = input.Split(',');
            string[] result = new string[int.Parse(datas[2], NumberStyles.HexNumber)];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = datas[i + 2];
            }
            return result;
        }
        /// <summary>
        /// 計算這個指令有幾個包
        /// </summary>
        /// <param name="hexstring">16進制字串</param>
        /// <returns></returns>
        private static DevicePakCount PackageCompute(string address, string type, string hexstring)
        {
            if (!packagecounter.ContainsKey(address))
            {
                packagecounter.Add(address, new DevicePakCount());
            }

            switch (type)
            {
                case "09":
                    if (packagecounter[address].i09 <= 0)
                    {
                        if (string.IsNullOrEmpty(hexstring))
                            return packagecounter[address];
                        //byte to int
                        int total = int.Parse(hexstring, NumberStyles.HexNumber);
                        int result = total / 14;
                        if (total % 14 != 0 && result != 0)
                            result += 1;
                        packagecounter[address].i09 = result;
                        return packagecounter[address];
                    }
                    else if (packagecounter[address].i09 > 0)
                    {
                        packagecounter[address].i09 -= 1;
                        return packagecounter[address];
                    }
                    break;
                case "0A":
                    if (packagecounter[address].i0A <= 0)
                    {
                        if (string.IsNullOrEmpty(hexstring))
                            return packagecounter[address];
                        //byte to int
                        int total = int.Parse(hexstring, NumberStyles.HexNumber);
                        int result = total / 14;
                        if (total % 14 != 0 && result != 0)
                            result += 1;
                        packagecounter[address].i0A = result;
                        return packagecounter[address];
                    }
                    else if (packagecounter[address].i0A > 0)
                    {
                        packagecounter[address].i0A -= 1;
                        return packagecounter[address];
                    }
                    break;
                case "32":
                    if (packagecounter[address].i32 <= 0)
                    {
                        if (string.IsNullOrEmpty(hexstring))
                            return packagecounter[address];
                        //byte to int
                        int total = int.Parse(hexstring, NumberStyles.HexNumber);
                        int result = total / 14;
                        if (total % 14 != 0 && result != 0)
                            result += 1;
                        packagecounter[address].i32 = result;
                        return packagecounter[address];
                    }
                    else if (packagecounter[address].i32 > 0)
                    {
                        packagecounter[address].i32 -= 1;
                        return packagecounter[address];
                    }
                    break;
                case "D1":
                    if (packagecounter[address].iD1 <= 0)
                    {
                        if (string.IsNullOrEmpty(hexstring))
                            return packagecounter[address];
                        //byte to int
                        int total = int.Parse(hexstring, NumberStyles.HexNumber);
                        int result = total / 14;
                        if (total % 14 != 0 && result != 0)
                            result += 1;
                        packagecounter[address].iD1 = result;
                        return packagecounter[address];
                    }
                    else if (packagecounter[address].iD1 > 0)
                    {
                        packagecounter[address].iD1 -= 1;
                        return packagecounter[address];
                    }
                    break;
                case "D2":
                    if (packagecounter[address].iD2 <= 0)
                    {
                        if (string.IsNullOrEmpty(hexstring))
                            return packagecounter[address];
                        //byte to int
                        int total = int.Parse(hexstring, NumberStyles.HexNumber);
                        int result = total / 14;
                        if (total % 14 != 0 && result != 0)
                            result += 1;
                        packagecounter[address].iD2 = result;
                        return packagecounter[address];
                    }
                    else if (packagecounter[address].iD2 > 0)
                    {
                        packagecounter[address].iD2 -= 1;
                        return packagecounter[address];
                    }
                    break;
                case "D3":
                    if (packagecounter[address].iD3 <= 0)
                    {
                        if (string.IsNullOrEmpty(hexstring))
                            return packagecounter[address];
                        //byte to int
                        int total = int.Parse(hexstring, NumberStyles.HexNumber);
                        int result = total / 14;
                        if (total % 14 != 0 && result != 0)
                            result += 1;
                        packagecounter[address].iD3 = result;
                        return packagecounter[address];
                    }
                    else if (packagecounter[address].iD3 > 0)
                    {
                        packagecounter[address].iD3 -= 1;
                        return packagecounter[address];
                    }
                    break;
                case "DB":
                    if (packagecounter[address].iDB <= 0)
                    {
                        if (string.IsNullOrEmpty(hexstring))
                            return packagecounter[address];
                        //byte to int
                        int total = int.Parse(hexstring, NumberStyles.HexNumber);
                        int result = total / 14;
                        if (total % 14 != 0 && result != 0)
                            result += 1;
                        packagecounter[address].iDB = result;
                        return packagecounter[address];
                    }
                    else if (packagecounter[address].iDB > 0)
                    {
                        packagecounter[address].iDB -= 1;
                        return packagecounter[address];
                    }
                    break;
            }
            Debug.LogError("PackageCompute error. return null.");
            return packagecounter[address];
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
                    return "DU4 Comfort-BIC PW-TE";
                case "5":
                    return "DU6 Comfort-BIC2 PW ST";
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
            return string.Format("{0:00000}", int.Parse(input[14] + input[13], NumberStyles.HexNumber));
        }

        public static string GetYamahaFWver(string hexYY, string hexMM, string hexDD, string fsn)
        {
            
            return string.Format("20{0}{1}{2}{3}", hexYY, hexMM, hexDD, (char)int.Parse(fsn, NumberStyles.HexNumber));
        }

        public static string GetShimanoFWver(string svf1, string svf2, string svf3)
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

        private static string GetRingButton(string input)
        {
            switch (int.Parse(input, NumberStyles.HexNumber))
            {
                case 0:
                    return "Empty";
                case 1:
                    return "Up";
                case 2:
                    return "Down";
                case 3:
                    return "Info";
                case 4:
                    return "Light";
                case 5:
                    return "Walk assist";
                case 6:
                    return "Smart assist";
                default:
                    return "Unknow Ring Button.";
            }
        }

        #endregion
    }
}
