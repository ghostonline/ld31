using UnityEngine;
using System.Collections;

public class RotatableUI : MonoBehaviour {

    public Transform rotationMirror;

    void Update () {
        transform.localRotation = Quaternion.Inverse(rotationMirror.parent.localRotation);
    }
}
