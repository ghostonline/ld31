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
        var down = Vector3.forward * CellHeight;
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
            PlaceWall(posL, 270, string.Format("Bound_L{0:D2}", row));
            PlaceWall(posR, 90, string.Format("Bound_R{0:D2}", row));
        }

        for (int col = 0; col < MazeWidth; ++col)
        {
            var posB = right * col + right * 0.5f;
            var posT = posB + down * MazeHeight;
            PlaceAugmentedWall(posB, 180, string.Format("AugmentedBound_B{0:D2}", col));
            PlaceAugmentedWall(posT, 0, string.Format("AugmentedBound_T{0:D2}", col));
            PlaceWall(posB, 180, string.Format("Bound_B{0:D2}", col));
            PlaceWall(posT, 0, string.Format("Bound_T{0:D2}", col));
        }

        Floor.transform.localPosition = right * -0.5f + down * -0.5f;
        Floor.transform.localScale = right * (MazeWidth + 1) + down * (MazeHeight + 1);
    }
}
