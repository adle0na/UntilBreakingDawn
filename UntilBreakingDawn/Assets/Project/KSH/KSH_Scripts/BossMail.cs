using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMail : InteractionObject
{
    public int mailHealth = 500;

    public override void TakeDamage(int damage)
    {
        mailHealth-= damage;
    }

}
