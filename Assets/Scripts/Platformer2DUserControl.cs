using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Gobzers
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : NetworkBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
				m_Jump = Input.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            if (!isLocalPlayer)
            {
                return;
            }
			bool crouch = Input.GetKey(KeyCode.LeftControl);
			bool run = Input.GetKey (KeyCode.LeftShift);
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			bool jp = Input.GetKeyDown (KeyCode.C);
            // Pass all parameters to the character control script.
			m_Character.Move(h, v, crouch, run, ref m_Jump, jp);
			jp = false;
        }
    }
}
