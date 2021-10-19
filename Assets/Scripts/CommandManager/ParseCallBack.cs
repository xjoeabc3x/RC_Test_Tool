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
        //package counter�W�L14byte�p��(�ٳѤU�X��package�n����)
        private static int packagecounter = 0;

        /// <summary>
        /// �ѪRRC/SG�^�ǰT��,!!!Notice:�@�w�n��Decode!!!
        /// </summary>
        /// <param name="datas">Ex[05]:FC,21,05,0A,37,08,15,01,45,14,00,00,00,00,00,00,00,00,0A,97</param>
        /// <returns>�U���O�۹������G</returns>
        public static string CallbackInfo(string address, string datas)
        {
            //Debug.Log(string.Format("OnUnity CallbackInfo :{0}|||{1}", address, datas));
            string[] data = datas.Split(',');
            if (data[0] == "FC")
            {
                if (data.Length >= 20)
                {
                    packagecounter = PackageCompute(data[3]);
                    switch (((byte)int.Parse(data[2], NumberStyles.HexNumber))&0xFF)
                    {//{ "02", "05", "09", "32", "12", "0A", "D4", "D1", "D2", "D3", "0D", "13", "0E", "37", "38", "39", "DD" }
                        case 0x01:
                            return address + "|01|" + Parse_01(datas);
                        case 0x02:
                            return address + "|02|" + Parse_02(datas);
                        case 0x05:
                            return address + "|05|" + Parse_05(datas);
                        case 0x09:
                            return address + "|09|" + Parse_09(datas);
                        case 0x0A:
                            return address + "|0A|" + Parse_0A(datas);
                        case 0x0D:
                            return address + "|0D|" + Parse_0D(datas);
                        case 0x0E:
                            return address + "|0E|" + Parse_0E(datas);
                        case 0x12:
                            return address + "|12|" + Parse_12(datas);
                        case 0x13:
                            return address + "|13|" + Parse_13(datas);
                        case 0x1A:
                            return address + "|1A|" + Parse_1A(datas);
                        case 0x2C:
                            return address + "|2C|" + Parse_2C(datas);
                        case 0x2D:
                            return address + "|2D|" + Parse_2D(datas);
                        case 0x30:
                            return address + "|30|" + Parse_30(datas);
                        case 0x32:
                            return address + "|32|" + Parse_32(datas);
                        case 0x33:
                            return address + "|33|" + Parse_33(datas);
                        case 0x37:
                            return address + "|37|" + Parse_37(datas);
                        case 0x38:
                            return address + "|38|" + Parse_38(datas);
                        case 0x39:
                            return address + "|39|" + Parse_39(datas);
                        case 0xD1:
                            return address + "|D1|" + Parse_D1(datas);
                        case 0xD2:
                            return address + "|D2|" + Parse_D2(datas);
                        case 0xD3:
                            return address + "|D3|" + Parse_D3(datas);
                        case 0xD4:
                            return address + "|D4|" + Parse_D4(datas);
                        case 0xD9:
                            return address + "|D9|" + Parse_D9(datas);
                        case 0xDA:
                            return address + "|DA|" + Parse_DA(datas);
                        case 0xDD:
                            return address + "|DD|" + Parse_DD(datas);
                        case 0xDE:
                            return address + "|DE|" + Parse_DE(datas);
                        default:
                            Debug.Log("Unknown TD :" + datas);
                            return "Unknown TD :" + datas;
                    }
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

        //Callback�Ȧs
        public static string[] DataCache = null;
        /// <summary>
        /// [01]Service Platform�s�u�T��
        /// </summary>
        /// <param name="input"></param>
        /// <returns>00:Fail, 01:success</returns>
        private static string Parse_01(string input)
        {
            string[] aes = GetAES(input);
            return aes[0];
        }
        /// <summary>
        /// [02]�Ť��s�u�T��
        /// </summary>
        /// <param name="input"></param>
        /// <returns>00:Fail, 01:success</returns>
        private static string Parse_02(string input)
        {
            string[] aes = GetAES(input);
            return aes[0];
        }
        /// <summary>
        /// �ѪR[05]RC/SG,UI���骩��,UI�w�骩��,ctg1,ctg2
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
        /// �ѪR[09]���F����,���F���骩��,���F�w�骩��,�Ͳ��y����,RCID
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
                //�̫�@�]
                case 1:
                    //�K�[�s��
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //�P�_���F
                    //�ѪR
                    DUFW = GetASCIItoString(DataCache, 0, 2);
                    DUHW = GetASCIItoString(DataCache, 8, 11);
                    SN = GetYamahaSN(DataCache[4], DataCache[5], DataCache[6], DataCache[7]);
                    evymc = GetRCID(DataCache);
                    switch (DUFW)
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
                            DUType = DU4orDU6(DataCache);
                            break;
                        case "1RC":
                            //DU4,6
                            DUType = DU4orDU6(DataCache);
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
                            DUFW = "EP800";
                            DUType = "DU16 EP8";
                            SN = GetShimanoSN(DataCache[5], DataCache[6], DataCache[7]);
                            break;
                        default:
                            Debug.Log("Unknow DU_Firmware.");
                            break;
                    }
                    //�M�żȦs
                    DataCache = null;
                    //packagecounter�k�s
                    packagecounter = 0;
                    return string.Format("{0},{1},{2},{3},{4}", DUType, DUFW, DUHW, SN, evymc);
                //�Ĥ@�]
                case 2:
                    //�@�Ȧs
                    DataCache = GetAES(input);
                    return "09_wait";
                default:
                    Debug.Log("[09] parse Error." + packagecounter);
                    return "";
            }
        }
        /// <summary>
        /// �ѪR[0A]���F��Tstct�N��Service tool�s�u������
        /// lstc�N��Z���W�@��Service tool�s�u�j�h�[
        /// fstc�N��Z���W�@��Service tool�s�u�j�h��
        /// baac�N��Boost�U�O�Ҧ��������q�y
        /// paac�N��Power�U�O�Ҧ��������q�y
        /// caac�N��Climb�U�O�Ҧ��������q�y
        /// naac�N��Normal�U�O�Ҧ��������q�y
        /// </summary>
        /// <param name="input"></param>
        /// <returns>stct,lstc,fstc,baac,paac,caac,naac,taac,eaac</returns>
        private static string Parse_0A(string input)
        {
            switch (packagecounter)
            {
                //�̫�@�]
                case 1:
                    //�K�[�s��
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //�ѪR
                    string stct = CombineByteToInt(DataCache[1] + "," + DataCache[0]).ToString();
                    string lstc = CombineByteToInt(DataCache[3] + "," + DataCache[2]).ToString() + "hr";
                    string fstc = CombineByteToInt(DataCache[5] + "," + DataCache[4]).ToString() + "km";
                    string baac = string.Format("{0:.00}A", ((decimal)CombineByteToInt(DataCache[7] + "," + DataCache[6])) / 10);
                    string paac = string.Format("{0:.00}A", ((decimal)CombineByteToInt(DataCache[9] + "," + DataCache[8])) / 10);
                    string caac = string.Format("{0:.00}A", ((decimal)CombineByteToInt(DataCache[11] + "," + DataCache[10])) / 10);
                    string naac = string.Format("{0:.00}A", ((decimal)CombineByteToInt(DataCache[13] + "," + DataCache[12])) / 10);
                    string taac = string.Format("{0:.00}A", ((decimal)CombineByteToInt(DataCache[15] + "," + DataCache[14])) / 10);
                    string eaac = string.Format("{0:.00}A", ((decimal)CombineByteToInt(DataCache[17] + "," + DataCache[16])) / 10);
                    //�M�żȦs
                    DataCache = null;
                    //packagecounter�k�s
                    packagecounter = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", stct, lstc, fstc, baac, paac, caac, naac, taac, eaac);
                //�Ĥ@�]
                case 2:
                    //�@�Ȧs
                    DataCache = GetAES(input);
                    return "0A_wait";
                default:
                    Debug.Log("[0A] parse Error." + packagecounter);
                    return "";
            }
        }
        /// <summary>
        /// �ѪR[0D]�D�q��Cell�������骩���Ͳ��y����
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
        /// �ѪR[0E]�D�q���R�q�`������,�R�q����,�j�q�y��q���100%
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
        /// �ѪR[12]���o���FODO,�`�ϥήɶ�
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
        /// �ѪR[13]�D(+��)�q���e�q,�D�q���ةR,�e���R���e�q
        /// </summary>
        /// <param name="input"></param>
        /// <returns>rsoc(%),eplife(%),fcc(Wh)</returns>
        private static string Parse_13(string input)
        {
            string[] aes = GetAES(input);
            string rsoc = string.Format("{0}%", ByteToInt_string(aes[0], 0));
            string eplife = string.Format("{0}%", ByteToInt_string(aes[1], 0));
            string fcc = string.Format("{0:.0}Wh", (decimal)CombineByteToInt(aes[3] + "," + aes[2])/10);
            return string.Format("{0},{1},{2}", rsoc, eplife, fcc);
        }
        /// <summary>
        /// �ѪR[1A]�]�m�O�_���\
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_1A(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// �ѪR[2C]���F�q��
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
        /// �ѪR[2D]�]�m�O�_���\
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_2D(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// �ѪR[30]�`���{,�W���^�t
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
        /// �ѪR[32]���[���XASCII to String
        /// </summary>
        /// <param name="input"></param>
        /// <returns>���[���X</returns>
        private static string Parse_32(string input)
        {
            switch (packagecounter)
            {
                //�̫�@�]
                case 1:
                    //�K�[�s��
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //�ѪR
                    string result = "";
                    for (int i = 0; i < 18; i++)
                    {
                        result += (char)(int.Parse(DataCache[i], NumberStyles.HexNumber));
                    }
                    //�M�żȦs
                    DataCache = null;
                    //packagecounter�k�s
                    packagecounter = 0;
                    return result;
                //�Ĥ@�]
                case 2:
                    //�@�Ȧs
                    DataCache = GetAES(input);
                    return "32_wait";
                default:
                    return "";
            }
        }
        /// <summary>
        /// �ѪR[33]�]�m���[���X�O�_���\
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_33(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// �ѪR[37]�ƹq���e�q,�ƹq���ةR,�e���R���e�q
        /// </summary>
        /// <param name="input"></param>
        /// <returns>rsoc(%),eplife(%),fcc(Wh)</returns>
        private static string Parse_37(string input)
        {
            string[] aes = GetAES(input);
            string rsoc = string.Format("{0}%", ByteToInt_string(aes[0], 0));
            string eplife = string.Format("{0}%", ByteToInt_string(aes[1], 0));
            string fcc = string.Format("{0:.0}Wh", (decimal)CombineByteToInt(aes[3] + "," + aes[2]) / 10);
            return string.Format("{0},{1},{2}", rsoc, eplife, fcc);
        }
        /// <summary>
        /// �ѪR[38]�ƹq��Cell�������骩���Ͳ��y����
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
        /// �ѪR[39]�ƹq���R�q�`������,�R�q����,�j�q�y��q���100%
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
        /// �ѪR[D1]Remote-1����
        /// </summary>
        /// <param name="input"></param>
        /// <returns>remote_type,fw_ver,hw_ver,pic_icon,pic_navi,pic_number,pic_eu_language,pic_tra_chinese,pic_sim_chinese,pic_jpn,pic_korea</returns>
        private static string Parse_D1(string input)
        {
            switch (packagecounter)
            {
                //�̫�@�]
                case 1:
                    //�K�[�s��
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //�ѪR
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
                    //�M�żȦs
                    DataCache = null;
                    //packagecounter�k�s
                    packagecounter = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", remote_type, fw_ver, hw_ver, icon, navi, number, eu_lan, tra_chi, sim_chi, jpn, korea);
                //�Ĥ@�]
                case 2:
                    //�@�Ȧs
                    DataCache = GetAES(input);
                    return "D1_wait";
                default:
                    return null;
            }
        }
        /// <summary>
        /// �ѪR[D2]Remote-2����
        /// </summary>
        /// <param name="input"></param>
        /// <returns>remote_type,fw_ver,hw_ver,pic_icon,pic_navi,pic_number,pic_eu_language,pic_tra_chinese,pic_sim_chinese,pic_jpn,pic_korea</returns>
        private static string Parse_D2(string input)
        {
            switch (packagecounter)
            {
                //�̫�@�]
                case 1:
                    //�K�[�s��
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //�ѪR
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
                    //�M�żȦs
                    DataCache = null;
                    //packagecounter�k�s
                    packagecounter = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", remote_type, fw_ver, hw_ver, icon, navi, number, eu_lan, tra_chi, sim_chi, jpn, korea);
                //�Ĥ@�]
                case 2:
                    //�@�Ȧs
                    DataCache = GetAES(input);
                    return "D2_wait";
                default:
                    return null;
            }
        }
        /// <summary>
        /// �ѪR[D3]Display����
        /// </summary>
        /// <param name="input">display_type,fw_ver,hw_ver,pic_icon,pic_navi,pic_number,pic_eu_language,pic_tra_chinese,pic_sim_chinese,pic_jpn,pic_korea</param>
        /// <returns></returns>
        private static string Parse_D3(string input)
        {
            switch (packagecounter)
            {
                //�̫�@�]
                case 1:
                    //�K�[�s��
                    DataCache = AddNewDatas(DataCache, GetAES(input));
                    //�ѪR
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
                    //�M�żȦs
                    DataCache = null;
                    //packagecounter�k�s
                    packagecounter = 0;
                    return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", display_type, fw_ver, hw_ver, icon, navi, number, eu_lan, tra_chi, sim_chi, jpn, korea);
                //�Ĥ@�]
                case 2:
                    //�@�Ȧs
                    DataCache = GetAES(input);
                    return "D3_wait";
                default:
                    return null;
            }
        }
        /// <summary>
        /// �ѪR[D4]SG�����t��O�_�s�b
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
        /// �ѪR[D9]EVO�I��Level,EVO�I���ƭ�,ONOFF�I��Level,ONOFF�I���ƭ�,���O�}�����A
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
        /// �ѪR[DA]�]�m�O�_���\
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_DA(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        /// <summary>
        /// �ѪR[DD]Ring�O�_�s�b�H�Ϋ��s���A
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
        /// �ѪR[DE]�]�m�O�_���\
        /// </summary>
        /// <param name="input"></param>
        /// <returns>0/1</returns>
        private static string Parse_DE(string input)
        {
            return int.Parse(GetAES(input)[0], NumberStyles.HexNumber).ToString();
        }
        #endregion

        #region [Tools]
        /// <summary>
        /// �զXbyte�^��int
        /// </summary>
        public static int CombineByteToInt(byte[] input)
        {
            if (input.Length <= 0)
                return -1;
            return CombineByteToInt(ByteArr_To_ByteString(input));
        }
        /// <summary>
        /// �զXbyteString[]�^��int
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
        /// �H�r�����Ϲj��String�զXbyte�^��int
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
        /// �զXbyte�^��int�r��
        /// </summary>
        /// <param name="input"></param>
        /// <param name="show">�n��ܴX�Ӧ��</param>
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
        /// ��byte array�ର�r��(0x5A -> 5A), �H�r���Ϲj
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
        /// �ഫ�H�r�����Ϲj��byteString�ܦ�byte[]
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
        /// �ഫstring[]�ܦ��H�r�����Ϲj��string
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
        /// �����ҿ�d��bit(index = 0~7),TD���Bit7������code������startIdx = 0
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
        /// �����ҿ�d��bit(index = 0~7),TD���Bit7������code������startIdx = 0
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
        /// �P�_RCType
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
        /// �������ļƭ�(14��byte)
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
        /// �p��o�ӫ��O���X�ӥ]
        /// </summary>
        /// <param name="hexstring">16�i��r��</param>
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
        /// ��Hex�r���ରASCII�r��
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
        /// ��J�r���ରbyte array
        /// </summary>
        /// <param name="input"></param>
        public static byte[] GetStringToASCIIbyte(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            return Encoding.ASCII.GetBytes(input);
        }
        /// <summary>
        /// �P�_�ODU4��DU6
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
        /// ���oRCID
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

        private static string GetRingButton(string input)
        {
            switch (int.Parse(input, NumberStyles.HexNumber))
            {
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
