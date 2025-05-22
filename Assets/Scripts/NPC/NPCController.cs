using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BoxSortingGame
{
    public class NPCController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        private IStateNPC _currentState;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        public Rigidbody2D Rigidbody => _rigidbody;

        #region States

        private IStateNPC _idleState;
        public IStateNPC IdleState => _idleState;
        
        private IStateNPC _moveToBoxState = new MoveToBoxStateNPC();
        public IStateNPC MoveToBoxState => _moveToBoxState;

        private IStateNPC _collectBoxState = new CollectBoxStateNPC();
        public IStateNPC CollectBoxState => _collectBoxState;
        
        private IStateNPC _moveToDropZoneState;
        public IStateNPC MoveToDropZoneState => _moveToDropZoneState;

        private IStateNPC _dropBoxState;
        public IStateNPC DropBoxState => _dropBoxState;

        #endregion

        [Inject] private BoxModel _boxModel;
        [Inject] private DropZoneModel _dropZoneModel;
        
        private BoxController _boxTarget;
        public BoxController BoxTarget => _boxTarget;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _idleState = new IdleStateNPC(_boxModel);
            _moveToDropZoneState = new MoveToDropZoneStateNPC(_dropZoneModel);
            _dropBoxState = new DropBoxStateNPC(_dropZoneModel);
            
            ChangeState(_idleState);
        }
        
        public void ChangeState(IStateNPC newState)
        {
            if (_currentState == newState)
                return;

            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter(this);
            
            StateExecute().Forget();
        }
        
        public void SetBoxTarget(BoxController boxController)
        {
            _boxTarget = boxController;
        }

        private async UniTask StateExecute()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                await UniTask.Yield(_cancellationTokenSource.Token);
                
                if (_currentState != null)
                {
                    await _currentState.Execute();
                }
                else
                {
                    await UniTask.CompletedTask;
                }
            }
        }
    }
}
