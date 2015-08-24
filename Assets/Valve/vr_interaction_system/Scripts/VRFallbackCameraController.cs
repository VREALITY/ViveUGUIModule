//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;

/// <summary>
/// Maneuvers a camera for no-SteamVR 2D fallback mode.
/// </summary>
[RequireComponent(typeof(Camera))]
public class VRFallbackCameraController : MonoBehaviour
{
	public float speed = 4.0f;
	public float shiftSpeed = 16.0f;
	public bool showInstructions = true;

	private Vector3 startEulerAngles;
	private Vector3 startMousePosition;
	private float realTime;

	void OnEnable()
	{
		realTime = Time.realtimeSinceStartup;
	}

	void Update()
	{
        Quaternion lastrot = this.transform.rotation;

		float forward = 0.0f;
		if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.UpArrow ) )
		{
			forward += 1.0f;
		}
		if ( Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.DownArrow ) )
		{
			forward -= 1.0f;
		}

		float right = 0.0f;
		if ( Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) )
		{
			right += 1.0f;
		}
		if ( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) )
		{
			right -= 1.0f;
		}

        float up = 0.0f;
        if (Input.GetKey(KeyCode.E) )
        {
            up += 1.0f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            up -= 1.0f;
        }


        float currentSpeed = speed;
		if ( Input.GetKey( KeyCode.LeftShift ) || Input.GetKey( KeyCode.RightShift ) )
		{
			currentSpeed = shiftSpeed;
		}

		float realTimeNow = Time.realtimeSinceStartup;
		float deltaRealTime = realTimeNow - realTime;
		realTime = realTimeNow;

		Vector3 delta = new Vector3( right, up, forward ) * currentSpeed * deltaRealTime;

		transform.position += transform.TransformDirection( delta );

		Vector3 mousePosition = Input.mousePosition;
		
		if ((Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0)) || Input.GetMouseButtonDown( 1 ) /* right mouse */)
		{
			startMousePosition = mousePosition;
			startEulerAngles = transform.localEulerAngles;
		}

		if ((Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0)) || Input.GetMouseButton( 1 ) /* right mouse */)
		{
			Vector3 offset = mousePosition - startMousePosition;
			transform.localEulerAngles = startEulerAngles + new Vector3( -offset.y * 360.0f / Screen.height, offset.x * 360.0f / Screen.width, 0.0f );
		}

        this.transform.rotation = Quaternion.Lerp(lastrot, this.transform.rotation, Time.deltaTime * 10f);
	}

	void OnGUI()
	{
		if ( showInstructions )
		{
			GUI.Label( new Rect( 10.0f, 10.0f, 600.0f, 400.0f ),
				"WASD/Arrow Keys to translate the camera\n" +
				"Right mouse click to rotate the camera\n" +
				"Left mouse click for standard interactions.\n" );
		}
	}
}
