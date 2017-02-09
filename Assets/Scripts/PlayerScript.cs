using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gobzers
{
	public class PlayerScript : NetworkBehaviour
	{
		public GameObject PlayerCamera;
		public GameObject bulletPrefab;
		public Transform bulletSpawn;

		[SerializeField]
		private Camera _curCamera;
		// Use this for initialization
		void Start ()
		{
			if (isLocalPlayer)
	        {
				_curCamera = PlayerCamera.GetComponent<Camera> ();
				_curCamera.enabled = true;
	            PlayerCamera.GetComponent<AudioListener>().enabled = true;
				PlayerCamera.GetComponent<Camera2DFollow>().enabled = true;
				if (GameObject.Find ("SceneCamera") != null) 
				{
					GameObject.Find ("SceneCamera").SetActive (false);
				}
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
				var mousePos = Input.mousePosition;
				mousePos.z = 5.23F; //The distance between the camera and object
				var spawnPos = _curCamera.WorldToScreenPoint(bulletSpawn.position);
				mousePos.x = mousePos.x - spawnPos.x;
				mousePos.y = mousePos.y - spawnPos.y;
				var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
				bulletSpawn.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

				Vector3 worldMousePos = _curCamera.ScreenToWorldPoint(Input.mousePosition);

				Vector2 direction = (Vector2)((worldMousePos - transform.position));
				direction.Normalize ();

				CmdFire(bulletSpawn.position + (Vector3)(direction * 0.5f), Quaternion.Euler(new Vector3(0, 0, angle)), direction * 50);
			}
		}

		[Command]
		void CmdFire(Vector2 bulletPos, Quaternion startRotation, Vector2 direction)
		{

			// Create the Bullet from the Bullet Prefab
			var bullet = (GameObject)Instantiate(
				bulletPrefab,
				bulletPos,
				startRotation);

			// Add velocity to the bullet
			bullet.GetComponent<Rigidbody2D>().velocity = direction;

			// Spawn the bullet on the Clients
			NetworkServer.Spawn(bullet);

			// Destroy the bullet after 2 seconds
			Destroy(bullet, 2.0f);
		}
	}
}
