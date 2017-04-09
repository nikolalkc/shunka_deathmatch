using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour {
	public GameObject[] props;
	// Use this for initialization
	void Start () {
		int idx = Random.Range (0, props.Length);
		props [idx].SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
