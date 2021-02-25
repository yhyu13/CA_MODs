using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CA_EagleRisingSpecialPowers.Utils
{
    public static class MyLogger
    {
        public static void Dump(string content, string fileName)
        {
            string inputUri = Path.Combine(BasePath.Name, "Modules", Main.ModNamePath, fileName);
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                ostrm = new FileStream(inputUri, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cannot open {inputUri} for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);
            Console.WriteLine(MyDateTime.GetLocalTime("en-US"));
            Console.WriteLine(content);
            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Done");
        }

        public static void SetTextVariable(string value)
        {
            MBTextManager.SetTextVariable("CalradiaAwakensItems_VALUE", value, false);
        }

        public static void DisplayInGameConsoleUserGetItem(string itemName)
        {
            MyLogger.DisplayInGameConsole("{=CalradiaAwakensItems_strAddADItem}", itemName);
        }

        public static void DisplayInGameConsole(string DisplayInGameConsoleInfo, string value = "")
        {
            string str = $"{Main.ModName}: {DisplayInGameConsoleInfo}";
            if (value != "")
            {
                MBTextManager.SetTextVariable("CalradiaAwakensItems_VALUE", value, false);
                str += "{CalradiaAwakensItems_VALUE}";
            }
            InformationManager.DisplayMessage(new InformationMessage(new TextObject(str, null).ToString()));
        }

        public static void DisplayInGameConsole(string DisplayInGameConsoleInfo, Color color, string value = "")
        {
            string str = $"{Main.ModName}: {DisplayInGameConsoleInfo}";
            if (value != "")
            {
                MBTextManager.SetTextVariable("CalradiaAwakensItems_VALUE", value, false);
                str += "{CalradiaAwakensItems_VALUE}";
            }
            InformationManager.DisplayMessage(new InformationMessage(new TextObject(str, null).ToString(), color));
        }
    }
}
