using UnityEngine;
using System.Collections;

public class MirrorRotation : MonoBehaviour {

    public Transform target;

    void Update () {
        transform.rotation = Quaternion.Inverse(target.rotation);
    }
}
