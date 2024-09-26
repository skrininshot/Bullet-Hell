using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IObjective
{
    [Inject] private ObjectiveTracker _objectiveTracker;

    [SerializeField] private ObjectiveScore _objective;

    [SerializeField] private Animator _animator;
    [SerializeField] private string _startAnimation;

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

    private void Start()
    {
        _objectiveTracker.AddObjective(_objective);

        if (!string.IsNullOrEmpty(_startAnimation))
            _animator.SetTrigger(_startAnimation);
    }

    private void OnEnable()
    {
        foreach (var bodyPart in _bodyParts)
            bodyPart.OnDamage.AddListener(OnDamage);
    }

    private void OnDisable()
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

        if (_objectiveTracker != null)
        {
            int score = _bodyPartScore[_damagedBodyPart.BodyPartType];

            _objective.SetScore(_objective.Score + score);
            _objectiveTracker.ObjectiveComplete(_objective);
        } 

        enabled = false;
    }

    [Serializable]
    public class DictionaryBodyPartScore : SerializableDictionary<BodyPartType, int>
    {

    }
}