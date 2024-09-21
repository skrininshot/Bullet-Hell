using UnityEngine;
using UnityEngine.Events;

public class BodyPart : MonoBehaviour, IDamagable
{
    public ScoreType ScoreType => _scoreType;

    public UnityEvent<BodyPart> OnDamage = new();

    [SerializeField] private ScoreType _scoreType;

    public void Damage()
    {
        OnDamage?.Invoke(this);
    }
}
