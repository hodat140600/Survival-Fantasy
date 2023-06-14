using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace Assets._SDK.Ads
{
    public class CappingTimer
    {
        private float _cappingTime;
        private Stopwatch _cappingStopwatch;
        private FirebaseService firebaseService;
        public CappingTimer()
        {
            _cappingTime = AdsConfig.CappingTimeDefaultValue;
            firebaseService = FirebaseService.Instance;

            if (firebaseService.IsConnected)
            {
                _cappingTime = (float)firebaseService.GetRemoteConfigByKey(AdsConfig.CappingTimeRemoteConfigKey).DoubleValue;
            }
            
            _cappingStopwatch = new Stopwatch();
            _cappingStopwatch.Start();
        }


        public bool IsInterstitialCapped()
        {
            return _cappingStopwatch.Elapsed.TotalSeconds > _cappingTime;
        }
        public double Cappingtime => _cappingStopwatch.Elapsed.TotalSeconds;

        public void Restart()
        {
            _cappingStopwatch.Restart();
        }

		public void Start()
		{
			_cappingStopwatch.Start();
		}
		public void Reset()
		{
			_cappingStopwatch.Reset();
		}

		public void Stop()
		{
			_cappingStopwatch.Stop();
		}
	}
}