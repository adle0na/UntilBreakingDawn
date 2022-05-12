using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    private class PoolItem
    {
        public bool       _isActive;
        public GameObject _gameObject;
    }

    private int _increaseCount = 5;
    private int _maxCount;
    private int _activeCount;

    private GameObject     _poolObject;
    private List<PoolItem> _poolItemList;

    public int MaxCount    => _maxCount;
    public int ActiveCount => _activeCount;

    public MemoryPool(GameObject poolObject)
    {
        _maxCount        = 0;
        _activeCount     = 0;
        this._poolObject = poolObject;

        _poolItemList    = new List<PoolItem>();

        InstantiateObjects();
    }

    public void InstantiateObjects()
    {
        _maxCount += _increaseCount;

        for (int i = 0; i < _increaseCount; ++i)
        {
            PoolItem poolItem    = new PoolItem();
            
            poolItem._isActive   = false;
            poolItem._gameObject = GameObject.Instantiate(_poolObject);
            poolItem._gameObject.SetActive(false);
            
            _poolItemList.Add(poolItem);
        }
    }

    public void DestroyObjects()
    {
        if (_poolItemList == null) return;

        int count = _poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(_poolItemList[i]._gameObject);
        }
        
        _poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()
    {
        if (_poolItemList == null) return null;

        if (_maxCount == _activeCount)
        {
            InstantiateObjects();
        }

        int count = _poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = _poolItemList[i];

            if (poolItem._isActive == false)
            {
                _activeCount++;

                poolItem._isActive = true;
                poolItem._gameObject.SetActive(true);

                return poolItem._gameObject;
            }
        }

        return null;
    }

    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (_poolItemList == null || removeObject == null) return;

        int count = _poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = _poolItemList[i];

            if (poolItem._gameObject == removeObject)
            {
                _activeCount--;

                poolItem._isActive = false;
                poolItem._gameObject.SetActive(false);

                return;
            }
        }
    }

    public void DeactivateAllPoolItems()
    {
        if (_poolItemList == null) return;

        int count = _poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = _poolItemList[i];

            if (poolItem._gameObject != null && poolItem._isActive == true)
            {
                poolItem._isActive = false;
                poolItem._gameObject.SetActive(false);
            }
        }

        _activeCount = 0;
    }
}
