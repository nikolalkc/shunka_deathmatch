using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shunka : MonoBehaviour {
	Game gameRef;
	// Use this for initialization
	void Start () {
		gameRef = Camera.main.GetComponent<Game> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		//print (col.gameObject.name);
		if (col.gameObject.tag == "celeb") {
			Game.ShowQuestion (false);	
			gameRef.SetQuestion ();
		}

	}
}
