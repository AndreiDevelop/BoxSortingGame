using UnityEngine;

namespace BoxSortingGame
{
    public class BoxController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        private BoxModel _boxModel;
        
        public void Initialize(BoxModel boxModel, Color color)
        {
            _boxModel = boxModel;
            _meshRenderer.material.color = color;
        }

        public void Activate(Vector3 velocity = default)
        {
            _rigidbody.useGravity = true;
            _rigidbody.linearVelocity = velocity;
        }

        public void Deactivate()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.useGravity = false;
        }
    }
}
