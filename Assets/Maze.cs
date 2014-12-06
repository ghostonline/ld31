using System;
using System.Collections;
using System.Collections.Generic;

public class Maze {
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        Count,
    }

    public class Cell
    {
        public Cell() { walls = new bool[(int)Direction.Count]; walls[0] = true; walls[1] = true; walls[2] = true; walls[3] = true; }
        public bool[] walls;
    }

    class Wall
    {
        public Wall(int cellX, int cellY, Direction wall) { this.cellX = cellX; this.cellY = cellY; this.wall = wall; }

        public int cellX;
        public int cellY;
        public Direction wall;
    }

    static void DirectionToPlace(int x, int y, Direction dir, out int targetX, out int targetY)
    {
        int[] dirToX = {
            0,
            1,
            0,
            -1,
            0,
        };
        int[] dirToY = {
            1,
            0,
            -1,
            0,
            0,
        };
        int dirIdx = (int)dir;
        targetX = x + dirToX[dirIdx];
        targetY = y + dirToY[dirIdx];
    }

    static Direction GetOpposite(Direction dir)
    {
        Direction[] opposite = {
            Direction.Down,
            Direction.Left,
            Direction.Up,
            Direction.Right,
            Direction.Count,
        };

        return opposite[(int)dir];
    }

    static bool ShouldAddWall(int x, int y, Direction dir, Cell[] layout, int width, int height)
    {
        int tX, tY;
        DirectionToPlace(x, y, dir, out tX, out tY);
        bool inBounds = 0 <= tX && tX < width && 0 <= tY && tY < height;
        if (!inBounds) { return false; }
        int idx = tX + tY * width;
        return layout[idx] == null;
    }

    public static Maze Generate(int width, int height)
    {
        var random = new Random((int)(UnityEngine.Random.value * Int32.MaxValue));
        var layout = new Cell[width * height];

        var walls = new List<Wall>();

        // Get start cell
        int startX = random.Next(width);
        int startY = random.Next(height);

        // Add all its walls to the active list
        int startIdx = startX + startY * width;
        layout[startIdx] = new Cell();
        for (int ii = (int)Direction.Count - 1; ii >= 0; --ii)
        {
            var dir = (Direction)ii;
            if (ShouldAddWall(startX, startY, dir, layout, width, height))
            {
                var wall = new Wall(startX, startY, dir);
                walls.Add(wall);
            }
        }

        while (walls.Count > 0)
        {
            var wallIdx = random.Next(walls.Count);
            var wall = walls[wallIdx];
            walls.RemoveAt(wallIdx);

            // Get new place
            int newX, newY;
            DirectionToPlace(wall.cellX, wall.cellY, wall.wall, out newX, out newY);
            int idx = newX + newY * width;
            
            if (layout[idx] != null) { continue; }

            // Create new cell
            var newCell = new Cell();
            var opposite = GetOpposite(wall.wall);
            newCell.walls[(int)opposite] = false;
            layout[idx] = newCell;
            var originalIdx = wall.cellX + wall.cellY * width;
            layout[originalIdx].walls[(int)wall.wall] = false;

            // Add the existing walls to the list
            for (int ii = (int)Direction.Count - 1; ii >= 0; --ii)
            {
                var dir = (Direction)ii;
                if (ShouldAddWall(newX, newY, dir, layout, width, height))
                {
                    var newWall = new Wall(newX, newY, dir);
                    walls.Add(newWall);
                }
            }
        }

        var maze = new Maze();
        maze.layout = layout;
        maze.width = width;
        maze.height = height;
        return maze;
    }

    public Cell Get(int x, int y)
    {
        if (x < 0 || width <= x || y < 0 || height <= y) { return null; }
        return layout[x + y * width];
    }

    Cell[] layout;
    int width;
    int height;
}
