using UnityEngine;
using System.Collections;

public class SimpleIDHandler : MonoBehaviour {
	
	private int ID = -1;
	
	public void setID(int i) {
		ID = i;
	}
	
	public int getID() {
		return ID;
	}
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
