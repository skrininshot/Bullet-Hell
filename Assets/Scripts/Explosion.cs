using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float _explosionRadius = 5f;
    public float _explosionForce = 5f;
    public GameObject _explosionEffectPrefab;

    private void Start()
    {
        Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamagable>(out var damagable))
                damagable.Damage();
            
            if (collider.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                rigidbody.AddExplosionForce(_explosionForce, transform.position, distance);
            }
        }
    }
}
