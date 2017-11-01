using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
        private Rigidbody m_Rigidbody;
        private GameController gameController;
        public GameObject distanceLine;
        public float keepDistance = 10f;

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            m_Rigidbody = GetComponent<Rigidbody>();
            gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            distanceLine.transform.localPosition = new Vector3(0f, 0.1f, keepDistance);
        }

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y, 0);
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            
            float h = CrossPlatformInputManager.GetAxis("Horizontal")/20;
            if(h == 0)
            {
                m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
                                          RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
                                         RigidbodyConstraints.FreezeRotationX |
                                         RigidbodyConstraints.FreezeRotationZ;
            }
            //if(CrossPlatformInputManager.GetAxis("Horizontal") < 0)
            //{
            //    h = -h;
            //}
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            float accel = v;
            if(accel >= 0)
            {
                accel = accel * accel;
            }
            else
            {
                accel = -(accel*accel);
            }
            //vÇ…ï‚ê≥ÇÇ©ÇØÇƒÇ©ÇÁâ∫ÇÃä÷êîÇ…Ç¢ÇÍÇÈ
            //ó· : î˜í≤êÆÇÇ´Ç©ÇπÇΩÇØÇÍÇŒïΩï˚ç™ÇÇ∆Ç¡Çƒkî{Ç∑ÇÈÇ»Ç«
           
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
    }
}
