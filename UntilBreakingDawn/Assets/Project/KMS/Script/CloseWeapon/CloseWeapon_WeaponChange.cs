using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon_WeaponChange : MonoBehaviour
{
    //private CloseWeapon_Axe weapon_Axe = null;
    //private void Awake()
    //{
    //    weapon_Axe = GetComponent<CloseWeapon_Axe>();
    //}
    private CloseWeapon_Base closeWeapon_Base = null;
    private CloseWeapon_Axe axe = null;
    private void Awake()
    {
        closeWeapon_Base = new CloseWeapon_Base();
        axe = new CloseWeapon_Axe();
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            axe.isAxe = true;
            Debug.Log(axe.isAxe);
        }
    }
}
