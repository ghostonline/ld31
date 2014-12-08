using UnityEngine;
using System.Collections;

public class PhoneController : MonoBehaviour {

    public Camera viewCamera;
    public Collider screen;
    public Transform phone;
    public Transform normal;
    public Transform aim;
    public PhoneUI ui;
    
    public int selectButton = 0;

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
        var aimBtnDown = Input.GetButton("Fire1");
        if (aimBtnDown && !aiming)
        {
            AlignPhone(aim);
            aiming = true;
        }
        else if (!aimBtnDown && aiming)
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
                ui.OnScreenTap();
            }
            hot = false;
            selectDown = false;
        }
    }
}
