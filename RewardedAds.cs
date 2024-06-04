using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace Admob
{
    public class RewardedAds
    {
        [Serializable]
        public class Settings
        {
            public string TestAdUnitId;
            public string AdUnitId;
        }

        private string _adUnitId = "unused";
        private RewardedAd _rewardedAd;

        public RewardedAds(string adUnitId)
        {
            Debug.Log($"Rewarded Init. adUnitId : {adUnitId}");
            _adUnitId = adUnitId;
        }

        /// <summary>
        /// Loads the rewarded ad.
        /// </summary>
        public void LoadAd()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                    LoadAd();
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                _rewardedAd = ad;
                RegisterEventHandlers(_rewardedAd);
            });
        }

        public void ShowAd(Action callback = null)
        {
            Debug.Log($"ShowRewardedAd start");

            if(_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) => 
                {
                    // TODO: Reward the user.
                    Debug.Log($"Rewarded ad rewarded the user. Type: {reward.Type}, amount: {reward.Amount}.");
                    callback?.Invoke();
                });
            }
        }

        private void RegisterEventHandlers(RewardedAd ad)
        {
            // Raised when the ad is estimated to have earned money.
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log($"Rewarded ad paid {adValue.Value}, {adValue.CurrencyCode}.");
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

            // Raised when an ad opened full screen content.
            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };

            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded Ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadAd();
            };

            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError($"Rewarded ad failed to open full screen content with error : {error}");

                // Reload the ad so that we can show another as soon as possible.
                LoadAd();
            };
        }
    }
}