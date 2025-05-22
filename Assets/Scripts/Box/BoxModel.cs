using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Zenject;

namespace BoxSortingGame
{
    public class BoxModel : IDisposable
    {
        public ReactiveCommand<BoxController> OnBoxSpawned = new ReactiveCommand<BoxController>();
        public ReactiveCollection<BoxController> Boxes = new ReactiveCollection<BoxController>();
        
        private PoolManager _poolManager;
        private DropZoneModel _dropZoneModel;
        private BoxSettingsSO _boxSettings;
        
        public int MaxBoxCount => _boxSettings.MaxBoxCount;
        
        private CancellationTokenSource _spawnCancellationTokenSource = new CancellationTokenSource();
        
        public BoxModel(BoxSettingsSO boxSettings, 
            PoolManager poolManager,
            DropZoneModel dropZoneModel)
        {
            _poolManager = poolManager;
            _boxSettings = boxSettings;
            _dropZoneModel = dropZoneModel;

            CancelSpawn();
            _spawnCancellationTokenSource.Dispose();
            _spawnCancellationTokenSource = new CancellationTokenSource();

            _dropZoneModel.OnDropZoneFull.Subscribe(_ =>
            {
                CancelSpawn();
            }).AddTo(_spawnCancellationTokenSource.Token);
            
            _poolManager.AddToPool<BoxController>(_boxSettings.BoxPrefab, _boxSettings.MaxBoxCount);

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

            Boxes.Add(boxController);
            OnBoxSpawned?.Execute(boxController);
        }

        public async UniTask SpawnBoxProcess()
        {
            while (!_spawnCancellationTokenSource.Token.IsCancellationRequested)
            {
                await UniTask.WaitForSeconds(_boxSettings.BoxSpawnDelayInSeconds, 
                    cancellationToken: _spawnCancellationTokenSource.Token);
                
                if (Boxes.Count < _boxSettings.MaxBoxCount)
                {
                    await SpawnBox();
                }
            }
        }

        public async UniTask<BoxController> GetBoxByDistance(Vector2 position)
        {
            BoxController selectedBox = null;
            float maxDistance = float.MaxValue;

            for(int i = 0; i < Boxes.Count; i++)
            {
                if (Boxes[i] == null)
                {
                    continue;
                }
                
                var box = Boxes[i];
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
                Boxes.Remove(selectedBox);
            }

            return selectedBox;
        }

        private void CancelSpawn()
        {
            if (_spawnCancellationTokenSource.Token.CanBeCanceled)
            {
                _spawnCancellationTokenSource.Cancel();
            }
        }

        public void Dispose()
        {
            if (_spawnCancellationTokenSource != null)
            {
                CancelSpawn();
                _spawnCancellationTokenSource.Dispose();
                _spawnCancellationTokenSource = null;
            }

            if (Boxes != null)
            {
                Boxes.Clear();
            }

            if (OnBoxSpawned != null)
            {
                OnBoxSpawned.Dispose();
            }
        }
    }
}
