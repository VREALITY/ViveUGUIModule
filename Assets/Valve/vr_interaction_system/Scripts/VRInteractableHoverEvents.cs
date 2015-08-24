//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent( typeof( VRInteractable ) )]
public class VRInteractableHoverEvents : MonoBehaviour
{
	public UnityEvent onHandHoverBegin;
	public UnityEvent onHandHoverEnd;

	void OnHandHoverBegin()
	{
		onHandHoverBegin.Invoke();
	}

	void OnHandHoverEnd()
	{
		onHandHoverEnd.Invoke();
	}
}
