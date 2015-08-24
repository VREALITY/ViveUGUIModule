//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;

[RequireComponent( typeof( VRInteractable ) )]
public class VRLinearDrive : MonoBehaviour
{
	public Transform startPosition;
	public Transform endPosition;
	public VRLinearMapping linearMapping;
	public bool repositionGameObject = true;

	void Start()
	{
		if ( linearMapping == null )
		{
			linearMapping = GetComponent<VRLinearMapping>();
		}

		if ( linearMapping == null )
		{
			linearMapping = gameObject.AddComponent<VRLinearMapping>();
		}

		if ( repositionGameObject )
		{
			UpdateLinearMapping( transform );
		}
	}

	void HandHoverUpdate( VRHand hand )
	{
		if ( hand.currentAttachedObject )
			return;

		if ( hand.GetStandardInteractionButtonDown() )
		{
			hand.HoverLock( GetComponent<VRInteractable>() );
		}

		if ( hand.GetStandardInteractionButtonUp() )
		{
			hand.HoverUnlock( GetComponent<VRInteractable>() );
		}

		if ( hand.GetStandardInteractionButton() )
		{
			UpdateLinearMapping( hand.transform );
		}
	}

	void UpdateLinearMapping( Transform tr )
	{
		Vector3 direction = endPosition.position - startPosition.position;
		float length = direction.magnitude;
		direction.Normalize();

		Vector3 displacement = tr.position - startPosition.position;

		float pull = Mathf.Clamp01( Vector3.Dot( displacement, direction ) / length );

		linearMapping.value = pull;

		if ( repositionGameObject )
		{
			transform.position = Vector3.Lerp( startPosition.position, endPosition.position, pull );
		}
	}
}
