using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	private float minSpeed = 0.2f;
	
	public enum PlayerState {
		Idle,
		Run,
		Slide,
		Jump,
		Air,
		Wall,
		Hurt,
		Attack,
		Death
	}
	
	public float jumpForce = 10f;
	public float acceleration = 5f;
	public float airTime = 1.5f;
	
	private float airTimer = 0f;

	private bool jumping = false;
	private bool colliding = false;
	
	private int player;
	
	private Vector3 normal;
	private Transform camara;
	
	private AnimationHandler anim;
	private Control control;
	
	private Vector3 lastPos;
	private int direction = 1;
	
	void Start () {
		camara = GameObject.FindGameObjectWithTag("MainCamera").transform;
		
		anim = GetComponent<AnimationHandler>();
		anim.setAnimation(PlayerState.Idle, 0.1f);
		
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		player = control.RegisterPlayer(Control.ControllerType.WiiMote, 0);
		
		BroadcastMessage("SetPlayer", player);
		
		lastPos = transform.position;
	}
	
	void Update () {
		if (WiiMoteControl.wiimote_count() > player) {
			bool jump = control.Jump(player);
			
			if (!jumping && colliding && jump) {
				jumping = true;
				rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
			}
			if (!colliding) {
				airTimer += Time.deltaTime;
			}
			if (airTimer < airTime) {
				rigidbody.velocity += control.HorizontalAxis(player)*camara.right*acceleration*Time.deltaTime;
				
				rigidbody.velocity += control.VerticalAxis(player)*camara.up*acceleration*Time.deltaTime;
			}
		}
		
		
		Vector3 dir = transform.position - lastPos;
		
		float wantedDir = Vector3.Cross(-Physics.gravity, dir).z;
		int newDir = (wantedDir < 0)? -1 : 1;
		
		if (Mathf.Abs(wantedDir) > 0.1 && newDir != direction) {
			anim.setDirection(newDir);
			direction = newDir;
		}
		
		lastPos = transform.position;
		
		//rigidbody.MoveRotation(transform.rotation*Quaternion.FromToRotation(-transform.forward, normal));
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position+normal*5);
	}
	
	void OnCollisionEnter(Collision col) {
		airTimer = 0f;
	}
	
	void OnCollisionStay(Collision col) {
		colliding = true;
		if (col.contacts.Length > 0) normal = col.contacts[0].normal;
		Debug.Log(col.contacts.Length);
		normal.z = 0;
		jumping = false;
		airTimer = 0f;
	}
	
	void OnCollisionExit(Collision col) {
		colliding = false;
	}
}
