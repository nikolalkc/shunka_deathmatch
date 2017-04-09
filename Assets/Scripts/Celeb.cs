using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celeb : MonoBehaviour {
	public GameObject[] types;
	// Use this for initialization
	void Start () {
		int idx = Random.Range (0, types.Length);
		types [idx].SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
