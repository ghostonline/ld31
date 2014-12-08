using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour {

    public int points;
    public Text ui;
    public AudioSource player;
    public AudioClip sfx;

    int total;

    public void SetTotal(int total)
    {
        this.total = total;
        UpdateUI();
    }

    public int GetTotal()
    {
        return total;
    }

    void UpdateUI()
    {
        ui.text = string.Format("{0}/{1}", points, total);
    }

    void OnPickup(GameObject point)
    {
        player.PlayOneShot(sfx);

        Destroy(point);
        ++points;
        UpdateUI();
    }

}
