using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Tree : Resource_Base
{
    [Header("³ª¹« HP")]
    public int TreeHp = 10;
    public AudioSource[] AudioSources;

    private void OnEnable()
    {
        base.HP = TreeHp;
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
