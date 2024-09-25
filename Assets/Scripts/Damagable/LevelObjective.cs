using UnityEngine;
using Zenject;

public enum ObjectiveType
{
    Prop,
    Enemy
}

public class LevelObjective : MonoBehaviour
{
    public ObjectiveType ObjectType => _objectiveType;
    public int Score => _score;

    [SerializeField] private ObjectiveType _objectiveType;
    [SerializeField] protected int _score;

    private ObjectiveTracker _objectvieTracker;

    [Inject]
    public void Construct(ObjectiveTracker objectiveTracker)
    {
        _objectvieTracker = objectiveTracker;
    }
    private void Awake()
    {
        _objectvieTracker.AddObjective(this);
    }

    protected virtual void Complete()
    {
        _objectvieTracker.ObjectiveComplete(this);
    }
}