using System.Collections.Generic;

public class LevelScoreRecorder
{
    private readonly List<IDamagable> _damagedObjects = new ();
    private readonly ScoreList _scoreList;
    private readonly ObjectiveTracker _objectiveTracker;

    public LevelScoreRecorder (GameSettings gameSettings, 
        ObjectiveTracker objectiveTracker)
    {
        _scoreList = gameSettings.ScoreList;
        _objectiveTracker = objectiveTracker;
    }

    public void AddToScore(IDamagable damagable)
    {
        if (!_damagedObjects.Contains(damagable))
            _damagedObjects.Add(damagable);
    }

    public int TotalScore()
    {
        int score = 0;

        foreach (var damagable in _damagedObjects)
            score += _scoreList.ScoreTypeDictionary[damagable.GetScoreType()];

        return score;
    }

    public float GetObjectiveScore() => 
        _objectiveTracker.CompletedObjectives.Count / (float)_objectiveTracker.TotalObjectives.Count;
}