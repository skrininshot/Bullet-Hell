using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public UnityEvent<bool> OnDestroy = new();
    public Transform CameraPoint => _cameraPoint;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _cameraPoint;

    private GameSettings.BulletSettings _settings;
    private BulletHitHandler _bulletHitHandler;

    private float _speed;

    [Inject]
    private void Construct(GameSettings gameSettings, BulletHitHandler bulletHitHandler)
    {
        _settings = gameSettings.Bullet;
        _bulletHitHandler = bulletHitHandler;
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyTimer(_settings.DestroyTime)); 
    }

    private void Start()
    {
        _speed = _settings.Speed;
    }

    private void Update()
    {
        _rigidbody.velocity = _speed * transform.forward;
    }

    private void OnCollisionEnter(Collision collision) => HandleCollision(collision);

    private void HandleCollision (Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamagable damagable))
        {
            _bulletHitHandler.Hit(damagable);
        }
        else
        {
            SelfDestroy(true);
        }
    }

    private IEnumerator DestroyTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SelfDestroy(false);
    }

    private void SelfDestroy(bool collision)
    {
        Destroy(gameObject);
        OnDestroy?.Invoke(collision);
    }

    public class Factory : PlaceholderFactory<Bullet> { }
}
