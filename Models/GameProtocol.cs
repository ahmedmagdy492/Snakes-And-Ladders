using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAndLadders.Models
{
    public enum MessageType : short
    {
        GameStart = 1,
    }

    public class GameProtocol
    {
        public MessageType Type { get; set; }
        public int DataLen { get; set; }
        public byte[] Data { get; set; }
    }
}
