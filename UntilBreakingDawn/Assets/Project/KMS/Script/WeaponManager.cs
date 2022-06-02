using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private CloseWeapon_Axe[] CWS;

    private Dictionary<string, CloseWeapon_Axe> CWDictionary = new Dictionary<string, CloseWeapon_Axe>();

    private void Start()
    {
        foreach(CloseWeapon_Axe closeWeapon in CWS)
        {
            CWDictionary.Add(closeWeapon.name, closeWeapon);
        }
    }
}
