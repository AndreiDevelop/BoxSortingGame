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

        private IStateNPC _idleState = new IdleStateNPC();
        public IStateNPC IdleState => _idleState;
        
        private IStateNPC _moveToBoxState = new MoveToBoxStateNPC();
        public IStateNPC MoveToBoxState => _moveToBoxState;

        private IStateNPC _collectBoxState = new CollectBoxStateNPC();
        public IStateNPC CollectBoxState => _collectBoxState;
        
        private MoveToDropZoneStateNPC _moveToDropZoneState = new MoveToDropZoneStateNPC();
        private DropBoxStateNPC _dropBoxState = new DropBoxStateNPC();

        #endregion

        private BoxController _boxTarget;
        public BoxController BoxTarget => _boxTarget;

        public void Initialize(IStateNPC initialState)
        {
            ChangeState(initialState);
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
            _boxTarget.transform.SetParent(transform);
            _boxTarget.transform.position = transform.position;
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
