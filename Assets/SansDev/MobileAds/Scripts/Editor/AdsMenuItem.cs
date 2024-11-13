using UnityEditor;

public static class AdsMenuItem
{
    [MenuItem("SansDev/Customize/Ads Data")]
    static void OpenAdsData()
    {
        string path = "Assets/SansDev/MobileAds/ScriptableObject/Ads Data.asset";
        AdsData data = (AdsData)AssetDatabase.LoadAssetAtPath(path, typeof(AdsData));
        Selection.activeObject = data;
    }
}
