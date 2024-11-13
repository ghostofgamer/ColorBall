using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AdmobImplementation : MonoBehaviour
{
    private const float reloadTime = 30;
    private readonly int maxRetryCount = 10;

    // Banner
    private string _bannerId;
    private BannerView _bannerView;
    private bool _bannerLoaded;

    // Interstitial
    private string _interstitialId;
    private InterstitialAd _interstitialAd;
    private int _currentRetryInterstitial;
    private bool _interstitialFailedToLoad;
    private UnityAction onInterstitialClosed;

    // Rewarded
    private string _rewardedId;
    private RewardedAd _rewardedAd;
    private int _currentRetryRewardedVideo;
    private bool _rewardedVideoFailedToLoad;
    private bool _rewardedVideoWatched;
    private UnityAction<bool> onRewardedVideoClosed;

    // consent
    private ConsentForm _consentForm;

    // Init
    private string designedForFamilies;
    private bool _directedForChildren;
    private bool _testMode;
    private bool _isInitialized = false;
    private UnityAction onInitialized;

    [SerializeField] private AdPosition _bannerPosition;

    [Header("Consent Test Mode")]
    [SerializeField] private bool _consentTest;
    [SerializeField] private DebugGeography _debugGeography;
    [SerializeField] private string[] _testDeviceIdList;

    public bool CanShowAd => _isInitialized;

    public void OpenAdInspector()
    {
        Debug.Log("Opening ad Inspector.");
        MobileAds.OpenAdInspector((AdInspectorError error) =>
        {
            // If the operation failed, an error is returned.
            if (error != null)
            {
                Debug.Log("Ad Inspector failed to open with error: " + error);
                return;
            }

            Debug.Log("Ad Inspector opened successfully.");
        });
    }

    public void InitializeAds(AdSettings settings, UnityAction onInitialized)
    {
        this.onInitialized = onInitialized;

        _testMode = settings.testMode;
        _directedForChildren = settings.directedForChildren;

        if (_testMode)
        {
#if UNITY_ANDROID
            _bannerId = "ca-app-pub-3940256099942544/6300978111";
            _interstitialId = "ca-app-pub-3940256099942544/1033173712";
            _rewardedId = "ca-app-pub-3940256099942544/5224354917";
#else
            _bannerId = "ca-app-pub-3940256099942544/2934735716";
            _interstitialId = "ca-app-pub-3940256099942544/4411468910";
            _rewardedId = "ca-app-pub-3940256099942544/1712485313";
#endif
        }
        else
        {
            _bannerId = settings.idBanner;
            _interstitialId = settings.idInterstitial;
            _rewardedId = settings.idRewarded;
        }

        TagForChildDirectedTreatment tagFororChildren;
        TagForUnderAgeOfConsent tagForUnderAge;
        MaxAdContentRating contentRating;

        if (_directedForChildren == true)
        {
            designedForFamilies = "true";
            tagFororChildren = TagForChildDirectedTreatment.True;
            tagForUnderAge = TagForUnderAgeOfConsent.True;
            contentRating = MaxAdContentRating.G;
        }
        else
        {
            designedForFamilies = "false";
            tagFororChildren = TagForChildDirectedTreatment.Unspecified;
            tagForUnderAge = TagForUnderAgeOfConsent.Unspecified;
            contentRating = MaxAdContentRating.Unspecified;
        }

        RequestConfiguration requestConfiguration = new RequestConfiguration()
        {
            TagForChildDirectedTreatment = tagFororChildren,
            MaxAdContentRating = contentRating,
            TagForUnderAgeOfConsent = tagForUnderAge
        };

        if (_consentTest && _testDeviceIdList.Length > 0)
        {
            foreach (var id in _testDeviceIdList)
                requestConfiguration.TestDeviceIds.Add(id);
        }

        MobileAds.SetRequestConfiguration(requestConfiguration);

        MobileAds.SetiOSAppPauseOnBackground(true);
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        // If we can request ads, we should initialize the Google Mobile Ads Unity plugin.
        if (ConsentInformation.CanRequestAds())
        {
            InitializeGoogleMobileAds();
        }

        // Ensures that privacy and consent information is up to date.
        InitializeGoogleMobileAdsConsent();
    }

    private void InitializeGoogleMobileAds()
    {
        if (_isInitialized)
            return;

        _isInitialized = false;

        // Initialize the Google Mobile Ads Unity plugin.
        Debug.Log("Google Mobile Ads Initializing.");
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                return;
            }

            // If you use mediation, you can check the status of each adapter.
            var adapterStatusMap = initstatus.getAdapterStatusMap();
            if (adapterStatusMap != null)
            {
                foreach (var item in adapterStatusMap)
                {
                    Debug.Log(string.Format("Adapter {0} is {1}",
                        item.Key,
                        item.Value.InitializationState));
                }
            }

            if (!string.IsNullOrEmpty(_interstitialId))
            {
                LoadInterstitial();
            }

            if (!string.IsNullOrEmpty(_rewardedId))
            {
                LoadRewardedAd();
            }

            Debug.Log("Google Mobile Ads initialization complete.");
            _isInitialized = true;
            onInitialized?.Invoke();
            AdEvents.TriggerOnAdInitialized();
        });
    }

    #region Consent
    public bool IsPrivacyOptionsRequire()
    {
        bool privacyRequired = ConsentInformation.PrivacyOptionsRequirementStatus == PrivacyOptionsRequirementStatus.Required;
        bool consentRequired = ConsentInformation.ConsentStatus == ConsentStatus.Required;
        bool consentObtained = ConsentInformation.ConsentStatus == ConsentStatus.Obtained;

        return privacyRequired;
    }

    private void InitializeGoogleMobileAdsConsent()
    {
        var debugSettings = new ConsentDebugSettings();

        if (_consentTest)
        {
            debugSettings.DebugGeography = _debugGeography;
            debugSettings.TestDeviceHashedIds.Add(AdRequest.TestDeviceSimulator);

            if (_testDeviceIdList.Length > 0)
            {
                foreach (var id in _testDeviceIdList)
                    debugSettings.TestDeviceHashedIds.Add(id);
            }
        }

        ConsentRequestParameters request = new ConsentRequestParameters
        {
            // False means users are not under age.
            TagForUnderAgeOfConsent = _directedForChildren,
            ConsentDebugSettings = debugSettings
        };

        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    private void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            Debug.LogError("Failed to gather consent with error: " + error);
            return;
        }

        LoadForm();
    }

    private void LoadForm()
    {
        if (ConsentInformation.IsConsentFormAvailable())
        {
            // Loads a consent form.
            ConsentForm.Load(OnLoadConsentForm);
        }
        else
        {
            Debug.Log("Consent form not available");
            OnConsentPopupClosed(null);
        }
    }

    private void OnLoadConsentForm(ConsentForm form, FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            OnConsentPopupClosed(error);
            return;
        }

        Debug.Log($"Consent Status: {ConsentInformation.ConsentStatus}");
        // The consent form was loaded.
        // Save the consent form for future requests.
        _consentForm = form;

        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            ShowForm();
        }
        else
        {
            OnConsentPopupClosed(null);
        }
    }

    public void ShowForm()
    {
        if (_consentForm != null)
        {
            _consentForm.Show(OnShowForm);
        }
        else
        {
            LoadForm();
        }
    }

    private void OnShowForm(FormError error)
    {
        if (error != null)
        {
            // Handle the error.
            OnConsentPopupClosed(error);
            return;
        }

        // Handle dismissal by reloading form.
        Debug.Log("Form Closed");
        OnConsentPopupClosed(null);
        LoadForm();
    }

    void OnConsentPopupClosed(FormError error)
    {
        if (error != null)
        {
            Debug.Log($"{error.ErrorCode} {error.Message}");
        }

        Debug.Log("Privacy Status : " + ConsentInformation.PrivacyOptionsRequirementStatus);
        AdEvents.TriggerConsentPopupClosed();
        InitializeGoogleMobileAds();
    }

    public void ResetConsentInformation()
    {
        ConsentInformation.Reset();
    }
    #endregion

    private AdRequest GetRequest()
    {
        AdRequest request = new AdRequest();
        request.Extras.Add("is_designed_for_families", designedForFamilies);
        return request;
    }

    #region Banner
    public void HideBanner()
    {
        if (_bannerView != null)
        {
            if (!_bannerLoaded)
                DestroyBanner();
            else
            {
                Debug.Log("Hiding banner view.");
                _bannerView.Hide();
            }
        }
    }

    public void ShowBanner()
    {
        _bannerLoaded = false;
        if (_bannerView != null)
        {
            _bannerLoaded = true;
            Debug.Log("Showing banner view.");
            _bannerView.Show();
        }
        else
            LoadBanner();
    }

    private void LoadBanner()
    {
        Debug.Log("Creating banner view.");

        DestroyBanner();

        AdSize adaptiveSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        _bannerView = new BannerView(_bannerId, adaptiveSize, _bannerPosition);

        // listen to events the banner may raise.
        _bannerView.OnBannerAdLoaded += BannerLoadSucces;
        _bannerView.OnBannerAdLoadFailed += BannerLoadFailed;
        _bannerView.OnAdPaid += BannerAdPaied;
        _bannerView.OnAdImpressionRecorded += BannerImpressionRecorded;
        _bannerView.OnAdClicked += BannerClicked;
        _bannerView.OnAdFullScreenContentOpened += BannerFullScreenOpened;
        _bannerView.OnAdFullScreenContentClosed += BannerFullScreenClose;

        var adRequest = GetRequest();

        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

    private void DestroyBanner()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");

            _bannerView.OnBannerAdLoaded -= BannerLoadSucces;
            _bannerView.OnBannerAdLoadFailed -= BannerLoadFailed;
            _bannerView.OnAdPaid -= BannerAdPaied;
            _bannerView.OnAdImpressionRecorded -= BannerImpressionRecorded;
            _bannerView.OnAdClicked += BannerClicked;
            _bannerView.OnAdFullScreenContentOpened -= BannerFullScreenOpened;
            _bannerView.OnAdFullScreenContentClosed -= BannerFullScreenClose;

            _bannerView.Destroy();
            _bannerView = null;
        }
    }
    #endregion

    #region Banner Events
    // Raised when an ad is loaded into the banner view.
    private void BannerLoadSucces()
    {
        Debug.Log("Banner Loaded");
        _bannerLoaded = true;
        AdEvents.TriggerBannerLoadSucces();
    }
    // Raised when an ad fails to load into the banner view.
    private void BannerLoadFailed(LoadAdError loadAdError)
    {
        Debug.Log("Admob Banner Failed To Load ");
        DestroyBanner();
        _bannerLoaded = false;
        AdEvents.TriggerBannerLoadFailed(loadAdError.ToString());
    }
    // Raised when the ad is estimated to have earned money.
    private void BannerAdPaied(AdValue adValue)
    {
        Debug.Log(String.Format("Banner view paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
    }
    // Raised when an impression is recorded for an ad.
    private void BannerImpressionRecorded()
    {
        Debug.Log("Banner view recorded an impression.");
    }
    // Raised when a click is recorded for an ad.
    private void BannerClicked()
    {
        Debug.Log("Banner view was clicked.");
        AdEvents.TriggerBannerClicked();
    }
    // Raised when an ad opened full screen content.
    private void BannerFullScreenOpened()
    {
        Debug.Log("Banner view full screen content opened.");
    }
    // Raised when the ad closed full screen content.
    private void BannerFullScreenClose()
    {
        Debug.Log("Banner view full screen content closed.");
    }
    #endregion

    #region Interstitial
    public bool IsInterstitialAvailable()
    {
        if (_interstitialAd != null)
        {
            return _interstitialAd.CanShowAd();
        }
        return false;
    }

    public void ShowInterstitial(UnityAction interstitialClosedCallback)
    {
        if (IsInterstitialAvailable())
        {
            onInterstitialClosed = interstitialClosedCallback;
            _interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad cannot be shown.");
        }
    }

    private void DestroyInterstitial()
    {
        if (_interstitialAd != null)
        {
            Debug.Log("Destroying interstitial ad.");

            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
    }

    private void LoadInterstitial()
    {
        // Clean up the old ad before loading a new one.
        DestroyInterstitial();

        Debug.Log("Loading interstitial ad.");

        var adRequest = GetRequest();

        InterstitialAd.Load(_interstitialId, adRequest, InterstitialLoadCallback);
    }

    private void InterstitialLoadCallback(InterstitialAd ad, LoadAdError error)
    {
        if (error != null || ad == null)
        {
            InterstitialFailed(error);
            return;
        }

        InterstitialLoaded(ad);
    }

    private void InterstitialLoaded(InterstitialAd ad)
    {
        _interstitialAd = ad;

        Debug.Log("Interstitial ad loaded");

        InterstitialRegisterEventHandlers(ad);
        _currentRetryInterstitial = 0;
    }

    private void InterstitialFailed(LoadAdError error)
    {
        //try again to load interstitial
        if (_currentRetryInterstitial < maxRetryCount)
        {
            _currentRetryInterstitial++;
            _interstitialFailedToLoad = true;
        }
    }

    private void InterstitialClosed()
    {
        Debug.Log("Reload Interstitial");

        //trigger complete event
        StartCoroutine(CompleteMethodInterstitial());

        //reload interstitial
        LoadInterstitial();
    }

    private IEnumerator CompleteMethodInterstitial()
    {
        yield return null;
        if (onInterstitialClosed != null)
        {
            onInterstitialClosed();
            onInterstitialClosed = null;
        }
    }

    private void InterstitialRegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            InterstitialClosed();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error : " + error);
            InterstitialClosed();
        };
    }
    #endregion

    #region Rewarded
    public bool IsRewardedVideoAvailable()
    {
        if (_rewardedAd != null)
        {
            return _rewardedAd.CanShowAd();
        }
        return false;
    }

    public void ShowRewardedVideo(UnityAction<bool> CompleteMethod)
    {
        if (IsRewardedVideoAvailable())
        {
            onRewardedVideoClosed = CompleteMethod;
            _rewardedVideoWatched = false;
            _rewardedAd.Show(RewardedVideoWatchedCallback);
        }
        else
        {
            Debug.Log("Rewarded video cannot be shown.");
        }
    }

    private void LoadRewardedAd()
    {
        Debug.Log("Start Loading Rewarded Ad");

        // create our request used to load the ad.
        var adRequest = GetRequest();

        // send the request to load the ad.
        RewardedAd.Load(_rewardedId, adRequest, LoadRewardedVideoCallback);
    }

    private void LoadRewardedVideoCallback(RewardedAd ad, LoadAdError error)
    {
        // if error is not null, the load request failed.
        if (error != null || ad == null)
        {
            RewardedVideoFailed(error);
            return;
        }
        else
        {
            RewardedVideoLoaded(ad);
        }
    }

    private void RewardedVideoFailed(object loadAdError)
    {
        //try again to load a rewarded video
        if (_currentRetryRewardedVideo < maxRetryCount)
        {
            _currentRetryRewardedVideo++;

            Debug.Log("Retry " + _currentRetryRewardedVideo);

            _rewardedVideoFailedToLoad = true;
        }
    }

    private void RewardedVideoLoaded(RewardedAd ad)
    {
        _rewardedAd = ad;
        Debug.Log("Rewarded Video Loaded.");

        RewardedRegisterEventHandlers(ad);

        _currentRetryRewardedVideo = 0;
    }

    private void RewardedVideoWatchedCallback(Reward reward)
    {
        _rewardedVideoWatched = true;
#if UNITY_EDITOR
        RewardedAdClosed();
#endif
    }

    private void RewardedAdClosed()
    {
        Debug.Log("Rewarded Ad Closed");
        //trigger complete method
        StartCoroutine(CompleteMethodRewardedVideo(_rewardedVideoWatched));
    }

    private IEnumerator CompleteMethodRewardedVideo(bool val)
    {
        yield return null;
        if (onRewardedVideoClosed != null)
        {
            onRewardedVideoClosed(val);
            onRewardedVideoClosed = null;
        }

        AdEvents.TriggerRewardedWatchedComplete(val);

        //reload
        LoadRewardedAd();
    }

    private void RewardedRegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(System.String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when the ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
#if !UNITY_EDITOR
            RewardedAdClosed();
#endif
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error : " + error);
            RewardedAdClosed();
        };
    }
    #endregion

    private void Update()
    {
        if (_interstitialFailedToLoad)
        {
            _interstitialFailedToLoad = false;
            Invoke("LoadInterstitial", reloadTime);
        }

        if (_rewardedVideoFailedToLoad)
        {
            _rewardedVideoFailedToLoad = false;
            Invoke("LoadRewardedAd", reloadTime);
        }
    }
}
