using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resourcePool : MonoBehaviour
{
    private const int DEFAULT_POOL_SIZE = 4;

    private int[] resourceQueueSize = null;

    public GameObject[] resourcePrefabs = null;
    private Queue<GameObject> poolsResource = null;

    private static resourcePool instance = null;
    public static resourcePool Inst
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void Initialized()
    {
        poolsResource = new Queue<GameObject>(resourcePrefabs.Length);
        resourceQueueSize = new int[resourcePrefabs.Length];
    }
    //public GameObject GetResource()
    //{
    //    GameObject result = null;

    //    return 0;
    //}
    public void ReturnResource()
    {
        Debug.Log("return");
    }
}
