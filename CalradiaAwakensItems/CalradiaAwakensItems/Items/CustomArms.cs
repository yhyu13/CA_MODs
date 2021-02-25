using System;
using System.Collections.Generic;
using System.Xml.Linq;
using CalradiaAwakensItems.Utils;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace CalradiaAwakensItems.Items
{
    public class CustomArms
    {
        public CustomArms(string armsId)
        {
            this._armId = armsId;
        }

        public static void Load(CampaignGameStarter campaignGameStarter)
        {
            foreach(var items in MyApplicationInterface.EnglishItemsName)
            {
                var item = new CustomArms(items.Key);
                item.AddedToDialog(campaignGameStarter, "CalradiaAwakensItems_strAdd_" + items.Key);
                ListCustomArms[items.Key] = item;
            }
        }

        public void AddedToDialog(CampaignGameStarter campaignGameStarter, string playerText)
        {
            string text = string.Format("CalradiaAwakensItems_te_{0}", this._armId);
            string text2 = MyApplicationInterface.AppendEnglishDialogToken(playerText);
            string text3 = string.Format("CalradiaAwakensItems_te_{0}_1", this._armId);
            string text4 = string.Format("CalradiaAwakensItems_te_{0}_reply", this._armId);
            campaignGameStarter.AddPlayerLine(text, "CalradiaAwakensItems_add_equiment_reply", text3, text2, new ConversationSentence.OnConditionDelegate(MyApplicationInterface.instance.IsConversationPlayerCompanion), new ConversationSentence.OnConsequenceDelegate(this.HandleSuccesss), 100, new ConversationSentence.OnClickableConditionDelegate(this.HandleClickableCondition), null);
            campaignGameStarter.AddDialogLine(text4, text3, "CalradiaAwakensItems_add_equiment_reply", MyApplicationInterface.AppendEnglishDialogToken("CalradiaAwakensItems_strAddReply"), null, null, 100, null);
        }

        private void HandleSuccesss()
        {
            ItemObject itemObjectById = MyItems.GetItemObjectById(this._armId);
            Hero.MainHero.PartyBelongedTo.ItemRoster.Add(new ItemRosterElement(itemObjectById, 1, null));
            MyLogger.DisplayInGameConsoleUserGetItem(itemObjectById.Name.ToString());
        }

        private bool HandleClickableCondition(out TextObject explanation)
        {
            explanation = TextObject.Empty;
            return true;
        }

        public static bool IsLoadedSaveData = false;

        public static Dictionary<string, CustomArms> ListCustomArms = new Dictionary<string, CustomArms>();

        private string _armId;
    }
}
