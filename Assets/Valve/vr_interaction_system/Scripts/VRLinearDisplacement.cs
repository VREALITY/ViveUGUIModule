//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;

public class VRLinearDisplacement : MonoBehaviour
{
	public Vector3 displacement;
	public VRLinearMapping linearMapping;

	private Vector3 initialPosition;

	void Start()
	{
		initialPosition = transform.localPosition;

		if ( linearMapping == null )
		{
			linearMapping = GetComponent<VRLinearMapping>();
		}
	}

	void Update()
	{
		if ( linearMapping )
		{
			transform.localPosition = initialPosition + linearMapping.value * displacement;
		}
	}
}
