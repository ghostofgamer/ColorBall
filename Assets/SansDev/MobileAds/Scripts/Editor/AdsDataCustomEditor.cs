using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AdsData))]
public class AdsDataCustomEditor : ExtendedCustomEditor
{
    public override void OnInspectorGUI()
    {
        AdsData adsData = target as AdsData;

        serializedObject.Update();

        //EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            DrawHeader("Ads Setting");

            if (adsData.controlInterstitial)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUI.BeginDisabledGroup(!adsData.InterstitialEnabled);
                    {
                        EditorGUILayout.HelpBox("'Interstitial Ad Interval' is the number of gameplay count before the next interstitial is shown", MessageType.Info);
                        DrawProperty("_interstitialAdInterval");
                    }
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUI.BeginDisabledGroup(!adsData.InterstitialEnabled);
                        DrawLabel("Minimum Duration Between Interstitial (seconds)");
                        DrawProperty("_minDelayBetweenInterstitial", GUIContent.none);
                    }

                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();
                GUILayout.Space(10);
            }
            if (adsData.controlRewarded)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUI.BeginDisabledGroup(!adsData.RewardedEnabled);
                    {
                        DrawProperty("_rewardedAdFrequency");
                        GUILayout.Space(5);
                    }
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        EditorGUI.BeginDisabledGroup(!adsData.RewardedEnabled);
                        DrawLabel("Minimum Duration Between Rewarded (seconds)");
                        DrawProperty("_minDelayBetweenRewarded", GUIContent.none);
                        EditorGUI.EndDisabledGroup();
                    }

                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(10);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                DrawLabel("Toggle Enable Ads");

                if (!adsData.controlBanner)
                {
                    SerializedProperty adProp = serializedObject.FindProperty("_enableBanner");
                    adProp.boolValue = adsData.controlBanner;
                }
                else
                {
                    DrawProperty("_enableBanner");
                }

                if (!adsData.controlInterstitial)
                {
                    SerializedProperty adProp = serializedObject.FindProperty("_enableInterstitial");
                    adProp.boolValue = adsData.controlInterstitial;
                }
                else
                {
                    DrawProperty("_enableInterstitial");
                }

                if (!adsData.controlRewarded)
                {
                    SerializedProperty adProp = serializedObject.FindProperty("_enableRewarded");
                    adProp.boolValue = adsData.controlRewarded;
                }
                else
                {
                    DrawProperty("_enableRewarded");
                }
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.HelpBox("Uncheck means users are not under age.", MessageType.Info);
                DrawProperty("directedForChildren");
                EditorGUILayout.HelpBox("Enabling test mode will force ads to use the test unit id", MessageType.Info);
                DrawProperty("testMode");
            }

            GUILayout.Space(20);
        }

        //EditorGUILayout.EndVertical();

        DrawHeader("Admob Ad Units (Android)");
        DrawAdmob(adsData);
        GUILayout.Space(10);

        DrawProperty("_iosBuild");
        if (adsData.IOSBuild)
        {
            DrawHeader("Admob Ad Units (iOS)");
            DrawAdmobIOS(adsData);
        }

        GUILayout.Space(10);
        serializedObject.ApplyModifiedProperties();
        DrawWatermark();
    }

    private void DrawAdmob(AdsData data)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);


        if (data.controlBanner)
        {
            EditorGUI.BeginDisabledGroup(!data.BannerEnabled);
            DrawProperty("idBanner");
            EditorGUI.EndDisabledGroup();
        }
        if (data.controlInterstitial)
        {
            EditorGUI.BeginDisabledGroup(!data.InterstitialEnabled);
            DrawProperty("idInterstitial");
            EditorGUI.EndDisabledGroup();
        }
        if (data.controlRewarded)
        {
            EditorGUI.BeginDisabledGroup(!data.RewardedEnabled);
            DrawProperty("idReward");
            EditorGUI.EndDisabledGroup();
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawAdmobIOS(AdsData data)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);


        if (data.controlBanner)
        {
            EditorGUI.BeginDisabledGroup(!data.BannerEnabled);
            DrawProperty("idBannerIOS");
            EditorGUI.EndDisabledGroup();
        }
        if (data.controlInterstitial)
        {
            EditorGUI.BeginDisabledGroup(!data.InterstitialEnabled);
            DrawProperty("idInterstitialIOS");
            EditorGUI.EndDisabledGroup();
        }
        if (data.controlRewarded)
        {
            EditorGUI.BeginDisabledGroup(!data.RewardedEnabled);
            DrawProperty("idRewardIOS");
            EditorGUI.EndDisabledGroup();
        }

        EditorGUILayout.EndVertical();
    }
}
