using System.Collections.Generic;

public class MazeCell
{
    // Determines which directions you can travel in this cell
    public List<Directions> directionsTraveled;
    // Uses directionsTraveled to determine the directionalId
    public int directionalId;
    // Row in the maze
    public int mazeRow;
    // Column in the maze
    public int mazeColumn;

    public MazeCell()
    {
        directionsTraveled = new List<Directions>();
    }

    public void ParseDirectionalId()
    {
        if (directionsTraveled.Contains(Directions.Left) && directionsTraveled.Contains(Directions.Down) &&
            directionsTraveled.Contains(Directions.Right) && directionsTraveled.Contains(Directions.Up))
        {
            directionalId = 14;
        }
        else if (directionsTraveled.Contains(Directions.Down) &&
            directionsTraveled.Contains(Directions.Right) && directionsTraveled.Contains(Directions.Up))
        {
            directionalId = 13;
        }
        else if (directionsTraveled.Contains(Directions.Left) && directionsTraveled.Contains(Directions.Up) &&
            directionsTraveled.Contains(Directions.Right))
        {
            directionalId = 12;
        }
        else if (directionsTraveled.Contains(Directions.Left) && directionsTraveled.Contains(Directions.Down) &&
            directionsTraveled.Contains(Directions.Right))
        {
            directionalId = 11;
        }
        else if (directionsTraveled.Contains(Directions.Left) && directionsTraveled.Contains(Directions.Down) &&
            directionsTraveled.Contains(Directions.Up))
        {
            directionalId = 10;
        }
        else if (directionsTraveled.Contains(Directions.Left) && directionsTraveled.Contains(Directions.Down))
        {
            directionalId = 9;
        }
        else if (directionsTraveled.Contains(Directions.Left) && directionsTraveled.Contains(Directions.Right))
        {
            directionalId = 8;
        }
        else if (directionsTraveled.Contains(Directions.Left) && directionsTraveled.Contains(Directions.Up))
        {
            directionalId = 7;
        }
        else if (directionsTraveled.Contains(Directions.Down) && directionsTraveled.Contains(Directions.Right))
        {
            directionalId = 6;
        }
        else if (directionsTraveled.Contains(Directions.Down) && directionsTraveled.Contains(Directions.Up))
        {
            directionalId = 5;
        }
        else if (directionsTraveled.Contains(Directions.Right) && directionsTraveled.Contains(Directions.Up))
        {
            directionalId = 4;
        }
        else if (directionsTraveled.Contains(Directions.Up))
        {
            directionalId = 3;
        }
        else if (directionsTraveled.Contains(Directions.Right))
        {
            directionalId = 2;
        }
        else if (directionsTraveled.Contains(Directions.Down))
        {
            directionalId = 1;
        }
        else if (directionsTraveled.Contains(Directions.Left))
        {
            directionalId = 0;
        }
    }

    public static int ExitCellConvert(MazeCell cell)
    {
        switch (cell.directionalId)
        {
            case 0:
                return 8;
            case 3:
                return 4;
            case 7:
                return 12;
            default:
                return 12;
        }
    }
}