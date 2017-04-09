using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celeb : MonoBehaviour {
	public GameObject[] types;
	public bool isActive = true;

	public void SetImage (int idx) {
		types [idx].SetActive (true);
	}


}
