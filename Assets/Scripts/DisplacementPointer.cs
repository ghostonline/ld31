﻿using UnityEngine;
using System.Collections;

public class DisplacementPointer : MonoBehaviour
{

    public Transform rotationMirror;
    public GameObject geometry;
    public GameObject prompt;
    public DistanceTrigger trigger;
    public Camera PhoneCamera;

    bool visible = true;
    int augmentedMask;

    void Start()
    {
        augmentedMask = 1 << LayerMask.NameToLayer("Augmented");
    }

    void Update () {
        if (trigger.shouldTrigger)
        {
            transform.rotation = Quaternion.Inverse(rotationMirror.rotation);
        }

        if (trigger.shouldTrigger != visible)
        {
            visible = trigger.shouldTrigger;
            geometry.SetActive(visible);
            prompt.SetActive(visible);

            if (visible)
            {
                PhoneCamera.cullingMask &= ~augmentedMask;
            }
            else
            {
                PhoneCamera.cullingMask |= augmentedMask;
            }
        }

        if (visible)
        {
            var point = PhoneCamera.WorldToViewportPoint(trigger.transform.position);
            geometry.SetActive(point.x < 0 || 1 < point.x || point.y < 0 || 1 < point.y || point.z < 0);
        }
    }
}
