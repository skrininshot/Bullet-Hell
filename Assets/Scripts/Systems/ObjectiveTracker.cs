using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTracker
{
    public Action OnObjectivesComplete;

    private readonly List<ObjectiveScore> _completedObjectives = new();
    private readonly List<ObjectiveScore> _totalObjectives = new();
    private readonly List<ObjectiveScore> _objectives = new();

    private readonly FloatingTextSpawner _floatingTextSpawner;

    public ObjectiveTracker(
        FloatingTextSpawner floatTextSpawner)
    {
        _floatingTextSpawner = floatTextSpawner;
    }
        
    public void ObjectiveComplete(ObjectiveScore objective)
    {
        if (!_objectives.Contains(objective) || _completedObjectives.Contains(objective)) return;

        _objectives.Remove(objective);
        _completedObjectives.Add(objective);

        OnObjectiveRemove();
    }

    public void AddObjective(ObjectiveScore objective)
    {
        if (_objectives.Contains(objective)) return;

        _objectives.Add(objective);
        _totalObjectives.Add(objective);
    }

    private void OnObjectiveRemove()
    {
        if (!HasEnemies())
            OnObjectivesComplete?.Invoke();
    }

    private bool HasEnemies()
    {
        foreach (var objective in _objectives)
            if (objective.Type == ObjectiveType.Enemy)
                return true;

        return false;
    }

    public int CountTotalScore()
    {
        int score = 0;

        foreach (var objective in _completedObjectives)
            score += objective.Score;

        return score;
    }
}

public interface IObjective { }

public enum ObjectiveType
{
    Default,
    Prop,
    Explosive,
    Enemy
}

[Serializable]
public class ObjectiveScore
{
    public ObjectiveType Type => _type;
    public int Score => _score;

    [SerializeField] private ObjectiveType _type;
    [SerializeField] private int _score;

    public void SetScore(int value) => _score = value;
}