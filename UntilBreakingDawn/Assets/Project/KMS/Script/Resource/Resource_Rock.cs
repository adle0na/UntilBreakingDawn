using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Rock : Resource_Base
{
    public AudioSource[] AudioSources;

    private const int rockHp = 20;

    private void OnEnable()
    {
        base.HP = rockHp;
    }
    public override void OnHit()
    {
        base.OnHit();
        AudioSources[0].Play();

        if (HP <= 0)
        {
            AudioSources[1].Play();
            base.ResourceGenerate();
            base.Destroyed();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        resource = this.gameObject;
    }
}
