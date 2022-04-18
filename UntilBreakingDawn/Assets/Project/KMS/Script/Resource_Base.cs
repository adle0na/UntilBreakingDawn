using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Base : MonoBehaviour, IHit
{
    private Transform resourceTranform = null;

    protected int HP;
    protected int damage = 0;

    protected GameObject resource = null;
    public GameObject[] ResourcePrefabs = null;

    private void Awake()
    {
        resourceTranform = GameObject.Find("GetResource").transform;
    }
    private void OnEnable()
    {
        HP = 10;
    }
    // 오브젝트가 사용되었을 때 실행할 함수
    public virtual void OnHit()
    {
        HP -= damage;
    }
    // 오브젝트가 파괴되었을 때 실행할 함수
    protected void Destroyed()
    {
        Debug.Log("destroyed");
        resource.SetActive(false);
    }
    // 자원 출현 함수
    public void ResourceGenerate()
    {
        int resourceType = Random.Range(0, ResourcePrefabs.Length);
        GameObject gameObject =  GameObject.Instantiate(ResourcePrefabs[resourceType], this.transform);
        gameObject.transform.parent = resourceTranform;
    }
}
