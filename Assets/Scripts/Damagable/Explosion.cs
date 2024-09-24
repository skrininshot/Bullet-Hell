using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float _explosionForce = 2000f;
    public float _explosionRadius = 5f;
    public GameObject _explosionEffectPrefab;

    private void Start()
    {
        Explode();
    }

    private void Explode()
    {
        Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider == null) return;

            if (collider.TryGetComponent<IDamagable>(out var damagable))
                damagable.Damage();

            if (collider.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                float distance = (transform.position - collider.transform.position).sqrMagnitude;
                rigidbody.AddExplosionForce(_explosionForce, transform.position, distance);
            }
        }

        Destroy(gameObject);
    }
}
