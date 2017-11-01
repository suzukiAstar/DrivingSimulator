using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof(CarController))]
    public class FrontCarControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private GameController gameController;
       
        public bool isBraking = false;
        

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            
        }

        private void Update()
        {
           
        }

        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = 0.6f;
            if (isBraking)
            {
                v = -0.02f;
            }
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            if (gameController.state == GameState.Game)
            {
                m_Car.Move(h, v, v, handbrake);
            }
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }

        public void StartBraking()
        {
            isBraking = true;
            Invoke("EndBraking", 2f);
        }

        public void EndBraking()
        {
            isBraking = false;
        }
    }
}
