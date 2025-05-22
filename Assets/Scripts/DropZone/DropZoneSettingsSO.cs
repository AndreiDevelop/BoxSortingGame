using UnityEngine;

namespace BoxSortingGame
{
    [CreateAssetMenu(fileName = "DropZoneSettingsSO", menuName = "Scriptable Objects/new DropZoneSettingsSO")]
    public class DropZoneSettingsSO : ScriptableObject
    {
        [SerializeField] private int _maxBoxCount;
        public int MaxBoxCount => _maxBoxCount;
    }
}
