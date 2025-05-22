using BoxSortingGame;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoxSortingGame
{
    public class MoveToBoxStateNPC : IStateNPC
    {
        private NPCController _npc;
        
        //TODO move to config
        private float _speed = 1f;
        private float _movementDelayInSeconds = 0.25f;
        private float _minDistanceToTarget = 1f;
        public void Enter(NPCController npc)
        {
            _npc = npc;
        }

        public async UniTask Execute()
        {
            if (_npc.BoxTarget == null)
            {
                return;
            }

            //TODO move to NPCMovement?
            var boxPosition = (Vector2)_npc.BoxTarget.transform.position;
            var direction = (boxPosition - _npc.Rigidbody.position).normalized;

            _npc.ChangeDirection(direction);
            _npc.Rigidbody.linearVelocity = direction * _speed;

            if (Vector2.Distance(_npc.Rigidbody.position, boxPosition) < _minDistanceToTarget)
            {
                _npc.ChangeState(_npc.CollectBoxState);
            }

            await UniTask.WaitForSeconds(_movementDelayInSeconds);
        }

        public void Exit()
        {
            if(_npc== null)
                return;
            
            _npc.Rigidbody.linearVelocity = Vector2.zero;
        }
    }
}

