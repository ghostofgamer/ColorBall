using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveMenu : Menu
{
    [Header("Inherit References :")]
    [SerializeField] private Button _watchAdButton;
    [SerializeField] private Button _skipButton;

    [Space]
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] Image _timerFill;

    private Timer _timer;

    protected override void Awake()
    {
        base.Awake();

        _timer = GetComponent<Timer>();
    }

    protected override void OnMenuOpened()
    {
        base.OnMenuOpened();

        _skipButton.interactable = true;
        _watchAdButton.interactable = true;

        // start timer
        _timer.PlayTimer(i => _timerText.text = i, j => _timerFill.fillAmount = j, GameOver);

        LeanTween.scale(_watchAdButton.gameObject, Vector2.one * 1.1f, .3f).setEase(LeanTweenType.easeOutQuad).setLoopPingPong();
    }

    private void Start()
    {
        OnButtonPressed(_watchAdButton, WatchAdButtonPressed);
        OnButtonPressed(_skipButton, SkipButtonPressed);
    }

    private void SkipButtonPressed()
    {
        _skipButton.interactable = false;
        ResetWatchAdButton();

        _timer.StopTimer();
        MenuController.Instance.SwitchMenu(MenuType.GameOver);
    }

    private void WatchAdButtonPressed()
    {
        _watchAdButton.interactable = false;
        ResetWatchAdButton();

        _timer.StopTimer();

        AdManager.Instance.TryShowRewardedVideoAd(null);
    }

    private void ResetWatchAdButton()
    {
        LeanTween.cancel(_watchAdButton.gameObject);
        _watchAdButton.transform.localScale = Vector3.one;
    }

    private void GameOver()
    {
        MenuController.Instance.SwitchMenu(MenuType.GameOver);
    }
}
