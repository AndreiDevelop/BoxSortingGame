using System.Collections.Generic;
using UnityEngine;

namespace BoxSortingGame
{
    [System.Serializable]
    public struct BoxColor
    {
        public Color color;
        public float colorChance;
    }
    
    [CreateAssetMenu(fileName = "BoxSettingsSO", menuName = "Scriptable Objects/new BoxSettingsSO")]
    public class BoxSettingsSO : ScriptableObject
    {
        [SerializeField] private GameObject _boxPrefab;
        [SerializeField] private int _maxBoxCount;
        [SerializeField] private float _boxSpawnDelayInSeconds;
        [SerializeField] private List<BoxColor> _boxColors;
        
        public GameObject BoxPrefab => _boxPrefab;
        public int MaxBoxCount => _maxBoxCount;
        public float BoxSpawnDelayInSeconds => _boxSpawnDelayInSeconds;

        public Color GetBoxColor(float chance)
        {
            float chanceSum = 0f;
            Color color = Color.white;
            
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
                    color = boxColor.color;
                    break;
                }
            }

            return color;
        }
    }
}
