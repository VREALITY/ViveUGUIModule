//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Singleton representing the local VR player/user, with methods for getting
/// the player's hands, head, tracking origin, and guesses for various properties.
/// </summary>
public class VRPlayer : MonoBehaviour
{
	[Tooltip( "Virtual transform corresponding to the meatspace tracking origin. Devices are tracked relative to this." )]
	public Transform trackingOriginTransform;

	[Tooltip( "List of possible transforms for the head/HMD, including the no-SteamVR fallback camera." )]
	public Transform[] hmdTransforms;

	[Tooltip( "List of possible VRHands, including no-SteamVR fallback VRHands." )]
	public VRHand[] hands;

	[Tooltip( "These objects are enabled when SteamVR is available" )]
	public GameObject rigSteamVR;

	[Tooltip( "These objects are enabled when SteamVR is not available, or when the user toggles out of VR" )]
	public GameObject rig2DFallback;

	[Tooltip( "The audio listener for this player" )]
	public Transform audioListener;

	public bool allowToggleTo2D = true;

	/// <summary>
	/// Singleton instance of the VRPlayer. Only one can exist at a time.
	/// </summary>
	public static VRPlayer instance { get; private set; }

	/// <summary>
	/// Get the number of active VRHands.
	/// </summary>
	public int handCount
	{
		get
		{
			int count = 0;
			for ( int i = 0; i < hands.Length; i++ )
			{
				if ( hands[i].gameObject.activeInHierarchy )
				{
					count++;
				}
			}
			return count;
		}
	}

	/// <summary>
	/// Get the i-th active VRHand.
	/// </summary>
	/// <param name="i">Zero-based index of the active VRHand to get</param>
	/// <returns>The ith active VRHand</returns>
	/// <seealso cref="handCount"/>
	public VRHand GetHand( int i )
	{
		for ( int j = 0; j < hands.Length; j++ )
		{
			if ( !hands[j].gameObject.activeInHierarchy )
			{
				continue;
			}

			if ( i > 0 )
			{
				i--;
				continue;
			}

			return hands[j];
		}

		return null;
	}

	/// <summary>
	/// Get the HMD transform. This might return the fallback camera transform if SteamVR is unavailable or disabled.
	/// </summary>
	public Transform hmdTransform
	{
		get
		{
			for ( int i = 0; i < hmdTransforms.Length; i++ )
			{
				if ( hmdTransforms[i].gameObject.activeInHierarchy )
					return hmdTransforms[i];
			}
			return null;
		}
	}

	/// <summary>
	/// Height of the eyes above the ground - useful for estimating player height.
	/// </summary>
	public float eyeHeight
	{
		get
		{
			Transform hmd = hmdTransform;
			if ( hmd )
			{
				Vector3 eyeOffset = Vector3.Project( hmd.position - trackingOriginTransform.position, trackingOriginTransform.up );
				return eyeOffset.magnitude / trackingOriginTransform.lossyScale.x;
			}
			return 0.0f;
		}
	}

	/// <summary>
	/// Guess for the world-space position of the player's feet, directly beneath the HMD.
	/// </summary>
	public Vector3 feetPositionGuess
	{
		get
		{
			Transform hmd = hmdTransform;
			if ( hmd )
			{
				return trackingOriginTransform.position + Vector3.ProjectOnPlane( hmd.position - trackingOriginTransform.position, trackingOriginTransform.up );
			}
			return trackingOriginTransform.position;
		}
	}

	/// <summary>
	/// Guess for the world-space direction of the player's hips/torso. This is effectively just the gaze direction projected onto the floor plane.
	/// </summary>
	public Vector3 bodyDirectionGuess
	{
		get
		{
			Transform hmd = hmdTransform;
			if ( hmd )
			{
				Vector3 direction = Vector3.ProjectOnPlane( hmd.forward, trackingOriginTransform.up );
				if ( Vector3.Dot( hmd.up, trackingOriginTransform.up ) < 0.0f )
				{
					// The HMD is upside-down. Either
					// -The player is bending over backwards
					// -The player is bent over looking through their legs
					direction = -direction;
				}
				return direction;
			}
			return trackingOriginTransform.forward;
		}
	}

