﻿using UnityEngine;
using System.Collections;

public class EndLevelTracker : MonoBehaviour {

    public KeyCounter keyCounter;
    public GameObject winMessage;
    public GameObject marker;

    public AudioSource player;
    public AudioClip sfx;

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
        player.PlayOneShot(sfx);

        winMessage.SetActive(true);
        keyCounter.SetUIVisible(false);
        SetVisible(false);
    }
}
