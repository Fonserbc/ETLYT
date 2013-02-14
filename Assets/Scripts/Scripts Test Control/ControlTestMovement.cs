using UnityEngine;
using System.Collections;

public class ControlTestMovement : MonoBehaviour {
	
	public float jumpForce = 10f;
	public float acceleration = 5f;
	public float airTime = 1.5f;
	
	private float airTimer = 0f;

	private bool jumping = false;
	private bool colliding = false;
	
	private int player = -1;
	
	private Vector3 normal;
	private Transform camara;
	
	private Control control;
	
	void Start () {
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		camara = GameObject.FindGameObjectWithTag("MainCamera").transform;		
	}
	
	void Update () {
		if (player < 0) player = control.RegisterPlayer(Control.ControllerType.WiiMote, 0);
		
		Debug.Log("Hx: "+WiiMoteControl.wiimote_getNunchuckStickX(0)+", Vy: "+WiiMoteControl.wiimote_getNunchuckStickY(0));
		
		bool jump = control.Jump(player);
		
		float hAxis = control.HorizontalAxis(player);
		float vAxis = control.VerticalAxis(player);
		
		if (!jumping && colliding && jump) {
			jumping = true;
			rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
		}
		if (!colliding) {
			airTimer += Time.deltaTime;
		}
		if (airTimer < airTime) {
			/*if (left) {
				rigidbody.velocity += camara.right*-acceleration*Time.deltaTime;
			}
			else if (right) {
				rigidbody.velocity += camara.right*acceleration*Time.deltaTime;
			}
			if (up) {
				rigidbody.velocity += camara.up*acceleration*Time.deltaTime;
			}
			else if (down) {
				rigidbody.velocity += camara.up*-acceleration*Time.deltaTime;
			}*/
		}
	}
	
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag.Equals("World")) {
			
		}
		airTimer = 0f;
	}
	
	void OnCollisionStay(Collision col) {
		colliding = true;
		if (col.contacts.Length > 0) normal = col.contacts[0].normal;
		normal.z = 0;
		jumping = false;
		airTimer = 0f;
	}
	
	void OnCollisionExit(Collision col) {
		colliding = false;
	}
}
