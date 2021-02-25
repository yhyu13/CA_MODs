using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CA_EagleRisingSpecialPowers.Settings
{
    [Serializable]
    public class MyBaseSpecialPowerSettings
    {
        [XmlElement]
        public string HasParticle { get; set; }

        [XmlElement]
        public string ShoudRepeatParticle { get; set; }

        [XmlElement]
        public string RepeatParticlePeriod { get; set; }

        [XmlElement]
        public string ParticleEffectName { get; set; }

        [XmlElement]
        public string ParticleBoneIndexes { get; set; }

        [XmlElement]
        public string HasLight { get; set; }

        [XmlElement]
        public string LightBone { get; set; }

        [XmlElement]
        public string LightRadius { get; set; }

        [XmlElement]
        public string LightIntensity { get; set; }

        [XmlElement]
        public string LightColor { get; set; }
    }

    [Serializable]
    public class MyBaseAuraSettings
    {
        [XmlElement]
        public string HasParticle { get; set; }

        [XmlElement]
        public string ParticleToEquippedOnly { get; set; }

        [XmlElement]
        public string ShoudRepeatParticle { get; set; }

        [XmlElement]
        public string RepeatParticlePeriod { get; set; }

        [XmlElement]
        public string ParticleEffectName { get; set; }

        [XmlElement]
        public string ParticleBoneIndexes { get; set; }

        [XmlElement]
        public string HasLight { get; set; }

        [XmlElement]
        public string LightBone { get; set; }

        [XmlElement]
        public string LightRadius { get; set; }

        [XmlElement]
        public string LightIntensity { get; set; }

        [XmlElement]
        public string LightColor { get; set; }

        [XmlElement]
        public string AuraRadius { get; set; }

        [XmlElement]
        public string BuffAmount { get; set; }

        [XmlElement]
        public string BuffPeriod { get; set; }
    }
}