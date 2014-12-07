using UnityEngine;
using System.Collections;

public class DistanceTrigger : MonoBehaviour {

    public Transform target;
    public float maxDistance = 0.5f;

    public bool shouldTrigger;
    public float currentDistance;
    
    void FixedUpdate () {
        currentDistance = Vector3.Distance(target.position, transform.position);
        shouldTrigger = currentDistance > maxDistance;
    }
}
