using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public enum ImpactType {Default = 0, Wood = 1, Metal = 2, Stone = 3, Enemy = 4, ExplosiveBarrel = 5, Animals = 6, }

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _impactPrefab;

    private MemoryPool[] _memoryPool;

    private void Awake()
    {
        _memoryPool = new MemoryPool[_impactPrefab.Length];
        for (int i = 0; i < _impactPrefab.Length; ++i)
        {
            _memoryPool[i] = new MemoryPool(_impactPrefab[i]);
        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        switch (hit.transform.tag)
        {
            
            case "Dirt" :
                OnSpawnImpact(ImpactType.Default, hit.point, Quaternion.LookRotation(hit.normal));
                break;
                
            case "Wood" :
                OnSpawnImpact(ImpactType.Wood, hit.point, Quaternion.LookRotation(hit.normal));
                break;
            
            case "Metal" :
                OnSpawnImpact(ImpactType.Metal, hit.point, Quaternion.LookRotation(hit.normal));
                break;
            
            case "Stone" :
                OnSpawnImpact(ImpactType.Stone, hit.point, Quaternion.LookRotation(hit.normal));
                break;
            
            case "Enemy" :
                OnSpawnImpact(ImpactType.Enemy, hit.point, Quaternion.LookRotation(hit.normal));
                break;
            
            case "Animals" :
                OnSpawnImpact(ImpactType.Animals, hit.point, Quaternion.LookRotation(hit.normal));
                break;
            
            case "ExplosiveBarrel" :
                Color color = hit.transform.GetComponentInChildren<MeshRenderer>().material.color;
                OnSpawnImpact(ImpactType.ExplosiveBarrel, hit.point, Quaternion.LookRotation(hit.normal), color);
                break;
                
        }
        
    }

    public void SpawnImpact(Collider other, Transform knifeTransform)
    {
        if (other.CompareTag("Enemy"))
        {
            OnSpawnImpact(ImpactType.Enemy, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        if (other.CompareTag("Animals"))
        {
            OnSpawnImpact(ImpactType.Animals, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation));
        }
        else if (other.CompareTag("ExplosiveBarrel"))
        {
            Color color = other.transform.GetComponentInChildren<MeshRenderer>().material.color;
            OnSpawnImpact(ImpactType.ExplosiveBarrel, knifeTransform.position, Quaternion.Inverse(knifeTransform.rotation), color);
        }
    }

    public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation, Color color = new Color())
    {
        GameObject item = _memoryPool[(int) type].ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impact>().Setup(_memoryPool[(int)type]);

        if (type == ImpactType.ExplosiveBarrel)
        {
            ParticleSystem.MainModule main = item.GetComponent<ParticleSystem>().main;
            main.startColor = color;
        }
    }
}
