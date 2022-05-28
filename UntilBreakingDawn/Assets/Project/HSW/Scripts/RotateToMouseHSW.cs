using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouseHSW : MonoBehaviour
{
    [SerializeField]
    private float rotCamXAxisSpeed = 5;

    [SerializeField]
    private float rotCamYAxisSpeed = 3;

    private float limitMinX = -80;
    private float limitMaxX = 50;
    private float eulerAngleX;
    private float eulerAngleY;

    private CraftManual craft;

    private void Awake()
    {
        craft = GameObject.Find("CraftTab").GetComponent<CraftManual>();
    }

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamXAxisSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
        
        if (craft.isActivated == true && craft.isPreviewActivated == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (craft.isActivated == true && craft.isPreviewActivated == true)
        {
            transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (craft.isActivated == false && craft.isPreviewActivated == false)
        {
            transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}
