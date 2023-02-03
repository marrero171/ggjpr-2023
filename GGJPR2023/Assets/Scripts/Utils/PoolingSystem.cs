using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using TMPro;


using UnityEngine;
// using UnityEngine.Pool;  
namespace Utils
{
    public class PoolingSystem
    {
        public static PoolingSystem instance;
        public List<Enemy> EnemyPool;
        public Dictionary<GameObject, List<GameObject>> MasterPool;
        public bool canSpawnEnemies = false;
        public Transform objectParent; //Literal Object Dumpster
        // Start is called before the first frame update
        public PoolingSystem()
        {
            PoolingSystem.instance = this;
            objectParent = new GameObject("Object Parent").transform;
            MasterPool = new Dictionary<GameObject, List<GameObject>>();
            EnemyPool = new List<Enemy>();
        }

        GameObject AddObjectToEntry(GameObject obj)
        {
            var o = MonoBehaviour.Instantiate(obj, Vector3.zero, Quaternion.identity, objectParent);
            o.SetActive(false);
            MasterPool[obj].Add(o);
            return o;
        }
        public GameObject GetObject(GameObject obj)
        {
            MonoBehaviour.print("Getting " + obj.name);
            GameObject rObj = null;
            if (MasterPool.ContainsKey(obj))
            {
                rObj = MasterPool[obj].Find(o => o != null && !o.activeInHierarchy);
                if (rObj == null) rObj = AddObjectToEntry(obj);
            }
            else
            {
                MasterPool.Add(obj, new List<GameObject>());
                rObj = AddObjectToEntry(obj);
                rObj.transform.parent = MakeParent(obj.name);
            }
            return rObj;
        }

        public Enemy GetEnemy()
        {
            Enemy returnable = null;
            List<Enemy> Srambled = EnemyPool.OrderBy(a => System.Guid.NewGuid()).ToList();
            Srambled.ForEach(e =>
            {
                if (!e.gameObject.activeInHierarchy) { returnable = e; }
            });
            return returnable;
        }
        public int EnemiesActive()
        {
            return EnemyPool.FindAll(e => e.gameObject.activeInHierarchy).Count();
        }

        public Transform MakeParent(string name)
        {
            return new UnityEngine.GameObject(name).transform;
        }

        public void CleanEnemyPool()
        {
            EnemyPool.RemoveAll(e => e == null);
        }

    }
}