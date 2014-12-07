using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

    public Transform target;
    public float speed = 10.0f;
    public float disconnectDistance = 0.5f;

    void Start()
    {
        transform.position = target.position;
    }

    void FixedUpdate () {
        var dist = Vector3.Distance(transform.position, target.position);
        if (dist > disconnectDistance)
        {
            rigidbody.velocity = Vector3.zero;
        }
        else
        {
            var nearest = Vector3.MoveTowards(transform.position, target.position, 1);
            rigidbody.velocity = (nearest - transform.position) * speed;
        }
    }
}
