using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public enum PlayerState {
		Idle,
		Run,
		Slide,
		Jump,
		Air,
		Wall,
		Hurt,
		Attack
	}
	
	public float jumpForce = 10f;
	public float acceleration = 5f;
	public float airTime = 1.5f;
	
	private float airTimer = 0f;

	private bool jumping = false;
	private bool colliding = false;
	
	private int player;
	private GameObject otherPlayer;
	
	private Vector3 normal;
	private Transform camara;
	
	void Start () {
		camara = GameObject.FindGameObjectWithTag("MainCamera").transform;
		player = int.Parse(gameObject.tag) - 1;
		
		otherPlayer = GameObject.FindGameObjectWithTag(((int)((player+1)%2) + 1).ToString());
	}
	
	void Update () {
		if (WiiMoteControl.wiimote_count() > player) {
			bool up = false;
			bool down = false;
			bool right = false;
			bool left = false;
			bool jump = false;
			
			left = WiiMoteControl.wiimote_getButtonUp(player);
			right = WiiMoteControl.wiimote_getButtonDown(player);
			up = WiiMoteControl.wiimote_getButtonRight(player);
			down = WiiMoteControl.wiimote_getButtonLeft(player);
			jump = WiiMoteControl.wiimote_getButton2(player);
			
			if (!jumping && colliding && jump) {
				jumping = true;
				rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
			}
			if (!colliding) {
				airTimer += Time.deltaTime;
			}
			if (airTimer < airTime) {
				if (left) {
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
				}
			}
		}
	}
	
	void OnCollisionEnter(Collision col) {
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
