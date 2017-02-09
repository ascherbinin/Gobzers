using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameGetComponent<Collider2D>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		var hit = collision.gameObject;
		var health = hit.GetComponent<Health> ();
		if (health != null) 
		{
			health.TakeDamage (10);
		}
		Destroy (gameObject);
	}
}
