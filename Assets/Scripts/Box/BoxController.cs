using UnityEngine;

namespace BoxSortingGame
{
    public class BoxController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        private BoxManager _boxManager;
        
        public void Initialize(BoxManager boxManager, Color color)
        {
            _boxManager = boxManager;
            _meshRenderer.material.color = color;
            
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.useGravity = true;
        }
    }
}
