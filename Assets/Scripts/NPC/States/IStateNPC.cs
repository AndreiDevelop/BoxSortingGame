using UnityEngine;

namespace BoxSortingGame
{
    public interface IStateNPC
    {
        void Enter(NPCPresenter npc);
        void Execute();
        void Exit();
    }
}
