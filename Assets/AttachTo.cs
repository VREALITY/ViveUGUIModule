using UnityEngine;
using System.Collections;

public class AttachTo : MonoBehaviour {
    public GameObject controller;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (controller != null )
        {
            this.transform.rotation = controller.transform.rotation;
            this.transform.rotation *= Quaternion.Euler(90, 0, 0);
            this.transform.position = controller.transform.position;
        }
    }
}
