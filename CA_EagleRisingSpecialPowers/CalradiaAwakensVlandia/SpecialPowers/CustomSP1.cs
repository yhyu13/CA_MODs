using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using CA_EagleRisingSpecialPowers.Utils;
using CA_EagleRisingSpecialPowers.Settings;

namespace CA_EagleRisingSpecialPowers.SpecialPowers
{
    internal class BaseAura : MySP
    {
        public float BuffAmount
        {
            get
            {
                return this._buffAmount;
            }
        }

        protected float _auraRadius = 5;
        protected float _buffAmount = 5;
        protected bool _applyToFriendly = true;
        protected MissionTimer _buffTimer = new MissionTimer(1.0f);

        protected override void InitSettings(string className)
        {
            MyBaseAuraSettings setttings = MyXmlHelper.SettingsFor<MyBaseAuraSettings>(className + "Settings");

            // Particle
            if (setttings.HasParticle != null)
            {
                _hasParticle = bool.Parse(setttings.HasParticle);
            }
            if (setttings.ParticleToEquippedOnly != null)
            {
                _onlyApplyParticleToRegisterdAgent = bool.Parse(setttings.ParticleToEquippedOnly);
            }
            if (setttings.ShoudRepeatParticle != null)
            {
                _shouldRepeatParticle = bool.Parse(setttings.ShoudRepeatParticle);
            }
            if (setttings.RepeatParticlePeriod != null)
            {
                _repeatParticlePeriod = float.Parse(setttings.RepeatParticlePeriod);
            }

            _particleEffectName = setttings.ParticleEffectName;

            if (setttings.ParticleBoneIndexes != null)
            {
                foreach (var index in setttings.ParticleBoneIndexes.Split(','))
                {
                    _particleBoneIndexes.Add(sbyte.Parse(index));
                }
            }

            // Lighting
            if (setttings.HasLight != null)
            {
                _hasLight = bool.Parse(setttings.HasLight);
            }
            if (setttings.LightBone != null)
            {
                _lightBone = sbyte.Parse(setttings.LightBone);
            }
            if (setttings.LightRadius != null)
            {
                _lightRadius = float.Parse(setttings.LightRadius);
            }
            if (setttings.LightIntensity != null)
            {
                _lightIntensity = float.Parse(setttings.LightIntensity);
            }
            if (setttings.LightColor != null)
            {
                _lightColor = Color.ConvertStringToColor(setttings.LightColor).ToVec3();
            }

            // Aura
            if (setttings.AuraRadius != null)
            {
                _auraRadius = float.Parse(setttings.AuraRadius);
            }
            if (setttings.BuffAmount != null)
            {
                _buffAmount = float.Parse(setttings.BuffAmount);
            }
            if (setttings.BuffPeriod != null)
            {
                _buffTimer = new MissionTimer(float.Parse(setttings.BuffPeriod));
            }
        }
    }

