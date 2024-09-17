using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public UnityEvent<GameObject> OnHit = new();
    public Transform CameraPoint => _cameraPoint;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraPoint;

    private GameSettings.BulletSettings _settings;

    private float _speed;

    [Inject]
    private void Construct(GameSettings gameSettings)
    {
        _settings = gameSettings.Bullet;
    }

    private void Start()
    {
        _speed = _settings.Speed;
    }

    private void Update()
    {
        _rigidbody.velocity = _speed * transform.forward;
    }

    private void OnTriggerEnter(Collider other) => OnHit?.Invoke(other.gameObject);

    public class Factory : PlaceholderFactory<Bullet> { }
}
