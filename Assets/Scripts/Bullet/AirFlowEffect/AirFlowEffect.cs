using UnityEngine;
using DG.Tweening;
using System;
using Zenject;

public class AirFlowEffect : MonoBehaviour, IPoolable<Vector3, Vector3, AirFlowEffect.Settings, float, IMemoryPool>
{
    public Transform Transform => _transform;

    private Settings _settings;
    private float _sizeMultiply;
    private IMemoryPool _pool;

    private Sequence _sequence;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void CreateSequence()
    {
        _sequence = DOTween.Sequence();

        _sequence.OnStart(() => _transform.localScale = Vector3.zero);

        _sequence.Append(_transform.DOScale(_settings.TargetScale * _sizeMultiply, _settings.ScaleUpDuration)
            .SetEase(_settings.ScaleUpEase));

        _sequence.AppendInterval(_settings.LifeTime);

        _sequence.Append(_transform.DOScale(0f, _settings.ScaleDownDuration)
            .SetEase(_settings.ScaleDownEase));

        _sequence.OnComplete( () => _pool.Despawn(this));

        _sequence.SetUpdate(UpdateType.Fixed, false);
    }

    public void OnSpawned(Vector3 position, Vector3 eulerAngles, Settings settings, float sizeMultiply, IMemoryPool pool)
    {
        _transform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles));
        _settings = settings;
        _sizeMultiply = sizeMultiply;
        _pool = pool;

        CreateSequence();
    }

    public void OnDespawned()
    {
        _pool = null;

        _sequence.Kill();
        _sequence = null;
    }

    [Serializable]
    public class Settings
    {
        [Header("Scale up")]
        public Vector3 TargetScale = new (0.25f, 0.25f, 0.1f);
        public float ScaleUpDuration = 0.025f;
        public Ease ScaleUpEase = Ease.OutQuint;

        [Header("Life time")]
        public float LifeTime = 0.025f;

        [Header("Scale down")]
        public float ScaleDownDuration = 0.025f;
        public Ease ScaleDownEase = Ease.InOutQuad;
    }

    public class Factory : PlaceholderFactory<Vector3, Vector3, Settings, float, AirFlowEffect> { }
}