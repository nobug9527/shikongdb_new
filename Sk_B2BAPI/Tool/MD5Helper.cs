using Sk_B2BAPI.App_Code;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sk_B2BAPI.Tool
{
    public class Md5Helper
    {
        #region Method

        /// <summary>
        /// 获得32位的MD5加密
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>返回值</returns>
        public static string GetMd532(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(input));
            var sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获得16位的MD5加密
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>返回值</returns>
        public static string GetMd516(string input)
        {
            return GetMd532(input).Substring(8, 16);
        }

        /// <summary>
        /// 获得8位的MD5加密
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>返回值</returns>
        public static string GetMd58(string input)
        {
            return GetMd532(input).Substring(8, 8);
        }

        /// <summary>
        /// 获得4位的MD5加密
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>返回值</returns>
        public static string GetMd54(string input)
        {
            return GetMd532(input).Substring(8, 4);
        }

        /// <summary>
        /// 添加MD5的前缀，便于检查有无篡改
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>返回值</returns>
        public static string AddMd5Profix(string input)
        {
            return GetMd54(input) + input;
        }

        /// <summary>
        /// 移除MD5的前缀
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>返回值</returns>
        public static string RemoveMd5Profix(string input)
        {
            return input.Substring(4);
        }

        /// <summary>
        /// 验证MD5前缀处理的字符串有无被篡改
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>返回值</returns>
        public static bool ValidateValue(string input)
        {
            bool res = false;
            if (input.Length >= 4)
            {
                string tmp = input.Substring(4);
                if (input.Substring(0, 4) == GetMd54(tmp))
                {
                    res = true;
                }
            }
            return res;
        }

        #endregion

        #region 其他加解密方法
        const string KEY_64 = "ms900930";
        const string IV_64 = "vt123456";
        /// <summary>
        /// 其他加密方法
        /// </summary>
        /// <param name="data">加密值</param>
        /// <returns></returns>
        public static string Encode(string data)
        {
            try
            {
                byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                int i = cryptoProvider.KeySize;
                MemoryStream ms = new MemoryStream();
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

                StreamWriter sw = new StreamWriter(cst);
                sw.Write(data);
                sw.Flush();
                cst.FlushFinalBlock();
                sw.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

            }
            catch (Exception ex)
            {
                Log.Error("加密异常Encode", ex.Message);
                return "";
            }


        }
        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="data">解密值</param>
        /// <returns></returns>
        public static string Decode(string data)
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
        #endregion
    }
    public class DES
    {
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt, string sKey)//加密方法
        {
            DESCryptoServiceProvider desCSP = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            desCSP.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            desCSP.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, desCSP.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder SB = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                SB.AppendFormat("{0:X2}", b);
            }
            SB.ToString();
            return SB.ToString();
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string pToDecrypt, string sKey)//解密
        {
            DESCryptoServiceProvider desCSP = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            desCSP.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            desCSP.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, desCSP.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
    }
}
