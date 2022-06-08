using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Base : MonoBehaviour, IHit
{
    // 생성된 자원이 속할 부모 트랜스폼
    private Transform resourceTranform = null;

    protected int HP;
    public int weaponDamage
    {
        get;
        set;
    }

    protected GameObject resource = null; 
    public GameObject[] ResourcePrefabs = null; // 생성될 드롭 자원

    // Unity Message
    private void Awake()
    {
        resourceTranform = GameObject.Find("GetResource").transform;
    }

    // Active
    // 오브젝트가 사용되었을 때 실행할 함수
    public virtual void OnHit()
    {
        Debug.Log("OnHit");
        HP -= weaponDamage;

        Debug.Log(HP);
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
        GameObject gameObject = GameObject.Instantiate(ResourcePrefabs[resourceType], this.transform);
        gameObject.transform.parent = resourceTranform;
    }
}
