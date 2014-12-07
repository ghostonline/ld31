using UnityEngine;
using System.Collections;

public class MazeBuilder : MonoBehaviour {

    public int MazeWidth = 10;
    public int MazeHeight = 10;

    public int CellWidth = 5;
    public int CellHeight = 5;

    public GameObject EdgePillar;
    public GameObject Floor;
    public GameObject Wall;
    public GameObject AugmentedWall;
    public GameObject PointMarker;
    public GameObject RoomMarker;
    public GameObject EntranceMarker;
    public GameObject Key;

    public Transform playerSpawn;
    public Transform endMazeMarker;
    public Transform floorCollider;

    public Boss turretBoss;

    public bool NoWalls;

    void AddTemplate(GameObject template)
    {
        var container = new GameObject();
        container.name = string.Format("{0}_container", template.name);
        container.transform.parent = transform;
        template.transform.parent = container.transform;
        template.SetActive(false);
    }

    void Awake () {
        AddTemplate(EdgePillar);
        AddTemplate(AugmentedWall);
        AddTemplate(Wall);
        AddTemplate(PointMarker);
        AddTemplate(RoomMarker);
        AddTemplate(EntranceMarker);
        AddTemplate(Floor);
        AddTemplate(turretBoss.gameObject);

        floorCollider.transform.parent = transform;
        floorCollider.transform.localPosition = Vector3.zero;

        GenerateFrame();

        if (NoWalls) { Wall.transform.parent.gameObject.SetActive(false); }
    }

    GameObject PlaceTemplate(GameObject obj, Vector3 pos, int angle, string name)
    {
        var wall = GameObject.Instantiate(obj) as GameObject;
        wall.SetActive(true);
        wall.name = name;
        wall.transform.parent = obj.transform.parent;
        wall.transform.localRotation = Quaternion.Euler(0, angle, 0);
        wall.transform.localPosition = pos;
        return wall;
    }

    GameObject PlaceWall(Vector3 pos, int angle, string name)
    {
        return PlaceTemplate(Wall, pos, angle, name);
    }

    GameObject PlaceAugmentedWall(Vector3 pos, int angle, string name)
    {
        return PlaceTemplate(AugmentedWall, pos, angle, name);
    }

