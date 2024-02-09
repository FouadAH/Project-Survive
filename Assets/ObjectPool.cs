using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PooledObject[] _prefab;
    public int[] _poolSizes;
    public List<PooledObject>[] _pools;

    void Start()
    {
        _pools = new List<PooledObject>[_prefab.Length];

        //Define each list. Set the initial length of each list by adding in the available PooledObject prefabs.
        ///Each list should have it's own prefab that populates it, they will not be mixed together
        for (int j = 0; j < _pools.Length; j++)
        {
            _pools[j] = new List<PooledObject>();
            for (int i = 0; i < _poolSizes[j]; i++)
            {
                PooledObject obj = Instantiate(_prefab[j], transform) as PooledObject;
                obj.gameObject.SetActive(false);
                _pools[j].Add(obj);
            }
        }
    }


    //array pooledobject
    public PooledObject Get(PooledObject _projectile)
    {
        for (int i = 0; i < _prefab.Length; i++)
        {
            //In the script that calls this method, we check to match if that enemy's projectile matches the projectile in our pool.  If so, we activate one, it is moved in the fire script.
            if (_prefab[i] == _projectile)
            {
                //check to see the length of each list - to be removed after debug.
                //Debug.Log("This pool is list " + i + ".  This pool's size = " + _pools[i].Count);
                for (int j = 0; j < _pools[i].Count; j++)
                {
                    if (_pools[i][j].gameObject.activeInHierarchy == false)
                    {
                        _pools[i][j].gameObject.SetActive(true);
                        return _pools[i][j];
                    }
                    //if we're at the end of the list's count/length, then we need to increase the pool and use an additional object
                    if ((j == (_pools[i].Count - 1)) && (_pools[i][j].gameObject.activeInHierarchy == true))
                    {
                        IncreasePool(i);

                        return _pools[i][j + 1];
                    }
                }

            }
        }
        return null;
    }

    //Increase the pool by instantiating another object correctly coorelating to the specified List, make sure it's active, and add it to the correct List.
    private void IncreasePool(int _specifiedPool)
    {
        PooledObject obj = Instantiate(_prefab[_specifiedPool]) as PooledObject;
        obj.gameObject.SetActive(true);
        _pools[_specifiedPool].Add(obj);
    }
}
