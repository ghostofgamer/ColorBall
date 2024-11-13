using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : Menu
{
    [Header("Inherit References :")]
    [SerializeField] private Button _adsButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _toggleMusicButton;
    [SerializeField] private Button _toggleSFXButton;
    [SerializeField] private Button _toggleVibrateButton;

    [Header("SFX Image Toggle")]
    [SerializeField] Sprite _sfxTrue;
    [SerializeField] Sprite _sfxFalse;

    [Header("Music Image Toggle")]
    [SerializeField] Sprite _musicTrue;
    [SerializeField] Sprite _musicFalse;

    [Header("Vibrate Image Toggle")]
    [SerializeField] Sprite _vibrateTrue;
    [SerializeField] Sprite _vibrateFalse;

    private Image _musicImage;
    private Image _sfxImage;
    private Image _vibrateImage;

    protected override void OnMenuOpened()
    {
        base.OnMenuOpened();

        // _adsButton.interactable = true;
        _closeButton.interactable = true;

        SetIconToggle();
    }

    private void Start()
    {
        _musicImage = _toggleMusicButton.GetComponent<Image>();
        _sfxImage = _toggleSFXButton.GetComponent<Image>();
        _vibrateImage = _toggleVibrateButton.GetComponent<Image>();

        // OnButtonPressed(_adsButton, AdsButtonListener);
        OnButtonPressed(_closeButton, CloseButtonListener);
        OnButtonPressed(_toggleMusicButton, ToggleMusicButtonListener);
        OnButtonPressed(_toggleSFXButton, ToggleSFXButtonListener);
        OnButtonPressed(_toggleVibrateButton, ToggleVibrateButtonListener);

        SetIconToggle();
    }

    private void SetIconToggle()
    {
        _musicImage.sprite = SaveData.GetMusicState() ? _musicTrue : _musicFalse;
        _sfxImage.sprite = SaveData.GetSfxState() ? _sfxTrue : _sfxFalse;
        _vibrateImage.sprite = SaveData.GetVibrateState() ? _vibrateTrue : _vibrateFalse;
    }

    private void ToggleMusicButtonListener()
    {
        SoundManager.Instance.ToggleMusic();
        _musicImage.sprite = SaveData.GetMusicState() ? _musicTrue : _musicFalse;
    }

    private void ToggleSFXButtonListener()
    {
        SoundManager.Instance.ToggleFX();
        _sfxImage.sprite = SaveData.GetSfxState() ? _sfxTrue : _sfxFalse;
    }

    private void ToggleVibrateButtonListener()
    {
        VibrationManager.Instance.ToggleVibration();
        _vibrateImage.sprite = SaveData.GetVibrateState() ? _vibrateTrue : _vibrateFalse;

        VibrationManager.Instance.StartVibration();
    }

    private void CloseButtonListener()
    {
        _closeButton.interactable = false;
        MenuController.Instance.CloseMenu();
    }

    private void AdsButtonListener()
    {
        _adsButton.interactable = false;

        AdManager.Instance.ShowConsentForm();
    }

    private void OnEnable() => AdEvents.OnConsentPopupClosed += ConsentClosedCallback;
    private void OnDisable() => AdEvents.OnConsentPopupClosed -= ConsentClosedCallback;

    private void ConsentClosedCallback()
    {
        bool isConsentRequired = AdManager.Instance.IsPrivacyRequire();
        // _adsButton.gameObject.SetActive(isConsentRequired);

        if (MenuController.Instance.MenuStack.Count > 0)
        {
            if (MenuController.CurrentMenu == MenuType.Setting)
                MenuController.Instance.CloseMenu();
        }
    }
}
