using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    public float health = 100.0f;

    private bool _thisobj1 = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            if (_thisobj1)
            {
                Bullet bullet = other.GetComponent<Bullet>();
                health -= bullet.damage;
            }
        }
    }
}
