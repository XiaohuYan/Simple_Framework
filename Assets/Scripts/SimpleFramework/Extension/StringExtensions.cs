using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SimpleFramework.Extension
{
    public static class StringExtensions
    {
        /// <summary>
        /// 使用 FNV1 计算字符串的哈希值
        /// Reference:https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function
        /// </summary>
        /// <param name="str">需要计算哈希值的字符串</param>
        /// <returns>哈希值</returns>
        public static int ComputeFNV1aHash(this string str)
        {
            uint hash = 2166136261;
            foreach (char c in str)
            {
                hash = (hash ^ c) * 16777619;
            }
            return unchecked((int)hash);
        }

        /// <summary>
        /// 根据文件流创建 MD5
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>MD5码</returns>
        public static string GetMD5(this string path)
        {
            using (FileStream file = new FileStream(path, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] md5Info = md5.ComputeHash(file);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < md5Info.Length; i++)
                {
                    stringBuilder.Append(md5Info[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }
    }

}
