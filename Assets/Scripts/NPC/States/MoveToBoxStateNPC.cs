using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

namespace BoxSortingGame
{
    public class MoveToBoxStateNPC : IStateNPC
    {
        private NPCController _npc;

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

            await _npc.NpcMove.Move(_npc.BoxTarget.transform, () =>
            {
                _npc.ChangeState(_npc.CollectBoxState);
            });
        }

        public void Exit()
        {
            if(_npc== null)
                return;
            
            _npc.Rigidbody.linearVelocity = Vector2.zero;
        }
    }
}

