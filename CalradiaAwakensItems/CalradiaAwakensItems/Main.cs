﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using CalradiaAwakensItems.Settings;
using CalradiaAwakensItems.Utils;
using CalradiaAwakensItems.Items;
using CalradiaAwakensItems.Dialog;

namespace CalradiaAwakensItems
{
    public class Main : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            try
            {
                base.OnSubModuleLoad();
                if (_interface == null)
                {
                    _interface = new MyApplicationInterface();
                }
            }
            catch (Exception ex)
            {
                MyException.DisplayInGameConsole("Failed at OnSubModuleLoad()!", ex);
            }
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            try
            {
                base.OnMissionBehaviorInitialize(mission);
                AddMissionBehaviors(mission);
            }
            catch (Exception ex)
            {
                MyException.DisplayInGameConsole("Failed to initialize on mission start!", ex);
            }
        }

        private void AddMissionBehaviors(Mission mission)
        {
            // Loading brand new logic on each mission start instead of reusing previous states.
            mission.AddMissionBehavior(new MyEquipmentMissionLogic());
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            try
            {
                base.OnCampaignStart(game, gameStarterObject);

                if (game.GameType is Campaign)
                {
                    // Add campaign behavior
                    AddCampaignBehaviors(gameStarterObject as CampaignGameStarter);
                }
            }
            catch (Exception ex)
            {
                MyException.DisplayInGameConsole("Failed to load on campaign start!", ex);
            }
        }

        private void AddCampaignBehaviors(CampaignGameStarter starter)
        {
            //TODO Dialog
            //starter.AddBehavior(new MainDialogBehavior());
        }

        public static string ModName => "CalradiaAwakensItems";
        public static string ModNamePath => "CalradiaAwakensItems";
        public static string Version => "1.0.0";

        private MyApplicationInterface _interface = null;
    }
}