using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("References")]
    [SerializeField] Button _themeButton;
    [SerializeField] Button _creditButton;
    [SerializeField] Button _rateButton;
    [SerializeField] Button _settingsButton;

    [Header("Theme SPrite Toggle")]
    [SerializeField] Sprite _lightImg;
    [SerializeField] Sprite _darkImg;

    [SerializeField] Image _themeImage;

    private void Start()
    {
        OnButtonPressed(_creditButton, CreditButtonPressed);
        OnButtonPressed(_rateButton, RateButtonPressed);
        OnButtonPressed(_settingsButton, SettingsButtonPressed);
        OnButtonPressed(_themeButton, ToggleThemeButton);

        _themeImage.sprite = SaveData.GetThemeState() ? _lightImg : _darkImg;
    }

    private void ToggleThemeButton()
    {
        GameManager.Instance.ToggleTheme();
        _themeImage.sprite = SaveData.GetThemeState() ? _lightImg : _darkImg;
    }

    private void SettingsButtonPressed()
    {
        MenuController.Instance.OpenMenu(MenuType.Setting);
    }

    private void RateButtonPressed()
    {
        MenuController.Instance.OpenMenu(MenuType.Rate);
    }

    private void CreditButtonPressed()
    {
        MenuController.Instance.OpenMenu(MenuType.Credit);
    }
}
