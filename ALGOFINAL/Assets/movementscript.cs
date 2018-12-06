using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementscript : MonoBehaviour {
	public float myspeed;
	// Use this for initialization
	void Start () {
		myspeed = 10f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Input.GetAxis("Horizontal") * myspeed * Time.deltaTime, 0f, Input.GetAxis("Vertical") * myspeed * Time.deltaTime);
	}
}
