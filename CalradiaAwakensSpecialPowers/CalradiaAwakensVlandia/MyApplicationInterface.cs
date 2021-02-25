using System;
using System.Collections.Generic;
using CalradiaAwakensSpecialPowers.Utils;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using System.Reflection;

namespace CalradiaAwakensSpecialPowers
{
    public class MyApplicationInterface
    {
        public MyApplicationInterface()
        {
            instance = this;
        }

        public static object SetNotPublicVar(object instance, string varName, object newVar)
        {
            FieldInfo field = instance.GetType().GetField(varName, BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(instance, newVar);
            return field.GetValue(instance);
        }

        /*
         * x-right, y-forward, z-up, right-handed coord
         */
        public static Mat3 LookAtWorld(in Vec3 At, in Vec3 Eye)
        {
            Vec3 yaxis = (At - Eye).NormalizedCopy();
            Vec3 xaxis = (Vec3.CrossProduct(yaxis, Vec3.Up)).NormalizedCopy();
            Vec3 zaxis = Vec3.CrossProduct(xaxis, yaxis);

            xaxis.w = -Vec3.DotProduct(xaxis, Eye);
            yaxis.w = -Vec3.DotProduct(yaxis, Eye);
            zaxis.w = -Vec3.DotProduct(zaxis, Eye);

            return new Mat3(xaxis, yaxis, zaxis);
        }

        public static MyApplicationInterface instance;
    }
}
