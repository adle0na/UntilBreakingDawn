using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponType2
{
    Axe = 0,
    PickAxe = 1,
    Hand = 2,
    Knife = 3
}
public class CloseWeapon_Another : MonoBehaviour
{
    private RaycastHit hitInfo;

    // ���� ����
    private int weaponDamage = 5;
    private float weaponRange;

    // ���� ���� �Ǵ�
    private bool isAttack = false;
    private bool isSwing = false;
}
