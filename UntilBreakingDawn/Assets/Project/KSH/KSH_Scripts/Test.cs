using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float health = 100.0f;
    private bool _thisobj = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            if (_thisobj)
            {
                Bullet bullet = other.GetComponent<Bullet>();
                health -= bullet.damage;
            }
        }
    }
}
