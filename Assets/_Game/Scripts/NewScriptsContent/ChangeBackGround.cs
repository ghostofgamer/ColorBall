using UnityEngine;
using UnityEngine.UI;

public class ChangeBackGround : Menu
{
    [SerializeField] Sprite _darkBackgroundImg;
    [SerializeField] Sprite _lightBackgroundImg;
    [SerializeField] private Image _backgroundImg;
    [SerializeField] Image _themeImage;
    [SerializeField] Button _themeButton;
    
    protected override void Awake()
    {
        // base.Awake();
        Debug.Log("Оставить пустым ! ");
    }

    private void Start()
    {
        _backgroundImg.sprite = SaveData.GetThemeState() ? _lightBackgroundImg : _darkBackgroundImg;
    }
}
