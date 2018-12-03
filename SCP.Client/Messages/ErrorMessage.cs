using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP.Client.Messages
{
    public class UserMessage
    {
        public enum Type
        {
            Unknown,
            Info,
            Warning,
            Error
        }

        public string Title { get; set; }

        public Type MessageType { get; set; }

        public string Message { get; set; }

    }
}
