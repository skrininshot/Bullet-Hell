using System;
using System.Collections.Generic;

public class ObjectiveTracker
{
    public Action OnObjectivesComplete;

    private readonly List<IObjective> _completedObjectives = new();
    private readonly List<IObjective> _totalObjectives = new();
    private readonly List<IObjective> _objectives = new();

    private readonly ScoreList _scoreList;

    public ObjectiveTracker(GameSettings gameSettings) =>
        _scoreList = gameSettings.ScoreList;

    public void AddObjective(IObjective objective)
    {
        if (_objectives.Contains(objective)) return;

        _objectives.Add(objective);
        _totalObjectives.Add(objective);
    }

    public void CompleteObjective(IObjective objective) 
    {
        if (!_objectives.Contains(objective) || _completedObjectives.Contains(objective)) return;

        _objectives.Remove(objective);
        _completedObjectives.Add(objective);

        OnObjectiveRemove();
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

    private int CountScore(List<IObjective> objectiveList)
    {
        int score = 0;

        foreach (var objective in objectiveList)
        {
            if (objective is IScorable scorable)
                score += _scoreList.ScoreTypeDictionary[scorable.GetScoreType()];
        }

        return score;
    }
}