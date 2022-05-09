using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairColor : MonoBehaviour
{
    [SerializeField] private Image crossHair;

    private void Start()
    {
        crossHair.color = new Color(1, 1, 1, 0.75f);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f))
        {
            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                crossHair.color = new Color(0, 1, 0, 0.75f);
            }
            else
            {
                crossHair.color = new Color(1, 1, 1, 0.75f);
            }
        }
        else
        {
            crossHair.color = new Color(1, 1, 1, 0.75f);
        }
    }
}
