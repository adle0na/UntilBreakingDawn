using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMail : MonoBehaviour
{
    public int mailHealth = 500;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            mailHealth -= bullet.damage;
        }
    }
}
