using UnityEngine;
using System.Collections;

public class KeyCounter : MonoBehaviour {

    public int keys;

    public GameObject[] keyIcons;
    public EndLevelTracker endGame;
    public AudioSource player;
    public AudioClip sfx;

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
        player.PlayOneShot(sfx);

        ++keys;
        SetUIVisible(true);
        Destroy(key);
        
        var area = key.GetComponent<BoundArea>();
        area.SetAreaVisible(false);

        if (keys >= keyIcons.Length)
        {
            endGame.SetVisible(true);
        }
    }

}
