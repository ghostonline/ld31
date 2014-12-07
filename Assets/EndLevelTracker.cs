using UnityEngine;
using System.Collections;

public class EndLevelTracker : MonoBehaviour {

    public KeyCounter counter;
    public int minKeyCount;
    public GameObject winMessage;

    void Start()
    {
        winMessage.SetActive(false);
    }

    void OnPickup(GameObject obj)
    {
        if (counter.keys < minKeyCount)
        {
            Debug.Log("Need more keys");
        }
        else
        {
            Destroy(obj);
            Debug.Log("Game complete!");
            counter.SetUIVisible(false);
            winMessage.SetActive(true);
        }
    }
}
