using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

using CalradiaAwakensItems.Utils;
using CalradiaAwakensItems.Items;

namespace CalradiaAwakensItems.Dialog
{
	public class MainDialogBehavior : CampaignBehaviorBase
	{
		/*
		 *  Constructor
		 */
		public MainDialogBehavior()
		{
		}

		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		public override void SyncData(IDataStore dataStore)
		{
			try
			{
			}
			catch (Exception ex)
			{
				MyException.DisplayInGameConsole("Could not sync MainDialogBehavior data", ex);
			}
		}

		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{

			// Main dialog entry
			{
				campaignGameStarter.AddPlayerLine("CalradiaAwakensItems_cheat_choice", "hero_main_options", "CalradiaAwakensItems_cheat_choice_1", MyApplicationInterface.AppendEnglishDialogToken("CalradiaAwakensItems_strCheatChoice"), new ConversationSentence.OnConditionDelegate(MyApplicationInterface.instance.IsConversationPlayerCompanion), null,
				100, null, null);
				campaignGameStarter.AddDialogLine("CalradiaAwakensItems_cheat_choice_reply", "CalradiaAwakensItems_cheat_choice_1", "CalradiaAwakensItems_cheat_choice_reply", "...", null, null, 100, null);

				// Item dialog entry
				{
					campaignGameStarter.AddPlayerLine("CalradiaAwakensItems_add_equiment", "CalradiaAwakensItems_cheat_choice_reply", "CalradiaAwakensItems_add_equiment_1", MyApplicationInterface.AppendEnglishDialogToken("CalradiaAwakensItems_strArmory"), new ConversationSentence.OnConditionDelegate(MyApplicationInterface.instance.IsConversationPlayerCompanion), null, 100, null, null);
					campaignGameStarter.AddDialogLine("CalradiaAwakensItems_add_equiment_reply", "CalradiaAwakensItems_add_equiment_1", "CalradiaAwakensItems_add_equiment_reply", "...", null, null, 100, null);
					this.AddEquipDialog(campaignGameStarter);
				}
				campaignGameStarter.AddPlayerLine("CalradiaAwakensItems_resume", "CalradiaAwakensItems_cheat_choice_reply", "lord_pretalk", MyApplicationInterface.AppendEnglishDialogToken("CalradiaAwakensItems_strResume"), null, null, 100, null, null);
			}
		}

		public void AddEquipDialog(CampaignGameStarter campaignGameStarter)
		{
			CustomArms.Load(campaignGameStarter);
			campaignGameStarter.AddPlayerLine("CalradiaAwakensItems_equiment_resume", "CalradiaAwakensItems_add_equiment_reply", "CalradiaAwakensItems_cheat_choice_1", MyApplicationInterface.AppendEnglishDialogToken("CalradiaAwakensItems_strResume"), null, null, 100, null, null);
		}

	}
}
