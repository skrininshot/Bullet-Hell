using UnityEngine;
using Zenject;

public class Explosive : MonoBehaviour, IDamagable, IObjective, IScorable
{
    [SerializeField] private float _expolsionForce;
    [SerializeField] private float _expolsionRadius;

    [SerializeField] private Explosion _explosion;

    [SerializeField] private ScoreType _scoreType;

    private ObjectiveTracker _objectiveTracker;

    [Inject]
    public void Construct(ObjectiveTracker objectiveTracker)
    {
        _objectiveTracker = objectiveTracker;
        _objectiveTracker.AddObjective(this);
    }

    public void Complete()
    {
        _objectiveTracker.CompleteObjective(this);
    }

    public void Damage()
    {
        Explode();
    }

    private void Explode()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public ScoreType GetScoreType() => _scoreType;
}