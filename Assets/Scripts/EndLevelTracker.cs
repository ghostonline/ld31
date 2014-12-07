using UnityEngine;
using System.Collections;

public class EndLevelTracker : MonoBehaviour {

    public KeyCounter keyCounter;
    public GameObject winMessage;
    public GameObject marker;

    BoundArea bound;

    void Start()
    {
        bound = marker.GetComponent<BoundArea>();
        winMessage.SetActive(false);
        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        bound.SetAreaVisible(visible);
        marker.SetActive(visible);
    }

    void OnPickup(GameObject obj)
    {
        winMessage.SetActive(true);
        keyCounter.SetUIVisible(false);
        SetVisible(false);
    }
}
