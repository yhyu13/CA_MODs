using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using CalradiaAwakensSpecialPowers.Utils;
using CalradiaAwakensSpecialPowers.Settings;

namespace CalradiaAwakensSpecialPowers.SpecialPowers
{
    internal class MySP
    {
        protected virtual void InitSettings(string className)
        {
            MyBaseSpecialPowerSettings setttings = MyXmlHelper.SettingsFor<MyBaseSpecialPowerSettings>(className + "Settings");
            _hasParticle = bool.Parse(setttings.HasParticle);
            if (setttings.ShoudRepeatParticle != null)
            {
                _shouldRepeatParticle = bool.Parse(setttings.ShoudRepeatParticle);
            }
            if (setttings.RepeatParticlePeriod != null)
            {
                _repeatParticlePeriod = float.Parse(setttings.RepeatParticlePeriod);
            }
            _particleEffectName = setttings.ParticleEffectName;
            foreach (var index in setttings.ParticleBoneIndexes.Split(','))
            {
                _particleBoneIndexes.Add(int.Parse(index));
            }

            _hasLight = bool.Parse(setttings.HasLight);
            _lightBone = int.Parse(setttings.LightBone);
            _lightRadius = float.Parse(setttings.LightRadius);
            _lightIntensity = float.Parse(setttings.LightIntensity);
            _lightColor = Color.ConvertStringToColor(setttings.LightColor).ToVec3();
        }

        public void Register(Agent agent)
        {
            this._registeredAgents.Add(agent);
        }

        public virtual void Tick(float dt)
        {
            foreach (var agent in this._registeredAgents.Where(agent => (!agent.IsActive() || agent.Health <= 0))
                         .ToList())
            {
                this._registeredAgents.Remove(agent);
            }
            foreach (var item in this._agentBuffed.Where(pair => (!pair.Key.IsActive() || pair.Key.Health <= 0))
                        .ToList())
            {
                this._agentBuffed.Remove(item.Key);
            }
            foreach (var item in this._agentBuffParticleDatas.Where(pair => (!pair.Key.IsActive() || pair.Key.Health <= 0))
                        .ToList())
            {
                RemoveBuffParticleEffect(item.Key, item.Value);
                this._agentBuffParticleDatas.Remove(item.Key);
            }

            UpdateBuffParticleEffect();
        }

        protected virtual void UpdateBuffParticleEffect()
        {
            // Update agents buff status
            foreach (var item in this._agentBuffed)
            {
                Agent agent = item.Key;
                bool buffed = item.Value;

                if (_onlyApplyParticleToRegisterdAgent && !this._registeredAgents.Contains(agent))
                {
                    continue;
                }

                // Process buffed agents
                if (buffed)
                {
                    // Keep playing partcile effects to managed agents that are buffed
                    if (this._agentBuffParticleDatas.TryGetValue(agent, out BuffParticleData particleData))
                    {
                        particleData.shoudPlay = true;
                        if (particleData.playingTimer.Check(reset: true) && _shouldRepeatParticle)
                        {
                            RemoveBuffParticleEffect(agent, particleData);
                            CreateBuffParticleEffect(agent, out BuffParticleData newParticleData);
                            if (newParticleData != null)
                            {
                                _agentBuffParticleDatas[agent] = newParticleData;
                            }
                        }
                    }
                    // Add buff particle effect to unmanaged agents that are buffed
                    else
                    {
                        CreateBuffParticleEffect(agent, out BuffParticleData newParticleData);
                        if (newParticleData != null)
                        {
                            this._agentBuffParticleDatas.Add(agent, newParticleData);
                        }
                    }
                }
                // Process unbuffed agents
                else
                {
                    if (this._agentBuffParticleDatas.TryGetValue(agent, out BuffParticleData particleData))
                    {
                        particleData.shoudPlay = false;
                    }
                }
            }

            // Update agent buff particle effects
            List<Agent> toRemove = new List<Agent>();
            foreach (var item in this._agentBuffParticleDatas)
            {
                Agent agent = item.Key;
                BuffParticleData particleData = item.Value;
                if (!particleData.shoudPlay && particleData.playingTimer.Check())
                {
                    RemoveBuffParticleEffect(agent, particleData);
                    toRemove.Add(agent);
                }
            }
            foreach (var agent in toRemove)
            {
                this._agentBuffParticleDatas.Remove(agent);
            }
        }

        /*
         * Create partcile
         */

