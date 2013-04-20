using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	private float MIN_SPEED = 0.2f;
	private float MIN_SLIDE_SPEED = 0.2f;
	private float SLIDE_HOLD_TIME = 0.5f;
	private float DEF_ANIM_SPEED = 0.1f;
	private float MIN_SLIDE_TIME = 0.5f;
	private float SLIDE_IMPULSE = 0.3f;
	
	private float MIN_ANGLE_SLIDE = 5f;
	private float MIN_ANGLE_WALL = 55f;
	private float MAX_ANGLE_WALL = 100f;
	private float WALL_TO_AIR_TIME = 1f;
	
	private float HIT_RESPONSE_INTENSITY = 5f;
	private float HURT_TIME = 1f;
	public float ATTACK_TIME = 0.5f;
	private float ATTACK_IMPULSE = 0.5f;
	
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
	public float maxSpeed = 10f;
	public float airAcceleration = 5f;
	public float maxAirSpeed = 15f;
	public float rotationSpeed = 5f;

	private bool colliding = false;
	
	private int player;
	private PlayerState state;
	
	private Vector3 normal;
	private Transform camara;
	
	private AnimationHandler[] anim;
	private Control control;
	private PlayerHitBoxControl hitBoxControl;
	
	private Vector3 lastPos;
	private int direction = 2;
	private int dirAux = 0;
	private float airTimer = 0f;
	private float slideTimer = 0f;
	private float violentTimer = 0f;
	
	private float currSpeedScale = 0f;
	
	public int controlMode = 0;
	
	void Start () {
		camara = GameObject.FindGameObjectWithTag("MainCamera").transform;
		
		anim = GetComponentsInChildren<AnimationHandler>();
		hitBoxControl = GetComponent<PlayerHitBoxControl>();
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		
		lastPos = transform.position;
		state = PlayerState.Air;
		anim[0].setAnimation(state, DEF_ANIM_SPEED);
		anim[1].setAnimation(state, DEF_ANIM_SPEED);
		normal = -Physics.gravity.normalized;
	}
	
	void Update () {
		Vector3 movementDir = GetMovementDir();
		
		bool jump = control.Jump(player);
		
		if (state == PlayerState.Hurt || state == PlayerState.Attack) {
			violentTimer += Time.deltaTime;
			
			float aux = 1000f;
			if (state == PlayerState.Hurt) aux = HURT_TIME;
			else aux = ATTACK_TIME;
			
			if (violentTimer > aux) {
				violentTimer = 0;
				
				hitBoxControl.finishAttack();
				if (rigidbody.velocity.magnitude > 0.5f) ChangeState(PlayerState.Run);
				else ChangeState(PlayerState.Idle);
			}
		}
		else {
			if (!colliding) {
				airTimer += Time.deltaTime;
				
				rigidbody.velocity += movementDir*airAcceleration*Time.deltaTime;
				
				if (state == PlayerState.Wall) { // Fa ja massa temps que no toquem paret
					if (airTimer > WALL_TO_AIR_TIME)
						ChangeState(PlayerState.Air);
				}
				else if (state == PlayerState.Jump) {	// Estem Baixant, toca posar Air state			
					if (Vector3.Angle(-Physics.gravity.normalized, rigidbody.velocity.normalized) > 90f) {
						ChangeState(PlayerState.Air);
					}
				}
				else ChangeState(PlayerState.Air);
			}
			else {
				if (state == PlayerState.Idle || state == PlayerState.Run) {
				
					if (dirAux == 0) { //time to slide?
						if (Vector3.Angle(-Physics.gravity, normal) > MIN_ANGLE_SLIDE) {
							slideTimer += Time.deltaTime;
							
							if (slideTimer > SLIDE_HOLD_TIME) {
								ChangeState(PlayerState.Slide);
								slideTimer = 0f;
								rigidbody.velocity += Physics.gravity.normalized*SLIDE_IMPULSE;
							}
						}
						else slideTimer = 0f;
					}
					else {
						rigidbody.velocity += movementDir*acceleration*Time.deltaTime;
						
						if (state == PlayerState.Idle && rigidbody.velocity.magnitude > 0.5f && movementDir.magnitude > 0.5f) {
							ChangeState(PlayerState.Run);
						}
						else if (state == PlayerState.Run) {
							if (rigidbody.velocity.magnitude < 0.5f) 
								ChangeState(PlayerState.Idle);
							else {
								float speedScale = (0.1f+rigidbody.velocity.magnitude)*1.5f/maxSpeed;
								
								anim[0].setAnimationSpeed(DEF_ANIM_SPEED/speedScale);
								anim[1].setAnimationSpeed(DEF_ANIM_SPEED/speedScale);
							}
						}
						
						/*if (state == PlayerState.Run) {
							anim[0].setAnimation(PlayerState.Run, 2*DEF_ANIM_SPEED*currSpeedScale);
							anim[1].setAnimation(PlayerState.Run, 2*DEF_ANIM_SPEED*currSpeedScale);
						}*/
						
						if (jump && colliding) {
							rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
							
							ChangeState(PlayerState.Jump);
						}
					}
				}
				else if (state == PlayerState.Slide) {
					slideTimer += Time.deltaTime;
					if (jump && colliding) {
						rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
						
						ChangeState(PlayerState.Jump);
					}
					else if (rigidbody.velocity.magnitude < MIN_SLIDE_SPEED && slideTimer > MIN_SLIDE_TIME) { //We are going too slow
						ChangeState(PlayerState.Idle);
					}
				}
				else if (state == PlayerState.Wall) {
					
					if (jump && colliding) {
						rigidbody.velocity += (normal-Physics.gravity.normalized).normalized*jumpForce;
						
						ChangeState(PlayerState.Jump);
					}
					
					rigidbody.velocity += movementDir*airAcceleration*0.2f*Time.deltaTime;
				}
			}
		}
	
		if (state != PlayerState.Run && state != PlayerState.Idle && state != PlayerState.Slide) slideTimer = 0f;
		
		if (colliding) {
			if (rigidbody.velocity.magnitude > maxSpeed) rigidbody.velocity = rigidbody.velocity.normalized*maxSpeed;
			currSpeedScale = rigidbody.velocity.magnitude / maxSpeed;
		}
		else {
			if (rigidbody.velocity.magnitude > maxAirSpeed) {
				rigidbody.velocity = rigidbody.velocity.normalized*maxAirSpeed;
			}
			currSpeedScale = rigidbody.velocity.magnitude / maxAirSpeed;
		}
		
		Vector3 wantedRot = -Physics.gravity.normalized;
		rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, wantedRot), Time.deltaTime*rotationSpeed));
		//transform.rotation.y = 0;
		
		// Veure si hem de girar l'sprite
		int newDir;
		if (state == PlayerState.Wall) {
			newDir = (Vector3.Cross(Physics.gravity, normal).z > 0)? -1 : 1;
			if (newDir != direction) {
				anim[0].setDirection(newDir);
				anim[1].setDirection(newDir);
				direction = newDir;
			}
		}
		else {
			//Vector3 dir = transform.position - lastPos;
			Vector3 dir = movementDir;
			if (dir.magnitude < 0.01f)
				dir = transform.position - lastPos;
			
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
		
		Vector3 aux;
		if (controlMode == 0) aux = norm;
		else aux = Vector3.up;
		float angle = Vector3.Angle(aux, dir);
		bool left = (Vector3.Cross(aux, dir).z < 0);
		
		
		if (Vector3.Angle(Physics.gravity.normalized, dir) < 15f) dirAux = 0;
		if (angle < 15f) { //Dir is going up
			return Vector3.zero;
		}
		else if (angle < 140f) {
			dirAux = (left)? 1 : 2;
			if (Vector3.Angle(Physics.gravity.normalized, dir) < 15f) dirAux = 0;
			return ((left)? -1 : 1)*(Quaternion.Euler(0, 0, 90f)*normal);
		}
		else {
			dirAux = 0;
			return Quaternion.Euler(0, 0, 180f)*normal;
		}
	}

	void ChangeState(PlayerState newState) {
		if (newState != anim[0].getAnimationState()) {
			state = newState;
			float speed = DEF_ANIM_SPEED;
			if (state == PlayerState.Slide) speed *= 2;
			anim[0].setAnimation(state, speed);
			anim[1].setAnimation(state, speed);
		}
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
			ChangeState(PlayerState.Air);
		}
	}
	
	public int GetDirection() {
		return direction;
	}
	
	void OnCollisionStay(Collision col) {
		if (col.gameObject.tag != "Player") {
			airTimer = 0f;
			colliding = true;
			if (col.contacts.Length > 0) {
				normal = col.contacts[0].normal;
				
				if (Vector3.Angle(normal, transform.position - col.contacts[0].point) > 90f)
					normal = -normal;
			}
			normal.z = 0;
			
			float angle = Vector3.Angle(-Physics.gravity, normal);
			
			if (angle > MAX_ANGLE_WALL) {
				ChangeState(PlayerState.Air);
			}
			else if (angle > MIN_ANGLE_WALL) {
				ChangeState(PlayerState.Wall);
			}
			else if (state == PlayerState.Wall) {
				if (angle > MIN_ANGLE_SLIDE) {
					ChangeState(PlayerState.Slide);
				}
				else {
					ChangeState(PlayerState.Idle);
				}
			}
			else if (state == PlayerState.Air) {
				ChangeState(PlayerState.Idle);
			}
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
	
	public bool Attack (int right) {
		if (state != PlayerState.Hurt) {
			
			violentTimer = 0;
			
			rigidbody.velocity += transform.right*right*ATTACK_IMPULSE;
			
			if (right != 0 && right != direction) {
				anim[0].setDirection(right);
				anim[1].setDirection(right);
				direction = right;
			}
			
			ChangeState(PlayerState.Attack);
			
			return true;
		}
		return false;
	}
	
	public void Hit (Vector3 dir) {
		violentTimer = 0;
		
		rigidbody.velocity += dir.normalized*HIT_RESPONSE_INTENSITY + -Physics.gravity.normalized*HIT_RESPONSE_INTENSITY;
		
		ChangeState(PlayerState.Hurt);
	}
}
