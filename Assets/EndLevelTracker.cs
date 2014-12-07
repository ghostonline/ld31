using UnityEngine;
using System.Collections;

public class EndLevelTracker : MonoBehaviour {

    public KeyCounter counter;
    public int minKeyCount;

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
        }
    }
}
