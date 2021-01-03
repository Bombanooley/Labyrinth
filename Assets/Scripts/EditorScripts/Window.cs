using UnityEditor;
using UnityEngine;


namespace Labyrinth
{
    public class Window : EditorWindow
    {
        public static GameObject objectInstantiate;
        public string nameObject = "Hello World";
        public bool groupEnabled;
        public bool randomColor = true;
        public int countObject = 1;
        public float _radius = 10;
        public int size = 1;
        public LabyrinthGenerator labyrinthGenerator;
        public GameController gameController;


        private void OnGUI()
        {
            labyrinthGenerator = FindObjectOfType<LabyrinthGenerator>();
            gameController = FindObjectOfType<GameController>();

            GUILayout.Label("Базовые настройки", EditorStyles.boldLabel);
            objectInstantiate =
               EditorGUILayout.ObjectField("Объект который хотим вставить",
                     objectInstantiate, typeof(GameObject), true)
                  as GameObject;
            nameObject = EditorGUILayout.TextField("Имя объекта", nameObject);
            groupEnabled = EditorGUILayout.BeginToggleGroup("Дополнительные настройки",
               groupEnabled);
            randomColor = EditorGUILayout.Toggle("Случайный цвет", randomColor);
            countObject = EditorGUILayout.IntSlider("Количество объектов",
               countObject, 1, 100);
            _radius = EditorGUILayout.Slider("Радиус окружности", _radius, 10, 50);
            EditorGUILayout.EndToggleGroup();
            var button = GUILayout.Button("Создать объекты");
            if (button)
            {
                if (objectInstantiate)
                {
                    GameObject root = new GameObject("Root");
                    for (int i = 0; i < countObject; i++)
                    {
                        float angle = i * Mathf.PI * 2 / countObject;
                        Vector3 pos = new Vector3(Mathf.Cos(angle), 0,
                                         Mathf.Sin(angle)) * _radius;
                        GameObject temp = Instantiate(objectInstantiate, pos,
                           Quaternion.identity);
                        temp.name = nameObject + "(" + i + ")";
                        temp.transform.parent = root.transform;
                        var tempRenderer = temp.GetComponent<Renderer>();
                        if (tempRenderer && randomColor)
                        {
                            tempRenderer.material.color = Random.ColorHSV();
                        }

                    }
                }
            }
            labyrinthGenerator.floor = EditorGUILayout.ObjectField("Пол",
                    labyrinthGenerator.floor,
                    typeof(GameObject), true) as GameObject;

            labyrinthGenerator.wallVertical = EditorGUILayout.ObjectField("Стены вдоль Z (вертикальные)",
                labyrinthGenerator.wallVertical,
                typeof(GameObject), true) as GameObject;

            labyrinthGenerator.wallHorisontal = EditorGUILayout.ObjectField("Стены вдоль X (горизонтальные)",
                labyrinthGenerator.wallHorisontal,
                typeof(GameObject), true) as GameObject;


            size = EditorGUILayout.IntSlider("Размер лабиринта",
       size, 5, 100);
            labyrinthGenerator.rows = size;
            labyrinthGenerator.columns = size;
            gameController.isNeedToGenerate = EditorGUILayout.Toggle("Генерировать лабиринn", gameController.isNeedToGenerate);
            var generateLabyrinth = GUILayout.Button("Создать лабиринт");
            if (generateLabyrinth)
            {
                labyrinthGenerator.isNeedToGenerate = false;
                labyrinthGenerator.Generate();
            }
        }
    }
}