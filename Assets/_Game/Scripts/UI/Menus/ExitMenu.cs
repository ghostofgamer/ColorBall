using UnityEngine;
using UnityEngine.UI;

public class ExitMenu : Menu
{
    [Header("References :")]
    [SerializeField] Button _yesButton;
    [SerializeField] Button _noButton;

    private void Start()
    {
        OnButtonPressed(_yesButton, YesButtonPressed);
        OnButtonPressed(_noButton, NoButtonPressed);
    }

    protected override void OnMenuOpened()
    {
        _noButton.interactable = true;
        _yesButton.interactable = true;
    }

    private void NoButtonPressed()
    {
        _noButton.interactable = false;

        MenuController.Instance.CloseMenu();
    }

    private void YesButtonPressed()
    {
        _yesButton.interactable = false;

        GameManager.Quit();
    }
}
