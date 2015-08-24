//===================== Copyright (c) Valve Corporation. All Rights Reserved. ======================

using UnityEngine;
using System.Collections;

public class VRLinearAnimator : MonoBehaviour
{
	public VRLinearMapping linearMapping;
	public Animator animator;

	private float currentLinearMapping;
	
	void Awake()
	{
		if ( animator == null )
		{
			animator = GetComponent<Animator>();
		}
		
		if ( linearMapping == null )
		{
			linearMapping = GetComponent<VRLinearMapping>();
		}
	}

	void Update()
	{
		if ( currentLinearMapping != linearMapping.value )
		{
			currentLinearMapping = linearMapping.value;
			animator.enabled = true;
			animator.Play( 0, 0, currentLinearMapping );
		}
		else
		{
			animator.enabled = false;
		}
		
	}
}
