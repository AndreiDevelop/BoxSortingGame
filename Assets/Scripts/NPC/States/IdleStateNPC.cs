using UnityEngine;

namespace BoxSortingGame
{
    public class IdleStateNPC : IStateNPC
    {
        private NPCPresenter _npc;
        public void Enter(NPCPresenter npc)
        {
            _npc = npc;
        }

        public void Execute()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}
