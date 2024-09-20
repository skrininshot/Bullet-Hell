using System;
using System.Collections.Generic;

public class ObjectiveTracker
{
    public Action OnObjectivesComplete;
    public IReadOnlyCollection<IObjective> TotalObjectives => _totalObjectives;
    public IReadOnlyCollection<IObjective> CompletedObjectives => _completedObjectives;

    private readonly List<IObjective> _completedObjectives = new();
    private readonly List<IObjective> _totalObjectives = new();

    private readonly List<IObjective> _objectives = new();

    public void AddObjective(IObjective objective)
    {
        if (_objectives.Contains(objective)) return;

        _objectives.Add(objective);
        _totalObjectives.Add(objective);
    }

    public void RemoveObjective(IObjective objective) 
    {
        if (!_objectives.Contains(objective) || _completedObjectives.Contains(objective)) return;

        _objectives.Remove(objective);

        _completedObjectives.Add(objective);

        OnObjectiveRemove();
    }

    private void OnObjectiveRemove()
    {
        if (_objectives.Count == 0)
            OnObjectivesComplete?.Invoke();
    }
}