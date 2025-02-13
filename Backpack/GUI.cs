using UnityEngine;
using VoidManager.CustomGUI;
using VoidManager.Utilities;

namespace Backpack
{
    internal class GUI : ModSettingsMenu
    {
        public override string Name() => "Backpack";

        public override void Draw()
        {
            GUITools.DrawChangeKeybindButton("Change backpack interact", ref Configs.ToggleBackpackItem);

            if (GUILayout.Button(Configs.Stack.Value ? "Stack (first in - last out)" : "Queue (first in - first out)"))
            {
                Configs.Stack.Value = !Configs.Stack.Value;
            }
        }
    }
}
