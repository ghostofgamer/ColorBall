using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    [SerializeField] private AdmobImplementation _admob;

    private bool _isInterstitialIntervalPassed = false;
    private float _interstitialTimer;
    private float _rewardedTimer;

    private AdsData _data => AdResources.Instance.AdsData;

    static int gameplayCount;

    #region Singleton
    private static AdManager instance;

    public static AdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AdManager>(includeInactive: true);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        else { DontDestroyOnLoad(gameObject); }
    }
    #endregion

    private void OnEnable() => SceneManager.sceneLoaded += HandleSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= HandleSceneLoaded;

    private void HandleSceneLoaded(Scene s, LoadSceneMode lsm)
    {
        ShowBanner();

        int interval = Mathf.Clamp(_data.InterstitialAdInterval, 1, 100);

        if (gameplayCount != 0 && gameplayCount % interval == 0)
            _isInterstitialIntervalPassed = true;

        // try to show interstitial every scene loaded
        TryShowInterstitial();

        gameplayCount++;
    }

    private void Start()
    {
        InitializeAds(HandleInitComplete);

        _interstitialTimer = _data.MinDelayBetweenInterstitial;
        _rewardedTimer = _data.MinDelayBetweenRewarded;
    }

    private void HandleInitComplete()
    {
        ShowBanner();
    }

    public void InitializeAds(UnityAction onInitialized)
    {
        AdSettings settings = new AdSettings(
#if UNITY_ANDROID
        _data.IdBanner, _data.IdInterstitial, _data.IdRewarded,
#else
            _data.IdBannerIOS, _data.IdInterstitialIOS, _data.IdRewardedIOS,
#endif
        _data.testMode,
        _data.directedForChildren
        );

        _admob.InitializeAds(settings, onInitialized);
    }

    public void TryShowInterstitial()
    {
        if (Time.time > _interstitialTimer && _isInterstitialIntervalPassed)
        {
            _isInterstitialIntervalPassed = false;
            _interstitialTimer = Time.time + _data.MinDelayBetweenInterstitial;

            // show interstitial ad
            ShowInterstitial();
        }
    }

    public void TryShowRewardedVideoAd(UnityAction<bool> onComplete)
    {
        if (CanShowRewardedVideoAd())
        {
            _rewardedTimer = Time.time + _data.MinDelayBetweenRewarded;

            // show rewarded ad
            ShowRewardedVideo(onComplete);
        }
    }

    public bool CanShowRewardedVideoAd()
    {
        return Time.time > _rewardedTimer;
    }

    public void ShowBanner()
    {
        if (!_data.BannerEnabled)
            return;

        if (_admob.CanShowAd)
            _admob.ShowBanner();
    }

    public void HideBanner()
    {
        if (!_data.BannerEnabled)
            return;

        if (_admob.CanShowAd)
            _admob.HideBanner();
    }

    public void ShowInterstitial()
    {
        if (!_data.InterstitialEnabled)
            return;

        if (_admob.CanShowAd)
            _admob.ShowInterstitial(null);
    }

    public void ShowRewardedVideo(UnityAction<bool> onComplete)
    {
        if (!_data.RewardedEnabled)
            return;

        if (_admob.CanShowAd)
            _admob.ShowRewardedVideo(onComplete);
    }

    public void ShowConsentForm()
    {
        _admob.ShowForm();
    }

    public bool IsPrivacyRequire()
    {
        return _admob.IsPrivacyOptionsRequire();
    }

    public bool IsInterstitialAvaliable() => _admob.IsInterstitialAvailable();
    public bool IsRewardedVideoAvaliable() => _admob.IsRewardedVideoAvailable();

    public void OpenAdInspector()
    {
        _admob.OpenAdInspector();
    }
}