    internal class BaseOffenseAura : BaseAura
    {
        public override void Tick(float dt)
        {
            base.Tick(dt);

            foreach (var target in Mission.Current.AllAgents)
            {
                // Already in buff list
                if (_agentBuffed.ContainsKey(target))
                {
                    bool buffed = false;
                    // Activate buff if within at least one aura
                    foreach (var agent in _registeredAgents)
                    {
                        var distance = (target.Position - agent.Position).Normalize();
                        buffed |= (distance < _auraRadius);
                    }
                    _agentBuffed[target] = buffed;
                }
                else
                {
                    foreach (var agent in _registeredAgents)
                    {
                        // Buff condition1
                        if (target.IsFriendOf(agent))
                        {
                            var distance = (target.Position - agent.Position).Normalize();
                            // Buff condition2
                            if (distance < _auraRadius)
                            {
                                _agentBuffed[target] = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    internal class BaseDefenseAura : BaseAura
    {
        public override void Tick(float dt)
        {
            base.Tick(dt);

            foreach (var target in Mission.Current.AllAgents)
            {
                // Already in buff list
                if (_agentBuffed.ContainsKey(target))
                {
                    bool buffed = false;
                    // Activate buff if within at least one aura
                    foreach (var agent in _registeredAgents)
                    {
                        var distance = (target.Position - agent.Position).Normalize();
                        buffed |= (distance < _auraRadius);
                    }
                    _agentBuffed[target] = buffed;
                }
                else
                {
                    foreach (var agent in _registeredAgents)
                    {
                        // Buff condition1
                        if (target.IsFriendOf(agent))
                        {
                            var distance = (target.Position - agent.Position).Normalize();
                            // Buff condition2
                            if (distance < _auraRadius)
                            {
                                _agentBuffed[target] = true;
                                target.OnAgentHealthChanged += delegate (Agent hitAgent, float oldHealth, float newHealth)
                                {
                                    bool isbuffed = false;
                                    _agentBuffed.TryGetValue(target, out isbuffed);
                                    if (isbuffed && oldHealth > newHealth && newHealth > 0f)
                                    {
                                        newHealth += (float)_buffAmount;
                                        if (newHealth > oldHealth)
                                        {
                                            newHealth = oldHealth;
                                            newHealth -= 1f;
                                        }
                                        MyApplicationInterface.SetNotPublicVar(target, "_health", newHealth);
                                    }
                                };
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    internal class BaseMoraleAura : BaseAura
    {
        public override void Tick(float dt)
        {
            base.Tick(dt);

            foreach (var target in Mission.Current.AllAgents)
            {
                // Already in buff list
                if (_agentBuffed.ContainsKey(target))
                {
                    bool buffed = false;
                    // Activate buff if within at least one aura
                    foreach (var agent in _registeredAgents)
                    {
                        var distance = (target.Position - agent.Position).Normalize();
                        buffed |= (distance < _auraRadius);
                    }
                    _agentBuffed[target] = buffed;
                }
                else
                {
                    foreach (var agent in _registeredAgents)
                    {
                        bool condition = (_applyToFriendly) ? target.IsFriendOf(agent) : target.IsEnemyOf(agent);
                        // Buff condition1
                        if (condition)
                        {
                            var distance = (target.Position - agent.Position).Normalize();
                            // Buff condition2
                            if (distance < _auraRadius)
                            {
                                _agentBuffed[target] = true;
                                target.SetMorale(target.GetMorale() + _buffAmount);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    internal class BaseMovementSpeedAura : BaseAura
    {
        public override void Tick(float dt)
        {
            base.Tick(dt);

            foreach (var target in Mission.Current.AllAgents)
            {
                // Already in buff list
                if (_agentBuffed.ContainsKey(target))
                {
                    bool buffed = false;
                    // Activate buff if within at least one aura
                    foreach (var agent in _registeredAgents)
                    {
                        var distance = (target.Position - agent.Position).Normalize();
                        buffed |= (distance < _auraRadius);
                    }
                    _agentBuffed[target] = buffed;
                    target.SetMaximumSpeedLimit((buffed) ? _buffAmount : 1f, true);
                }
                else
                {
                    foreach (var agent in _registeredAgents)
                    {
                        bool condition = (_applyToFriendly) ? target.IsFriendOf(agent) : target.IsEnemyOf(agent);
                        // Buff condition1
                        if (condition)
                        {
                            var distance = (target.Position - agent.Position).Normalize();
                            // Buff condition2
                            if (distance < _auraRadius)
                            {
                                _agentBuffed[target] = true;
                                target.SetMaximumSpeedLimit(_buffAmount, true);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    internal class BaseHealAura : BaseAura
    {
        public override void Tick(float dt)
        {
            base.Tick(dt);

            bool isTimered = _buffTimer.Check(reset: true);

            foreach (var target in Mission.Current.AllAgents)
            {
                // Already in buff list
                if (_agentBuffed.ContainsKey(target))
                {
                    bool buffed = false;
                    // Activate buff if within at least one aura
                    foreach (var agent in _registeredAgents)
                    {
                        var distance = (target.Position - agent.Position).Normalize();
                        buffed |= (distance < _auraRadius);
                    }
                    _agentBuffed[target] = buffed;
                    if (isTimered && buffed)
                    {
                        var newHealth = target.Health + BuffAmount;
                        target.Health = (newHealth < target.HealthLimit) ? newHealth : target.HealthLimit;
                    }
                }
                else
                {
                    foreach (var agent in _registeredAgents)
                    {
                        bool condition = (_applyToFriendly) ? target.IsFriendOf(agent) : target.IsEnemyOf(agent);
                        // Buff condition1
                        if (condition)
                        {
                            var distance = (target.Position - agent.Position).Normalize();
                            // Buff condition2
                            if (distance < _auraRadius)
                            {
                                _agentBuffed[target] = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    internal class BaseDamagingBlowAura : BaseAura
    {
        public BaseDamagingBlowAura()
        {
            _applyToFriendly = false;
        }

        public override void Tick(float dt)
        {
            base.Tick(dt);
            bool isTimered = _buffTimer.Check(reset: true);

            foreach (var target in Mission.Current.AllAgents)
            {
                // Already in buff list
                if (_agentBuffed.ContainsKey(target))
                {
                    float max_distance = 500f;
                    Agent closestAgent = null;
                    bool buffed = false;
                    // Activate buff if within at least one aura
                    foreach (var agent in _registeredAgents)
                    {
                        var distance = (target.Position - agent.Position).Normalize();
                        buffed |= (distance < _auraRadius);
                        if (distance < max_distance)
                        {
                            closestAgent = agent;
                            max_distance = distance;
                        }
                    }
                    _agentBuffed[target] = buffed;
                    if (isTimered && buffed && closestAgent != null)
                    {
                        Blow blow = CreateBlow(closestAgent, target);
                        target.RegisterBlow(blow, new AttackCollisionData());
                    }
                }
                else
                {
                    foreach (var agent in _registeredAgents)
                    {
                        bool condition = (_applyToFriendly) ? target.IsFriendOf(agent) : target.IsEnemyOf(agent);
                        // Buff condition1
                        if (condition)
                        {
                            var distance = (target.Position - agent.Position).Normalize();
                            // Buff condition2
                            if (distance < _auraRadius)
                            {
                                _agentBuffed[target] = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        virtual protected Blow CreateBlow(Agent attacker, Agent victim)
        {
            Blow blow = new Blow(attacker.Index);
            blow.DamageType = DamageTypes.Blunt;
            blow.BoneIndex = victim.Monster.HeadLookDirectionBoneIndex;
            blow.Position = victim.Position;
            blow.Position.z = blow.Position.z + victim.GetEyeGlobalHeight();
            blow.BaseMagnitude = 0;
            blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, -1); // Hang Yu Oct4.2020 : e.1.5.3 changed
            blow.InflictedDamage = (int)_buffAmount;
            blow.SwingDirection = victim.LookDirection;
            blow.SwingDirection.Normalize();
            blow.Direction = blow.SwingDirection;
            blow.DamageCalculated = true;
            return blow;
        }
    }
}