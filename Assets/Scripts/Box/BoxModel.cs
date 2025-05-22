using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Zenject;

namespace BoxSortingGame
{
    public class BoxModel
    {
        public ReactiveCommand<BoxController> OnBoxSpawned = new ReactiveCommand<BoxController>();
        
        private PoolManager _poolManager;
        private BoxSettingsSO _boxSettings;
        
        private List<BoxController> _boxes = new List<BoxController>();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        public BoxModel(BoxSettingsSO boxSettings, PoolManager poolManager)
        {
            _poolManager = poolManager;
            _boxSettings = boxSettings;

            _poolManager.AddToPool<BoxController>(_boxSettings.BoxPrefab, _boxSettings.MaxBoxCount);

            if (_cancellationTokenSource.Token.CanBeCanceled)
            {
                _cancellationTokenSource.Cancel();
            }
            
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            
            SpawnBoxProcess().
                Forget();
        }

        public async UniTask SpawnBox()
        {
            float boxChance = UnityEngine.Random.Range(0f, 1f);
            var boxColor = _boxSettings.GetBoxColorData(boxChance);
            
            var boxObject = await _poolManager.
                GetFromPool<BoxController>();
            
            //TODO refactoring
            var boxController = boxObject.GetComponent<BoxController>();
            
            boxController.Initialize(this, boxColor);

            _boxes.Add(boxController);
            OnBoxSpawned?.Execute(boxController);
        }

        public async UniTask SpawnBoxProcess()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(_boxSettings.BoxSpawnDelayInSeconds, 
                    cancellationToken: _cancellationTokenSource.Token);
                
                if (_boxes.Count < _boxSettings.MaxBoxCount)
                {
                    await SpawnBox();
                }
            }
        }

        public async UniTask<BoxController> GetBoxByDistance(Vector2 position)
        {
            BoxController selectedBox = null;
            float maxDistance = float.MaxValue;

            for(int i = 0; i < _boxes.Count; i++)
            {
                if (_boxes[i] == null)
                {
                    continue;
                }
                
                var box = _boxes[i];
                float distance = Vector2.Distance(position, box.transform.position);
                if (distance < maxDistance)
                {
                    maxDistance = distance;
                    selectedBox = box;
                }

                await UniTask.DelayFrame(1);
            }

            if (selectedBox != null)
            {
                _boxes.Remove(selectedBox);
            }

            return selectedBox;
        }
    }
}
