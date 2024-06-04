
using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Admob
{
    public class GoogleAdmobManager : MonoSingleton<GoogleAdmobManager>
    {
        private RewardedAds _rewardedAds;
        private InterstitialAds _interstitialAds;
        private AdmobSettings _settings;

        private void Awake()
        {
            _settings = Resources.Load<AdmobSettings>("Settings/AdmobSettings");

#if DEBUG_ADMOB
            string rewardedAdsUnitId = _settings.RewardedAdsSettings.TestAdUnitId;
            string interstitialAdsUnitId = _settings.InterstitialAdsSettings.TestAdUnitId;
#else
            string rewardedAdsUnitId = _settings.RewardedAdsSettings.AdUnitId;
            string interstitialAdsUnitId = _settings.InterstitialAdsSettings.AdUnitId;
#endif

            _rewardedAds = new RewardedAds(rewardedAdsUnitId);
            _interstitialAds = new InterstitialAds(interstitialAdsUnitId);

            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                // This callback is called once the MobileAds SDK is initialized.
                Debug.Log($"MobileAds.Initialize rewarded and interstitial");
                _rewardedAds.LoadAd();
                _interstitialAds.LoadAd();
            });

            DontDestroyOnLoad(this);
        }

        public void ShowRewardedAd(Action callback = null)
        {
            _rewardedAds.ShowAd(callback);
        }

        public void ShowInterstitialAd()
        {
            _interstitialAds.ShowAd();
        }
    }
}