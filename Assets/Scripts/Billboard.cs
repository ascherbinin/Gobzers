using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour 
{
	public Camera PlayerCamera;
	void Update () 
	{
		transform.LookAt(PlayerCamera.transform);
	}

}
