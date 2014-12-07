using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour {

    public int points;
    public Text ui;

    int total;

    public void SetTotal(int total)
    {
        this.total = total;
        UpdateUI();
    }

    void UpdateUI()
    {
        ui.text = string.Format("{0}/{1}", points, total);
    }

    void OnPickup(GameObject point)
    {
        Destroy(point);
        ++points;
        UpdateUI();
    }

}
