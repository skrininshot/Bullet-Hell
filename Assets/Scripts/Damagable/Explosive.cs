using UnityEngine;
using Zenject;

public class Explosive : MonoBehaviour, IDamagable, IObjective
{
    [Inject] private ObjectiveTracker _objectiveTracker;

    [SerializeField] private ObjectiveScore _objective;

    [SerializeField] private float _expolsionForce;
    [SerializeField] private float _expolsionRadius;

    [SerializeField] private Explosion _explosion;

    private void Awake()
    {
        _objectiveTracker.AddObjective(_objective);
    }

    public void Damage()
    {
        Explode();
    }

    private void Explode()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);

        _objectiveTracker.ObjectiveComplete(_objective);

        Destroy(gameObject);
    }
}