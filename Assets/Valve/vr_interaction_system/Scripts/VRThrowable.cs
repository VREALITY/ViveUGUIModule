//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;

[RequireComponent( typeof( VRInteractable ) )]
[RequireComponent( typeof( Rigidbody ) )]
[RequireComponent( typeof( VelocityEstimator ) )]
public class VRThrowable : MonoBehaviour
{
	public bool snapOnAttach = false;
	public string attachmentPoint;
	public bool detachOthers = false;
	public float catchSpeedThreshold = 0.0f;

	private VelocityEstimator velocityEstimator;
	private bool attached = false;

	void Awake()
	{
		velocityEstimator = GetComponent<VelocityEstimator>();
	}

	void OnHandHoverBegin( VRHand hand )
	{
		// "Catch" the throwable by holding down the interaction button instead of pressing it.
		// Only do this if the throwable is moving faster than the prescribed threshold speed,
		// and if it isn't attached to another hand
		if ( !attached )
		{
			if ( hand.GetStandardInteractionButton() )
			{
				Rigidbody rb = GetComponent<Rigidbody>();
				if ( rb.velocity.magnitude >= catchSpeedThreshold )
				{
					hand.AttachObject( gameObject, snapOnAttach, attachmentPoint, detachOthers );
				}
			}
		}
	}

	void HandHoverUpdate( VRHand hand )
	{
		//Trigger got pressed
		if ( hand.GetStandardInteractionButtonDown() )
		{
			hand.AttachObject( gameObject, snapOnAttach, attachmentPoint, detachOthers );
		}
	}

	void OnAttachedToHand( VRHand hand )
	{
		attached = true;

		hand.HoverLock( null );
		
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.isKinematic = true;
		rb.interpolation = RigidbodyInterpolation.None;

		velocityEstimator.BeginEstimatingVelocity();
	}

	void OnDetachedFromHand( VRHand hand )
	{
		attached = false;

		hand.HoverUnlock( null );

		Rigidbody rb = GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.interpolation = RigidbodyInterpolation.Interpolate;

		velocityEstimator.FinishEstimatingVelocity();
		rb.velocity = velocityEstimator.GetVelocityEstimate();
		rb.angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
	}

	void HandAttachedUpdate( VRHand hand )
	{
		//Trigger got released
		if ( hand.GetStandardInteractionButtonUp() )
		{
			hand.DetachObject( gameObject );
		}
	}
}
