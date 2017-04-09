using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour {
	public static float _velocity = -0.0314f;
	public float velocity;
	// Use this for initialization
	void Start () {
		//GetComponent<Rigidbody2D> ().velocity = new  Vector2(-1.9f,0);
		velocity = _velocity;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3 (velocity, 0, 0));
	}
}
