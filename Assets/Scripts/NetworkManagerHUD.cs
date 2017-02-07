using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

#if ENABLE_UNET

namespace Gobzers
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class NetworkManagerHUD : MonoBehaviour
	{
		public GameObject ConPanel;
		public GameObject InfoPanel;
		public Text ifServerAdress;
		public Text lblInfo;
		public NetworkManager manager;

		[SerializeField] public bool showGUI = true;
		[SerializeField] public int offsetX;
		[SerializeField] public int offsetY;


		// Runtime variable
		//bool showServer = false;

		void Start ()
		{
			ChangeUI ();
		}

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		void Update()
		{
			if (!showGUI)
				return;

				if (NetworkServer.active)
				{
					lblInfo.text = "Server: port=" + manager.networkPort;
				}
				if (NetworkClient.active)
				{
					lblInfo.text = "Client: address=" + manager.networkAddress + " port=" + manager.networkPort;
				}

			if (NetworkServer.active || NetworkClient.active)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					manager.StopHost();
				}
			}
		}

		public void OnClickStartHost ()
		{
			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null) {
				
				manager.StartHost ();
				ChangeUI ();
				Debug.Log ($"Start HOST: HOST {manager.networkAddress}");
			}
		}

		public void OnClickStopHost ()
		{
			if (NetworkServer.active || NetworkClient.active) {
				manager.StopHost ();
				ChangeUI ();
			}
		}

		public void OnClickStartServer ()
		{
			if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null) {
				manager.StartServer();
				ChangeUI ();
			}
		}

        public void OnClickStartClient()
        {
			Debug.Log ($"NC ACTIVE: {NetworkClient.active}");
			Debug.Log ($"NS ACTIVE: {NetworkServer.active}");
            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
                manager.StartClient();
				ChangeUI ();
				Debug.Log ($"ADRESS: {ifServerAdress.text.ToString ()}");
				manager.networkAddress = ifServerAdress.text.ToString ();
            }
        }

		void ChangeUI()
		{
			ConPanel.SetActive (!ConPanel.activeSelf);
			InfoPanel.SetActive (!InfoPanel.activeSelf);
		}

  //      void OnGUI()
		//{
		//	if (!showGUI)
		//		return;

		//	int xpos = 10 + offsetX;
		//	int ypos = 40 + offsetY;
		//	int spacing = 24;

		//	if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
		//	{
		//		if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
		//		{
		//			manager.StartHost();
		//		}
		//		ypos += spacing;

		//		if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
		//		{
		//			manager.StartClient();
		//		}
		//		manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), manager.networkAddress);
		//		ypos += spacing;

		//		if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
		//		{
		//			manager.StartServer();
		//		}
		//		ypos += spacing;
		//	}
		//	else
		//	{
		//		if (NetworkServer.active)
		//		{
		//			GUI.Label(new Rect(xpos, ypos, 300, 20), "Server: port=" + manager.networkPort);
		//			ypos += spacing;
		//		}
		//		if (NetworkClient.active)
		//		{
		//			GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
		//			ypos += spacing;
		//		}
		//	}

		//	if (NetworkClient.active && !ClientScene.ready)
		//	{
		//		if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
		//		{
		//			ClientScene.Ready(manager.client.connection);
				
		//			if (ClientScene.localPlayers.Count == 0)
		//			{
		//				ClientScene.AddPlayer(0);
		//			}
		//		}
		//		ypos += spacing;
		//	}

		//	if (NetworkServer.active || NetworkClient.active)
		//	{
		//		if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
		//		{
		//			manager.StopHost();
		//		}
		//		ypos += spacing;
		//	}


		//}
	}


};
#endif //ENABLE_UNET
