﻿using UnityEngine;
using System.Collections;

[UnityEngine.ExecuteInEditMode]
public class Pointer : MonoBehaviour {

    public Transform origin;
    public Transform target;

    void Update () {
        var forward = origin.position - target.position;
        transform.localRotation = Quaternion.LookRotation(forward);
    }
}
