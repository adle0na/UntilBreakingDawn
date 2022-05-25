using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [Header("ItemBase")]
    [SerializeField]
    public float rotateSpeed = 50;
    
    private IEnumerator Start()
    {
        while (true)
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
