using UnityEngine;
using System.Collections;

public class PowerupMove : MonoBehaviour {

    public Transform mesh;
    public float turnSpeed = 80f;
    public float bounceSpeed = 5f;
    public float bounceHeight = 0.1f;

    float bounceProgress;
    Vector3 baseHeight;

    void Start()
    {
        baseHeight = mesh.localPosition;
    }

    void Update ()
    {
        mesh.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        
        bounceProgress += Time.deltaTime * bounceSpeed;
        if (bounceProgress > 2 * Mathf.PI)
        {
            bounceProgress -= 2 * Mathf.PI;
        }

        mesh.localPosition = baseHeight + Vector3.up * bounceHeight * Mathf.Sin(bounceProgress);
    }
}
