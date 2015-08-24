//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class VRInputModule : BaseInputModule
{
	private GameObject submitObject;

	private static VRInputModule _instance;
	public static VRInputModule instance
	{
		get
		{
			if ( _instance == null )
				_instance = GameObject.FindObjectOfType<VRInputModule>();

			return _instance;
		}
	}

	public void HoverBegin( GameObject gameObject )
	{
		PointerEventData pointerEventData = new PointerEventData( eventSystem );
		ExecuteEvents.Execute( gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler );
	}

	public void HoverEnd( GameObject gameObject )
	{
		PointerEventData pointerEventData = new PointerEventData( eventSystem );
		ExecuteEvents.Execute( gameObject, pointerEventData, ExecuteEvents.pointerExitHandler );
	}

	public void Submit( GameObject gameObject )
	{
		submitObject = gameObject;
	}

	public override void Process()
	{
		if ( submitObject )
		{
			BaseEventData data = GetBaseEventData();
			data.selectedObject = submitObject;
			ExecuteEvents.Execute( submitObject, data, ExecuteEvents.submitHandler );

			submitObject = null;
		}
	}
}
