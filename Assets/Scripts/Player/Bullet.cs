using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Bullet : MonoBehaviour
{
    public Transform Transform => _transform;

    [HideInInspector] public UnityEvent<GameObject> OnHit = new();
    public Transform CameraPoint => _cameraPoint;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraPoint;

    private GameSettings.BulletSettings _settings;

    private Transform _transform;

    private float _speed;

    [Inject]
    private void Construct(GameSettings gameSettings)
    {
        _settings = gameSettings.Bullet;
    }

    private void Awake()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _speed = _settings.Speed;
    }

    private void Update()
    {
        _rigidbody.velocity = _speed * _transform.forward;
    }

    private void OnTriggerEnter(Collider other) => OnHit?.Invoke(other.gameObject);
}
