using UnityEngine;
using System.Collections;

public class MazeBuilder : MonoBehaviour {

    public int MazeWidth = 10;
    public int MazeHeight = 10;

    public int CellWidth = 5;
    public int CellHeight = 5;

    public GameObject EdgePillar;
    public GameObject Floor;
    public GameObject AugmentedWall;

    void Start () {
        EdgePillar.SetActive(false);
        EdgePillar.transform.parent = transform;
        AugmentedWall.SetActive(false);
        AugmentedWall.transform.parent = transform;
        Floor.transform.parent = transform;

        GenerateFrame();
    }

    void PlaceWall(Vector3 pos, int angle, string name)
    {
        var wall = GameObject.Instantiate(AugmentedWall) as GameObject;
        wall.SetActive(true);
        wall.name = name;
        wall.transform.localRotation = Quaternion.Euler(0, angle, 0);
        wall.transform.localPosition = pos;
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
                var pillar = GameObject.Instantiate(EdgePillar) as GameObject;
                pillar.SetActive(true);
                pillar.name = string.Format("Pillar_{0:D2}_{1:D2}", col, row);
                pillar.transform.localPosition = pos;
            }
        }

        for (int row = 0; row < MazeHeight; ++row)
        {
            var posX = down * row + down * 0.5f;
            var nameA = string.Format("AugmentedBound_L{0:D2}", row);
            PlaceWall(posX, 270, nameA);
            var nameB = string.Format("AugmentedBound_R{0:D2}", row);
            PlaceWall(posX + right * MazeWidth, 90, nameB);
        }

        for (int col = 0; col < MazeWidth; ++col)
        {
            var posY = right * col + right * 0.5f;
            var nameA = string.Format("AugmentedBound_B{0:D2}", col);
            PlaceWall(posY, 180, nameA);
            var nameB = string.Format("AugmentedBound_T{0:D2}", col);
            PlaceWall(posY + down * MazeHeight, 0, nameB);
        }

        Floor.transform.localPosition = right * -0.5f + down * -0.5f;
        Floor.transform.localScale = right * (MazeWidth + 1) + down * (MazeHeight + 1);
    }
}
