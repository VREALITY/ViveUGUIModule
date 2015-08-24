//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent( typeof( VRInteractable ) )]
public class VRHapticRack : MonoBehaviour
{
	[Tooltip("The linear mapping driving the haptic rack")]
	public VRLinearMapping linearMapping;

	[Tooltip( "The number of haptic pulses evenly distributed along the mapping" )]
	public int teethCount = 128;

	[Tooltip( "Minimum duration of the haptic pulse" )]
	public int minimumPulseDuration = 500;

	[Tooltip( "Maximum duration of the haptic pulse" )]
	public int maximumPulseDuration = 900;

	[Tooltip( "This event is triggered every time a haptic pulse is made" )]
	public UnityEvent onPulse;

	private VRHand hand;
	private int previousToothIndex = -1;

	void Awake()
	{
		if ( linearMapping == null )
		{
			linearMapping = GetComponent<VRLinearMapping>();
		}
	}

	void OnHandHoverBegin( VRHand hand )
	{
		this.hand = hand;
	}

	void OnHandHoverEnd( VRHand hand )
	{
		this.hand = null;
	}

	void Update()
	{
		int currentToothIndex = Mathf.RoundToInt( linearMapping.value * teethCount - 0.5f );
		if ( currentToothIndex != previousToothIndex )
		{
			Pulse();
			previousToothIndex = currentToothIndex;
		}
	}

	private void Pulse()
	{
		if ( hand && ( hand.controller != null ) && ( hand.GetStandardInteractionButton() ) )
		{
			ushort duration = ( ushort )Random.Range( minimumPulseDuration, maximumPulseDuration + 1 );
			hand.controller.TriggerHapticPulse( duration );

			onPulse.Invoke();
		}
	}
}
