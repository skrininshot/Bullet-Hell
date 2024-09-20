using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AwardView : PauseBaseView
{
    public Button NextLevelButton => _nextLevelButton;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _star1;
    [SerializeField] private Image _star2;
    [SerializeField] private Image _star3;

    private Vector3 _star1InitialPosition;
    private Vector3 _star2InitialPosition;
    private Vector3 _star3InitialPosition;

    [SerializeField] private Button _nextLevelButton;

    private Settings _settings;
    private LevelScoreRecorder _levelScoreRecorder;

    private Sequence _sequence;

    [Inject]
    private void Construct(GameSettings gameSettings, 
        LevelScoreRecorder levelScoreRecorder)
    {
        _settings = gameSettings.UI.Game.AwardView;
        _levelScoreRecorder = levelScoreRecorder;
    }

    public override void SetVisibility(bool visible)
    {
        base.SetVisibility(visible);

        if (visible)
            CreateSequence();
        else
            _sequence.Kill();
    }

    private void CreateSequence()
    {
        int totalScore = _levelScoreRecorder.TotalScore();
        float objectiveScore = _levelScoreRecorder.GetObjectiveScore();
        int scoreText = 0;

        HideStars();

        _sequence = DOTween.Sequence();

        _sequence.Append(DOTween.To(() => scoreText, x => scoreText = x, totalScore, _settings.ScoreTextDuration)
            .OnUpdate(() => _scoreText.SetText($"Score: {scoreText}")));

        if (objectiveScore > 0)
        {
            _sequence.Append(_star1.rectTransform.DOScale(1f, _settings.DurationPerStar));
            _sequence.Join(_star1.rectTransform.DOLocalMove(_star1InitialPosition, _settings.DurationPerStar));
            _sequence.Join(_star1.DOFade(1f, _settings.DurationPerStar));
        }

        if (objectiveScore > 0.33)
        {
            _sequence.Append(_star2.rectTransform.DOScale(1f, _settings.DurationPerStar));
            _sequence.Join(_star2.rectTransform.DOLocalMove(_star2InitialPosition, _settings.DurationPerStar));
            _sequence.Join(_star2.DOFade(1f, _settings.DurationPerStar));
        }

        if (objectiveScore > 0.66)
        {
            _sequence.Append(_star3.rectTransform.DOScale(1f, _settings.DurationPerStar));
            _sequence.Join(_star3.rectTransform.DOLocalMove(_star3InitialPosition, _settings.DurationPerStar));
            _sequence.Join(_star3.DOFade(1f, _settings.DurationPerStar));
        }

        _sequence.Play();
    }

    private void HideStars()
    {
        Color transparent = Color.white;
        transparent.a = 0f;

        _star1.color = transparent;
        _star2.color = transparent;
        _star3.color = transparent;

        _star1.rectTransform.localScale = Vector3.one * _settings.StarInitialScale;
        _star2.rectTransform.localScale = Vector3.one * _settings.StarInitialScale;
        _star3.rectTransform.localScale = Vector3.one * _settings.StarInitialScale;

        _star1InitialPosition = _star1.rectTransform.localPosition;
        _star2InitialPosition = _star2.rectTransform.localPosition;
        _star3InitialPosition = _star3.rectTransform.localPosition;

        _star1.rectTransform.localPosition += Quaternion.Euler(0, 0, 45f) * Vector3.left * _settings.StarInitialDistance;
        _star2.rectTransform.localPosition += Vector3.down * _settings.StarInitialDistance;
        _star3.rectTransform.localPosition += Quaternion.Euler(0, 0, 45f) * Vector3.right * _settings.StarInitialDistance;
    }

    [Serializable]
    public class Settings
    {
        public float ScoreTextDuration = 1f;
        public float StarInitialScale = 3f;
        public float DurationPerStar = 1f;
        public float DurationBetweenStars = 1f;
        public float StarInitialDistance = 1f;
    }
}
