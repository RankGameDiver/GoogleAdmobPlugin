
using System;
using UnityEngine;

namespace Admob
{
    public class GoogleAdmobManager : MonoSingleton<GoogleAdmobManager>
    {
        private RewardedAds _rewardedAds;
        private InterstitialAds _interstitialAds;
        private AdmobSettings _settings;

        private void Start()
        {
            _settings = Resources.Load<AdmobSettings>("Settings/AdmobSetting");

#if DEBUG_ADMOB
            _rewardedAds = new RewardedAds(_settings.RewardedAdsSettings.TestAdUnitId);
            _interstitialAds = new InterstitialAds(_settings.InterstitialAdsSettings.TestAdUnitId);
#else
            _rewardedAds = new RewardedAds(_settings.RewardedAdsSettings.AdUnitId);
            _interstitialAds = new InterstitialAds(_settings.InterstitialAdsSettings.AdUnitId);
#endif

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