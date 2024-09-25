using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LevelObjective
{
    [SerializeField] private Animator _animator;
    [SerializeField] private List<BodyPart> _bodyParts = new();
    [SerializeField] private DictionaryBodyPartScore _bodyPartScore = new () 
    { 
        new KeyValuePair<BodyPartType, int>(BodyPartType.Head, 100),
        new KeyValuePair<BodyPartType, int>(BodyPartType.Torso, 50),
        new KeyValuePair<BodyPartType, int>(BodyPartType.Arm, 25),
        new KeyValuePair<BodyPartType, int>(BodyPartType.Leg, 25)
    };

    private BodyPart _damagedBodyPart;

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
        Complete();

        _animator.SetTrigger("Dead");
        enabled = false;
    }

    protected override void Complete()
    {
        _score += _bodyPartScore[_damagedBodyPart.BodyPartType];

        base.Complete();
    }

    [Serializable]
    public class DictionaryBodyPartScore : SerializableDictionary<BodyPartType, int>
    {

    }
}