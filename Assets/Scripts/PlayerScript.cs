using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
    public Camera PlayerCamera;
	// Use this for initialization
	void Start () {
		if (isLocalPlayer == false)
        {
            PlayerCamera.enabled = false;
            PlayerCamera.GetComponent<AudioListener>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Update");
	}
}
