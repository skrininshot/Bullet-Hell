using System;
using System.Collections.Generic;
using Zenject;

public class ObjectiveTracker
{
    public Action OnObjectivesComplete;

    private readonly List<LevelObjective> _completedObjectives = new();
    private readonly List<LevelObjective> _totalObjectives = new();
    private readonly List<LevelObjective> _objectives = new();

    private readonly FloatingTextSpawner _floatingTextSpawner;

    public ObjectiveTracker(
        FloatingTextSpawner floatTextSpawner)
    {
        _floatingTextSpawner = floatTextSpawner;
    }
        
    public void ObjectiveComplete(LevelObjective objective)
    {
        if (!_objectives.Contains(objective) || _completedObjectives.Contains(objective)) return;

        _objectives.Remove(objective);
        _completedObjectives.Add(objective);

        _floatingTextSpawner.Spawn(objective.transform.position, objective.Score.ToString());
        OnObjectiveRemove();
    }

    public void AddObjective(LevelObjective objective)
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
            if (objective is Enemy)
                return true;

        return false;
    }

    public int CountTotalScore() => CountScore(_completedObjectives);

    public int CountBestPossibleScore() => CountScore(_totalObjectives);

    private int CountScore(List<LevelObjective> objectiveList)
    {
        int score = 0;

        foreach (var objective in objectiveList)
            score += objective.Score;

        return score;
    }
}