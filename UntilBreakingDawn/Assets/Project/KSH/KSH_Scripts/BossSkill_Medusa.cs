using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill_Medusa : MonoBehaviour
{
    public GameObject Destroy1;
    public GameObject Destroy2;
    public GameObject Destroy3;
    public GameObject Destroy4;

    public bool allDestroy;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 150, 0) * Time.deltaTime);

        CheckDestroy();
    }

    private void CheckDestroy()
    {
        if(Destroy1.GetComponent<Destroy_Medusa>().health < 0 && Destroy2.GetComponent<Destroy_Medusa>().health < 0 &&
            Destroy3.GetComponent<Destroy_Medusa>().health < 0 && Destroy4.GetComponent<Destroy_Medusa>().health < 0)
        {
            allDestroy = true;
        }
    }
}
