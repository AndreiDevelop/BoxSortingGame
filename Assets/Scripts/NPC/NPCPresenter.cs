using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoxSortingGame
{
    public class NPCPresenter : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        
        public Animator Animator => _animator;
        public Rigidbody2D Rigidbody => _rigidbody;
        
        private IStateNPC _currentState;
        
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
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
        
        private async UniTask StateExecute()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                await UniTask.Yield(_cancellationTokenSource.Token);
                _currentState?.Execute();
            }
        }
    }
}
