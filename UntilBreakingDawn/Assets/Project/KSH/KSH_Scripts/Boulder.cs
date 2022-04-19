using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            Destroy(gameObject);
        }

        else if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    

}
