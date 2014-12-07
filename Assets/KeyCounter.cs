using UnityEngine;
using System.Collections;

public class KeyCounter : MonoBehaviour {

    public int keys;

    public GameObject[] keyIcons;

    void Start()
    {
        foreach (var icon in keyIcons)
        {
            if (icon != null) { icon.SetActive(false); }
        }
    }

    void OnPickup(GameObject key)
    {
        if (keys < keyIcons.Length && keyIcons[keys] != null)
        {
            keyIcons[keys].SetActive(true);
        }

        ++keys;
        Destroy(key);
    }

}
