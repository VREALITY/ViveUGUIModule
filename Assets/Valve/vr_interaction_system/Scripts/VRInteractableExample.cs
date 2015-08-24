//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VRInteractable))]
public class VRInteractableExample : MonoBehaviour
{
	private TextMesh textMesh;
	private Vector3 oldPosition;
	private Quaternion oldRotation;

	void Awake()
	{
		textMesh = GetComponentInChildren<TextMesh>();
		textMesh.text = "No Hand Hovering";
	}

	/// <summary>
	/// Called when a VRHand starts hovering over me.
	/// </summary>
	/// <param name="hand"></param>
	void OnHandHoverBegin( VRHand hand )
	{
		textMesh.text = "Hovering hand: " + hand.name;
	}

	/// <summary>
	/// Called when a VRHand stops hovering over me.
	/// </summary>
	/// <param name="hand"></param>
	void OnHandHoverEnd( VRHand hand )
	{
		textMesh.text = "No Hand Hovering";
	}

	/// <summary>
	/// Called every Update() while a VRHand is hovering over me.
	/// </summary>
	/// <param name="hand"></param>
	void HandHoverUpdate( VRHand hand )
	{
		if ( hand.GetStandardInteractionButtonDown() || ( ( hand.controller != null ) && hand.controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_Grip ) ) )
		{
			if ( hand.currentAttachedObject != gameObject )
			{
				// Save our position/rotation so that we can restore it when we detach
				oldPosition = transform.position;
				oldRotation = transform.rotation;

				// Call this to continue receiving HandHoverUpdate messages,
				// and prevent the hand from hovering over anything else
				hand.HoverLock( GetComponent<VRInteractable>() );

				// Attach this object to the hand
				hand.AttachObject( gameObject, false );
			}
			else
			{
				// Detach this object from the hand
				hand.DetachObject( gameObject );

				// Call this to undo HoverLock
				hand.HoverUnlock( GetComponent<VRInteractable>() );

				// Restore position/rotation
				transform.position = oldPosition;
				transform.rotation = oldRotation;
			}
		}
	}

	/// <summary>
	/// Called when our GameObject becomes attached to the hand
	/// </summary>
	/// <param name="hand"></param>
	void OnAttachedToHand( VRHand hand )
	{
		textMesh.text = "Attached to hand: " + hand.name;
	}

	/// <summary>
	/// Called when our GameObject detaches from the hand
	/// </summary>
	/// <param name="hand"></param>
	void OnDetachedFromHand( VRHand hand )
	{
		textMesh.text = "Detached from hand: " + hand.name;
	}

	/// <summary>
	/// Called every Update() while our GameObject is attached to the hand
	/// </summary>
	/// <param name="hand"></param>
	void HandAttachedUpdate( VRHand hand )
	{

	}

	/// <summary>
	/// Called when our attached GameObject becomes the primary attached object
	/// </summary>
	/// <param name="hand"></param>
	void OnHandFocusAcquired( VRHand hand )
	{

	}

	/// <summary>
	/// Called when another attached GameObject becomes the primary attached object
	/// </summary>
	/// <param name="hand"></param>
	void OnHandFocusLost( VRHand hand )
	{

	}


}
