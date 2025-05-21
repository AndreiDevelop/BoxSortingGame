using BoxSortingGame;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoxSortingGame
{
    public class MoveToBoxStateNPC : IStateNPC
    {
        private NPCController _npc;
        private float _speed = 0.5f;
        private float _movementDelayInSeconds = 1f;
        public void Enter(NPCController npc)
        {
            _npc = npc;
        }

        public async UniTask Execute()
        {
            if (_npc.BoxTarget == null)
            {
                _npc.ChangeState(_npc.IdleState);
                return;
            }

            var boxPosition = (Vector2)_npc.BoxTarget.transform.position;
            var direction = (boxPosition - _npc.Rigidbody.position).normalized;

            _npc.Rigidbody.linearVelocity = direction * _speed;
            
            if (Vector2.Distance(_npc.Rigidbody.position, boxPosition) < 0.1f)
            {
                _npc.ChangeState(_npc.CollectBoxState);
            }

            await UniTask.WaitForSeconds(_movementDelayInSeconds);
        }

        public void Exit()
        {

        }
    }
}

