using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using static UnityEngine.Random;

namespace Labyrinth
{
    public struct genCell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public bool Visited { get; set; }
        public int Value { get; set; }

    }

    public class LabyrinthGenerator : MonoBehaviour
    {
        private static int rowsS;
        private static int columnsS;
        private static Random _rand = new Random();
        private static genCell[,] _map;
        private GameObject labyrinth;
        public GameObject floor;
        public GameObject wallHorisontal;
        public GameObject wallVertical;
        public List<GameObject> labyrinthHolder;
        public int rows;
        public int columns;
        [SerializeField] public bool isNeedToGenerate;

        public int Rows 
        { 
            get
            { return rows; }
        }
        public int Columns
        {
            get
            { return columns; }
        }
        public genCell[,] Map { get { return _map; } }

        public LabyrinthGenerator(int inputRows, int inputColumns, List<GameObject> labyrinthHolder)
        { }

        public LabyrinthGenerator(int inputRows, int inputColumns)
        {
            rows = inputRows;
            columns = inputColumns;
            LoadResourses();
            Generate();
        }



        public void Generate()
        {
            rowsS = rows * 2 + 1;
            columnsS = columns * 2 + 1;

            labyrinth = GameObject.FindGameObjectWithTag("Labyrinth");
            if (labyrinth == null) labyrinth = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity);

            labyrinth.transform.position = new Vector3(0f, 0f, 0f);
            _map = new genCell[rowsS, columnsS];

            ClearLabyrinth();
            ClearMap(ref _map);
            RemoveWall(ref _map);
            PrintMap(_map);
        }



        private void LoadResourses()
        {
            floor = Resources.Load<GameObject>("Prefabs/Labyrinth/Floor");
            wallHorisontal = Resources.Load<GameObject>("Prefabs/Labyrinth/Wall Horisontal");
            wallVertical = Resources.Load<GameObject>("Prefabs/Labyrinth/Wall Vertical");
        }

        private void PrintMap(genCell[,] M)
        {
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    if (i % 2 == 0 && j % 2 == 0) continue;
                    if (i % 2 == 1 && j % 2 == 1) 
                    {
                        labyrinthHolder.Add(Instantiate(floor, new Vector3(i - 1f, 0f, j - 1), Quaternion.identity, labyrinth.transform));
                    }            
                    if (M[i, j].Value == -1)
                    {
                        if (i % 2 == 1)
                            labyrinthHolder.Add(
                                Instantiate(
                                wallHorisontal,
                                new Vector3(i - 0.95f, 0.975f, j - 0.95f),
                                Quaternion.Euler(
                                    new Vector3(
                                        labyrinth.transform.rotation.x,
                                        90f,
                                        labyrinth.transform.rotation.z)),
                                labyrinth.transform));
                        else
                            labyrinthHolder.Add(Instantiate(wallVertical, new Vector3(i - 0.95f, 0.975f, j - 1f), Quaternion.identity, labyrinth.transform));
                    }
                }
            }
        }

        private void ClearLabyrinth()
        {
            if (labyrinthHolder.Count != 0)
                foreach (var item in labyrinthHolder)
                {
                    Destroy(item);
                }
            labyrinthHolder.Clear();
        }

        private static void ClearMap(ref genCell[,] M)
        {
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    if ((i % 2 != 0 && j % 2 != 0) && (i < rowsS - 1 && j < columnsS - 1))
                    {
                        M[i, j].Value = 0;
                    }
                    else
                    {
                        M[i, j].Value = -1;
                    }
                    M[i, j].Row = i;
                    M[i, j].Col = j;
                    M[i, j].Visited = false;
                }
            }
        }

        private static void RemoveWall(ref genCell[,] M)
        {
            genCell current = M[1, 1];
            current.Visited = true;

            Stack<genCell> stack = new Stack<genCell>();

            do
            {
                List<genCell> cells = new List<genCell>();

                int row = current.Row;
                int col = current.Col;

                if (row - 1 > 0 && !M[row - 2, col].Visited) cells.Add(M[row - 2, col]);
                if (col - 1 > 0 && !M[row, col - 2].Visited) cells.Add(M[row, col - 2]);

                if (row < rowsS - 3 && !M[row + 2, col].Visited) cells.Add(M[row + 2, col]);
                if (col < columnsS - 3 && !M[row, col + 2].Visited) cells.Add(M[row, col + 2]);

                if (cells.Count > 0)
                {
                    genCell selected = cells[Range(0, cells.Count)];
                    //genCell selected = cells[_rand.Next(cells.Count)];
                    RemoveCurrentWall(ref M, current, selected);

                    selected.Visited = true;
                    M[selected.Row, selected.Col].Visited = true;
                    stack.Push(selected);
                    current = selected;
                }
                else
                {
                    current = stack.Pop();
                }

            } while (stack.Count > 0);
        }

        private static void RemoveCurrentWall(ref genCell[,] M, genCell current, genCell selected)
        {
            if (current.Row == selected.Row)
            {
                if (current.Col > selected.Col) { M[current.Row, current.Col - 1].Value = 0; }
                else { M[current.Row, selected.Col - 1].Value = 0; }
            }
            else
            {
                if (current.Row > selected.Row) { M[current.Row - 1, current.Col].Value = 0; }
                else { M[selected.Row - 1, selected.Col].Value = 0; }
            }
        }



    }
}

