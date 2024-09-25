using UnityEngine;

public class Explosive : LevelObjective, IDamagable
{
    [SerializeField] private float _expolsionForce;
    [SerializeField] private float _expolsionRadius;

    [SerializeField] private Explosion _explosion;

    public void Damage()
    {
        Explode();
        Complete();
    }

    private void Explode()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}