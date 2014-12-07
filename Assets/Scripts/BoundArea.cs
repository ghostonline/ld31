using UnityEngine;
using System.Collections;

public class BoundArea : MonoBehaviour {

    public GameObject[] objects;

    public void SetAreaVisible(bool visible)
    {
        foreach (var o in objects)
        {
            if (o != null) { o.SetActive(visible); }
        }
    }

}
