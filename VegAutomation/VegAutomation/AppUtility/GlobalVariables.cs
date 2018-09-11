using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VegAutomation.AppUtlity
{
    static class GlobalVariables
    {
        private static string m_globalVarUserName = "";
        public static Boolean flag = true;
        public static string GlobalVarUserName
        {
            get { return m_globalVarUserName; }
            set { m_globalVarUserName = value; }
        }
    }
}
