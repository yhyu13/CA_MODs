using System;
using System.Collections.Generic;
using CalradiaAwakensItems.Utils;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CalradiaAwakensItems
{
    public class MyApplicationInterface
    {
        public MyApplicationInterface()
        {
            instance = this;
            // TODO Dialog
            //MyXmlHelper.ReadEnglishLanguageFile("CalradiaAwakensItems_dialog", ref EnglishDiaglogToken);
            //MyXmlHelper.ReadEnglishLanguageFile("CalradiaAwakensItems_item", ref EnglishItemsName);
            //MyXmlHelper.ReadEquipmentSpecialPowerFile("CalradiaAwakensVlandia_item_special_power", ref ItemsSpecialPower);
        }

        public bool IsConversationPlayerCompanion()
        {
            return Hero.OneToOneConversationHero.IsPlayerCompanion || Hero.OneToOneConversationHero == Hero.MainHero.Spouse;
        }

        public static string AppendEnglishDialogToken(string token)
        {
            string value;
            return "{=" + token + "}" + (EnglishDiaglogToken.TryGetValue(token, out value) ? value : "DNE.");
        }

        /*
         * x-right, y-forward, z-up, right-handed coord
         */
        public static Mat3 LookAtWorld(in Vec3 At, in Vec3 Eye)
        {
            Vec3 yaxis = (At - Eye).NormalizedCopy();
            Vec3 xaxis = (Vec3.CrossProduct(yaxis, Vec3.Up)).NormalizedCopy();
            Vec3 zaxis = Vec3.CrossProduct(xaxis, yaxis);

            xaxis.w = -Vec3.DotProduct(xaxis, Eye);
            yaxis.w = -Vec3.DotProduct(yaxis, Eye);
            zaxis.w = -Vec3.DotProduct(zaxis, Eye);

            return new Mat3(xaxis, yaxis, zaxis);
        }

        public static MyApplicationInterface instance;

        public static Dictionary<string, string> EnglishItemsName = new Dictionary<string, string>();
        public static Dictionary<string, string> EnglishDiaglogToken = new Dictionary<string, string>();

        public static Dictionary<string, string> ItemsSpecialPower = new Dictionary<string, string>();
    }
}
