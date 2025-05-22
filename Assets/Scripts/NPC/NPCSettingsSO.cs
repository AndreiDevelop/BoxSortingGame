using UnityEngine;

namespace BoxSortingGame
{
    [CreateAssetMenu(fileName = "NPCSettingsSO", menuName = "Scriptable Objects/new NPCSettingsSO")]
    public class NPCSettingsSO : ScriptableObject
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _movementDelayInSeconds = 0.25f;
        [SerializeField] private float _minDistanceToTarget = 1f;
        
        public float Speed => _speed;
        public float MovementDelayInSeconds => _movementDelayInSeconds;
        public float MinDistanceToTarget => _minDistanceToTarget;
    }
}
