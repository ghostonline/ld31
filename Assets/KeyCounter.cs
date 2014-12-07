using UnityEngine;
using System.Collections;

public class KeyCounter : MonoBehaviour {

    public int keys;

    public GameObject[] keyIcons;

    public void SetUIVisible(bool visible)
    {
        int idx = 0;
        foreach (var icon in keyIcons)
        {
            if (icon != null) { icon.SetActive(idx < keys && visible); }
            ++idx;
        }
    }

    void Start()
    {
        SetUIVisible(true);
    }

    void OnPickup(GameObject key)
    {
        ++keys;
        SetUIVisible(true);
        Destroy(key);
    }

}
