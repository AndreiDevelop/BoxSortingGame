using UnityEngine;
using UnityEngine.Serialization;

namespace BoxSortingGame
{
    //TODO add animator?
    public class BoxController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private BoxModel _boxModel;

        private ColorData _colorData;
        public ColorData ColorData => _colorData;
        
        public void Initialize(BoxModel boxModel, ColorData colorData)
        {
            _boxModel = boxModel;
            _colorData = colorData;
            _spriteRenderer.material.color = colorData.color;
        }

        public void AttachBox(Transform target)
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.simulated = false;
            _rigidbody.transform.position = target.position;
            _rigidbody.transform.SetParent(target);
        }
        
        public void DeattachBox()
        {
            _rigidbody.simulated = true;
            _rigidbody.transform.SetParent(null);
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
