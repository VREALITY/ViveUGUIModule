using UnityEngine;
using System.Collections;

public class VRLinearAudioPitch : MonoBehaviour
{
	public VRLinearMapping linearMapping;
	public AnimationCurve pitchCurve;
	public float minPitch;
	public float maxPitch;
	public bool applyContinuously = true;

	private AudioSource audioSource;

	void Awake()
	{
		if ( audioSource == null )
		{
			audioSource = GetComponent<AudioSource>();
		}

		if ( linearMapping == null )
		{
			linearMapping = GetComponent<VRLinearMapping>();
		}
	}

	void Update()
	{
		if ( applyContinuously )
		{
			Apply();
		}
	}

	public void Apply()
	{
		float y = pitchCurve.Evaluate( linearMapping.value );

		audioSource.pitch = Mathf.Lerp( minPitch, maxPitch, y );
	}
}
