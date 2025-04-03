using SnakeAndLadders.Helpers;
using SnakeAndLadders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Services
{
    public static class MessageParserService
    {
        public static string DecodeString(byte[] data)
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

        public static GameProtocol Decode(byte[] rawData)
        {
            int dataLen = BitConverter.ToInt32([rawData[2], rawData[3], rawData[4], rawData[5]]);
            byte[] data = new byte[dataLen];
            Array.Copy(rawData, 6, data, 0, dataLen);
            return new GameProtocol
            {
                Type = (MessageType)BitConverter.ToInt16([rawData[0], rawData[1]]),
                DataLen = dataLen,
                Data = data,
            };
        }

        public static byte[] Encode(GameProtocol msg)
        {
            byte[] data = new byte[Constants.MSG_TYPE_LEN + Constants.MSG_DATA_LEN_LEN + msg.DataLen];
            byte[] msgType = BitConverter.GetBytes((short)msg.Type);
            Array.Copy(msgType, 0, data, 0, msgType.Length);
            byte[] dataLen = BitConverter.GetBytes(msg.DataLen);
            Array.Copy(dataLen, 0, data, 2, dataLen.Length);
            Array.Copy(msg.Data, 0, data, 6, msg.Data.Length);

            return data;
        }
    }
}
