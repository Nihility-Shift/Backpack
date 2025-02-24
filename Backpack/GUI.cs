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

            bool temp = Configs.Stack.Value;
            if (GUITools.DrawCheckbox("Stack (first in - last out)", ref temp) && temp)
            {
                Configs.Stack.Value = true;
            }
            temp = !Configs.Stack.Value;
            if (GUITools.DrawCheckbox("Queue (first in - first out)", ref temp) && temp)
            {
                Configs.Stack.Value = false;
            }
        }
    }
}
