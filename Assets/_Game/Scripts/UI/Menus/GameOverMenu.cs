using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : Menu
{
    [Header("References")]
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _bestScoreText;
    [SerializeField] Button _restartButton;
    [SerializeField] Button _homeButton;
    [SerializeField] Button _shareButton;
    [SerializeField] private GameObject _panel;
    [SerializeField]private CoinViewer _coinViewer;
    
    private ShareOnSocialMedia _share;

    protected override void Awake()
    {
        base.Awake();

        _share = GetComponent<ShareOnSocialMedia>();
    }

    protected override void OnMenuOpened()
    {
        base.OnMenuOpened();
        _panel.SetActive(true);
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying) return;
#endif

        SetScoreDisplay();
    }

    protected override void OnMenuClosed()
    {
        _panel.SetActive(false);
        base.OnMenuClosed();
    }

    private void Start()
    {
        OnButtonPressed(_restartButton, RestartButton);
        OnButtonPressed(_homeButton, HomeButton);
        OnButtonPressed(_shareButton, HandleShareButton);
    }

    private void SetScoreDisplay()
    {
        ScoreManager.Instance.UpdateBestScore();

        // PlayerPrefs.SetInt("Coin",ScoreManager.Instance.Score);
        ScoreManager.Instance.AddCoin(ScoreManager.Instance.Score);
        // _coinViewer.ShowCoinText();
        _scoreText.text = ScoreManager.Instance.Score.ToString();
        _bestScoreText.text = SaveData.GetBestScore().ToString();
    }

    private void HandleShareButton()
    {
        _shareButton.interactable = false;

        _share.HandleShare();
    }

    private void HomeButton()
    {
        _homeButton.interactable = false;

        StartCoroutine(ReloadLevelAsync(() =>
        {
            MenuController.Instance.SwitchMenu(MenuType.Main);
        }));
    }

    private void RestartButton()
    {
        _restartButton.interactable = false;
        // _panel.SetActive(false);
        // Debug.Log("Проиграл ");
        StartCoroutine(ReloadLevelAsync(() =>
        {
            MenuController.Instance.SwitchMenu(MenuType.Gameplay);
        }));
    }

    IEnumerator ReloadLevelAsync(System.Action OnSceneLoaded = null)
    {
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        OnSceneLoaded?.Invoke();
    }
}
