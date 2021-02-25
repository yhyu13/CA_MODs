using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalradiaAwakensSpecialPowers.SpecialPowers
{

    internal class OffenseAura1 : BaseOffenseAura
    {
        public OffenseAura1()
        {
            InitSettings(this.GetType().Name);
        }
    }

    internal class OffenseAura2 : BaseOffenseAura
    {
        public OffenseAura2()
        {
            InitSettings(this.GetType().Name);
        }
    }

    internal class OffenseAura3 : BaseOffenseAura
    {
        public OffenseAura3()
        {
            InitSettings(this.GetType().Name);
        }
    }

    internal class DefenseAura1 : BaseDefenseAura
    {
        public DefenseAura1()
        {
            InitSettings(this.GetType().Name);
        }
    }

    internal class DefenseAura2 : BaseDefenseAura
    {
        public DefenseAura2()
        {
            InitSettings(this.GetType().Name);
        }
    }

    internal class DefenseAura3 : BaseDefenseAura
    {
        public DefenseAura3()
        {
            InitSettings(this.GetType().Name);
        }
    }

    internal class HeroicAura1 : BaseMoraleAura
    {
        public HeroicAura1()
        {
            _applyToFriendly = true;
            InitSettings(this.GetType().Name);
        }
    }

    internal class HeroicAura2 : BaseMoraleAura
    {
        public HeroicAura2()
        {
            _applyToFriendly = true;
            InitSettings(this.GetType().Name);
        }
    }

    internal class HeroicAura3 : BaseMoraleAura
    {
        public HeroicAura3()
        {
            _applyToFriendly = true;
            InitSettings(this.GetType().Name);
        }
    }

    internal class DreadAura1 : BaseMoraleAura
    {
        public DreadAura1()
        {
            _applyToFriendly = false;
            InitSettings(this.GetType().Name);
        }
    }

    internal class DreadAura2 : BaseMoraleAura
    {
        public DreadAura2()
        {
            _applyToFriendly = false;
            InitSettings(this.GetType().Name);
        }
    }

    internal class DreadAura3 : BaseMoraleAura
    {
        public DreadAura3()
        {
            _applyToFriendly = false;
            InitSettings(this.GetType().Name);
        }
    }

    internal class HasteAura1 : BaseMovementSpeedAura
    {
        public HasteAura1()
        {
            _applyToFriendly = true;
            InitSettings(this.GetType().Name);
        }
    }

    internal class HasteAura2 : BaseMovementSpeedAura
    {
        public HasteAura2()
        {
            _applyToFriendly = true;
            InitSettings(this.GetType().Name);
        }
    }

    internal class HasteAura3 : BaseMovementSpeedAura
    {
        public HasteAura3()
        {
            _applyToFriendly = true;
            InitSettings(this.GetType().Name);
        }
    }

    internal class TemporalChainedAura1 : BaseMovementSpeedAura
    {
        public TemporalChainedAura1()
        {
            _applyToFriendly = false;
            InitSettings(this.GetType().Name);
        }
    }

    internal class TemporalChainedAura2 : BaseMovementSpeedAura
    {
        public TemporalChainedAura2()
        {
            _applyToFriendly = false;
            InitSettings(this.GetType().Name);
        }
    }

    internal class TemporalChainedAura3 : BaseMovementSpeedAura
    {
        public TemporalChainedAura3()
        {
            _applyToFriendly = false;
            InitSettings(this.GetType().Name);
        }
    }

    internal class SpringOfLifeAura1 : BaseHealAura
    {
        public SpringOfLifeAura1()
        {
            _applyToFriendly = true;
            InitSettings(this.GetType().Name);
        }
    }

    internal class SpringOfLifeAura2 : BaseHealAura
    {
        public SpringOfLifeAura2()
        {
            _applyToFriendly = true;
            InitSettings(this.GetType().Name);
        }
    }

    internal class SpringOfLifeAura3 : BaseHealAura
    {
        public SpringOfLifeAura3()
        {
            _applyToFriendly = true;
            InitSettings(this.GetType().Name);
        }
    }

    internal class DamagingBlowAura1 : BaseDamagingBlowAura
    {
        public DamagingBlowAura1()
        {
            _applyToFriendly = false;
            InitSettings(this.GetType().Name);
        }
    }

    internal class DamagingBlowAura2 : BaseDamagingBlowAura
    {
        public DamagingBlowAura2()
        {
            _applyToFriendly = false;
            InitSettings(this.GetType().Name);
        }
    }

    internal class DamagingBlowAura3 : BaseDamagingBlowAura
    {
        public DamagingBlowAura3()
        {
            _applyToFriendly = false;
            InitSettings(this.GetType().Name);
        }
    }
}
