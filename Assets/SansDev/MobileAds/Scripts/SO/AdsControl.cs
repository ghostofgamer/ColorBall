using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ads Control", fileName = "Ads Control")]
public class AdsControl : ScriptableObject
{
    [SerializeField] AdsData _adsData;

    [Space]
    [SerializeField] bool _bannerEnabled;
    [SerializeField] bool _interstitialEnabled;
    [SerializeField] bool _rewardedEnabled;

#pragma warning disable
    [Space]
    [SerializeField][TextArea] string _admobTestAppID = "ca-app-pub-3940256099942544~3347511713";
#pragma warning restore

    private void OnValidate()
    {
        if (_adsData == null)
        {
            Debug.LogWarning("Ads Data References not set in the Inspector!");
        }
        else
        {
            _adsData.controlBanner = _bannerEnabled;
            _adsData.controlInterstitial = _interstitialEnabled;
            _adsData.controlRewarded = _rewardedEnabled;
        }

    }
}
