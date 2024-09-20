using UnityEngine;
using UnityEngine.Events;

public class BodyPart : MonoBehaviour, IDamagable
{
    public UnityEvent<BodyPart> OnHit = new();

    [SerializeField] private ScoreType _scoreType;

    public void Damage()
    {
        OnHit?.Invoke(this);
    }

    public ScoreType GetScoreType() => _scoreType;
}
