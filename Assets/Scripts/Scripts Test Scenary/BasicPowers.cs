using UnityEngine;
using System.Collections;

public class BasicPowers : MonoBehaviour {
	
	private Control control;
	private int player;
	
	/*** Control de movimientos en ejecucion ***/
	//Se accedera al mundo


	// Use this for initialization
	void Start () {
		control = (Control)(GameObject.FindGameObjectWithTag("Control").GetComponent("Control"));	
	}
	
	// Update is called once per frame
	void Update () {
		
		/*
		if(control.AbilityWorld(player)) {
			if(!world.isRotating()) {
				world.rotateToAngle(control.Slope(player));
			}
		}		
		if(control.AbilityGravity(player)) {
			if(!world.isGravitating()) {
				world.gravitateToAngle(control.Slope(player));
			}
		}
		*/

	
	}
	
	public void SetPlayer(int p) {
		player = p;
	}
}