using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Lemon.Team.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string sessionKey = "5_2a2606fb-9311-4696-9b26-7ab6a570086e_fde02358842310199bf7fe147c97ad8a";
            string[] spar = sessionKey.Split('_');
            string serverSignal = GetSessionSignal(spar[0], spar[1]);
            if (serverSignal == spar[2])
            {
                Console.WriteLine("ok");
            }
            else
                Console.WriteLine("error");
            Console.Read();
        }

        private static string GetSessionSignal(string userID, string guid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("lemon20120530");
            sb.Append(userID);
            sb.Append(guid);
            MD5 newMd5 = new MD5CryptoServiceProvider();
            byte[] sourceBit = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] directBit = newMd5.ComputeHash(sourceBit);
            return BitConverter.ToString(directBit).Replace("-", "").ToLower();
        }
    }
}
