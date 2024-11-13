using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera _mainCam;
    [SerializeField] Sprite _darkBackgroundImg;
    [SerializeField] Sprite _lightBackgroundImg;
    [SerializeField] Sprite _lightDeadZone;
    [SerializeField] Sprite _darkDeadZone;
    [SerializeField] SpriteRenderer[] _deadZoneSprites;
    [SerializeField] private Image _backgroundImg;
    
    [Header("External Components")]
    [SerializeField] Player _player;
    [SerializeField] GameCustomizationSO _gameCustom;

    bool _isDefaultTheme = true;
    bool _playerRevived = false;


    static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>(includeInactive: true);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        GameEvents.OnGameStarted += GameStart;
        GameEvents.OnGameOver += GameOver;
        GameEvents.OnPlayerRedirect += HandleJumpAudio;

        AdEvents.OnRewardedWatchedComplete += HandleRewardedAdWatchedComplete;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStarted -= GameStart;
        GameEvents.OnGameOver -= GameOver;
        GameEvents.OnPlayerRedirect -= HandleJumpAudio;

        AdEvents.OnRewardedWatchedComplete -= HandleRewardedAdWatchedComplete;
    }

    private void HandleRewardedAdWatchedComplete(bool complete)
    {
        if (complete)
        {
            Revive();
            return;
        }

        MenuController.Instance.SwitchMenu(MenuType.GameOver);
    }

    private void Start()
    {
        _isDefaultTheme = SaveData.GetThemeState();
        _mainCam.backgroundColor = _isDefaultTheme ? _gameCustom.DefaultColor : _gameCustom.DarkColor;
        
        foreach (SpriteRenderer sprite in _deadZoneSprites)
            sprite.sprite = SaveData.GetThemeState() ? _lightDeadZone : _darkDeadZone;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
        }
#endif
    }

    private void HandleJumpAudio()
    {
        SoundManager.Instance.PlayAudio(AudioType.WALLHIT);
        SoundManager.Instance.PlayAudio(AudioType.SCORE);

        VibrationManager.Instance.StartVibration();
    }

    public void ToggleTheme()
    {
        _isDefaultTheme = !_isDefaultTheme;
        SaveData.SetThemeState(_isDefaultTheme);

        foreach (SpriteRenderer sprite in _deadZoneSprites)
            sprite.sprite = SaveData.GetThemeState() ? _lightDeadZone : _darkDeadZone;
                
        _backgroundImg.sprite = SaveData.GetThemeState() ? _lightBackgroundImg : _darkBackgroundImg;
        _mainCam.backgroundColor = _isDefaultTheme ? _gameCustom.DefaultColor : _gameCustom.DarkColor;
    }

    private void GameOver()
    {
        SoundManager.Instance.PlayAudio(AudioType.FAIL);
        VibrationManager.Instance.StartVibration();

        if (CanRevive())
        {
            _playerRevived = true;

            MenuController.Instance.SwitchMenu(MenuType.Revive);
        }
        else
        {
            MenuController.Instance.SwitchMenu(MenuType.GameOver);
        }
    }

    private bool CanRevive()
    {
        bool isRewardLoaded = AdManager.Instance.CanShowRewardedVideoAd();
        return ScoreManager.Instance.Score > 2 && !_playerRevived && isRewardLoaded;
    }

    private void Revive()
    {
        MenuController.Instance.SwitchMenu(MenuType.Gameplay);

        _player.Revive();
    }

    private void GameStart()
    {
        MenuController.Instance.SwitchMenu(MenuType.Gameplay);
    }

    public static void Quit()
    {
        if (Application.isEditor)
        {
            ExitPlayMode();
        }
        else
        {
            Application.Quit();
        }
    }

    static void ExitPlayMode()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
