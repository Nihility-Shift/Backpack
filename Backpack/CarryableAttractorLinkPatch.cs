using CG.Objects;
using CG.Ship.Hull;
using CG.Ship.Modules;
using Gameplay.Carryables;
using HarmonyLib;
using System;

namespace Backpack
{
    [HarmonyPatch(typeof(CarryableAttractorLink), MethodType.Constructor, new Type[] { typeof(CarryablesSocket), typeof(CarryableObject), typeof(CarrierCarryableHandler) })]
    internal class CarryableAttractorLinkPatch
    {
        //For host, allow the scoop to remove backpack items. Maintains consistency with clients
        static void Postfix(CarryableObject carryable)
        {
            if (Common.backpackItems.Contains(carryable))
            {
                Common.RemoveItem(carryable);
            }
        }
    }
}
