//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;

public class VelocityEstimator : MonoBehaviour
{
	[Tooltip( "How many frames to average over for computing velocity" )]
	public int velocityAverageFrames = 5;
	[Tooltip( "How many frames to average over for computing angular velocity" )]
	public int angularVelocityAverageFrames = 11;

	private Coroutine routine;
	private int sampleCount;
	private Vector3[] velocitySamples;
	private Vector3[] angularVelocitySamples;

	public Vector3 estimatedVelocity { get; private set; }
	public Vector3 estimatedAngularVelocity { get; private set; }

	public void BeginEstimatingVelocity()
	{
		FinishEstimatingVelocity();
		
		routine = StartCoroutine( EstimateVelocityCoroutine() );
	}

	public void FinishEstimatingVelocity()
	{
		if ( routine != null )
		{
			StopCoroutine( routine );
			routine = null;
		}
	}

	public Vector3 GetVelocityEstimate()
	{
		// Compute average velocity
		Vector3 velocity = Vector3.zero;
		int velocitySampleCount = Mathf.Min( sampleCount, velocitySamples.Length );
		for ( int i = 0; i < velocitySampleCount; i++ )
		{
			velocity += velocitySamples[i];
		}
		velocity *= ( 1.0f / velocitySampleCount );
		return velocity;
	}

	public Vector3 GetAngularVelocityEstimate()
	{
		// Compute average angular velocity
		Vector3 angularVelocity = Vector3.zero;
		int angularVelocitySampleCount = Mathf.Min( sampleCount, angularVelocitySamples.Length );
		for ( int i = 0; i < angularVelocitySampleCount; i++ )
		{
			angularVelocity += angularVelocitySamples[i];
		}
		angularVelocity *= ( 1.0f / angularVelocitySampleCount );
		return angularVelocity;
	}

	public Vector3 GetAccelerationEstimate()
	{
		Vector3 average = Vector3.zero;
		for ( int i = 2 + sampleCount - velocitySamples.Length; i < sampleCount; i++ )
		{
			if ( i < 2 )
				continue;

			int first = i - 2;
			int second = i - 1;

			Vector3 v1 = velocitySamples[first % velocitySamples.Length];
			Vector3 v2 = velocitySamples[second % velocitySamples.Length];
			average += v2 - v1;
		}
		average *= ( 1.0f / Time.deltaTime );
		return average;
	}

	void Awake()
	{
		velocitySamples = new Vector3[velocityAverageFrames];
		angularVelocitySamples = new Vector3[angularVelocityAverageFrames];
	}

#if DEBUG
	void Start()
	{
		const float idealFixedTimestep = 0.012f;
		if ( Time.fixedDeltaTime > idealFixedTimestep )
		{
			Debug.LogWarning( string.Format(
				"VelocityEstimator: Time.fixedDeltaTime {0} exceeds recommendation for VR {1} physics interactions.",
				Time.fixedDeltaTime,
				idealFixedTimestep ) );
		}
	}
#endif

	IEnumerator EstimateVelocityCoroutine()
	{
		sampleCount = 0;

		Vector3 previousPosition = transform.position;
		Quaternion previousRotation = transform.rotation;

		float velocityFactor = 1.0f / Time.fixedDeltaTime;
		float angularVelocityFactor = Mathf.Deg2Rad / Time.fixedDeltaTime;

		while ( true )
		{
			yield return new WaitForFixedUpdate();

			int v = sampleCount % velocitySamples.Length;
			int w = sampleCount % angularVelocitySamples.Length;
			sampleCount++;

			// Estimate linear velocity
			velocitySamples[v] = velocityFactor * ( transform.position - previousPosition );

			// Estimate angular velocity
			float angle;
			Vector3 axis;
			Quaternion deltaRotation = transform.rotation * Quaternion.Inverse( previousRotation );
			deltaRotation.ToAngleAxis( out angle, out axis );
			angularVelocitySamples[w] = angle * axis * angularVelocityFactor;

			previousPosition = transform.position;
			previousRotation = transform.rotation;
		}
	}
}
