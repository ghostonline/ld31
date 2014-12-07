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

    static List<int> GenerateRange(int max)
    {
        var lst = new List<int>();
        for (int ii = 0; ii < max; ++ii)
        {
            lst.Add(ii);
        }
        return lst;
    }

    static void Shuffle(List<int> lst, Random rnd)
    {
        for (int ii = 0; ii < lst.Count; ++ii)
        {
            var tgtIdx = ii + rnd.Next(lst.Count - ii);
            var tgt = lst[tgtIdx];
            lst[tgtIdx] = lst[ii];
            lst[ii] = tgt;
        }
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
            const int roomWidth = 3;
            const int roomHeight = 3;

            int maxOffsetX = areaWidth - roomWidth;
            int maxOffsetY = areaHeight - roomHeight;

            Point[] startPos = {
                new Point(1,1),
                new Point(1,areaHeight + 1),
                new Point(areaWidth + 1,1),
                new Point(areaWidth + 1,areaHeight + 1),
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
                int offsetX = start.x + random.Next(maxOffsetX);
                int offsetY = start.y + random.Next(maxOffsetY);

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

        // Get start cell
        int startX, startY;
        do
        {
            startX = random.Next(width);
            startY = random.Next(height);
        } while (layout[startX + startY * width] != null);

        // Sprinkle maze with special locations (done just before generating maze to prevent spawning in boss rooms)
        var specials = new List<Point>();
        const int PointDistribution = 5;
        int widthInterval = width / PointDistribution;
        int heightInterval = height / PointDistribution;
        var rangeX = GenerateRange(widthInterval);
        var rangeY = GenerateRange(heightInterval);
        for (int col = 0; col <= width - widthInterval; col += widthInterval)
        {
            for (int row = 0; row <= height - heightInterval; row += heightInterval)
            {
                Shuffle(rangeX, random);
                Shuffle(rangeY, random);
                Point point = null;
                foreach (int rndX in rangeX)
                {
                    foreach (int rndY in rangeY)
                    {
                        int posX = col + rndX;
                        int posY = row + rndY;

                        if (layout[posX + posY * width] == null && !(posX == startX && posY == startY))
                        {
                            point = new Point(posX, posY);
                            specials.Add(point);
                            break;
                        }
                    }

                    if (point != null)
                    {
                        break;
                    }
                }
            }
        }

        var walls = new List<Wall>();

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
