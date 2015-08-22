using System;
using System.Security.Cryptography;
using System.Text;

namespace Intelligent.Community.Infrastructure
{
    /// <summary>
    ///     表示用于整个Intelligent.Community的框架工具类。
    /// </summary>
    public class Utils
    {
        #region Private Fields
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Intelligent.Community.Logger");
        #endregion

        #region Public Static Methods

        #region Log Methods
        /// <summary>
        /// 将指定的字符串信息写入日志。
        /// </summary>
        /// <param name="message">需要写入日志的字符串信息。</param>
        public static void Log(string message)
        {
            log.Info(message);
        }

        /// <summary>
        /// 将指定的<see cref="Exception"/>实例详细信息写入日志。
        /// </summary>
        /// <param name="ex">需要将详细信息写入日志的<see cref="Exception"/>实例。</param>
        public static void Log(Exception ex)
        {
            log.Error("Exception caught", ex);
        }
        #endregion

        #region Encrypt/Decrypt Methods
        /// <summary>
        ///     字符串MD5加密。
        /// </summary>
        /// <param name="strValue">需要加密都字符串。</param>
        /// <param name="key">MD5通配Key。</param>
        /// <returns>加密后字符串。</returns>
        public static string MD5Encrypt(string strValue,string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(Encoding.Default.GetBytes(strValue + key));
            return BitConverter.ToString(result).Replace("-", "");
        }
        #endregion

        #region Encode/Decode Methods
        /// <summary>
        ///     服务器端Base64编码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }
        /// <summary>
        ///     服务器端Base64解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }
        #endregion

        #endregion
    }
}
