using UnityEngine;

public class Explosive : MonoBehaviour, IDamagable
{
    [SerializeField] private float _expolsionForce;
    [SerializeField] private float _expolsionRadius;

    [SerializeField] private Explosion _explosion;

    [SerializeField] private ScoreType _scoreType;

    public void Damage()
    {
        Explode();
    }

    public ScoreType GetScoreType() => _scoreType;

    private void Explode()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}