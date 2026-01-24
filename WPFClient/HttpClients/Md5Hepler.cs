using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.HttpClients
{
    public class Md5Hepler
    {
        /// <summary>
        /// Md5 utf-8 32
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetMd5(string content)
        {
            return string.Join("",MD5.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(content)).Select(x=>x.ToString("x2")));
        }
    }
}
