using UnityEngine;
using UnityEngine.Events;

public class BodyPart : MonoBehaviour, IDamagable
{
    public BodyPartType BodyPartType => _bodyPartType;

    public UnityEvent<BodyPart> OnDamage = new();

    [SerializeField] private BodyPartType _bodyPartType;

    public void Damage() => OnDamage?.Invoke(this);
}

public enum BodyPartType
{
    Head,
    Torso,
    Arm,
    Leg
}