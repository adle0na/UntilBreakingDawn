using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft_Stats : MonoBehaviour
{
    public float Hp = 50.0f;

    void Update()
    {
        Dead();
    }

    void Dead()
    {
        if (Hp <= 0f)
            Destroy(this);
    }
}