    void GenerateFrame()
    {
        var down = Vector3.back * CellHeight;
        var right = Vector3.right * CellWidth;
        var cellCenter = down * 0.5f + right * 0.5f;

        for (int row = 0; row < MazeHeight; ++row)
        {
            var posL = down * row + down * 0.5f;
            var posR = posL + right * MazeWidth;
            PlaceAugmentedWall(posL, 270, string.Format("AugmentedBound_L{0:D2}", row));
            PlaceAugmentedWall(posR, 90, string.Format("AugmentedBound_R{0:D2}", row));
        }

        for (int col = 0; col < MazeWidth; ++col)
        {
            var posB = right * col + right * 0.5f;
            var posT = posB + down * MazeHeight;
            PlaceAugmentedWall(posB, 0, string.Format("AugmentedBound_B{0:D2}", col));
            PlaceAugmentedWall(posT, 180, string.Format("AugmentedBound_T{0:D2}", col));
        }

        var maze = Maze.Generate(MazeWidth, MazeHeight);
        Vector3[] directionOffset = {
            right * 0.5f,
            right + down * 0.5f,
            right * 0.5f + down,
            down * 0.5f,
            Vector3.zero,
        };

        int[] directionAngle = {
            0,
            90,
            180,
            270,
        };

        int[] directionLimit = {
            //(int)Maze.Direction.Up, // Enable this for debug
            (int)Maze.Direction.Right,
            (int)Maze.Direction.Down,
            //(int)Maze.Direction.Left, // Enable this for debug
        };

        {
            int dirIdx = (int)Maze.Direction.Left;
            int col = 0;
            for (int row = 0; row < MazeHeight; ++row)
            {
                var pos = row * down + col * right;
                var cell = maze.Get(col, row);
                if (cell != null && cell.walls[dirIdx])
                {
                    PlaceWall(pos + directionOffset[dirIdx], directionAngle[dirIdx], string.Format("MazeWall_{0:D2}_{1:D2}_{2}", col, row, dirIdx));
                }
            }
        }

        {
            int dirIdx = (int)Maze.Direction.Up;
            int row = 0;
            for (int col = 0; col < MazeWidth; ++col)
            {
                var pos = row * down + col * right;
                var cell = maze.Get(col, row);
                if (cell != null && cell.walls[dirIdx])
                {
                    PlaceWall(pos + directionOffset[dirIdx], directionAngle[dirIdx], string.Format("MazeWall_{0:D2}_{1:D2}_{2}", col, row, dirIdx));
                }
            }
        }

        for (int col = 0; col < MazeWidth; ++col)
        {
            for (int row = 0; row < MazeHeight; ++row)
            {
                var cell = maze.Get(col, row);
                var pos = col * right + row * down;
                if (cell == null) { Debug.LogWarning("Uninitialized cell found!"); continue; }
                for (int ii = directionLimit.Length - 1; ii >= 0; --ii)
                {
                    int dirIdx = directionLimit[ii];
                    if (cell.walls[dirIdx])
                    {
                        PlaceWall(pos + directionOffset[dirIdx], directionAngle[dirIdx], string.Format("MazeWall_{0:D2}_{1:D2}_{2}", col, row, dirIdx));
                    }
                }
            }
        }

        // Place pillars when walls are adjacent
        for (int col = 0; col < MazeWidth + 1; ++col)
        {
            for (int row = 0; row < MazeHeight + 1; ++row)
            {
                var cellLower = maze.Get(col, row);
                var cellUpper = maze.Get(col - 1, row - 1);
                bool place = cellLower == null || cellUpper == null
                    || cellLower.walls[(int)Maze.Direction.Up] || cellLower.walls[(int)Maze.Direction.Left]
                    || cellUpper.walls[(int)Maze.Direction.Down] || cellUpper.walls[(int)Maze.Direction.Right]
                    ;
                if (place)
                {
                    var pos = down * row + right * col;
                    var name = string.Format("Pillar_{0:D2}_{1:D2}", col, row);
                    PlaceTemplate(EdgePillar, pos, 0, name);
                }
            }
        }

        // Lay down floor tiles
        for (int col = 0; col < MazeWidth; ++col)
        {
            for (int row = 0; row < MazeHeight; ++row)
            {
                var pos = down * row + right * col;
                var name = string.Format("Floor_{0:D2}_{1:D2}", col, row);
                PlaceTemplate(Floor, pos, 0, name);
            }
        }

        // Stretch floor collider
        floorCollider.transform.localScale = right * MazeWidth + down * -MazeHeight; // Compensate for negative zoom

        // Place player in maze
        var spawnX = 0;
        var spawnY = 0;

        var playerLocalPos = right * spawnX + down * spawnY + cellCenter;
        
        // Find first wall-less direction
        var startCell = maze.Get(spawnX, spawnY);
        var playerLocalDir = Quaternion.identity;
        for (int ii = (int)Maze.Direction.Count - 1; ii >= 0; --ii)
        {
            if (startCell != null && !startCell.walls[ii])
            {
                playerLocalDir = Quaternion.Euler(0, directionAngle[ii], 0);
            }
        }

        playerSpawn.position = transform.TransformVector(playerLocalPos);
        playerSpawn.rotation = transform.rotation * playerLocalDir;

        var endX = MazeWidth - 1;
        var endY = MazeHeight - 1;

        // Place end marker
        var markerLocalPos = right * endX + down * endY + cellCenter;
        endMazeMarker.position = transform.TransformPoint(markerLocalPos);

        // Add points to maze
        foreach (var special in maze.Specials)
        {
            var specialPos = right * special.x + down * special.y + cellCenter;
            PlaceTemplate(PointMarker, specialPos, 0, string.Format("Points_{0:D2}_{1:D2}", special.x, special.y));
        }

        // Mark boss areas
        int roomId = 0;
        foreach (var room in maze.BossRooms)
        {
            var roomPos = right * room.startX + down * room.startY;
            var roomScale = right * room.width + down * -room.height; // Compensate for down being negative 

            var marker = PlaceTemplate(RoomMarker, roomPos, 0, string.Format("BossRoom_{0}", roomId));
            marker.transform.localScale = roomScale;

            var entrance = room.entrance;
            var entranceOffset = directionOffset[(int)entrance.wall];
            var entranceAngle = directionAngle[(int)entrance.wall];
            var entrancePos = right * entrance.cellX + down * entrance.cellY + entranceOffset;
            PlaceTemplate(EntranceMarker, entrancePos, entranceAngle, string.Format("BossEntrance_{0}", roomId));

            var center = roomPos + (right * room.width + down * room.height) * 0.5f;
            PlaceTemplate(Key, center, entranceAngle, string.Format("Key_{0}", roomId));

            ++roomId;
        }
    }
}
