using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace BoxSortingGame
{
    [System.Serializable]
    public struct ColorData
    {
        public string id;
        public Color color;
    }
    
    [CreateAssetMenu(fileName = "ColorDataSO", menuName = "Scriptable Objects/new ColorDataSO")]
    public class ColorDataSO : ScriptableObject
    {
        [SerializeField] private ColorData _data;
        public ColorData Data => _data;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_data.id))
            {
                _data.id = Guid.NewGuid().ToString();
            }
        }
    }
}

