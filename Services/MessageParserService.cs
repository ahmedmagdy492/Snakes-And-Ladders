using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Services
{
    public static class MessageParserService
    {
        public static string Decode(byte[] data)
        {
            string decodedData = Encoding.UTF8.GetString(data);
            StringBuilder finalMsg = new StringBuilder();

            foreach (char c in decodedData)
            {
                if(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                {
                    finalMsg.Append(c);
                }
            }

            return finalMsg.ToString();
        }
    }
}
