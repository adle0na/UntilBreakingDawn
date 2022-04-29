using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerHSW : MonoBehaviour
{
    private RotateToMouseHSW _rotateToMouseHsw;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _rotateToMouseHsw = GetComponent<RotateToMouseHSW>();
    }

    private void Update()
    {
        UpdateRotate();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        _rotateToMouseHsw.UpdateRotate(mouseX, mouseY);
    }

}
