using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.GetComponent<ItemBase>().Use(transform.parent.gameObject);
        }
    }
}
