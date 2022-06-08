using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource_Base : MonoBehaviour, IHit
{
    // ������ �ڿ��� ���� �θ� Ʈ������
    private Transform resourceTranform = null;

    protected int HP;
    public int weaponDamage
    {
        get;
        set;
    }

    protected GameObject resource = null; 
    public GameObject[] ResourcePrefabs = null; // ������ ��� �ڿ�

    // Unity Message
    private void Awake()
    {
        resourceTranform = GameObject.Find("GetResource").transform;
    }

    // Active
    // ������Ʈ�� ���Ǿ��� �� ������ �Լ�
    public virtual void OnHit()
    {
        Debug.Log("OnHit");
        HP -= weaponDamage;

        Debug.Log(HP);
    }
    // ������Ʈ�� �ı��Ǿ��� �� ������ �Լ�
    protected void Destroyed()
    {
        Debug.Log("destroyed");
        resource.SetActive(false);
    }
    // �ڿ� ���� �Լ�
    public void ResourceGenerate()
    {
        int resourceType = Random.Range(0, ResourcePrefabs.Length);
        GameObject gameObject = GameObject.Instantiate(ResourcePrefabs[resourceType], this.transform);
        gameObject.transform.parent = resourceTranform;
    }
}
