using UnityEditor;
using UnityEngine;


namespace Labyrinth
{
    [CustomEditor(typeof(LabyrinthGenerator))]
    public class CustomScriptWindow : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            LabyrinthGenerator labyrinthGenerator = (LabyrinthGenerator)target;
            labyrinthGenerator.rows = 
                EditorGUILayout.IntSlider(labyrinthGenerator.rows, 10, 100);
            labyrinthGenerator.columns =
                EditorGUILayout.IntSlider(labyrinthGenerator.columns, 10, 100);
            var isPressButton = GUILayout.Button("Создать лабиринт",
            EditorStyles.miniButtonLeft);
            if (isPressButton)
            {
                labyrinthGenerator.isNeedToGenerate = false;
                labyrinthGenerator.Generate();
            }



        }
    }
}
