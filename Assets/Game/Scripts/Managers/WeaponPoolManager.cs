using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;

namespace HyperlabCase.Managers
{
    public class WeaponPoolManager : SerializedMonoBehaviour
    {
        [DictionaryDrawerSettings(KeyLabel = "Weapon Type", ValueLabel = "Prefab")]
        [SerializeField] private Dictionary<PoolObjectTypes, GameObject> poolObjects = new Dictionary<PoolObjectTypes, GameObject>();

        private Dictionary<PoolObjectTypes, List<GameObject>> poolDictionary = new Dictionary<PoolObjectTypes, List<GameObject>>();
        private GameObject poolParentGO;


        private void Awake()
        {
            poolParentGO = new GameObject("Object Pool");
        }

        private void OnEnable()
        {
            EventManager.GetWeaponPoolManager += GetPoolManager;
        }

        private void OnDisable()
        {
            EventManager.GetWeaponPoolManager -= GetPoolManager;
        }

        public GameObject GetPooledObject(PoolObjectTypes objectType)
        {
            if (poolDictionary.ContainsKey(objectType))
            {
                List<GameObject> pool = poolDictionary[objectType];

                for (int i = 0; i < pool.Count; i++)
                {
                    if (!pool[i].activeInHierarchy)
                    {
                        pool[i].SetActive(true);
                        return pool[i];
                    }
                }
                GameObject newObject = Instantiate(GetObjectPrefab(objectType));
                poolDictionary[objectType].Add(newObject);
                newObject.transform.SetParent(poolParentGO.transform);

                return newObject;
            }
            else
            {
                GameObject newObject = Instantiate(GetObjectPrefab(objectType));
                poolDictionary.Add(objectType, new List<GameObject>() { newObject });
                newObject.transform.SetParent(poolParentGO.transform);

                return newObject;
            }
        }

        public void SendObjectToPool(GameObject go)
        {
            go.SetActive(false);
        }

        private GameObject GetObjectPrefab(PoolObjectTypes weaponType)
        {
            return poolObjects[weaponType];
        }

        private WeaponPoolManager GetPoolManager()
        {
            return this;
        }
    }
}
