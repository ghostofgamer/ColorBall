
public class AdSettings
{
    public string appId;
    public string idBanner;
    public string idInterstitial;
    public string idRewarded;

    public bool testMode;
    public bool directedForChildren;

    public AdSettings(string idBanner, string idInterstitial, string idRewarded, bool testMode, bool directedFirChildren)
    {
        this.idBanner = idBanner;
        this.idInterstitial = idInterstitial;
        this.idRewarded = idRewarded;
        this.testMode = testMode;
        this.directedForChildren = directedFirChildren;
    }
}
