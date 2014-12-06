using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

    public Transform target;

    void FixedUpdate () {
        rigidbody.MovePosition(target.position);
    }
}
