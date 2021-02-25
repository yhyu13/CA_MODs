using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CA_EagleRisingSpecialPowers.Utils
{
    public static class MyException
    {
        public static void DisplayInGameConsole(string message, Exception ex)
        {
            string str = $"{Main.ModName}: {message}. Exception: " + ((ex != null) ? ex.ToString() : "null");
            InformationManager.DisplayMessage(new InformationMessage(str, Color.White));
            MyLogger.Dump(str, "Exception.txt");
        }
    }
}
