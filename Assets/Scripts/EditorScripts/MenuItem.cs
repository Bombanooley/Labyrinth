using UnityEditor;

namespace Labyrinth
{
    public class MenuItems
    {
        [MenuItem("Лабиринт/Генерация лабиринта ")]
        private static void MenuOption()
        {
            EditorWindow.GetWindow(typeof(Window), false, "Create");
        }
    }

}
