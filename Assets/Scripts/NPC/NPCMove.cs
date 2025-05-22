using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BoxSortingGame
{
    public class NPCMove
    {
        private NPCSettingsSO _npcSettings;
        private NPCController _npc;
        
        public NPCMove(NPCController npc, NPCSettingsSO npcSettings)
        {
            _npcSettings = npcSettings;
            _npc = npc;
        }

        public async UniTask Move(Transform target, Action OnMoveCompleted)
        {
            var position = (Vector2)target.position;
            var direction = (position - _npc.Rigidbody.position).normalized;

            _npc.ChangeDirection(direction);
            _npc.Rigidbody.linearVelocity = direction * _npcSettings.Speed;

            if (Vector2.Distance(_npc.Rigidbody.position, position) < _npcSettings.MinDistanceToTarget)
            {
                OnMoveCompleted?.Invoke();
            }
            
            await UniTask.WaitForSeconds(_npcSettings.MovementDelayInSeconds);
        }
    }
}
