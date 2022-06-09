using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnifeCollider : MonoBehaviour
{
    [SerializeField] private ImpactMemoryPool _impactMemoryPool;
    [SerializeField] private Transform        _knifeTransform;

    private new Collider _collider;
    private     int      _damage;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    public void StartCollider(int damage)
    {
        Debug.Log("���ݻ��� ����");
        this._damage = damage;
        _collider.enabled = true;

        StartCoroutine("DisablebyTime",0.1f);
    }

    private IEnumerator DisablebyTime(float time)
    {
        yield return new WaitForSeconds(time);

        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _impactMemoryPool.SpawnImpact(other, _knifeTransform);

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<InteractionObject>().TakeDamage(_damage);
            Debug.Log("�� Į�� ����");
        }
        else if (other.CompareTag("ExplosiveBarrel"))
        {
            other.GetComponent<InteractionObject>().TakeDamage(_damage);
        }
    }
}
