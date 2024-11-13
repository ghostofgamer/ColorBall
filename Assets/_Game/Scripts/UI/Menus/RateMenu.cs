using UnityEngine;
using UnityEngine.UI;

public class RateMenu : Menu
{
    [Header("References :")]
    [SerializeField] Button _laterButton;
    [SerializeField] Button _rateButton;
    [SerializeField] RateUs _rate;

    private void Start()
    {
        OnButtonPressed(_laterButton, LaterButtonListener);
        OnButtonPressed(_rateButton, RateButtonListener);
    }

    protected override void OnMenuOpened()
    {
        _rateButton.interactable = true;
        _laterButton.interactable = true;
    }

    private void LaterButtonListener()
    {
        _laterButton.interactable = false;
        MenuController.Instance.CloseMenu();
    }

    private void RateButtonListener()
    {
        _rateButton.interactable = false;
        _rate.RateNow();
    }
}
