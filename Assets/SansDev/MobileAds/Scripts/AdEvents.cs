using System;
public static class AdEvents
{
    public static event Action OnConsentPopupClosed;
    public static void TriggerConsentPopupClosed() => OnConsentPopupClosed?.Invoke();

    public static event Action OnAdInitialized;
    public static void TriggerOnAdInitialized() => OnAdInitialized?.Invoke();

    public static event Action OnBannerLoadSucces;
    public static void TriggerBannerLoadSucces() => OnBannerLoadSucces?.Invoke();

    public static event Action<string> OnBannerLoadFailed;
    public static void TriggerBannerLoadFailed(string error) => OnBannerLoadFailed?.Invoke(error);

    public static event Action OnBannerClicked;
    public static void TriggerBannerClicked() => OnBannerClicked?.Invoke();

    public static event Action<bool> OnRewardedWatchedComplete;
    public static void TriggerRewardedWatchedComplete(bool complete) => OnRewardedWatchedComplete?.Invoke(complete);

}
