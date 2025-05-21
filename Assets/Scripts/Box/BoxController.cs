using UnityEngine;
using UnityEngine.Serialization;

namespace BoxSortingGame
{
    public class BoxController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private BoxModel _boxModel;
        
        public void Initialize(BoxModel boxModel, Color color)
        {
            _boxModel = boxModel;
            _spriteRenderer.material.color = color;
        }

        public void Activate(float boxActivationSpeed)
        {
            float rotationVelocity = Random.Range(-360f, 360f);
            Vector2 linearVelocity = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)).normalized * boxActivationSpeed;
            
            _rigidbody.simulated = true;
            _rigidbody.linearVelocity = linearVelocity;
            _rigidbody.angularVelocity = rotationVelocity;
        }

        public void Deactivate()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.simulated = false;
        }
    }
}
