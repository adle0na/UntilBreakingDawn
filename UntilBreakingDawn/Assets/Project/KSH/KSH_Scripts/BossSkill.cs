using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : InteractionObject
{
    public float skillHealth = 100.0f;

    public override void TakeDamage(int damage)
    {
        skillHealth -= damage;
    }
}
