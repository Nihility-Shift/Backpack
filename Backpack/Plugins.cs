using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using VoidManager;
using VoidManager.MPModChecks;

namespace Backpack
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.USERS_PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Void Crew.exe")]
    [BepInDependency(VoidManager.MyPluginInfo.PLUGIN_GUID)]
    public class BepinPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = Logger;
            Configs.Load(this);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }
    }

    public class VoidManagerPlugin : VoidPlugin
    {
        public VoidManagerPlugin()
        {
            Events.Instance.LeftRoom += (o, e) =>
            {
                if (Common.backpackItems.Count > 0)
                {
                    Events.Instance.LateUpdate -= Common.MoveItems;
                    Common.backpackItems.Clear();
                }
            };

            Events.Instance.LateUpdate += Common.ButtonPressed;
        }

        public static bool Enabled {  get; private set; }

        public override MultiplayerType MPType => MultiplayerType.Client;

        public override string Author => MyPluginInfo.PLUGIN_AUTHORS;

        public override string Description => MyPluginInfo.PLUGIN_DESCRIPTION;

        public override string ThunderstoreID => MyPluginInfo.PLUGIN_THUNDERSTORE_ID;

        public override SessionChangedReturn OnSessionChange(SessionChangedInput input)
        {
            Enabled = input.IsMod_Session;
            return new SessionChangedReturn() { SetMod_Session = true };
        }
    }
}