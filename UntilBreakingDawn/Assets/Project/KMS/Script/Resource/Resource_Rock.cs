using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Rock : Resource_Base
{
    [Header("???? HP")]
    public int rockHp = 20;
    public AudioSource[] AudioSources;

    private void OnEnable()
    {
        base.HP = rockHp;
    }
    public override void OnHit()
    {
        base.OnHit();
        AudioSources[0].Play();
        resource = this.gameObject;
        if (HP <= 0)
        {
            AudioSources[1].Play();
            base.ResourceGenerate();
            base.Destroyed();
        }
    }
}
