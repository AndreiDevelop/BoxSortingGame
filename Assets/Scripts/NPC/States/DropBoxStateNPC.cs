using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoxSortingGame
{
    public class DropBoxStateNPC : IStateNPC
    {
        public void Enter(NPCController npc)
        {
            npc.BoxTarget.DeattachBox();
            npc.ChangeState(npc.IdleState);
        }

        public async UniTask Execute()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
