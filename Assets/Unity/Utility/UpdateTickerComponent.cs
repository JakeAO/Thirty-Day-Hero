using System;
using UnityEngine;

namespace Unity.Utility
{
    public class UpdateTickerComponent : MonoBehaviour
    {
        public event Action<float> DeltaTimeTick;
        public event Action<float> SmoothDeltaTimeTick;
        public event Action<float> UnscaledDeltaTimeTick;
        public event Action<float> FixedDeltaTimeTick;

        private void Update()
        {
            DeltaTimeTick?.Invoke(Time.deltaTime);
            SmoothDeltaTimeTick?.Invoke(Time.smoothDeltaTime);
            UnscaledDeltaTimeTick?.Invoke(Time.unscaledDeltaTime);
        }

        private void FixedUpdate()
        {
            FixedDeltaTimeTick?.Invoke(Time.fixedDeltaTime);
        }
    }
}