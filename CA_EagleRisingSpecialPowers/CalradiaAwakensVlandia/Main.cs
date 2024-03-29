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

using CA_EagleRisingSpecialPowers.Settings;
using CA_EagleRisingSpecialPowers.Utils;
using CA_EagleRisingSpecialPowers.Items;

namespace CA_EagleRisingSpecialPowers
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
        }

        public static string ModName => "CA_EagleRisingSpecialPowers";
        public static string ModNamePath => "CA_EagleRising";
        public static string Version => "1.0.0";

        private MyApplicationInterface _interface = null;
    }
}
