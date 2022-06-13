using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void FootStep(AudioClip clip)
    {
        SoundManager_K.instance.soundManager_KSH.seAudio.PlayOneShot(clip);
    }

    public void EnemyAttack(AudioClip clip)
    {
        SoundManager_K.instance.soundManager_KSH.seAudio.PlayOneShot(clip);
    }

    public void EnemyHit(AudioClip clip)
    {
        SoundManager_K.instance.soundManager_KSH.seAudio.PlayOneShot(clip);
    }

    public void EnemyDie(AudioClip clip)
    {
        SoundManager_K.instance.soundManager_KSH.seAudio.PlayOneShot(clip);
    }

    public void MinoTaunt(AudioClip clip)
    {
        SoundManager_K.instance.soundManager_KSH.seAudio.PlayOneShot(clip);
    }

    public void AnimalsRun(AudioClip clip)
    {
        SoundManager_K.instance.soundManager_KSH.seAudio.PlayOneShot(clip);
    }

    public void AnimalsTaunt(AudioClip clip)
    {
        SoundManager_K.instance.soundManager_KSH.seAudio.PlayOneShot(clip);
    }

    public void AnimalsIdle(AudioClip clip)
    {
        SoundManager_K.instance.soundManager_KSH.seAudio.PlayOneShot(clip);
    }
}
