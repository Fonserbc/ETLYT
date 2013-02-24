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
	
	public float jumpForce = 10f;
	public float jumpAnimationLenght = 1f;
	public float acceleration = 10f;
	public float airAcceleration = 5f;

	private bool colliding = false;
	private float jumpTimer = 0f;
	
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
		
		anim = GetComponent<AnimationHandler>();
		
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		player = control.RegisterPlayer(Control.ControllerType.WiiMote, 0);
		
		BroadcastMessage("SetPlayer", player);
		
		lastPos = transform.position;
		state = PlayerState.Air;
		anim.setAnimation(state, DEF_ANIM_SPEED);
	}
	
	void Update () {
		if (WiiMoteControl.wiimote_count() > player) {
			Vector3 movementDir = GetMovementDir();
			
			bool jump = control.Jump(player);
			
			if (state == PlayerState.Air || state == PlayerState.Jump) {
				if (state == PlayerState.Jump) {
					jumpTimer -= Time.deltaTime;
					
					if (jumpTimer < 0f) {
						state = PlayerState.Air;
						anim.setAnimation(state, DEF_ANIM_SPEED);
					}
				}
				rigidbody.velocity += movementDir*airAcceleration*Time.deltaTime;
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
					
					jumpTimer = 1f;
					
					state = PlayerState.Jump;
					anim.setAnimation(state, DEF_ANIM_SPEED/2f);
				}
			}
			else if (state == PlayerState.Slide) {
				rigidbody.velocity += movementDir*acceleration*Time.deltaTime;
				
				if (jump && colliding) {
					rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
					
					jumpTimer = 1f;
					
					state = PlayerState.Jump;
					anim.setAnimation(state, DEF_ANIM_SPEED/2f);
				}
				else if (rigidbody.velocity.magnitude < 0.5f) {
					state = PlayerState.Run;
					anim.setAnimation(state, DEF_ANIM_SPEED);
				}
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
	
	Vector3 GetMovementDir () {
		Vector3 norm = -transform.forward;
		Vector3 dir = new Vector3(control.HorizontalAxis(player), control.VerticalAxis(player), 0);
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
			return transform.forward;
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position+normal*5);
	}
	
	void OnCollisionStay(Collision col) {
		colliding = true;
		if (col.contacts.Length > 0) normal = col.contacts[0].normal;
		normal.z = 0;
		
		if (Vector3.Angle(-Physics.gravity, normal) > 45f) {
			state = PlayerState.Wall;
			anim.setAnimation(state, DEF_ANIM_SPEED);
		}
		else if (state == PlayerState.Air) {
			state = PlayerState.Idle;
			anim.setAnimation(state, DEF_ANIM_SPEED);
		}
		else if (state == PlayerState.Wall) {
			state = PlayerState.Slide;
			anim.setAnimation(state, DEF_ANIM_SPEED);
		}
	}
	
	void OnCollisionExit(Collision col) {
		colliding = false;
	}
	
	public void SetPlayer (int p) {
		player = p;
	}
	
	public void Death () {
		anim.setAnimation(PlayerState.Death, DEF_ANIM_SPEED);
	}
}
