using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] CanvasGroup _canvasGroup;

    private void OnEnable()
    {
        ScoreManager.OnScoreUpdated += UpdateScoreDisplay;

        GameEvents.OnGameStarted += FadeIn;
        GameEvents.OnGameOver += FadeOut;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreUpdated -= UpdateScoreDisplay;

        GameEvents.OnGameStarted -= FadeIn;
        GameEvents.OnGameOver -= FadeOut;
    }

    private void FadeIn()
    {
        _canvasGroup.DOFade(1f, 1f);
    }

    private void FadeOut()
    {
        _canvasGroup.DOFade(0f, 1f);
    }

    private void UpdateScoreDisplay(int score)
    {
        _scoreText.text = score.ToString();
    }
}
