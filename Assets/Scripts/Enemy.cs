using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IObjective
{
    [SerializeField] private Animator _animator;
    [SerializeField] private List<BodyPart> _bodyParts = new ();
    [SerializeField] private BodyPart _head;
    [SerializeField] private int _health = 1;

    private bool _isDead = false;

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
            bodyPart.OnHit.AddListener(Hit);
    }

    void OnDisable()
    {
        foreach (var bodyPart in _bodyParts)
            bodyPart.OnHit.RemoveListener(Hit);
    }

    private void Hit(BodyPart bodyPart)
    {
        if (_health > 0)
            _health--;

        if (bodyPart == _head)
            _health = 0;

        if (_health == 0 && !_isDead)
            Dead();
    }

    private void Dead()
    {
        _isDead = true;
        _animator.SetTrigger("Dead");

        Complete();
        enabled = false;
    }

    public void Complete()
    {
        _objectiveTracker.RemoveObjective(this);
    }
}