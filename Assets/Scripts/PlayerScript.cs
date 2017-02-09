using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gobzers
{
	public class PlayerScript : NetworkBehaviour
	{
	    public Camera PlayerCamera;
		public GameObject bulletPrefab;
		public Transform bulletSpawn;
		// Use this for initialization
		void Start ()
		{
			if (!isLocalPlayer)
	        {
	            PlayerCamera.enabled = false;
	            PlayerCamera.GetComponent<AudioListener>().enabled = false;
	        }
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (!isLocalPlayer)
			{
				return;
			}
	        //Debug.Log("Update");
			if (Input.GetButtonDown("Fire1"))
			{
				CmdFire();
			}
		}

		[Command]
		void CmdFire()
		{
			var mousePos = Input.mousePosition;
			mousePos.z = 5.23F; //The distance between the camera and object
			var spawnPos = PlayerCamera.WorldToScreenPoint(bulletSpawn.position);
			mousePos.x = mousePos.x - spawnPos.x;
			mousePos.y = mousePos.y - spawnPos.y;
			var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			bulletSpawn.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

			Vector3 worldMousePos = PlayerCamera.ScreenToWorldPoint(Input.mousePosition);

			Vector2 direction = (Vector2)((worldMousePos - transform.position));
			direction.Normalize ();



			// Create the Bullet from the Bullet Prefab


			var bullet = (GameObject)Instantiate(
				bulletPrefab,
				bulletSpawn.position + (Vector3)( direction * 0.5f),
				bulletSpawn.rotation);

			// Add velocity to the bullet
			bullet.GetComponent<Rigidbody2D>().velocity = direction * 50;

			// Spawn the bullet on the Clients
			NetworkServer.Spawn(bullet);

			// Destroy the bullet after 2 seconds
			Destroy(bullet, 2.0f);
		}
	}
}
