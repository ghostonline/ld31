using UnityEngine;
using System.Collections;

public class DisplacementPointer : MonoBehaviour
{

    public Transform rotationMirror;
    public GameObject geometry;
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

            if (visible)
            {
                PhoneCamera.cullingMask &= ~augmentedMask;
            }
            else
            {
                PhoneCamera.cullingMask |= augmentedMask;
            }
        }
    }
}
