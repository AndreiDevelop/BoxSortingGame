using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoxSortingGame
{
    public class PoolManager : MonoBehaviour
    {
        //prefabs to instantiate if object in pool is finished
        public Dictionary<Type, GameObject> Prefabs = new Dictionary<Type, GameObject>();
        
        public Dictionary<Type, Queue<GameObject>> PoolableObjects = new Dictionary<Type, Queue<GameObject>>();

        private int _defaultPoolExtendSize = 10;
        
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        public async UniTask AddToPool<T>(GameObject prefab, int count) where T : Component
        {
            var key = typeof(T);
            
            //add prefab to dictionary
            if(!Prefabs.ContainsKey(key))
            {
                Prefabs.Add(key, prefab);
            }

            await InstantiatePoolObject<T>(count);
        }

        public async UniTask<GameObject> GetFromPool<T>() where T : Component
        {
            var key = typeof(T);
            
            if (Prefabs.ContainsKey(key))
            {
                if(PoolableObjects.ContainsKey(key) || PoolableObjects[key].Count == 0)
                {
                    //extend pool size
                    await InstantiatePoolObject<T>(_defaultPoolExtendSize);
                }
                
                GameObject obj = PoolableObjects[key].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                Debug.LogError($"No objects found by key {key} in pool. Pool could not be extended!");
                return null;
            }
        }

        private async UniTask InstantiatePoolObject<T>(int count)
        {
            var key = typeof(T);
            
            if (!Prefabs.ContainsKey(key))
            {
                Debug.LogError($"No objects found by key {key} in pool. Pool could not be extended!");
                return;
            }
            
            var prefab = Prefabs[key];

            if (!PoolableObjects.ContainsKey(key))
            {
                PoolableObjects.Add(key, new Queue<GameObject>());
            }
            
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                PoolableObjects[key].Enqueue(obj);

                await UniTask.DelayFrame(1, cancellationToken: _cancellationTokenSource.Token);
            }
        }
        
        private async UniTaskVoid ClearPool<T>() where T : Component
        {
            var key = typeof(T);
            
            if(_cancellationTokenSource!=null && _cancellationTokenSource.Token.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
            }
            
            if (PoolableObjects.ContainsKey(key))
            {
                foreach (var obj in PoolableObjects[key])
                {
                    Destroy(obj);
                }

                PoolableObjects[key].Clear();
            }
            else
            {
                Debug.LogError($"No objects found by key {key} in pool. Pool could not be cleared!");
            }
        }
    }
}
