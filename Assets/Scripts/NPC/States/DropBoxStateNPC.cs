using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoxSortingGame
{
    public class DropBoxStateNPC : IStateNPC
    {
        private DropZoneModel _dropZoneModel;
        public DropBoxStateNPC(DropZoneModel dropZoneModel)
        {
            _dropZoneModel = dropZoneModel;
        }
        
        public void Enter(NPCController npc)
        {
            npc.BoxTarget.DeattachBox();
            _dropZoneModel.DropBoxOnSelectedDropZone();
            
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
