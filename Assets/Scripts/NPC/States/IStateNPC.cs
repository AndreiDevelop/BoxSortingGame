using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoxSortingGame
{
    public interface IStateNPC
    {
        void Enter(NPCController npc);
        UniTask Execute();
        void Exit();
    }
}
