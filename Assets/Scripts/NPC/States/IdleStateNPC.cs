using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BoxSortingGame
{
    public class IdleStateNPC : IStateNPC
    {
        [Inject] private BoxModel _boxModel;
        
        private NPCController _npc;
        public void Enter(NPCController npc)
        {
            _npc = npc;
        }

        public async UniTask Execute()
        {
            var box = await _boxModel.GetBoxByDistance(_npc.Rigidbody.position);
            
            if (box != null)
            {
                _npc.SetBoxTarget(box);
                _npc.ChangeState(_npc.MoveToBoxState);
            }
        }

        public void Exit()
        {
            
        }
    }
}
