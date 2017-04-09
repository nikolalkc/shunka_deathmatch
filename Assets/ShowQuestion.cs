using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowQuestion : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "celeb") {
			if (col.gameObject.GetComponent<Celeb>().isActive) {
				Game.currentCeleb = col.gameObject;	
				Game.ShowQuestion (true);
			}


		}

	}
}
