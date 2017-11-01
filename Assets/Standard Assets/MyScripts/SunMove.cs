using UnityEngine;
using System.Collections;

public class SunMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
            transform.rotation = transform.rotation * Quaternion.Euler(Time.deltaTime, 0, 0);
    }
}
