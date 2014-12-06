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

    public Transform playerSpawn;

    void Start () {
        EdgePillar.SetActive(false);
        EdgePillar.transform.parent = transform;
        AugmentedWall.SetActive(false);
        AugmentedWall.transform.parent = transform;
        Wall.SetActive(false);
        Wall.transform.parent = transform;
        Floor.transform.parent = transform;

        GenerateFrame();
    }

    void PlaceTemplate(GameObject obj, Vector3 pos, int angle, string name)
    {
        var wall = GameObject.Instantiate(obj) as GameObject;
        wall.SetActive(true);
        wall.name = name;
        wall.transform.localRotation = Quaternion.Euler(0, angle, 0);
        wall.transform.localPosition = pos;
    }

    void PlaceWall(Vector3 pos, int angle, string name)
    {
        PlaceTemplate(Wall, pos, angle, name);
    }

    void PlaceAugmentedWall(Vector3 pos, int angle, string name)
    {
        PlaceTemplate(AugmentedWall, pos, angle, name);
    }

    void GenerateFrame()
    {
        var down = Vector3.back * CellHeight;
        var right = Vector3.right * CellWidth;

        for (int col = 0; col < MazeWidth + 1; ++col)
        {
            for (int row = 0; row < MazeHeight + 1; ++row)
            {
                var pos = down * row + right * col;
                var name = string.Format("Pillar_{0:D2}_{1:D2}", col, row);
                PlaceTemplate(EdgePillar, pos, 0, name);
            }
        }

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
            right * 0.5f + down,
            right + down * 0.5f,
            right * 0.5f,
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
            (int)Maze.Direction.Right,
            (int)Maze.Direction.Down,
        };

        {
            int dirIdx = (int)Maze.Direction.Left;
            int col = 0;
            for (int row = 0; row < MazeHeight; ++row)
            {
                var pos = row * down + col * right;
                var cell = maze.Get(col, row);
                if (cell.walls[dirIdx])
                {
                    PlaceWall(pos + directionOffset[dirIdx], directionAngle[dirIdx], string.Format("MazeWall_{0:D2}_{1:D2}_{2}", col, row, dirIdx));
                }
            }
        }

        {
            int dirIdx = (int)Maze.Direction.Up;
            int row = MazeHeight - 1;  // Note Bart: This is not correct, should be 0 (maze inverted?)
            for (int col = 0; col < MazeWidth; ++col)
            {
                var pos = row * down + col * right;
                var cell = maze.Get(col, row);
                if (cell.walls[dirIdx])
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

        Floor.transform.localPosition = right * -0.5f + down * -0.5f;
        Floor.transform.localScale = right * (MazeWidth + 1) + down * (MazeHeight + 1);

        // Place player in maze
        var spawnX = 0;
        var spawnY = 0;

        var playerLocalPos = right * (0.5f + spawnX) + down * (0.5f + spawnY);
        
        // Find first wall-less direction
        var startCell = maze.Get(spawnX, spawnY);
        var playerLocalDir = Quaternion.identity;
        for (int ii = (int)Maze.Direction.Count - 1; ii >= 0; --ii)
        {
            if (!startCell.walls[ii])
            {
                playerLocalDir = Quaternion.Euler(0, directionAngle[ii] + 180, 0); // Note Bart: addition of 180 is should not be neccesary (inverted maze?)
            }
        }

        playerSpawn.position = transform.TransformVector(playerLocalPos);
        playerSpawn.rotation = transform.rotation * playerLocalDir;
    }
}
