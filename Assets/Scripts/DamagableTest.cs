using UnityEngine;

public class DamagableTest : MonoBehaviour, IDamagable
{
    [SerializeField] private Explosion _explosion;

    public void Damage()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}