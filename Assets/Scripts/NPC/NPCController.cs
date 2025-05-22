using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace BoxSortingGame
{
    //TODO separate for NPCModel and NPCPresenter
    public class NPCController : MonoBehaviour, IDisposable
    {
        [SerializeField] private NPCSettingsSO _npcSettings;
        [SerializeField] private SpriteRenderer _view;
        
        [SerializeField] private Rigidbody2D _rigidbody;
        public Rigidbody2D Rigidbody => _rigidbody;

        private IStateNPC _currentState;
        private CancellationTokenSource _stateExecutionCancellationTokenSource = new CancellationTokenSource();

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

        private NPCMove _npcMove;
        public NPCMove NpcMove => _npcMove;
        
        private bool _isExecutingRequired = true;
        
        private void Start()
        {
            _stateExecutionCancellationTokenSource.Dispose();
            _stateExecutionCancellationTokenSource = new CancellationTokenSource();
            
            Initialize();
        }

        public void Initialize()
        {
            _npcMove = new NPCMove(this, _npcSettings);
            
            _idleState = new IdleStateNPC(_boxModel);
            _moveToDropZoneState = new MoveToDropZoneStateNPC(_dropZoneModel);
            _dropBoxState = new DropBoxStateNPC(_dropZoneModel);

            _dropZoneModel.OnDropZoneFull
                .Subscribe(_=>
                {
                    _isExecutingRequired = false;
                    CancelStateExecution();
                })
                .AddTo(this);
            
            ChangeState(_idleState);
        }
        
        public void ChangeState(IStateNPC newState)
        {
            if (_currentState == newState || !_isExecutingRequired)
                return;

            CancelStateExecution();
            _stateExecutionCancellationTokenSource.Dispose();
            _stateExecutionCancellationTokenSource = new CancellationTokenSource();
            
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter(this);
            
            StateExecute().Forget();
        }
        
        public void SetBoxTarget(BoxController boxController)
        {
            _boxTarget = boxController;
        }

        public void ChangeDirection(Vector2 direction)
        {
            direction = direction.normalized;

            _view.flipX = direction.x < 0;
        }
        
        private async UniTask StateExecute()
        {
            while (!_stateExecutionCancellationTokenSource.Token.IsCancellationRequested)
            {
                await UniTask.Yield(_stateExecutionCancellationTokenSource.Token);
                
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

        private void CancelStateExecution()
        {
            if(_stateExecutionCancellationTokenSource.Token.CanBeCanceled)
            {
                _stateExecutionCancellationTokenSource.Cancel();
            }
        }

        public void Dispose()
        {
            CancelStateExecution();
            
            _stateExecutionCancellationTokenSource.Dispose();
            _stateExecutionCancellationTokenSource = null;
        }
    }
}
