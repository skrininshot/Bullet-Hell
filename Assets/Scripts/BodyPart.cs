using UnityEngine;
using UnityEngine.Events;

public class BodyPart : MonoBehaviour, IDamagable
{
    public UnityEvent<BodyPart> OnHit = new();

    public void Damage()
    {
        OnHit?.Invoke(this);
    }
}
