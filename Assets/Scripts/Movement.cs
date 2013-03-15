using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	private float MIN_SPEED = 0.2f;
	private float MIN_SLIDE_SPEED = 0.2f;
	private float DEF_ANIM_SPEED = 0.1f;
	
	private float MIN_ANGLE_SLIDE = 5f;
	private float MIN_ANGLE_WALL = 55f;
	private float WALL_TO_AIR_TIME = 1f;
	
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
	
	private AnimationHandler[] anim;
	private Control control;
	
	private Vector3 lastPos;
	private int direction = 2;
	private int dirAux = 0;
	private float airTimer = 0f;
	
	void Start () {
		camara = GameObject.FindGameObjectWithTag("MainCamera").transform;
		
		anim = GetComponentsInChildren<AnimationHandler>();
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		player = control.RegisterPlayer(debugControl, 0);
		
		BroadcastMessage("SetPlayer", player);
		
		lastPos = transform.position;
		state = PlayerState.Air;
		anim[0].setAnimation(state, DEF_ANIM_SPEED);
		anim[1].setAnimation(state, DEF_ANIM_SPEED);
		normal = -Physics.gravity.normalized;
	}
	
	void Update () {
		if (debugControl != Control.ControllerType.WiiMote || WiiMoteControl.wiimote_count() > player) {
			Vector3 movementDir = GetMovementDir();
			
			bool jump = control.Jump(player);
			
			if (!colliding) {
				airTimer += Time.deltaTime;
				
				rigidbody.velocity += movementDir*airAcceleration*Time.deltaTime;
				
				if (state == PlayerState.Wall && airTimer > WALL_TO_AIR_TIME) { // Fa ja massa temps que no toquem paret
					ChangeState(PlayerState.Air);
				}
				else if (state == PlayerState.Jump) {	// Estem Baixant, toca posar Air state			
					if (Vector3.Angle(-Physics.gravity.normalized, rigidbody.velocity.normalized) > 90f) {
						ChangeState(PlayerState.Air);
					}
				}
			}
			else {
				if (state == PlayerState.Idle || state == PlayerState.Run) {
				
					if (dirAux == 0) { //time to slide?
						if (Vector3.Angle(-Physics.gravity, normal) > MIN_ANGLE_SLIDE) {
							ChangeState(PlayerState.Slide);
							//Debug.Log("Slide");
						}
					}
					else {
						rigidbody.velocity += movementDir*acceleration*Time.deltaTime;
						
						if (state == PlayerState.Idle && rigidbody.velocity.magnitude > 0.5f) {
							ChangeState(PlayerState.Run);
						}
						else if (state == PlayerState.Run && rigidbody.velocity.magnitude < 0.5f) {
							ChangeState(PlayerState.Idle);
						}
						
						if (jump && colliding) {
							rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
							
							ChangeState(PlayerState.Jump);
						}
					}
				}
				else if (state == PlayerState.Slide) {
					rigidbody.velocity += movementDir*acceleration*Time.deltaTime;
					
					if (jump && colliding) {
						rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
						
						ChangeState(PlayerState.Jump);
					}
					else if (rigidbody.velocity.magnitude < MIN_SLIDE_SPEED) { //We are going too slow
						ChangeState(PlayerState.Run);
					}
				}
				else if (state == PlayerState.Wall) {
					
					if (jump && colliding) {
						rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
						
						ChangeState(PlayerState.Jump);
					}
					
					rigidbody.velocity += movementDir*airAcceleration*Time.deltaTime;
				}
			}
			
			Vector3 wantedRot = -Physics.gravity.normalized;
			rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, wantedRot), Time.deltaTime*rotationSpeed));
			
		}
		
		// Veure si hem de girar l'sprite
		int newDir;
		if (state == PlayerState.Wall) {
			newDir = (normal.x < 0)? 1 : -1;			
			if (newDir != direction) {
				anim[0].setDirection(newDir);
				anim[1].setDirection(newDir);
				direction = newDir;
			}
		}
		else {
			Vector3 dir = transform.position - lastPos;
			
			float wantedDir = Vector3.Cross(-Physics.gravity, dir).z;
			newDir = (wantedDir < 0)? -1 : 1;
			
			if (Mathf.Abs(wantedDir) > 0.1 && newDir != direction) {
				anim[0].setDirection(newDir);
				anim[1].setDirection(newDir);
				direction = newDir;
			}
		}
		
	
		lastPos = transform.position;
	}
	
	Vector3 GetMovementDir () {
		Vector3 norm = normal;
		float hAxis = control.HorizontalAxis(player);
		float vAxis = control.VerticalAxis(player);
		
		if (Mathf.Abs(hAxis) < 0.1f) hAxis = 0f;
		if (Mathf.Abs(vAxis) < 0.1f) vAxis = 0f;
		Vector3 dir = new Vector3(hAxis, vAxis, 0);
		
		dirAux = -1;
		if (hAxis == 0f && vAxis == 0f) return Vector3.zero;
		
		dir.Normalize();
		
		float angle = Vector3.Angle(norm, dir);
		bool left = (Vector3.Cross(norm, dir).z < 0);
		
		if (angle < 15f) { //Dir is going up
			return Vector3.zero;
		}
		else if (angle < 140f) {
			dirAux = (left)? 1 : 2;
			return ((left)? -1 : 1)*(Quaternion.Euler(0, 0, 90f)*normal);
		}
		else {
			dirAux = 0;
			return Quaternion.Euler(0, 0, 180f)*normal;
		}
	}

	void ChangeState(PlayerState newState) {
		state = newState;
		anim[0].setAnimation(state, DEF_ANIM_SPEED);
		anim[1].setAnimation(state, DEF_ANIM_SPEED);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position+normal*5);
		
		if (control) {
			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position+ new Vector3(control.HorizontalAxis(player), control.VerticalAxis(player), 0)*5);
			
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, transform.position+GetMovementDir()*5);
		}
	}
	
	void OnCollisionEnter(Collision col) {
		if (state == PlayerState.Jump) {
			state = PlayerState.Air;
			anim[0].setAnimation(state, DEF_ANIM_SPEED);
			anim[1].setAnimation(state, DEF_ANIM_SPEED);
		}
	}
	
	void OnCollisionStay(Collision col) {
		airTimer = 0f;
		colliding = true;
		if (col.contacts.Length > 0) {
			normal = col.contacts[0].normal;
			
			if (Vector3.Angle(normal, transform.position - col.contacts[0].point) > 90f)
				normal = -normal;
		}
		normal.z = 0;
		
		if (Vector3.Angle(-Physics.gravity, normal) > MIN_ANGLE_WALL) {
			ChangeState(PlayerState.Wall);
		}
		else if (state == PlayerState.Wall) {
			if (Vector3.Angle(-Physics.gravity, normal) > MIN_ANGLE_SLIDE) {
				ChangeState(PlayerState.Slide);
			}
			else {
				ChangeState(PlayerState.Run);
			}
		}
		else if (state == PlayerState.Air) {
			ChangeState(PlayerState.Run);
		}
	}
	
	void OnCollisionExit(Collision col) {
		colliding = false;
		normal = -Physics.gravity.normalized;
		
		if (state == PlayerState.Wall) {
			ChangeState(PlayerState.Air);
		}
	}
	
	public void SetPlayer (int p) {
		player = p;
	}
	
	public void Death () {
		ChangeState(PlayerState.Death);
	}
}
