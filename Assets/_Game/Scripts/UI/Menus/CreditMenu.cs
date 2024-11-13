using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditMenu : Menu
{
    [Header("References :")]
    [SerializeField] Button _backButton;

    [Space]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descText;

    [Header("Game Database :")]
    [SerializeField] private CreditDataSO _data;

    private void Start()
    {
        OnButtonPressed(_backButton, BackButtonPressed);

        SetCreditData();
    }

    private void SetCreditData()
    {
        if (_data == null)
        {
            Debug.LogWarning("Credit Data is not attached in the Inspector.");
            return;
        }

        _titleText.text = _data.GetTitle;
        _descText.text = _data.GetDesc;
    }

    protected override void OnMenuOpened()
    {
        _backButton.interactable = true;
    }

    private void BackButtonPressed()
    {
        Debug.Log("close credit");
        //_backButton.interactable = false;
        MenuController.Instance.CloseMenu();
    }
}
