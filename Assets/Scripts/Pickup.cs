using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    public GameObject PickupListener;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("Pickup {0}", name));
        PickupListener.SendMessage("OnPickup", gameObject);
    }
}
