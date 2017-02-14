using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
	public const int MAX_HEALTH = 100;
	public RectTransform healthBar;

	[SyncVar(hook = "OnChangeHealth")]
	public int CurrentHealth = MAX_HEALTH;

	public void TakeDamage(int amount) 
	{
		if (!isServer)
			return;
		
		CurrentHealth -= amount;
		if (CurrentHealth <= 0)
		{
			CurrentHealth = MAX_HEALTH;
			RpcRespawn ();
		}

	}

	void OnChangeHealth (int health)
	{
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
	}

	[ClientRpc]
	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
            // move back to zero location
            transform.position = GameManager.Instance.GetRespawnPos();
		}
	}
}
