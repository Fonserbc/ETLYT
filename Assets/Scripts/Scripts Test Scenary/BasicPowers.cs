using UnityEngine;
using System.Collections;

public class BasicPowers : MonoBehaviour {
	
	private WorldMovement world;
	private Control control;
	private int player = 0;
	
	/*** Control de movimientos en ejecucion ***/
	//Se accedera al mundo


	// Use this for initialization
	void Start () {
		control = (Control)(GameObject.FindGameObjectWithTag("Control").GetComponent("Control"));	
		world = (WorldMovement)(GameObject.FindGameObjectWithTag("World").GetComponent("WorldMovement"));
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if(control.AbilityWorld(player)) {
			//if(!world.isRotating()) {
				world.rotateToAngle(control.Slope(player));
			//}
		}		
		if(control.AbilityGravity(player)) {
			//if(!world.isGravitating()) {
				float angle = (control.Slope(player)*2*Mathf.PI)/360;
				Vector3 gravity = new Vector3(Mathf.Sin(angle), -Mathf.Cos(angle),0);
				world.gravitateToAngle(gravity);
			//}
		}
		

	}
	
	public void SetPlayer(int p) {
		player = p;
	}
}