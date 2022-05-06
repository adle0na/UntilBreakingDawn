using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Tree : Resource_Base
{
    public AudioSource[] AudioSources;
    //private AudioSource playSource = null;
    //private Dictionary<string, AudioSource> treeAudio = new Dictionary<string, AudioSource>();

    private const int TreeHp = 10;

    //private void Start()
    //{        
    //    foreach(AudioSource audioSource in AudioSources)
    //    {
    //        treeAudio.Add(audioSource.name, audioSource);
    //    }
    //}
    private void OnEnable()
    {
        base.HP = TreeHp;
        //playSource = treeAudio["Tree_Hit"];
    }
    public override void OnHit()
    {
        base.OnHit();
        AudioSources[0].Play();
        
        if (HP <= 0)
        {
            //playSource = treeAudio["Tree_Destroyed"];
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
