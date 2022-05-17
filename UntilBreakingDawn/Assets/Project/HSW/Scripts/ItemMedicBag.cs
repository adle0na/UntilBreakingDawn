using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMedicBag : ItemBase
{
    [SerializeField] private GameObject _hpEffectPrefab;
    [SerializeField] private int        _increaseHP    = 50;
    [SerializeField] private float      _moveDistance  = 0.2f;
    [SerializeField] private float      _pingpongSpeed = 0.5f;
    [SerializeField] private float      _rotateSpeed    = 50;

    private IEnumerator Start()
    {
        float y = transform.position.y;

        while (true)
        {
            transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);

            Vector3 position   = transform.position;
            position.y         = Mathf.Lerp(y, y + _moveDistance, Mathf.PingPong(Time.time * _pingpongSpeed, 1));
            transform.position = position;

            yield return null;
        }
    }

    public override void Use(GameObject entity)
    {
        entity.GetComponent<Status>().IncreaseHP(_increaseHP);

        Instantiate(_hpEffectPrefab, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}
