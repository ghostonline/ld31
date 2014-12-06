using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

    public Transform target;
    public float speed = 10.0f;

    void FixedUpdate () {
        var nearest = Vector3.MoveTowards(transform.position, target.position, 1);
        rigidbody.velocity = (nearest - transform.position) * speed;
    }
}
