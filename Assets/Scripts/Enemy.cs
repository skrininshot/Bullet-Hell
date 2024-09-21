using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IObjective, IScorable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private List<BodyPart> _bodyParts = new ();

    private BodyPart _damagedBodyPart;

    private ObjectiveTracker _objectiveTracker;

    [Inject]
    public void Construct(ObjectiveTracker objectiveTracker)
    {
        _objectiveTracker = objectiveTracker;
        _objectiveTracker.AddObjective(this);
    }

    private void OnValidate()
    {
        _bodyParts = FindComponentRecursive<BodyPart>.FindBodyParts(transform);
    }

    void OnEnable()
    {
        foreach (var bodyPart in _bodyParts)
            bodyPart.OnDamage.AddListener(OnDamage);
    }

    void OnDisable()
    {
        foreach (var bodyPart in _bodyParts)
            bodyPart.OnDamage.RemoveListener(OnDamage);
    }

    private void OnDamage(BodyPart bodyPart)
    {
        _damagedBodyPart = bodyPart;

        Dead();
    }

    private void Dead()
    {
        _animator.SetTrigger("Dead");

        Complete();
        enabled = false;
    }

    public void Complete()
    {
        _objectiveTracker.CompleteObjective(this);
    }

    public ScoreType GetScoreType() => 
        _damagedBodyPart == null ? ScoreType.Default : _damagedBodyPart.ScoreType;
}
