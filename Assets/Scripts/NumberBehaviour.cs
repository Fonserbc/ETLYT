using UnityEngine;
using System.Collections;

public class NumberBehaviour : MonoBehaviour {
	
	public Texture2D[] numbers;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setNumber (int number) {
		renderer.material.mainTexture = numbers[number+1];
	}
	
}
