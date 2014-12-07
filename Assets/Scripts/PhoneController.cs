using UnityEngine;
using System.Collections;

public class PhoneController : MonoBehaviour {

    public Camera viewCamera;
    public Collider screen;
    public Transform phone;
    public Transform normal;
    public Transform aim;
    
    public int aimButton = 0;
    public int selectButton = 1;

    bool aiming;
    bool hot;
    bool selectDown;

    void AlignPhone(Transform anchor)
    {
        phone.parent = anchor;
        phone.localPosition = Vector3.zero;
        phone.localRotation = Quaternion.identity;
    }

    bool IsOnScreen()
    {
        var ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return screen.Raycast(ray, out hit, float.PositiveInfinity);
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

        if (Input.GetMouseButton(selectButton))
        {
            if (!selectDown)
            {
                hot = IsOnScreen();
                selectDown = true;
            }
        }
        else if (selectDown)
        {
            if (hot && IsOnScreen())
            {
                Debug.Log("Screen tap");
            }
            hot = false;
            selectDown = false;
        }
    }
}
