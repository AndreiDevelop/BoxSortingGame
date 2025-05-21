using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BoxSortingGame
{
    public class MoveToDropZoneStateNPC : IStateNPC
    {
        private NPCController _npc;
        private Transform _dropZoneTarget;
        
        private DropZoneModel _dropZoneModel;
        
        //TODO move to config
        private float _speed = 1f;
        private float _movementDelayInSeconds = 0.25f;
        private float _minDistanceToTarget = 1f;
        
        public MoveToDropZoneStateNPC(DropZoneModel dropZoneModel)
        {
            _dropZoneModel = dropZoneModel;
        }
        
        public void Enter(NPCController npc)
        {
            _npc = npc;
            
            _dropZoneModel.OnDropZoneFinded.Subscribe(dropZoneTransform=>
            {
                _dropZoneTarget = dropZoneTransform;
            }).AddTo(_npc);
            
            _dropZoneModel.SearchForDropZone(_npc.BoxTarget.ColorData);
        }

        public async UniTask Execute()
        {
            if (_dropZoneTarget == null)
            {
                return;
            }

            //TODO move to NPCMovement?
            var position = (Vector2)_dropZoneTarget.position;
            var direction = (position - _npc.Rigidbody.position).normalized;

            _npc.Rigidbody.linearVelocity = direction * _speed;

            if (Vector2.Distance(_npc.Rigidbody.position, position) < _minDistanceToTarget)
            {
                _npc.ChangeState(_npc.DropBoxState);
            }

            await UniTask.WaitForSeconds(_movementDelayInSeconds);
        }

        public void Exit()
        {
            _dropZoneTarget = null;
            
            if(_npc== null)
                return;
            
            _npc.Rigidbody.linearVelocity = Vector2.zero;
        }
    }
}
