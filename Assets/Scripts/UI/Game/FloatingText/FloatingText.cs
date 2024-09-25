using UnityEngine;
using DG.Tweening;
using System;
using Zenject;
using TMPro;

public class FloatingText : MonoBehaviour, IPoolable<Vector3, string, FloatingText.Settings, Transform, IMemoryPool>
{
    public Transform Transform => _transform;

    [SerializeField] private TMP_Text _text;

    private string _value;
    private Settings _settings;
    private Transform _cameraTransform;
    private IMemoryPool _pool;

    private Sequence _sequence;
    private Transform _transform;

    private Color _transparentColor;

    private void Awake()
    {
        _transform = transform;

        var transparentColor = _text.color;
        transparentColor.a = 0f;

        _transparentColor = transparentColor;
    }

    public void OnSpawned(Vector3 position, string value, Settings settings, Transform cameraTransform, IMemoryPool pool)
    {
        _cameraTransform = cameraTransform;
        _transform.SetPositionAndRotation(position, Quaternion.Euler(_cameraTransform.eulerAngles));

        _value = value;
        _text.SetText(_value);
        _settings = settings;

        _pool = pool;

        CreateSequence();
    }

    private void CreateSequence()
    {
        _sequence = DOTween.Sequence();

        _sequence.OnStart(() => _text.color = _transparentColor);

        _sequence.Prepend(_text.DOFade(1f, _settings.ShowDuration));

        _sequence.AppendInterval(_settings.VisibleDuration);

        _sequence.Append(_text.DOFade(0f, _settings.HideDuration));

        _sequence.OnComplete(() => _pool.Despawn(this));

        _sequence.OnUpdate(() => _transform.forward = _transform.position - _cameraTransform.position);

        _sequence.SetUpdate(false);
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
        public float ShowDuration = 0.1f;
        public float VisibleDuration = 1f;  
        public float HideDuration = 0.1f;
    }

    public class Factory : PlaceholderFactory<Vector3, string, Settings, Transform, FloatingText> { }
}
