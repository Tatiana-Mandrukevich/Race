using UnityEngine;
using DG.Tweening;

public class TrafficCone : MonoBehaviour, IPooledObject
{
    public float pushDistance = 4f;
    public float jumpPower = 1.5f;
    public float duration = 0.6f;
    public bool usePhysicsAfter = true; // после анимации включить Rigidbody
    public float enablePhysicsDelay = 0.05f;
    public float despawnDelay = 2f; // время после отлета перед исчезновением
    public ParticleSystem hitParticles;
    public AudioClip hitSfx;

    private bool _isHit = false;
    private Collider _collider;
    private Rigidbody _rb;
    private AudioSource _audioSource;
    private IPool<IPooledObject> _pool;
    private Transform _initialParent;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Vector3 _initialScale;
    private ChunkManager _chunkManager;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        
        _rb.isKinematic = true;

        _audioSource = GetComponent<AudioSource>();
        _chunkManager = FindObjectOfType<ChunkManager>();
        
        _initialParent = transform.parent;
        _initialPosition = transform.localPosition;
        _initialRotation = transform.localRotation;
        _initialScale = transform.localScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject, collision.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleCollision(other.gameObject, other.transform);
    }

    private void HandleCollision(GameObject other, Transform otherTransform)
    {
        if (_isHit) return;
        if (other == null) return;
        
        if (!other.CompareTag("MyCar")) return;

        _isHit = true;
        
        if (transform.parent != null)
        {
            transform.SetParent(transform.parent.parent, true);
        }

        if (hitParticles != null) hitParticles.Play();
        if (hitSfx != null && _audioSource != null) _audioSource.PlayOneShot(hitSfx);

        // Направление отбрасывания — назад относительно машины
        Vector3 knockDir = -otherTransform.forward;
        knockDir.y = 0f; 
        if (knockDir.sqrMagnitude < 0.001f) knockDir = -otherTransform.TransformDirection(Vector3.forward);
        knockDir.Normalize();

        Vector3 targetPos = transform.position + knockDir * pushDistance;

        // Отключение коллайдера, чтобы не получать повторные коллизии во время анимации
        if (_collider != null) _collider.enabled = false;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOJump(targetPos, jumpPower, 1, duration).SetEase(Ease.OutQuad));
        seq.Join(transform.DORotate(new Vector3(Random.Range(-20f, 20f), Random.Range(-90f, 90f), Random.Range(-20f, 20f)), duration, RotateMode.FastBeyond360).SetEase(Ease.OutQuad));

        CameraShakeDOTween.Instance?.Shake(0.18f, 0.25f);

        seq.OnComplete(() =>
        {
            if (usePhysicsAfter && _rb != null)
            {
                Invoke(nameof(EnablePhysics), enablePhysicsDelay);
            }
            else
            {
                if (_collider != null) _collider.enabled = true;
            }
        });
    }

    private void EnablePhysics()
    {
        _rb.isKinematic = false;
        // Небольшой импульс, чтобы не застрял прямо на месте
        _rb.AddForce(Vector3.down * 0.1f, ForceMode.Impulse);
        if (_collider != null) _collider.enabled = true;
        
        // Исчезновение конуса после отлета
        Invoke(nameof(DespawnCone), despawnDelay);
    }

    private void DespawnCone()
    {
        ReturnToPool();
    }
    
    public void ResetCone()
    {
        gameObject.SetActive(true);
        _isHit = false;
        
        if (transform.parent != _initialParent)
        {
            transform.SetParent(_initialParent, false);
        }

        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        if (_collider != null) _collider.enabled = true;

        transform.DOKill();
        CancelInvoke();
        transform.localPosition = _initialPosition;
        transform.localRotation = _initialRotation;
        transform.localScale = _initialScale;
    }
    
    public void ReturnToPool()
    {
        if (transform.parent != _initialParent)
        {
            transform.SetParent(_initialParent, false);
        }

        if (_pool != null)
        {
            gameObject.SetActive(false);
            _pool.Push(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        gameObject.SetActive(true);
        ResetCone();
    }

    public void SetPool<T>(IPool<T> poolParent) where T : IPooledObject
    {
        _pool = poolParent as IPool<IPooledObject>;
    }

    private void Update()
    {
        if (_isHit && transform.parent != _initialParent && _chunkManager != null)
        {
            float speed = _chunkManager.CurrentSpeed;
            transform.position += Vector3.back * (speed * Time.deltaTime);
        }
    }
}
