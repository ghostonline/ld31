using UnityEngine;
using System.Collections;

public class KeyCounter : MonoBehaviour {

    public int keys;

    void OnPickup(GameObject key)
    {
        ++keys;
        Destroy(key);
    }

}
