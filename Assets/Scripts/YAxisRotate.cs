using UnityEngine;
using System.Collections;

public class YAxisRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(transform.up*Time.deltaTime*150);
	}
}
