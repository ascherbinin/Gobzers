using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Gobzers
{
	public class PlayerScript : NetworkBehaviour
	{
		//Sounds
		public AudioClip empty_rev;
		public AudioClip single_shot;

		public GameObject PlayerCamera;
		public GameObject bulletPrefab;
		public Transform bulletSpawn;

		public Canvas reloadBarCanvas;
		public RectTransform reloadBar;

		[SerializeField]
		private Camera _curCamera;
        [SerializeField]
        private Health _health;
        private float _fireRate = 0.5f;
		private float _lastFireTime;
		private int _playerBullet = 6;
		private int _reloadTime = 5;
		private AudioSource _audio;
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
				PlayerCamera.transform.parent = transform;
                _health = GetComponent<Health>();
				_audio = GetComponent<AudioSource> ();
            }
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (!isLocalPlayer)
			{
				return;
			}

			if ((Input.GetButtonDown ("Fire1") ||
			    Input.GetButtonDown ("Fire2")) && _playerBullet == 0)
				_audio.PlayOneShot (empty_rev);
			
	        //Debug.Log("Update");
			if (Input.GetButtonDown("Fire1") &&
				Time.time > _fireRate + _lastFireTime &&
				_playerBullet != 0)
			{
				SingleShot ();
				_lastFireTime = Time.time;
				_playerBullet--;
			}

			if (Input.GetButton ("Fire2") &&
			    Time.time > _fireRate / 3 + _lastFireTime &&
			    _playerBullet != 0) {
				RapidFire ();
				_lastFireTime = Time.time;
				_playerBullet--;
			}

			if (Input.GetKeyDown (KeyCode.R)) {
				Debug.Log("Reload");
				StartCoroutine(Reload());
			}

            if(Input.GetKeyDown(KeyCode.T))
            {
                _health.TakeDamage(100);
            }
		}

		[Command]
		void CmdFire(Vector2 bulletPos, Quaternion startRotation, Vector2 velocity)
		{
			_audio.PlayOneShot (single_shot);
			// Create the Bullet from the Bullet Prefab
			var bullet = (GameObject)Instantiate(
				bulletPrefab,
				bulletPos,
				startRotation);

			// Add velocity to the bullet
			bullet.GetComponent<Rigidbody2D>().velocity = velocity;

			// Spawn the bullet on the Clients
			NetworkServer.Spawn(bullet);

			// Destroy the bullet after 2 seconds
			Destroy(bullet, 2.0f);
		}

		void RapidFire()
		{
			Vector2 direction = new Vector2 ();
			float angle;
			CalculateShotDirection (out direction, out angle);
			CmdFire(bulletSpawn.position + (Vector3)(direction * 0.5f), Quaternion.Euler(new Vector3(0, 0, angle)), direction * 50);
		}

		void SingleShot ()
		{
			Vector2 direction = new Vector2 ();
			float angle;
			CalculateShotDirection (out direction, out angle);
			CmdFire(bulletSpawn.position + (Vector3)(direction * 0.5f), Quaternion.Euler(new Vector3(0, 0, angle)), direction * 50);
		}

		void CalculateShotDirection (out Vector2 direction, out float angle)
		{
			var mousePos = Input.mousePosition;
			mousePos.z = 5.23F; //The distance between the camera and object
			var spawnPos = _curCamera.WorldToScreenPoint(bulletSpawn.position);
			mousePos.x = mousePos.x - spawnPos.x;
			mousePos.y = mousePos.y - spawnPos.y;
			angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			bulletSpawn.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

			Vector3 worldMousePos = _curCamera.ScreenToWorldPoint(Input.mousePosition);

			direction = (Vector2)((worldMousePos - transform.position));
			direction.Normalize ();
		}

		IEnumerator Reload ()
		{
			float reloadTime = Time.deltaTime + _reloadTime;
			reloadBarCanvas.gameObject.SetActive(true);
			reloadBarCanvas.GetComponent<Canvas> ().enabled = true;
			reloadBar.sizeDelta = new Vector2(reloadTime - Time.deltaTime, reloadBar.sizeDelta.y);
			yield return new WaitForSeconds (_reloadTime);
			_playerBullet = 6;
		}
	}
}
