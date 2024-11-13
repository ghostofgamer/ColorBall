using GoogleMobileAds.Ump.Api;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestAdManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI consentText;
    [SerializeField] private TextMeshProUGUI privacyText;
    [SerializeField] private Button interstitialButton;
    [SerializeField] private Button rewardedVideoButton;
    [SerializeField] private Button privacyButton;

    private int coins = 0;

    private void OnEnable() => AdEvents.OnConsentPopupClosed += HandleConsentPopupClosed;
    private void OnDisable() => AdEvents.OnConsentPopupClosed -= HandleConsentPopupClosed;

    private void HandleConsentPopupClosed()
    {
        UpdateConsentStat();
    }

    private void UpdateConsentStat()
    {
        privacyButton.interactable = AdManager.Instance.IsPrivacyRequire();
        consentText.text = $"Consent Status: {ConsentInformation.ConsentStatus}";
        privacyText.text = $"Privacy Options: {ConsentInformation.PrivacyOptionsRequirementStatus}";
    }

    public void ShowBanner()
    {
        AdManager.Instance.ShowBanner();
    }

    public void HideBanner()
    {
        AdManager.Instance.HideBanner();
    }

    public void ShowInterstitial()
    {
        AdManager.Instance.ShowInterstitial();
    }

    public void ShowRewarded()
    {
        AdManager.Instance.ShowRewardedVideo(TriggerRewardedVideoComplete);
    }

    public void ResetConsent()
    {
        ConsentInformation.Reset();
        UpdateConsentStat();
    }

    private void TriggerRewardedVideoComplete(bool onComplete)
    {
        if (onComplete)
        {
            coins += 100;
        }

        coinText.text = $"Rewarded coins: {coins}";
        Debug.Log("Complete" + onComplete);
    }

    private void Update()
    {
        bool interstitialAvaliable =  AdManager.Instance.IsInterstitialAvaliable();
        bool rewardedVideoAvaliable = AdManager.Instance.IsRewardedVideoAvaliable();

        interstitialButton.interactable = interstitialAvaliable;
        rewardedVideoButton.interactable = rewardedVideoAvaliable;
    }
}
