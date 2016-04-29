using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WattsUpFramework
{
    public static class WUCommands
    {
        public static Char Abort = (char)030;
        public static string RequestData = "#D,R,0;";
        public static string ExternalModeStartString = "#L,W,3,E,0,";
        public static string ExternalModeEndString = ";";
        public static string RequestHeaderRecord = "#H,R,0;";
        public static string ResetDataMemory = "#R,W,0;";
        public static string SoftRestart = "#V,W,0;";
        public static string VersionRequest = "#V,R,0;";
    }
}
