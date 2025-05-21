using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoxSortingGame
{
    public class CollectBoxStateNPC : IStateNPC
    {
        private NPCController _npc;
        public void Enter(NPCController npc)
        {
            _npc = npc;
        }

        public async UniTask Execute()
        {
            /*
            if (_npc.BoxTarget == null)
            {
                _npc.ChangeState(_npc.IdleState);
                return;
            }
            
            await _npc.MoveToBox();
            
            _npc.CollectBox();
            _npc.ChangeState(_npc.IdleState);
            */
        }

        public void Exit()
        {
            
        }

    }
}
