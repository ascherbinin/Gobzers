using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log ("ENTER");
	}
}
