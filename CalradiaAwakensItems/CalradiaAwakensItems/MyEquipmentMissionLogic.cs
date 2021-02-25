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

using CalradiaAwakensItems.Settings;
using CalradiaAwakensItems.Utils;
using CalradiaAwakensItems.Items;

namespace CalradiaAwakensItems
{
    internal class MyEquipmentMissionLogic : MissionLogic
    {
        private struct CalradiaAwakensItemsItemData
        {
            public ItemObject Item { get; set; }
            public int Num { get; set; }
            public float Spread { get; set; }
            public float SpeedDiff { get; set; }
        }

        /*
         * Init function
         */

        private void Initialize()
        {
            try
            {
                MyEquipmentList _equipmentData = MyXmlHelper.SettingsFor<MyEquipmentList>("EquipmentList");
                foreach (var equipmentData in _equipmentData.data)
                {
                    CalradiaAwakensItemsItemData data = new CalradiaAwakensItemsItemData();
                    {
                        data.Item = MyItems.GetItemObjectById(equipmentData.Name);
                        data.Num = int.Parse(equipmentData.ShotgunNum);
                        data.Spread = float.Parse(equipmentData.ShotgunSpread);
                        data.SpeedDiff = float.Parse(equipmentData.ShotgunSpeedDiff);
                    }
                    if (data.Item != null)
                    {
                        _items.Add(data);
                    }
                    else
                    {
                        MyException.DisplayInGameConsole(equipmentData.Name + " is not a valid item!", null);
                    }
                }
            }
            catch (Exception ex)
            {
                MyException.DisplayInGameConsole("Failed to load CalradiaAwakensItems.MyEquipmentMissionLogic: EquipmentList", ex);
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
        }

        /*
         * Handle agent shoot missile that can do automatic shotgun
         */

        public override void OnAgentShootMissile(Agent shooterAgent, EquipmentIndex weaponIndex, Vec3 position, Vec3 velocity, Mat3 orientation, bool hasRigidBody, int forcedMissileIndex)
        {
            base.OnAgentShootMissile(shooterAgent, weaponIndex, position, velocity, orientation, hasRigidBody, forcedMissileIndex);

            // Prevent recusive calling of this method
            if (_once)
            {
                return;
            }

            bool allowed = true;
            MissionWeapon? _weapon = null;
            MissionWeapon? _ammoWeapon = null;
            MissionWeapon _ammoWeapon2;

            _weapon = shooterAgent.Equipment[weaponIndex];
            _ammoWeapon = shooterAgent.Equipment[weaponIndex].AmmoWeapon;

            if (shooterAgent.Equipment[weaponIndex].CurrentUsageItem.IsRangedWeapon && shooterAgent.Equipment[weaponIndex].CurrentUsageItem.IsConsumable)
            {
                _ammoWeapon2 = shooterAgent.Equipment[weaponIndex];
            }
            else
            {
                _ammoWeapon2 = shooterAgent.Equipment[weaponIndex].AmmoWeapon;
            }

            bool found = false;
            int _ShotgunNum = -1;
            float _ShotgunSpread = -1f;
            float _SpeedDiff = -1f;
            // Find the correct equipment
            if (_weapon != null)// && _weapon?.CurrentUsageItem != null) // Hang Yu Oct4.2020 : e.1.5.3 changed
            {
                foreach (var data in _items)
                {
                    if (data.Item == _weapon?.Item)
                    {
                        found = true;
                        _ShotgunNum = data.Num;
                        _ShotgunSpread = data.Spread;
                        _SpeedDiff = data.SpeedDiff;
                        if (_ShotgunNum < 0)
                        {
                            _ShotgunNum = 0;
                        }
                        break;
                    }
                }
            }
            if (_ammoWeapon != null)// && _ammoWeapon?.CurrentUsageItem != null) // Hang Yu Oct4.2020 : e.1.5.3 changed
            {
                foreach (var data in _items)
                {
                    if (data.Item == _ammoWeapon?.Item)
                    {
                        found = true;
                        _ShotgunNum = data.Num;
                        _ShotgunSpread = data.Spread;
                        _SpeedDiff = data.SpeedDiff;
                        if (_ShotgunNum < 0)
                        {
                            _ShotgunNum = 0;
                        }
                        break;
                    }
                }
            }
            allowed &= found;

            if (allowed)
            {
                foreach (Mission.Missile missile in Mission.Current.Missiles)
                {
                    if (missile.ShooterAgent == shooterAgent)
                    {
                        _once = true;
                        for (int i = 0; i < _ShotgunNum; ++i)
                        {
                            // Create multiple copies of missile with randoness
                            var direction = velocity;
                            float missileSpeed = (float)_ammoWeapon2.CurrentUsageItem.MissileSpeed + MBRandom.RandomFloatNormal * _SpeedDiff;
                            if (missileSpeed < 1f)
                            {
                                missileSpeed = 1f;
                            }
                            direction.x += MBRandom.RandomFloatNormal * _ShotgunSpread;
                            direction.y += MBRandom.RandomFloatNormal * _ShotgunSpread;
                            direction.z += MBRandom.RandomFloatNormal * _ShotgunSpread;
                            float speed = direction.Normalize();
                            var dest = position + direction;
                            Mat3 _orientation = MyApplicationInterface.LookAtWorld(in dest, in position);
                            Mission.Current.AddCustomMissile(shooterAgent, _ammoWeapon2, position, direction, _orientation, missileSpeed, speed, hasRigidBody, missile.MissionObjectToIgnore, forcedMissileIndex);
                            foreach (MissionBehaviour missionBehaviour in Mission.Current.MissionBehaviours)
                            {
                                missionBehaviour.OnAgentShootMissile(shooterAgent, weaponIndex, position, direction, orientation, hasRigidBody, forcedMissileIndex);
                            }
                        }
                        _once = false;
                        break;
                    }
                }
            }
        }

        private bool _initialized = false;
        private bool _once = false;

        private List<CalradiaAwakensItemsItemData> _items = new List<CalradiaAwakensItemsItemData>();
    }
}