//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using UnityEngine.Events;

//Sends simple controller button events to UnityEvents
[RequireComponent( typeof( VRInteractable ) )]
public class VRInteractableButtonEvents : MonoBehaviour
{
	public UnityEvent onTriggerDown;
	public UnityEvent onTriggerUp;
	public UnityEvent onGripDown;
	public UnityEvent onGripUp;
	public UnityEvent onTouchpadDown;
	public UnityEvent onTouchpadUp;
	public UnityEvent onTouchpadTouch;
	public UnityEvent onTouchpadRelease;
	
	void Start ()
	{
	
	}

	void HandHoverUpdate ( VRHand hand )
	{
		if ( hand.controller != null )
		{
			SteamVR_Controller.Device controller = hand.controller;

			if ( controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger ) )
			{
				onTriggerDown.Invoke();
			}

			if ( controller.GetPressUp( Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger ) )
			{
				onTriggerUp.Invoke();
			}

			if ( controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_Grip ) )
			{
				onGripDown.Invoke();
			}

			if ( controller.GetPressUp( Valve.VR.EVRButtonId.k_EButton_Grip ) )
			{
				onGripUp.Invoke();
			}

			if ( controller.GetPressDown( Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad ) )
			{
				onTouchpadDown.Invoke();
			}

			if ( controller.GetPressUp( Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad ) )
			{
				onTouchpadUp.Invoke();
			}

			if ( controller.GetTouchDown( Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad ) )
			{
				onTouchpadTouch.Invoke();
			}

			if ( controller.GetTouchUp( Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad ) )
			{
				onTouchpadRelease.Invoke();
			}
		}
	}
}
