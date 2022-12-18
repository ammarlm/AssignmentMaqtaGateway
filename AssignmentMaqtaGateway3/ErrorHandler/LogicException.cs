using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AssignmentMaqtaGateway3.ErrorHandler
{
    public class LogicException : Exception
    {
        public LogicException(string message) : base(message)
        {
        }
    }
}
