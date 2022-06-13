using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : SkillManager
{
    public float skillHealth = 100.0f;

    public override void TakeDamage2(int damage)
    {
        skillHealth -= damage;
    }
}
