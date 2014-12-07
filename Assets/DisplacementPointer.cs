using UnityEngine;
using System.Collections;

public class DisplacementPointer : MonoBehaviour
{

    public Transform rotationMirror;
    public GameObject geometry;
    public DistanceTrigger trigger;

    bool visible = true;

    void Update () {
        if (trigger.shouldTrigger)
        {
            transform.rotation = Quaternion.Inverse(rotationMirror.rotation);
        }

        if (trigger.shouldTrigger != visible)
        {
            visible = trigger.shouldTrigger;
            geometry.SetActive(visible);
        }
    }
}
