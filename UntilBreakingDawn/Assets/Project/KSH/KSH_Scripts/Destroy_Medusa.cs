using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Medusa : MonoBehaviour
{
    public float health = 100.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            health -= bullet.damage;
        }
    }
}