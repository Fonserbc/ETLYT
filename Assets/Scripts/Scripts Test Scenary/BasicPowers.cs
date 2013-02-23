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
		control.RegisterPlayer(Control.ControllerType.WiiMote, player);

	}
	
	// Update is called once per frame
	void Update () {
		float angle = (control.Slope(player)*2*Mathf.PI)/360;
		Debug.Log (angle +  " " + Mathf.Cos(angle));
		Vector3 j = new Vector3(Mathf.Sin(angle), -Mathf.Cos(angle),0);
		Debug.DrawRay(transform.position, j*3);
		
		if(control.AbilityWorld(player)) {
			//if(!world.isRotating()) {
				world.rotateToAngle(control.Slope(player));
			//}
		}		
		if(control.AbilityGravity(player)) {
			//if(!world.isGravitating()) {
				//world.gravitateToAngle();
			//}
		}
		

	}
	
	public void SetPlayer(int p) {
		player = p;
	}
}