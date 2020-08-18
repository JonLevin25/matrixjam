using UnityEngine;

namespace MatrixJam.Team17
{
	[System.Serializable]
	public class PID
	{
		[Tooltip("Proportional factor")]
		public float pFactor;
		[Tooltip("Integral factor")]
		public float iFactor;
		[Tooltip("Derivative factor")]
		public float dFactor;

		[Header("Debug")]
		public float integral;
		public float lastError;

		public void Reset()
		{
			integral = lastError = 0.0f;
		}

		public void Set(float pFactor, float iFactor, float dFactor)
		{
			this.pFactor = pFactor;
			this.iFactor = iFactor;
			this.dFactor = dFactor;
		}

		public float Update(float setpoint, float actual, float timeFrame)
		{
			float present = setpoint - actual;
			integral += present * timeFrame;
			float deriv = (present - lastError) / timeFrame;
			lastError = present;
			return present * pFactor + integral * iFactor + deriv * dFactor;
		}
	}
}