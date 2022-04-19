using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        StartCoroutine(bull());
    }

    IEnumerator bull()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
