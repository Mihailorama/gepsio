using System;
using System.Collections.Generic;
using System.Text;

namespace JeffFerguson.Gepsio
{
    public class XbrlException : Exception
    {
        public XbrlException(string Message)
            : base(Message)
        {
        }

        public XbrlException(string Message, Exception Inner)
            : base(Message, Inner)
        {
        }
    }
}