	void Awake()
	{
		if ( trackingOriginTransform == null )
		{
			trackingOriginTransform = this.transform;
		}
	}

	void OnEnable()
	{
		if ( instance == null )
		{
			instance = this;
		}

		if ( SteamVR.instance != null )
		{
			ActivateRig( rigSteamVR );
		}
		else
		{
			ActivateRig( rig2DFallback );
		}
	}

	void OnDisable()
	{
		if ( instance == this )
		{
			instance = null;
		}
	}

	void OnDrawGizmos()
	{
		if ( this != instance )
		{
			return;
		}

		Gizmos.color = Color.green;
		Gizmos.DrawIcon( feetPositionGuess, "vr_interaction_system_feet.png" );

		Gizmos.color = Color.cyan;
		Gizmos.DrawLine( feetPositionGuess, feetPositionGuess + trackingOriginTransform.up * eyeHeight );

		// Body direction arrow
		Gizmos.color = Color.blue;
		Vector3 bodyDirection = bodyDirectionGuess;
		Vector3 bodyDirectionTangent = Vector3.Cross( trackingOriginTransform.up, bodyDirection );
		Vector3 startForward = feetPositionGuess + trackingOriginTransform.up * eyeHeight * 0.75f;
		Vector3 endForward = startForward + bodyDirection * 0.33f;
		Gizmos.DrawLine( startForward, endForward );
		Gizmos.DrawLine( endForward, endForward - 0.033f * ( bodyDirection + bodyDirectionTangent ) );
		Gizmos.DrawLine( endForward, endForward - 0.033f * ( bodyDirection - bodyDirectionTangent ) );

		Gizmos.color = Color.red;
		int count = handCount;
		for ( int i = 0; i < count; i++ )
		{
			VRHand hand = GetHand( i );

			if ( hand.startingHandType == VRHand.HandType.Left )
			{
				Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_left_hand.png" );
			}
			else if ( hand.startingHandType == VRHand.HandType.Right )
			{
				Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_right_hand.png" );
			}
			else
			{
				VRHand.HandType guessHandType = hand.GuessCurrentHandType();

				if ( guessHandType == VRHand.HandType.Left )
				{
					Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_left_hand_question.png" );
				}
				else if ( guessHandType == VRHand.HandType.Right )
				{
					Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_right_hand_question.png" );
				}
				else
				{
					Gizmos.DrawIcon( hand.transform.position, "vr_interaction_system_unknown_hand.png" );
				}
			}

			Gizmos.DrawIcon( hand.transform.position, "right_hand_question.png" );
		}
	}

	void OnGUI()
	{
		if ( !allowToggleTo2D )
			return;

		if ( !SteamVR.active )
			return;

		int width = 100;
		int height = 25;
		int left = Screen.width / 2 - width / 2;
		int top = Screen.height - height - 10;

		string text = ( rigSteamVR.activeSelf ) ? "2D Debug" : "VR";

		if ( GUI.Button( new Rect( left, top, width, height ), text ) )
		{
			if ( rigSteamVR.activeSelf )
			{
				ActivateRig( rig2DFallback );
			}
			else
			{
				ActivateRig( rigSteamVR );
			}
		}
	}

	private void ActivateRig( GameObject rig )
	{
		rigSteamVR.SetActive( rig == rigSteamVR );
		rig2DFallback.SetActive( rig == rig2DFallback );

		if ( audioListener )
		{
			audioListener.transform.parent = hmdTransform;
			audioListener.transform.localPosition = Vector3.zero;
			audioListener.transform.localRotation = Quaternion.identity;
		}
	}
}
