using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Serializable]
    public class ObjectPoolEntry 
    {
        [SerializeField] private GameObject mPrefab;
        [SerializeField] private int mPoolSize =3;

        public GameObject Prefab => mPrefab;
        public int PoolCount => mPoolSize;

    }
    


  private static PoolManager instance = null;

    public static PoolManager Instance=>instance;
    private const string OBJECT_POOL = "ObjectPool";

    [SerializeField] private ObjectPoolEntry[] mEntries;
    private List<GameObject>[] mPools;
    private GameObject mContainer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else 
        {
            instance = this;
            InitializePool();
        }
    }

    public void PoolObject(GameObject gameObj, bool keepActive)
    {
        var found = false;
        for (int i = 0; i < mEntries.Length && !found; i++)
        {
            //verify this object has a pool
            if (gameObj.name == mEntries[i].Prefab.name)
            {
                gameObj.SetActive(keepActive);
                gameObj.transform.SetParent(mContainer.transform);
                mPools[i].Add(gameObj);
                found = true;

            }
        }
    }

  
    private void InitializePool()
    {
        //create the hiearchy container
        mContainer = new GameObject(OBJECT_POOL);
        mContainer.transform.SetParent(transform);

        // create pools for each entry defined
        mPools = new List<GameObject>[mEntries.Length];
        for (int i = 0; i < mEntries.Length; i++) 
        {
            var poolEntry = mEntries[i];
            //initalize the pool it will use
            mPools[i] = new List<GameObject>();
            for (int j = 0; j < poolEntry.PoolCount; j++)
            {
                //instantiate the prefab
                var gameObj = Instantiate(poolEntry.Prefab);
                //name it accordingly
                gameObj.name = poolEntry.Prefab.name;
                //place it in its poool deactivated 
                PoolObject(gameObj,false);
            
            }
            
        }

    }

    public GameObject GetObjectOfType(string objectType, bool onlyPooled = true) 
    {
        GameObject gameObj = null;
        var found = false;

        for (int i = 0; i < mEntries.Length && !found; i++)
        {
            // Look for the matching entry
            if (mEntries[i].Prefab.name == objectType)
            {
                found = true;
                // verify an object is available in the pool
                if (mPools[i].Count > 0)
                {
                    //retrieve the object
                    gameObj = mPools[i][0];

                    //remove it from the pool \
                    mPools[i].RemoveAt(0);

                    //remove it from the hirachy container
                    gameObject.transform.SetParent(null);

                    //active
                    gameObj.SetActive(true);
                }
                //pool is emtpy, verify if permitted to create a new object
                else if (!onlyPooled)
                {
                    var newObject = Instantiate(mEntries[i].Prefab);
                    newObject.name = mEntries[i].Prefab.name;
                    gameObj = newObject;

                }
            }
        }
        return gameObj;
    }
}
