using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BoxSortingGame
{
    public class MoveToDropZoneStateNPC : IStateNPC
    {
        private NPCController _npc;
        private Transform _dropZoneTarget;
        
        private DropZoneModel _dropZoneModel;

        public MoveToDropZoneStateNPC(DropZoneModel dropZoneModel)
        {
            _dropZoneModel = dropZoneModel;
        }
        
        public void Enter(NPCController npc)
        {
            _npc = npc;
            
            _dropZoneModel.OnDropZoneFinded.Subscribe(dropZoneTransform=>
            {
                _dropZoneTarget = dropZoneTransform;
            }).AddTo(_npc);
            
            _dropZoneModel.SearchForDropZone(_npc.BoxTarget.ColorData);
        }

        public async UniTask Execute()
        {
            if (_dropZoneTarget == null)
            {
                return;
            }

            await _npc.NpcMove.Move(_dropZoneTarget, ()=>
            {
                _npc.ChangeState(_npc.DropBoxState);
            });
        }

        public void Exit()
        {
            _dropZoneTarget = null;
            
            if(_npc== null)
                return;
            
            _npc.Rigidbody.linearVelocity = Vector2.zero;
        }
    }
}
