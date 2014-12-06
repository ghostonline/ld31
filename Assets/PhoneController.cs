using UnityEngine;
using System.Collections;

public class PhoneController : MonoBehaviour {

    public Transform phone;
    public Transform normal;
    public Transform aim;
    
    public int aimButton = 0;

    bool aiming;

    void AlignPhone(Transform anchor)
    {
        phone.parent = anchor;
        phone.localPosition = Vector3.zero;
        phone.localRotation = Quaternion.identity;
    }

    void Start () {
        AlignPhone(normal);
    }

    void Update () {
        if (Input.GetMouseButton(aimButton) && !aiming)
        {
            AlignPhone(aim);
            aiming = true;
        }
        else if (!Input.GetMouseButton(aimButton) && aiming)
        {
            AlignPhone(normal);
            aiming = false;
        }
    }
}
