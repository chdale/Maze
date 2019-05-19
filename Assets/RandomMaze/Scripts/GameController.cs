using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public bool hideCeiling = false;
    public int mazeRows;
    public int mazeColumns;
    public int[] customStartingPoint = new int[2];
    public int[] customEndPoint = new int[2] { -1, -1 };
    public float modularSize = 10f;
    public GameObject[] mapObjects;
    public GameObject treasureObject;
    public GameObject[] keys;

    private bool backtrack = false;
    private int[] defaultStartingPoint = new int[2] { 0, 0 };
    private int[] defaultEndPoint = new int[2] { 19, 19 };
    private MazeCell[,] defaultMazeArray = new MazeCell[20, 20];
    private MazeCell[,] mazeArray;
    private Stack<MazeCell> backtrackStack;
    private bool stopTime = false;
    private Text easyText;
    private Text mediumText;
    private Text hardText;
    [SerializeField]
    private int difficulty;

    private float gameTimer = 0f;

    private void Awake()
    {
        GenerateMaze();
    }

    // Use this for initialization
    void Start () {
        gameTimer = 0f;
        easyText = GameObject.Find("EasyTime").GetComponent<Text>();
        mediumText = GameObject.Find("MediumTime").GetComponent<Text>();
        hardText = GameObject.Find("HardTime").GetComponent<Text>();
        UpdateHighscores(true);
        if (GlobalStats.FirstRun)
        {

            GlobalStats.FirstRun = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!stopTime)
        {
            gameTimer += Time.deltaTime;
        }
    }

    public float GetTime()
    {
        return gameTimer;
    }

    public void StopTime()
    {
        stopTime = true;
        UpdateHighscores(false);
    }

    private void UpdateHighscores(bool levelLoad)
    {
        if (levelLoad)
        {
            if (GlobalStats.HardSpeed != default(float))
            {
                hardText.text = GlobalStats.GetStringRepresentation(GlobalStats.HardSpeed);
            }
            if (GlobalStats.MediumSpeed != default(float))
            {
                mediumText.text = GlobalStats.GetStringRepresentation(GlobalStats.MediumSpeed);
            }
            if (GlobalStats.EasySpeed != default(float))
            {
                easyText.text = GlobalStats.GetStringRepresentation(GlobalStats.EasySpeed);
            }
        }
        else
        {
            if (mazeRows == 20)
            {
                if (GlobalStats.HardSpeed == default(float) || GlobalStats.HardSpeed > gameTimer)
                {
                    GlobalStats.HardSpeed = gameTimer;
                    hardText.text = GlobalStats.GetStringRepresentation(GlobalStats.HardSpeed);
                }
            }
            if (mazeRows == 10)
            {
                if (GlobalStats.MediumSpeed == default(float) || GlobalStats.MediumSpeed > gameTimer)
                {
                    GlobalStats.MediumSpeed = gameTimer;
                    mediumText.text = GlobalStats.GetStringRepresentation(GlobalStats.MediumSpeed);
                }
            }
            if (mazeRows == 5)
            {
                if (GlobalStats.EasySpeed == default(float) || GlobalStats.EasySpeed > gameTimer)
                {
                    GlobalStats.EasySpeed = gameTimer;
                    easyText.text = GlobalStats.GetStringRepresentation(GlobalStats.EasySpeed);
                }
            }
        }
    }

    private void GenerateMaze()
    {
        EstablishMazeArray();
        GenerateMazePath();
        GenerateDirectionalIds();
        GenerateLevel();
    }

    // Creates the array of MazeCells that will make up the maze based on either custom or default values
    private void EstablishMazeArray()
    {
        if (mazeRows > 0 && mazeColumns > 0)
        {
            mazeArray = new MazeCell[mazeRows, mazeColumns];
            if (customStartingPoint[0] < 0 || customStartingPoint[0] > mazeRows || customStartingPoint[1] < 0 || customStartingPoint[1] > mazeColumns)
            {
                customStartingPoint = defaultStartingPoint;
            }
            if (customEndPoint[0] < 0 || customEndPoint[0] > mazeRows || customEndPoint[1] < 0 || customEndPoint[1] > mazeColumns)
            {
                customEndPoint = new int[2] { mazeRows - 1, mazeColumns - 1 };
            }
        }
        else
        {
            mazeArray = defaultMazeArray;
            if (customStartingPoint[0] < 0 || customStartingPoint[0] > mazeColumns || customStartingPoint[1] < 0 || customStartingPoint[1] > mazeColumns)
            {
                customStartingPoint = defaultStartingPoint;
            }
            if (customEndPoint[0] < 0 || customEndPoint[0] > mazeColumns || customEndPoint[1] < 0 || customEndPoint[1] > mazeColumns)
            {
                customEndPoint = defaultEndPoint;
            }
        }
    }

    // Depth-first search algorithm to be used to create the maze
    private void GenerateMazePath()
    {
        backtrackStack = new Stack<MazeCell>();
        MazeSearch(customStartingPoint[0], customStartingPoint[1]);
    }

    // Recursive Maze Search call
    private void MazeSearch(int targetMazeRow, int targetMazeColumn, int? originRow = null, int? originColumn = null)
    {
        if (mazeArray[targetMazeRow, targetMazeColumn] == null)
        {
            mazeArray[targetMazeRow, targetMazeColumn] = new MazeCell();
        }

        if (originRow != null && originColumn != null && !backtrack)
        {
            GetDirectionTraveled(originRow.Value, originColumn.Value, targetMazeRow, targetMazeColumn);
        }

        backtrack = false;

        if (!AllVisited())
        {
            mazeArray[targetMazeRow, targetMazeColumn].mazeRow = targetMazeRow;
            mazeArray[targetMazeRow, targetMazeColumn].mazeColumn = targetMazeColumn;

            int[] newTargetCell = SearchTargetMazePosition(targetMazeRow, targetMazeColumn);

            if (!backtrack)
            {
                backtrackStack.Push(mazeArray[targetMazeRow, targetMazeColumn]);
            }

            MazeSearch(newTargetCell[0], newTargetCell[1], targetMazeRow, targetMazeColumn);
        }
    }

    // Adds the directions traveled to itself and the origin (if it hasn't been added already)
    private void GetDirectionTraveled(int originRow, int originColumn, int targetMazeRow, int targetMazeColumn)
    {
        if (originRow > targetMazeRow)
        {
            if (!mazeArray[originRow, originColumn].directionsTraveled.Contains(Directions.Up))
            {
                mazeArray[originRow, originColumn].directionsTraveled.Add(Directions.Up);
            }
            if (!mazeArray[targetMazeRow, targetMazeColumn].directionsTraveled.Contains(Directions.Down))
            {
                mazeArray[targetMazeRow, targetMazeColumn].directionsTraveled.Add(Directions.Down);
            }
        }
        else if (originRow < targetMazeRow)
        {
            if (!mazeArray[originRow, originColumn].directionsTraveled.Contains(Directions.Down))
            {
                mazeArray[originRow, originColumn].directionsTraveled.Add(Directions.Down);
            }
            if (!mazeArray[targetMazeRow, targetMazeColumn].directionsTraveled.Contains(Directions.Up))
            {
                mazeArray[targetMazeRow, targetMazeColumn].directionsTraveled.Add(Directions.Up);
            }
        }
        else if (originColumn > targetMazeColumn)
        {
            if (!mazeArray[originRow, originColumn].directionsTraveled.Contains(Directions.Left))
            {
                mazeArray[originRow, originColumn].directionsTraveled.Add(Directions.Left);
            }
            if (!mazeArray[targetMazeRow, targetMazeColumn].directionsTraveled.Contains(Directions.Right))
            {
                mazeArray[targetMazeRow, targetMazeColumn].directionsTraveled.Add(Directions.Right);
            }
        }
        else if (originColumn < targetMazeColumn)
        {
            if (!mazeArray[originRow, originColumn].directionsTraveled.Contains(Directions.Right))
            {
                mazeArray[originRow, originColumn].directionsTraveled.Add(Directions.Right);
            }
            if (!mazeArray[targetMazeRow, targetMazeColumn].directionsTraveled.Contains(Directions.Left))
            {
                mazeArray[targetMazeRow, targetMazeColumn].directionsTraveled.Add(Directions.Left);
            }
        }
    }

    // Find all possible moves from the location and either backtracks to look for non-visited locations or randomizes the next location if there are choices
    private int[] SearchTargetMazePosition(int newOriginRow, int newOriginColumn)
    {
        List<int[]> possibleCellTargets = new List<int[]>();
        if (newOriginRow - 1 >= 0 && mazeArray[newOriginRow - 1, newOriginColumn] == null)
        {
            possibleCellTargets.Add(new int[] { newOriginRow - 1, newOriginColumn });
        }
        if (newOriginRow + 1 < mazeArray.GetLength(0) && mazeArray[newOriginRow + 1, newOriginColumn] == null)
        {
            possibleCellTargets.Add(new int[] { newOriginRow + 1, newOriginColumn });
        }
        if (newOriginColumn - 1 >= 0 && mazeArray[newOriginRow, newOriginColumn - 1] == null)
        {
            possibleCellTargets.Add(new int[] { newOriginRow, newOriginColumn - 1 });
        }
        if (newOriginColumn + 1 < mazeArray.GetLength(1) && mazeArray[newOriginRow, newOriginColumn + 1] == null)
        {
            possibleCellTargets.Add(new int[] { newOriginRow, newOriginColumn + 1 });
        }

        if (!possibleCellTargets.Any())
        {
            MazeCell backtrackedCell = backtrackStack.Pop();
            backtrack = true;
            return new int[2] { backtrackedCell.mazeRow, backtrackedCell.mazeColumn };
        }

        int[][] possibleCellTargetArray = possibleCellTargets.ToArray();

        if (possibleCellTargetArray.GetLength(0) > 1)
        {
            return possibleCellTargetArray[UnityEngine.Random.Range(0, possibleCellTargetArray.GetLength(0))];
        }
        else
        {
            return possibleCellTargetArray[0];
        }
    }

    // Checks to see if every cell has been visited
    private bool AllVisited()
    {
        int arrayCount = 0;

        for (int i = 0; i < mazeArray.GetLength(0); i++)
        {
            for (int j = 0; j < mazeArray.GetLength(1); j++)
            {
                if (mazeArray[i, j] != null)
                {
                    arrayCount++;
                }
                else
                {
                    return false;
                }
            }
        }

        return arrayCount == mazeArray.GetLength(0) * mazeArray.GetLength(1);
    }

    // Goes through each of the MazeCells and determines the ID that will be used for generating the level based off of the directions travelable in the cell
    private void GenerateDirectionalIds()
    {
        for (int i = 0; i < mazeArray.GetLength(0); i++)
        {
            for (int j = 0; j < mazeArray.GetLength(1); j++)
            {
                mazeArray[i, j].ParseDirectionalId();
            }
        }
    }

    // Instantiates the GameObjects that will create the level based off of the directional ids assigned
    private void GenerateLevel()
    {
        List<MazeCell> possibleKeyLocations = new List<MazeCell>();

        // Generate the map and get possible key locations
        for (int i = 0; i < mazeArray.GetLength(0); i++)
        {
            for (int j = 0; j < mazeArray.GetLength(1); j++)
            {
                GameObject mazeSegment = null; 
                if (i == customEndPoint[0] && j == customEndPoint[1])
                {
                    mazeSegment = mapObjects[MazeCell.ExitCellConvert(mazeArray[i, j])];
                }
                else
                {
                    mazeSegment = mapObjects[mazeArray[i, j].directionalId];
                }

                if (hideCeiling)
                {
                    mazeSegment.transform.Find("Ceiling").gameObject.GetComponentInChildren<Renderer>().enabled = false;
                }
                else
                {
                    mazeSegment.transform.Find("Ceiling").gameObject.GetComponentInChildren<Renderer>().enabled = true;
                }

                var segment = Instantiate(mazeSegment, new Vector3(i * modularSize, 0, j * modularSize), Quaternion.identity);

                if (IsPossibleKeyLocation(i,j))
                {
                    possibleKeyLocations.Add(mazeArray[i, j]);
                }
            }
        }

        // Generate keys
        if (possibleKeyLocations.Count >= keys.GetLength(0))
        {
            foreach (var key in keys)
            {
                var randomIndex = UnityEngine.Random.Range(0, possibleKeyLocations.Count);
                var randomCell = possibleKeyLocations.ElementAt(randomIndex);

                Instantiate(key, new Vector3(randomCell.mazeRow * modularSize, 0, randomCell.mazeColumn * modularSize), Quaternion.identity);

                possibleKeyLocations.RemoveAt(randomIndex);
            }
        }
    }

    // Verifies that the location is a dead end and is neither the starting point, nor the ending point
    private bool IsPossibleKeyLocation(int row, int column)
    {
        if (mazeArray[row, column].directionalId < 4 && customStartingPoint != new int[] { row, column } && customEndPoint != new int[] { row, column } )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public enum Difficulty
{
    Easy = 0,
    Medium = 1,
    Hard = 2
}