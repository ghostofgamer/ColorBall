using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class RateUs : MonoBehaviour
{
    [SerializeField] int _countToRate = 3;

    int _playCount;

    static bool _rateOff = false;

    public void SetRateOff(bool value)
    {
        _rateOff = value;
    }

    public void ClickPlay()
    {
        _playCount++;
        if(_playCount % _countToRate == 0 && !_rateOff)
        {
#if UNITY_IOS
            Device.RequestStoreReview();
#else
            // open rate menu panel
#endif
        }
    }

    public void RateNow()
    {
#if UNITY_ANDROID

#if UNITY_EDITOR
        Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}");
#else
        Application.OpenURL($"market://details?id={Application.identifier}");
#endif
#endif
#if UNITY_IOS
            Device.RequestStoreReview();
#endif
    }
}
