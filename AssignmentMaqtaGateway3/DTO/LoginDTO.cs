using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentMaqtaGateway3.DTO
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public int ExpiredInMinute { get; set; }

    }
}
