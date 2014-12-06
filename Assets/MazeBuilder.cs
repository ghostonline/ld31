using UnityEngine;
using System.Collections;

public class MazeBuilder : MonoBehaviour {

    public int MazeWidth = 10;
    public int MazeHeight = 10;

    public int CellWidth = 5;
    public int CellHeight = 5;

    public GameObject EdgePillar;
    public GameObject Floor;

    void Start () {
        EdgePillar.SetActive(false);
        Floor.transform.parent = transform;

        GenerateFrame();
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
                pillar.transform.parent = transform;
                pillar.transform.localPosition = pos;
            }
        }

        Floor.transform.localPosition = right * -0.5f + down * -0.5f;
        Floor.transform.localScale = right * (MazeWidth + 1) + down * (MazeHeight + 1);
    }
}
