using BepInEx.Configuration;
using UnityEngine;

namespace Backpack
{
    internal class Configs
    {
        internal static ConfigEntry<KeyCode> ToggleBackpackItem;
        internal static ConfigEntry<bool> Stack;

        internal static void Load(BepinPlugin plugin)
        {
            ToggleBackpackItem = plugin.Config.Bind("backpack", "toggleItem", KeyCode.G);
            Stack = plugin.Config.Bind("backpack", "isStack", true);
        }
    }
}
