//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;

[RequireComponent( typeof( VRInteractable ) )]
public class VRGUIElement : MonoBehaviour
{
	void OnHandHoverBegin( VRHand hand )
	{
		VRInputModule.instance.HoverBegin( gameObject );
	}

	void OnHandHoverEnd( VRHand hand )
	{
		VRInputModule.instance.HoverEnd( gameObject );
	}

	void HandHoverUpdate( VRHand hand )
	{
		if ( hand.GetStandardInteractionButtonDown() )
		{
			VRInputModule.instance.Submit( gameObject );
		}
	}
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor( typeof( VRGUIElement ) )]
public class VRGUIElementEditor : UnityEditor.Editor
{
	// Custom Inspector GUI allows us to click from within the UI
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		VRGUIElement vrGuiElement = ( VRGUIElement )target;
		if ( GUILayout.Button( "Click" ) )
		{
			VRInputModule.instance.Submit( vrGuiElement.gameObject );
		}
	}
}
#endif
