using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ads Data", fileName = "Ads Data")]
public class AdsData : ScriptableObject
{
    [SerializeField] int _interstitialAdInterval;
    [SerializeField] float _minDelayBetweenInterstitial = 20f;

    [SerializeField][Range(0f, 1f)] float _rewardedAdFrequency = .5f;
    [SerializeField] float _minDelayBetweenRewarded = 20f;

    [SerializeField] bool _enableBanner = true;
    [SerializeField] bool _enableInterstitial = true;
    [SerializeField] bool _enableRewarded = true;

    public bool controlBanner = true;
    public bool controlInterstitial = true;
    public bool controlRewarded = true;

    public bool directedForChildren;
    public bool testMode;

    // ADMOB
    // Test App Id = "ca-app-pub-3940256099942544~3347511713";
    [SerializeField][TextArea(1, 2)] string idBanner;
    [SerializeField][TextArea(1, 2)] string idInterstitial;
    [SerializeField][TextArea(1, 2)] string idReward;

    // iOS
    // Test App Id = "ca-app-pub-3940256099942544~3347511713";
    [SerializeField][TextArea(1, 2)] string idBannerIOS;
    [SerializeField][TextArea(1, 2)] string idInterstitialIOS;
    [SerializeField][TextArea(1, 2)] string idRewardIOS;

    public string IdBanner => idBanner;
    public string IdInterstitial => idInterstitial;
    public string IdRewarded => idReward;

    public string IdBannerIOS => idBannerIOS;
    public string IdInterstitialIOS => idInterstitialIOS;
    public string IdRewardedIOS => idRewardIOS;

    public int InterstitialAdInterval => _interstitialAdInterval;
    public float RewardedAdFrequency => _rewardedAdFrequency;

    public float MinDelayBetweenInterstitial => _minDelayBetweenInterstitial;
    public float MinDelayBetweenRewarded => _minDelayBetweenRewarded;

    public bool BannerEnabled => _enableBanner;
    public bool InterstitialEnabled => _enableInterstitial;
    public bool RewardedEnabled => _enableRewarded;

    [SerializeField] bool _iosBuild;
    public bool IOSBuild => _iosBuild;
}

