using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagazine : ItemBase
{
    [SerializeField]
    private GameObject _magazineEffetPrefab;
    [SerializeField]
    private int        _increaseMagazine = 2;
    [SerializeField]
    private float      _rotateSpeed      = 50;

    private IEnumerator Start()
    {
        while (true)
        {
            transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);

            yield return null;
        }
    }

    public override void Use(GameObject entity)
    {
        entity.GetComponentInChildren<WeaponAssultRifle>().IncreaseMagazine(_increaseMagazine);

        Instantiate(_magazineEffetPrefab, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}
