using UnityEngine;
using System.Collections;

public class MenuMovement : MonoBehaviour {
	
	private Control control;
	private Transform camara;
	private int player;
	public float acceleration = 5f;
	private GameObject selection = null;
	private GameObject selected = null;
	// Use this for initialization
	void Start () {
		camara = GameObject.FindGameObjectWithTag("MainCamera").transform;
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();

		player = control.RegisterPlayer(Control.ControllerType.WiiMote, 0);
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.MovePosition(transform.position+control.HorizontalAxis(player)*camara.right*acceleration*Time.deltaTime+control.VerticalAxis(player)*camara.up*acceleration*Time.deltaTime);
		
		if(control.Pause(player)) {
			Application.LoadLevel("Scene Demo");	
		}
		
		if(selection) {
			if(control.Jump(player)) {
				if(selected) selected.renderer.material.color = Color.white;
				selected = selection;
				selected.renderer.material.color = Color.blue;
			}
		}
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.tag == "Character") {
			selection = c.gameObject;
			c.renderer.material.color = Color.red;
		}
	}
	
	void OnTriggerExit(Collider c) {
		if (c.gameObject.tag == "Character") {
			selection = null;
			if(selected != c.gameObject) c.renderer.material.color = Color.white;
			else c.renderer.material.color = Color.blue;
		}
	}
}
