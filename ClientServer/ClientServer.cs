using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ClientServer
{
    public enum MessageType
    {
        Error = 0,
        Authentification = 1,
        Message = 2,
        ListMessages = 3
    }
    public static class ClientServer
    {
        private static char[] splitArr = { ',' };
        public static string ParseMessage(string message, out MessageType messageType)
        {
            string[] spType = message.Split(splitArr, 2);
            messageType = (MessageType)Int32.Parse(spType[0]);
            return (spType.Length < 2) ? "" : spType[1];
        }

        public static string WriteResponse(MessageType mt, params string[] data)
        {
            List<string> message = new List<string>();
            message.Add(((int)mt).ToString());
            message.AddRange(data);
            return String.Join(",", message);
        }



    }
}

