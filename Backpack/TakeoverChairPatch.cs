using CG.Client;
using CG.Game.Player;
using Gameplay.Carryables;
using HarmonyLib;
using Opsive.Shared.StateSystem;

namespace Backpack
{
    [HarmonyPatch(typeof(TakeoverChair), "TrySitInChair")]
    internal class TakeoverChairPatch
    {
        static bool Prefix(TakeoverChair __instance)
        {
            //Checks copied from TrySitInChair to maintain consistency
            if (!__instance.IsAvailable) return true;
            if (StateManager.GetState(LocalPlayer.Instance.gameObject, "OxygenMaskOn") && !__instance.WorksWithJetpack) return true;
            if (__instance._playerCarryableCarrier == null)
            {
                if (LocalPlayer.Instance.gameObject.GetComponent<Carrier>().Payload != null) return true;
            }
            
            //Show warning and prevent sitting if backpack is not empty
            if (Common.backpackItems.Count > 0)
            {
                ViewEventBus.Instance.OnShowQuickWarning.Publish(DataTable<LocalizedStringsDataTable>.Instance.WarningHelperEntry.CannotUseCarryingSomething.TryGetLocalizedString());
                return false;
            }

            return true;
        }
    }
}
