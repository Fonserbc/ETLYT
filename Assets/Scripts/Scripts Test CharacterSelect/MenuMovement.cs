using UnityEngine;
using System.Collections;

public class MenuMovement : MonoBehaviour {
	
	private Control control;
	private Transform camara;
	private int player;
	public float acceleration = 5f;

	// Use this for initialization
	void Start () {
		camara = GameObject.FindGameObjectWithTag("MainCamera").transform;
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();

		player = control.RegisterPlayer(Control.ControllerType.WiiMote, 0);
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.MovePosition(transform.position+control.HorizontalAxis(player)*camara.right*acceleration*Time.deltaTime+control.VerticalAxis(player)*camara.up*acceleration*Time.deltaTime);
		
	}
}
