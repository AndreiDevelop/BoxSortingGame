using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace BoxSortingGame
{
    [System.Serializable]
    public struct BoxColor
    {
        public ColorDataSO colorDataSO;
        public float colorChance;
    }
    
    [CreateAssetMenu(fileName = "BoxSettingsSO", menuName = "Scriptable Objects/new BoxSettingsSO")]
    public class BoxSettingsSO : ScriptableObject
    {
        [SerializeField] private GameObject _boxPrefab;
        [SerializeField] private int _maxBoxCount;
        [SerializeField] private Vector2 _boxSpawnRangeDelayInSeconds;
        [SerializeField] private List<BoxColor> _boxColors;
        
        public GameObject BoxPrefab => _boxPrefab;
        public int MaxBoxCount => _maxBoxCount;
        public float BoxSpawnDelayInSeconds => 
            Random.Range(_boxSpawnRangeDelayInSeconds.x, _boxSpawnRangeDelayInSeconds.y);

        public ColorData GetBoxColorData(float chance)
        {
            float chanceSum = 0f;
            ColorData color = default;
            
            if(_boxColors.Count == 0)
            {
                Debug.LogError("Box Colors list is empty.");
                return color;
            }
            
            foreach (var boxColor in _boxColors)
            {
                chanceSum += boxColor.colorChance;

                if (chance <= chanceSum)
                {
                    color = boxColor.colorDataSO.Data;
                    break;
                }
            }

            return color;
        }
    }
}
