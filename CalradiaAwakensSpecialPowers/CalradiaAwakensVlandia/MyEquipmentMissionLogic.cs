using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using CalradiaAwakensSpecialPowers.Settings;
using CalradiaAwakensSpecialPowers.Utils;
using CalradiaAwakensSpecialPowers.Items;
using CalradiaAwakensSpecialPowers.SpecialPowers;

namespace CalradiaAwakensSpecialPowers
{
    internal class MyEquipmentMissionLogic : MissionLogic
    {
        /*
         * Init function
         */

        private void Initialize()
        {
            try
            {
                /*
                 * Init all equipments with special powers
                 */
                Dictionary<string, HashSet<string>> ItemsSpecialPower = new Dictionary<string, HashSet<string>>();
                MyXmlHelper.ReadEquipmentSpecialPowerFile("EquipmentSpecialPowers", ref ItemsSpecialPower);
                foreach (var item in ItemsSpecialPower)
                {
                    var equipment = MyItems.GetItemObjectById(item.Key);
                    if (equipment != null)
                    {
                        _items2SpecialPowers[equipment] = item.Value;
                    }
                    else
                    {
                        MyException.DisplayInGameConsole(item.Key + " is not a valid item!", null);
                    }
                }
            }
            catch (Exception ex)
            {
                MyException.DisplayInGameConsole("Failed to load settings for CalradiaAwakensSpecialPowers.MyEquipmentMissionLogic!", ex);
            }

            try
            {
                /*
                 * Init all special powers
                 */
                _specialPowers["OffenseAura1"] = new OffenseAura1();
                _specialPowers["OffenseAura2"] = new OffenseAura2();
                _specialPowers["OffenseAura3"] = new OffenseAura3();
                _specialPowers["DefenseAura1"] = new DefenseAura1();
                _specialPowers["DefenseAura2"] = new DefenseAura2();
                _specialPowers["DefenseAura3"] = new DefenseAura3();

                _specialPowers["HeroicAura1"] = new HeroicAura1();
                _specialPowers["HeroicAura2"] = new HeroicAura2();
                _specialPowers["HeroicAura3"] = new HeroicAura3();
                _specialPowers["DreadAura1"] = new DreadAura1();
                _specialPowers["DreadAura2"] = new DreadAura2();
                _specialPowers["DreadAura3"] = new DreadAura3();

                _specialPowers["HasteAura1"] = new HasteAura1();
                _specialPowers["HasteAura2"] = new HasteAura2();
                _specialPowers["HasteAura3"] = new HasteAura3();
                _specialPowers["TemporalChainedAura1"] = new TemporalChainedAura1();
                _specialPowers["TemporalChainedAura2"] = new TemporalChainedAura2();
                _specialPowers["TemporalChainedAura3"] = new TemporalChainedAura3();

                _specialPowers["SpringOfLifeAura1"] = new SpringOfLifeAura1();
                _specialPowers["SpringOfLifeAura2"] = new SpringOfLifeAura2();
                _specialPowers["SpringOfLifeAura3"] = new SpringOfLifeAura3();
                _specialPowers["DamagingBlowAura1"] = new DamagingBlowAura1();
                _specialPowers["DamagingBlowAura2"] = new DamagingBlowAura2();
                _specialPowers["DamagingBlowAura3"] = new DamagingBlowAura3();
            }
            catch (Exception ex)
            {
                MyException.DisplayInGameConsole("Failed to init special powers for CalradiaAwakensSpecialPowers.MyEquipmentMissionLogic!", ex);
            }
        }

        public override void OnMissionTick(float dt)
        {
            // Hang Yu Jan3.2021 : remove the Agent.Main check because this module should work even in tournament (where Agent.Main == null)
            if (!_initialized && Mission.Current != null)// && Agent.Main != null)
            {
                _initialized = true;
                Initialize();
            }

            if (!_initialized)
            {
                return;
            }

            // Add all agents with special powers to their corresponding special power handler

            // Mission.Current.Agents gets all active agents
            // Mission.Current.AllAgents gets all agents
            foreach (var agent in Mission.Current.AllAgents)
            {
                if (_alreadyManagedAgent.Add(agent))
                {
                    // Loop through all armor equipments
                    for (var i = EquipmentIndex.ArmorItemBeginSlot; i < EquipmentIndex.ArmorItemEndSlot; ++i)
                    {
                        var equipment = agent.Character?.Equipment?.GetEquipmentFromSlot(i).Item;
                        if (equipment != null)
                        {
                            // Check if this piece of equipment has special power
                            if (_items2SpecialPowers.ContainsKey(equipment))
                            {
                                foreach (var spName in _items2SpecialPowers[equipment])
                                {
                                    if (_specialPowers.TryGetValue(spName, out MySP sp))
                                    {
                                        sp.Register(agent);
                                    }
                                    else
                                    {
                                        MyException.DisplayInGameConsole(spName + " is not a managed special power!", null);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /*
             * Update all special powers
             */
            foreach (var item in _specialPowers)
            {
                item.Value.Tick(dt);
            }
        }

        // Hang Yu Oct4.2020 : e.1.5.3 changed
        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, int damage, in MissionWeapon affectorWeapon)
        {
            base.OnAgentHit(affectedAgent, affectorAgent, damage, affectorWeapon);

            string[] offsenAura = { "OffenseAura3", "OffenseAura2", "OffenseAura1" };
            foreach (var name in offsenAura)
            {
                BaseOffenseAura aura = (BaseOffenseAura)_specialPowers[name];
                if (aura.BuffedAgents.TryGetValue(affectorAgent, out bool buffed1))
                {
                    if (buffed1)
                    {
                        affectedAgent.Health -= aura.BuffAmount;
                        break;
                    }
                }
            }
        }

        private bool _initialized = false;

        private HashSet<Agent> _alreadyManagedAgent = new HashSet<Agent>();
        private Dictionary<ItemObject, HashSet<string>> _items2SpecialPowers = new Dictionary<ItemObject, HashSet<string>>();
        private Dictionary<string, MySP> _specialPowers = new Dictionary<string, MySP>();
    }
}