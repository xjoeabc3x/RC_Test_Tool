using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public class AES : MonoBehaviour
{
    public static AES Instance = new AES();

    byte[] key = new byte[16 * (10 + 1)];
    int keyLen = 16;
    int expandKeyLen;
    /******************************************************************************/
    static byte[,] AES_Keybox = {
        {0x39,0xfa,0xd4,0xc3,0x93,0x42,0xae,0x41,0x42,0xa9,0xa7,0x77,0x89,0xa1,0x13,0xaf},
        {0x30,0xec,0x00,0xbd,0x96,0xf7,0x21,0x45,0xd8,0x46,0xb0,0x9a,0x87,0x29,0xa6,0x37},
        {0x6e,0x0d,0xe7,0xe3,0x04,0xae,0x67,0x2f,0xe4,0xa0,0xbc,0x3f,0xf5,0x04,0x4d,0x21},
        {0xb0,0xb9,0xc4,0x7a,0x62,0x67,0x67,0xd0,0x9d,0x40,0xe4,0x82,0xe2,0xd7,0x65,0xee},
        {0x5d,0x2c,0xb8,0xe0,0x04,0xb0,0x63,0x57,0xb0,0x75,0x92,0xf4,0xb2,0x61,0x84,0xc1},
        {0x0d,0x5e,0x2f,0x33,0x96,0x8a,0x63,0xee,0x5e,0xf1,0xfe,0x06,0x0e,0x29,0xce,0xf6},
        {0x58,0xed,0x11,0xd1,0xf8,0x82,0x82,0x22,0xe8,0x86,0x22,0x63,0x5b,0xc8,0x88,0xc1},
        {0x13,0xef,0x0a,0x98,0x51,0xff,0xf3,0x55,0x21,0xf2,0x06,0xc0,0xaa,0xd5,0xd6,0x06},
        {0x87,0x18,0xa0,0xef,0xea,0x5a,0xb7,0x35,0xec,0xbf,0x1d,0xa1,0xa2,0x39,0x19,0x8b},
        {0xa6,0x4c,0xd4,0x19,0x7a,0xe3,0x99,0x4c,0x19,0x1e,0xcc,0x98,0x26,0xb9,0x70,0x8d},
        {0xfa,0xac,0x80,0x64,0x4b,0xf8,0x46,0xdd,0xdf,0x7c,0xd0,0xfa,0x19,0x85,0xac,0x0b},
        {0x28,0x98,0xf9,0x81,0x44,0xb6,0xc3,0x09,0x64,0x06,0x7e,0xbf,0x27,0x15,0x6b,0x2b},
        {0x17,0xcb,0x16,0x36,0x14,0xab,0x6a,0xa3,0xe8,0x4d,0x26,0x87,0x4c,0x0f,0xd3,0x47},
        {0x2a,0xf5,0x57,0x69,0xae,0x8a,0xc8,0x0d,0x3b,0x45,0xad,0xaf,0x35,0xed,0xaa,0x06},
        {0xe7,0xc2,0x2e,0x96,0xb0,0x74,0x71,0x9c,0xcf,0x19,0x16,0x1c,0x69,0x41,0x79,0xf0},
        {0x96,0xb5,0xf6,0x8a,0xab,0xdf,0xe4,0xb8,0x7d,0x6e,0x65,0x67,0x51,0xcd,0xf3,0x9e}};
    // The following lookup tables and functions are for internal use only!
    byte[] AES_Sbox = {99,124,119,123,242,107,111,197,48,1,103,43,254,215,171,
        118,202,130,201,125,250,89,71,240,173,212,162,175,156,164,114,192,
        183,253,147,38,54,63,247,204,52,165,229,241,113,216,49,21,4,199,35,
        195,24,150,5,154,7,18,128,226,235,39,178,117,9,131,44,26,27,110,90,160,82,59,214,179,41,227,
        47,132,83,209,0,237,32,252,177,91,106,203,190,57,74,76,88,207,208,239,170,
        251,67,77,51,133,69,249,2,127,80,60,159,168,81,163,64,143,146,157,56,245,
        188,182,218,33,16,255,243,210,205,12,19,236,95,151,68,23,196,167,126,61,
        100,93,25,115,96,129,79,220,34,42,144,136,70,238,184,20,222,94,11,219,224,
        50,58,10,73,6,36,92,194,211,172,98,145,149,228,121,231,200,55,109,141,213,
        78,169,108,86,244,234,101,122,174,8,186,120,37,46,28,166,180,198,232,221,
        116,31,75,189,139,138,112,62,181,102,72,3,246,14,97,53,87,185,134,193,29,
        158,225,248,152,17,105,217,142,148,155,30,135,233,206,85,40,223,140,161,
        137,13,191,230,66,104,65,153,45,15,176,84,187,22};

    public static byte creat_crc(byte[] data, int num)
    {
        byte crc = 0;
        for (int i = 0; i < num; i++)
        {
            crc ^= data[i];
        }
        Debug.Log((int)crc);
        return crc;
    }

    private AES()
    {
        AES_Init();
    }
    byte[] AES_ShiftRowTab = { 0, 5, 10, 15, 4, 9, 14, 3, 8, 13, 2, 7, 12, 1, 6, 11 };

    byte[] AES_Sbox_Inv = new byte[256];
    byte[] AES_ShiftRowTab_Inv = new byte[16];
    byte[] AES_xtime = new byte[256];

    byte[] AES_SubBytes(byte[] state, byte[] sbox)
    {
        int i;
        for (i = 0; i < 16; i++)
            state[i] = sbox[state[i] & 0xFF];
        return state;
    }

    byte[] AES_AddRoundKey(byte[] state, byte[] rkey)
    {
        int i;
        for (i = 0; i < 16; i++)
            state[i] ^= rkey[i];
        return state;
    }

    byte[] AES_ShiftRows(byte[] state, byte[] shifttab)
    {
        byte[] h = copyOf(state, 16);
        int i;
        for (i = 0; i < 16; i++)
            state[i] = h[shifttab[i] & 0xFF];
        return state;
    }

    byte[] AES_MixColumns(byte[] state)
    {
        int i;
        for (i = 0; i < 16; i += 4)
        {
            byte s0 = state[i + 0], s1 = state[i + 1];
            byte s2 = state[i + 2], s3 = state[i + 3];
            byte h;
            h = (byte)(s0 ^ s1 ^ s2 ^ s3);
            state[i + 0] ^= (byte)(h ^ AES_xtime[(s0 ^ s1) & 0xFF]);
            state[i + 1] ^= (byte)(h ^ AES_xtime[(s1 ^ s2) & 0xFF]);
            state[i + 2] ^= (byte)(h ^ AES_xtime[(s2 ^ s3) & 0xFF]);
            state[i + 3] ^= (byte)(h ^ AES_xtime[(s3 ^ s0) & 0xFF]);
        }
        return state;
    }

    byte[] AES_MixColumns_Inv(byte[] state)
    {
        int i;
        for (i = 0; i < 16; i += 4)
        {
            byte s0 = state[i + 0], s1 = state[i + 1];
            byte s2 = state[i + 2], s3 = state[i + 3];
            byte h = (byte)(s0 ^ s1 ^ s2 ^ s3);
            byte xh = AES_xtime[h & 0xFF];
            byte h1 = (byte)(AES_xtime[AES_xtime[(xh ^ s0 ^ s2) & 0xFF] & 0xFF] ^ h);
            byte h2 = (byte)(AES_xtime[AES_xtime[(xh ^ s1 ^ s3) & 0xFF] & 0xFF] ^ h);
            state[i + 0] ^= (byte)(h1 ^ AES_xtime[(s0 ^ s1) & 0xFF]);
            state[i + 1] ^= (byte)(h2 ^ AES_xtime[(s1 ^ s2) & 0xFF]);
            state[i + 2] ^= (byte)(h1 ^ AES_xtime[(s2 ^ s3) & 0xFF]);
            state[i + 3] ^= (byte)(h2 ^ AES_xtime[(s3 ^ s0) & 0xFF]);
        }
        return state;
    }

    /* AES_ExpandKey: expand a cipher key. Depending on the desired encryption
       strength of 128, 192 or 256 bits 'key' has to be a byte array of length
       16, 24 or 32, respectively. The key expansion is done "in place", meaning
       that the array 'key' is modified.
    */
    byte[] AES_ExpandKey(byte[] key, int keyLen)
    {
        int kl = keyLen;
        int ks = 0;
        int Rcon = 1, i, j;
        byte[] temp = new byte[4];
        byte[] temp2 = new byte[4];
        switch (kl)
        {
            case 16: ks = 16 * (10 + 1); break;
            case 24: ks = 16 * (12 + 1); break;
            case 32: ks = 16 * (14 + 1); break;
        }
        for (i = kl; i < ks; i += 4)
        {
            temp = copyOfRange(key, i - 4, i);
            if (i % kl == 0)
            {
                temp2[0] = (byte)(AES_Sbox[temp[1] & 0xFF] ^ Rcon);
                temp2[1] = AES_Sbox[temp[2] & 0xFF];
                temp2[2] = AES_Sbox[temp[3] & 0xFF];
                temp2[3] = AES_Sbox[temp[0] & 0xFF];
                temp = copyOf(temp2, 4);
                if ((Rcon <<= 1) >= 256)
                    Rcon ^= 0x11b;
            }
            else if ((kl > 24) && (i % kl == 16))
            {
                temp2[0] = AES_Sbox[temp[0] & 0xFF];
                temp2[1] = AES_Sbox[temp[1] & 0xFF];
                temp2[2] = AES_Sbox[temp[2] & 0xFF];
                temp2[3] = AES_Sbox[temp[3] & 0xFF];
                temp = copyOf(temp2, 4);
            }
            for (j = 0; j < 4; j++)
                key[i + j] = (byte)(key[i + j - kl] & 0xFF ^ temp[j] & 0xFF);
        }
        expandKeyLen = ks;
        return key;
    }

    // AES_Init: initialize the tables needed at runtime.
    // Call this function before the (first) key expansion.
    void AES_Init()
    {
        int i;
        for (i = 0; i < 256; i++)
            AES_Sbox_Inv[AES_Sbox[i] & 0xFF] = (byte)i;

        for (i = 0; i < 16; i++)
            AES_ShiftRowTab_Inv[AES_ShiftRowTab[i] & 0xFF] = (byte)i;

        for (i = 0; i < 128; i++)
        {
            AES_xtime[i] = (byte)(i << 1);
            AES_xtime[128 + i] = (byte)((i << 1) ^ 0x1b);
        }
    }

    public static byte[] copyOfRange(byte[] Input, int start, int end)
    {
        byte[] newByte = new byte[Mathf.Abs(end - start) + 1];

        return newByte;
    }

    public static byte[] copyOf(byte[] Input, int newLength)
    {
        byte[] newByte = new byte[newLength];
        if (newLength > Input.Length)
        {
            for (int i = 0; i < newLength; i++)
            {
                if (i < Input.Length)
                {
                    newByte[i] = Input[i];
                }
                else if (i >= Input.Length)
                {
                    newByte[i] = (byte)0x00;
                }
            }
        }
        else if (newLength <= Input.Length)
        {
            for (int i = 0; i < newLength; i++)
            {
                newByte[i] = Input[i];
            }
        }
        return newByte;
    }

    // AES_Encrypt: encrypt the 16 byte array 'block' with the previously expanded key 'key'.
    byte[] AES_Encrypt(byte[] block, byte[] key, int keyLen)
    {
        int l = keyLen, i;
        AES_AddRoundKey(block, key);
        for (i = 16; i < l - 16; i += 16)
        {
            block = AES_SubBytes(block, AES_Sbox);
            block = AES_ShiftRows(block, AES_ShiftRowTab);
            block = AES_MixColumns(block);
            block = AES_AddRoundKey(block, copyOfRange(key, i, i + 16));
        }
        block = AES_SubBytes(block, AES_Sbox);
        block = AES_ShiftRows(block, AES_ShiftRowTab);
        block = AES_AddRoundKey(block, copyOfRange(key, i, i + 16));
        return block;
    }

    // AES_Decrypt: decrypt the 16 byte array 'block' with the previously expanded key 'key'.
    byte[] AES_Decrypt(byte[] block, byte[] key, int keyLen)
    {
        int l = keyLen, i;
        block = AES_AddRoundKey(block, copyOfRange(key, l - 16, l));
        block = AES_ShiftRows(block, AES_ShiftRowTab_Inv);
        block = AES_SubBytes(block, AES_Sbox_Inv);
        for (i = l - 32; i >= 16; i -= 16)
        {
            block = AES_AddRoundKey(block, copyOfRange(key, i, i + 16));
            block = AES_MixColumns_Inv(block);
            block = AES_ShiftRows(block, AES_ShiftRowTab_Inv);
            block = AES_SubBytes(block, AES_Sbox_Inv);
        }
        block = AES_AddRoundKey(block, key);
        return block;
    }
    // AES_Encode:
    public byte[] AES_Encode(byte key_num, byte[] block)
    {
        int i;
        for (i = 0; i < keyLen; i++)
            key[i] = AES_Keybox[key_num, i];

        key = AES_ExpandKey(key, keyLen);
        block = AES_Encrypt(block, key, expandKeyLen);
        return block;
    }

    // AES_Decode:
    public byte[] AES_Decode(byte key_num, byte[] block)
    {
        int i;
        for (i = 0; i < keyLen; i++)
            key[i] = AES_Keybox[key_num, i];

        key = AES_ExpandKey(key, keyLen);
        block = AES_Decrypt(block, key, expandKeyLen);
        return block;
    }









    /// <summary>
    /// AES 128 CBC加解密測試
    /// </summary>
    private void AES_128_CBC()
    {
        RijndaelManaged rijalg = new RijndaelManaged();

        //-----------------
        //設定 cipher 格式 AES-256-CBC
        rijalg.BlockSize = 128;
        rijalg.KeySize = 256;
        rijalg.FeedbackSize = 128;
        rijalg.Padding = PaddingMode.PKCS7;
        rijalg.Mode = CipherMode.CBC;

        rijalg.Key = (new SHA256Managed()).ComputeHash(Encoding.ASCII.GetBytes("IHazSekretKey"));
        rijalg.IV = Encoding.ASCII.GetBytes("1234567890123456");

        //-----------------
        //加密
        ICryptoTransform encryptor = rijalg.CreateEncryptor(rijalg.Key, rijalg.IV);

        byte[] encrypted;
        // Create the streams used for encryption.
        using (MemoryStream msEncrypt = new MemoryStream())
        {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write("text to be encrypted");
                }
                encrypted = msEncrypt.ToArray();
            }
        }

        //-----------------
        //加密後的 base64 字串 :
        //eiLbdhFSFrDqvUJmjbUgwD8REjBRoRWWwHHImmMLNZA=
        Debug.Log(Convert.ToBase64String(encrypted));

        //-----------------
        //解密
        ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

        string plaintext;
        // Create the streams used for decryption. 
        using (MemoryStream msDecrypt = new MemoryStream(encrypted))
        {
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {

                    // Read the decrypted bytes from the decrypting stream 
                    // and place them in a string.
                    plaintext = srDecrypt.ReadToEnd();
                }
            }
        }
        Debug.Log(plaintext);
    }
}
