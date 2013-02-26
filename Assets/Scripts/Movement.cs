using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	private float MIN_SPEED = 0.2f;
	private float DEF_ANIM_SPEED = 0.1f;
	
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
	
	public Control.ControllerType debugControl;
	
	public float jumpForce = 10f;
	public float jumpAnimationLenght = 1f;
	public float acceleration = 10f;
	public float airAcceleration = 5f;
	public float rotationSpeed = 5f;

	private bool colliding = false;
	
	private int player;
	private PlayerState state;
	
	private Vector3 normal;
	private Transform camara;
	
	private AnimationHandler anim;
	private Control control;
	
	private Vector3 lastPos;
	private int direction = 2;
	private int dirAux = 0;
	
	void Start () {
		camara = GameObject.FindGameObjectWithTag("MainCamera").transform;
		
		anim = GetComponentInChildren<AnimationHandler>();
		
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		player = control.RegisterPlayer(debugControl, 0);
		
		BroadcastMessage("SetPlayer", player);
		
		lastPos = transform.position;
		state = PlayerState.Air;
		anim.setAnimation(state, DEF_ANIM_SPEED);
		
		normal = -Physics.gravity.normalized;
	}
	
	void Update () {
		if (debugControl != Control.ControllerType.WiiMote || WiiMoteControl.wiimote_count() > player) {
			Vector3 movementDir = GetMovementDir();
			
			bool jump = control.Jump(player);
			
			if (state == PlayerState.Air || state == PlayerState.Jump) {
				if (state == PlayerState.Jump) {					
					if (Vector3.Angle(-Physics.gravity.normalized, rigidbody.velocity.normalized) > 90f) {
						state = PlayerState.Air;
						anim.setAnimation(state, DEF_ANIM_SPEED);
					}
				}
				rigidbody.velocity += movementDir*airAcceleration*Time.deltaTime;
				rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, -Physics.gravity.normalized), Time.deltaTime*rotationSpeed));
			}
			else if (state == PlayerState.Idle || state == PlayerState.Run) {
				
				if (dirAux == 0) { //time to slide?
					if (Vector3.Angle(-Physics.gravity, normal) > 5f) {
						state = PlayerState.Slide;
						anim.setAnimation(state, DEF_ANIM_SPEED);
					}
				}
				
				rigidbody.velocity += movementDir*acceleration*Time.deltaTime;
				
				if (state == PlayerState.Idle && rigidbody.velocity.magnitude > 0.5f) {
					state = PlayerState.Run;
					anim.setAnimation(state, DEF_ANIM_SPEED);
				}
				else if (state == PlayerState.Run && rigidbody.velocity.magnitude < 0.5f) {
					state = PlayerState.Idle;
					anim.setAnimation(state, DEF_ANIM_SPEED);
				}
				
				if (jump && colliding) {
					rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
					
					state = PlayerState.Jump;
					anim.setAnimation(state, DEF_ANIM_SPEED/2f);
				}
				
				rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, normal), Time.deltaTime*rotationSpeed));
			}
			else if (state == PlayerState.Slide) {
				rigidbody.velocity += movementDir*acceleration*Time.deltaTime;
				
				if (jump && colliding) {
					rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
					
					state = PlayerState.Jump;
					anim.setAnimation(state, DEF_ANIM_SPEED/2f);
				}
				else if (rigidbody.velocity.magnitude < 1f) {
					state = PlayerState.Run;
					anim.setAnimation(state, DEF_ANIM_SPEED);
				}
				rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, normal), Time.deltaTime*rotationSpeed));
			}
			else if (state == PlayerState.Wall) {
				
				if (jump && colliding) {
					rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
					
					state = PlayerState.Jump;
					anim.setAnimation(state, DEF_ANIM_SPEED/2f);
				}
				else if (rigidbody.velocity.magnitude < 0.5f) {
					state = PlayerState.Run;
					anim.setAnimation(state, DEF_ANIM_SPEED);
				}
				rigidbody.velocity += movementDir*airAcceleration*Time.deltaTime;
				
				Vector3 wantedRot = normal;
				if (wantedRot.x > 0f) wantedRot = Quaternion.Euler(0, 0, 90f)*wantedRot;
				else wantedRot = Quaternion.Euler(0, 0, -90f)*wantedRot;
				rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, wantedRot), Time.deltaTime*rotationSpeed));
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
	}
	
	Vector3 GetMovementDir () {
		Vector3 norm = transform.up;
		float hAxis = control.HorizontalAxis(player);
		float vAxis = control.VerticalAxis(player);
		
		if (Mathf.Abs(hAxis) < 0.1f) hAxis = 0f;
		if (Mathf.Abs(vAxis) < 0.1f) vAxis = 0f;
		Vector3 dir = new Vector3(hAxis, vAxis, 0);
		
		if (hAxis == 0f && vAxis == 0f) return Vector3.zero;
		
		dir.Normalize();
		
		float angle = Vector3.Angle(norm, dir);
		bool left = (Vector3.Cross(norm, dir).z > 0);
		
		if (angle < 50f) {
			return Vector3.zero;
		}
		else if (angle < 135f) {
			dirAux = (left)? 1 : 2;
			return ((left)? 1 : -1)*transform.right;
		}
		else {
			dirAux = 0;
			return -transform.up;
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position+normal*5);
	}
	
	void OnCollisionEnter(Collision col) {
		if (state == PlayerState.Jump) {
			state = PlayerState.Air;
			anim.setAnimation(state, DEF_ANIM_SPEED);
		}
	}
	
	void OnCollisionStay(Collision col) {
		colliding = true;
		if (col.contacts.Length > 0) normal = col.contacts[0].normal;
		normal.z = 0;
		
		if (Vector3.Angle(-Physics.gravity, normal) > 45f) {
			state = PlayerState.Wall;
			anim.setAnimation(state, DEF_ANIM_SPEED);
		}
		else if (state == PlayerState.Wall) {
			if (Vector3.Angle(-Physics.gravity, normal) > 5f) {
				state = PlayerState.Slide;
				anim.setAnimation(state, DEF_ANIM_SPEED);
			}
			else {
				state = PlayerState.Run;
				anim.setAnimation(state, DEF_ANIM_SPEED);
			}
		}
		else if (state == PlayerState.Air) {
			state = PlayerState.Run;
			anim.setAnimation(state, DEF_ANIM_SPEED);
		}
	}
	
	void OnCollisionExit(Collision col) {
		colliding = false;
		
		/*if (state == PlayerState.Wall) {
			state = PlayerState.Air;
			anim.setAnimation(state, DEF_ANIM_SPEED);
		}*/
	}
	
	public void SetPlayer (int p) {
		player = p;
	}
	
	public void Death () {
		anim.setAnimation(PlayerState.Death, DEF_ANIM_SPEED);
	}
}
