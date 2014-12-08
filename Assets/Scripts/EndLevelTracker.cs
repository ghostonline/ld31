using UnityEngine;
using System.Collections;

public class EndLevelTracker : MonoBehaviour {

    public KeyCounter keyCounter;
    public PointCounter pointCounter;
    public PhoneUI phoneUI;
    public GameObject marker;

    public AudioSource player;
    public AudioClip sfx;

    BoundArea bound;

    void Start()
    {
        bound = marker.GetComponent<BoundArea>();
        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        bound.SetAreaVisible(visible);
        marker.SetActive(visible);
    }

    void OnPickup(GameObject obj)
    {
        player.PlayOneShot(sfx);

        phoneUI.ShowVictory(pointCounter.points, pointCounter.GetTotal());

        keyCounter.SetUIVisible(false);
        SetVisible(false);
    }
}
