using UnityEngine;
using GoogleMobileAds.Api;
using System;

namespace Admob
{   
    public class InterstitialAds
    {
        [Serializable]
        public class Settings
        {
            public string TestAdUnitId;
            public string AdUnitId;
        }

        private string _adUnitId = "unused";
        private InterstitialAd _interstitialAd;

        public InterstitialAds(string adUnitId)
        {
            AdmobSettings _admobSettings = Resources.Load<AdmobSettings>("Settings/AdmobSetting");
            _adUnitId = adUnitId;

            Debug.Log($"Interstitial Init. adUnitId : {_adUnitId}");
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                // This callback is called once the MobileAds SDK is initialized.
                Debug.Log($"MobileAds.Initialize interstitial");
                LoadAd();
            });
        }

        /// <summary>
        /// Loads the interstitial ad.
        /// </summary>
        public void LoadAd()
        {
            // Clean up the old ad before loading a new one.
            if (_interstitialAd != null)
            {
                    _interstitialAd.Destroy();
                    _interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad with error : " + error);
                    LoadAd();
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
                _interstitialAd = ad;
                RegisterEventHandlers(_interstitialAd);
            });
        }

        /// <summary>
        /// Shows the interstitial ad.
        /// </summary>
        public void ShowAd()
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                _interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        }

        private void RegisterEventHandlers(InterstitialAd interstitialAd)
        {
            // Raised when the ad is estimated to have earned money.
            interstitialAd.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log($"Interstitial ad paid {adValue.Value}, {adValue.CurrencyCode}.");
            };

            // Raised when an impression is recorded for an ad.
            interstitialAd.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Interstitial ad recorded an impression.");
            };

            // Raised when a click is recorded for an ad.
            interstitialAd.OnAdClicked += () =>
            {
                Debug.Log("Interstitial ad was clicked.");
            };

            // Raised when an ad opened full screen content.
            interstitialAd.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Interstitial ad full screen content opened.");
            };

            // Raised when the ad closed full screen content.
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial ad full screen content closed.");

                // Reload the ad so that we can show another as soon as possible.
                LoadAd();
            };

            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content with error : " + error);

                // Reload the ad so that we can show another as soon as possible.
                LoadAd();
            };
        }
    }
}