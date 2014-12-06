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

    public class Point
    {
        public Point(int x, int y) { this.x = x; this.y = y; }
        public int x { get; private set; }
        public int y { get; private set; }
    }

    public class Wall
    {
        public Wall(int cellX, int cellY, Direction wall) { this.cellX = cellX; this.cellY = cellY; this.wall = wall; }

        public int cellX;
        public int cellY;
        public Direction wall;
    }

    public class BossRoom
    {
        public int startX;
        public int startY;
        public int width;
        public int height;
        public Wall entrance;
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
            -1,
            0,
            1,
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

        var bossRooms = new List<BossRoom>();
        var bossEntrances = new List<Wall>();
        // Create four boss rooms
        {
            int areaWidth = width / 2;
            int areaHeight = height / 2;
            int roomWidth = 4;
            int roomHeight = 4;

            int minOffsetX = 1; // Make sure there is at least a border of a single row between boss rooms
            int minOffsetY = 1;
            int maxOffsetX = areaWidth - roomWidth - minOffsetX * 2;
            int maxOffsetY = areaHeight - roomHeight - minOffsetY * 2;

            Point[] startPos = {
                new Point(0,0),
                new Point(0,areaHeight),
                new Point(areaWidth,0),
                new Point(areaWidth,areaHeight),
            };

            Point[] entranceOptions = {
                new Point(roomWidth / 2, 0),
                new Point(roomWidth - 1, roomHeight / 2),
                new Point(roomWidth / 2, roomHeight - 1),
                new Point(0, roomHeight / 2),
            };

            for (int ii = 0; ii < startPos.Length; ++ii)
            {
                var start = startPos[ii];
                int offsetX = start.x + minOffsetX + random.Next(maxOffsetX);
                int offsetY = start.y + minOffsetY + random.Next(maxOffsetY);

                for (int col = 0; col < roomWidth; ++col)
                {
                    for (int row = 0; row < roomHeight; ++row)
                    {
                        var cell = new Cell();
                        cell.walls[(int)Direction.Up] = row == 0;
                        cell.walls[(int)Direction.Down] = row == (roomHeight - 1);
                        cell.walls[(int)Direction.Left] = col == 0;
                        cell.walls[(int)Direction.Right] = col == (roomWidth - 1);
                        int idx = (offsetX + col) + (offsetY + row) * width;
                        layout[idx] = cell;
                    }
                }

                var entranceDir = (Direction)random.Next((int)Direction.Count);
                var entrancePos = entranceOptions[(int)entranceDir];
                var wall = new Wall(entrancePos.x + offsetX, entrancePos.y + offsetY, entranceDir);
                bossEntrances.Add(wall);

                var room = new BossRoom();
                room.startX = offsetX;
                room.startY = offsetY;
                room.width = roomWidth;
                room.height = roomHeight;
                room.entrance = wall;
                bossRooms.Add(room);
            }

        }

        var walls = new List<Wall>();

        // Get start cell
        int startX, startY;
        do
        {
            startX = random.Next(width);
            startY = random.Next(height);
        } while (layout[startX + startY * width] != null);


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

        // Open up boss entrances
        for (int ii = 0; ii < bossEntrances.Count; ++ii)
        {
            var innerWall = bossEntrances[ii];
            var innerIdx = innerWall.cellX + innerWall.cellY * width;
            layout[innerIdx].walls[(int)innerWall.wall] = false;
            int outerX, outerY;
            DirectionToPlace(innerWall.cellX, innerWall.cellY, innerWall.wall, out outerX, out outerY);
            var opposite = GetOpposite(innerWall.wall);
            int outerIdx = outerX + outerY * width;
            layout[outerIdx].walls[(int)opposite] = false;
        }

        // Sprinkle maze with special locations
        var specials = new List<Point>();
        const int PointDistribution = 3;
        int widthInterval = width / PointDistribution;
        int heightInterval = height / PointDistribution;
        for (int col = 0; col < width - widthInterval; col += widthInterval)
        {
            for (int row = 0; row < height - heightInterval; row += heightInterval)
            {
                int posX = col + random.Next(widthInterval);
                int posY = row + random.Next(heightInterval);
                var point = new Point(posX, posY);
                specials.Add(point);
            }
        }

        var maze = new Maze();
        maze.layout = layout;
        maze.width = width;
        maze.height = height;
        maze.Specials = specials;
        maze.BossRooms = bossRooms;
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
    public List<Point> Specials { get; private set; }
    public List<BossRoom> BossRooms { get; private set; }
}