        protected virtual void CreateBuffParticleEffect(Agent agent, out BuffParticleData particleData)
        {
            particleData = null;

            if (!agent.IsActive() || agent.Health <= 0)
            {
                return;
            }

            MBAgentVisuals agentVisuals = agent.AgentVisuals;
            if (agentVisuals == null)
            {
                return;
            }

            // Weird agent.Equipment==null for some weapons in CalradiaAwakensItems
            EquipmentIndex index = EquipmentIndex.None;
            GameEntity wieldedWeaponEntity = null;
            //if (agent.IsPlayerControlled)
            {
                if (agent.Equipment == null)
                {
                    return;
                }

                index = agent.GetWieldedItemIndex(Agent.HandIndex.OffHand);
                if (index == EquipmentIndex.None)
                {
                    index = agent.GetWieldedItemIndex(Agent.HandIndex.MainHand);
                }
                if (index == EquipmentIndex.None)
                {
                    return;
                }

                if (_shouldRepeatParticle)
                {
                    MissionWeapon _ammoWeapon2 = agent.Equipment[index];

                    if (_ammoWeapon2.CurrentUsageItem?.IsRangedWeapon ?? false)
                    {
                        return;
                    }

                    //switch (_ammoWeapon2.CurrentUsageItem?.WeaponClass)
                    //{
                    //    case WeaponClass.Arrow:
                    //    case WeaponClass.Bolt:
                    //    case WeaponClass.Bow:
                    //    case WeaponClass.Crossbow:
                    //        return;
                    //}
                }

                wieldedWeaponEntity = agent.GetWeaponEntityFromEquipmentSlot(index);
            }

            particleData = new BuffParticleData();
            particleData.shoudPlay = true;
            particleData.playingTimer = new MissionTimer(_repeatParticlePeriod);

            Skeleton skeleton = agentVisuals.GetSkeleton();

            // Buff particle effect
            if (this._hasParticle && this._particleEffectName != null)
            {
                particleData.particles = new ParticleSystem[this._particleBoneIndexes.Count];
                for (byte i = 0; i < this._particleBoneIndexes.Count; i++)
                {
                    MatrixFrame localFrame = new MatrixFrame(Mat3.Identity, new Vec3(0, 0, 0));

                    //if (agent.IsPlayerControlled)
                    {
                        ParticleSystem particle = ParticleSystem.CreateParticleSystemAttachedToEntity(this._particleEffectName,
                        wieldedWeaponEntity, ref localFrame);
                        if (particle != null)
                        {
                            skeleton.AddComponentToBone(this._particleBoneIndexes[i], particle);
                            particleData.particles[i] = particle;
                        }
                    }
                    //else
                    //{
                    //    ParticleSystem particle = ParticleSystem.CreateParticleSystemAttachedToBone(this._particleEffectName, skeleton, (sbyte)this._particleBoneIndexes[i], ref localFrame);
                    //    if (particle != null)
                    //    {
                    //        particleData.particles[i] = particle;
                    //    }
                    //}
                }
            }

            //The particle effect/lighting could only be shown after dropping and re-equiping the parent item.
            //if (agent.IsPlayerControlled)
            {
                agent.DropItem(index);
                SpawnedItemEntity spawnedItemEntity = wieldedWeaponEntity.GetFirstScriptOfType<SpawnedItemEntity>();
                if (spawnedItemEntity != null)
                {
                    agent.OnItemPickup(spawnedItemEntity, EquipmentIndex.None, out bool removeItem);
                }
                particleData.parentEntity = wieldedWeaponEntity;
            }

            // Buff lightning effect
            if (this._hasLight && this._lightRadius > 0 && this._lightIntensity > 0 && this._lightBone >= 0)
            {
                Light light = Light.CreatePointLight(this._lightRadius);
                light.ShadowEnabled = false;
                light.Intensity = this._lightIntensity;
                light.LightColor = this._lightColor;
                light.SetVisibility(true);
                skeleton.AddComponentToBone(this._lightBone, light);
                particleData.light = light;
            }
        }

        /*
         * Remove particle
         */

        protected virtual void RemoveBuffParticleEffect(Agent agent, BuffParticleData particleData)
        {
            GameEntity entity = particleData.parentEntity;
            if (entity != null && particleData.particles != null)
            {
                foreach (var particle in particleData.particles)
                {
                    particleData.parentEntity.RemoveComponent(particle);
                }
            }
            MBAgentVisuals agentVisuals = agent.AgentVisuals;
            if (agentVisuals != null)
            {
                Skeleton skeleton = agentVisuals.GetSkeleton();
                if (skeleton != null && particleData.light != null)
                {
                    skeleton.RemoveComponent(particleData.light);
                }
                if (skeleton != null && particleData.particles != null)
                {
                    foreach (var particle in particleData.particles)
                    {
                        skeleton.RemoveComponent(particle);
                    }
                }
            }
        }

        // Reference : FireLord mod - FireArrowLogic.cs
        protected class BuffParticleData
        {
            public bool shoudPlay = false;
            public MissionTimer playingTimer;
            public GameEntity parentEntity;
            public Light light;
            public ParticleSystem[] particles;
        }

        public Dictionary<Agent, bool> BuffedAgents
        {
            get
            {
                return new Dictionary<Agent, bool>(this._agentBuffed);
            }
        }

        protected static int[] ParticleBoneIndexes = { 0, 1, 2, 3, 5, 6, 7, 9, 12, 13, 15, 17, 22, 24 };

        protected HashSet<Agent> _registeredAgents = new HashSet<Agent>();
        protected Dictionary<Agent, bool> _agentBuffed = new Dictionary<Agent, bool>();
        private Dictionary<Agent, BuffParticleData> _agentBuffParticleDatas = new Dictionary<Agent, BuffParticleData>();

        protected bool _onlyApplyParticleToRegisterdAgent = true;
        protected bool _hasParticle = false;
        protected bool _shouldRepeatParticle = false;
        protected float _repeatParticlePeriod = 1f;
        protected string _particleEffectName = null;
        protected List<int> _particleBoneIndexes = new List<int>();

        protected bool _hasLight = false;
        protected int _lightBone = 0;
        protected float _lightRadius = 10f;
        protected float _lightIntensity = 125f;
        protected Vec3 _lightColor = Color.White.ToVec3();
    }
}