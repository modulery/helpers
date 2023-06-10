using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moduler.Helpers
{
    public static class LogHelper
    {
        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("Date=" + DateTime.Now.ToMyString() + "," + logMessage);
        }
    }
}
