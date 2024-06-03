using UnityEngine;

namespace Admob
{
    [CreateAssetMenu(fileName = "AdmobSettings", menuName = "Settings/Admob")]
    public class AdmobSettings : ScriptableObject
    {
        public RewardedAds.Settings RewardedAdsSettings;
        public InterstitialAds.Settings InterstitialAdsSettings;
    }
}