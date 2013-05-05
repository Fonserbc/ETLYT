using UnityEngine;
using System.Collections;

public class PlayerHitBoxControl : MonoBehaviour {
	public GameObject hitBox;
	public GameObject shieldObject;
	private GameObject hittingBox;
	private Control control;
	private int player = 0;
	private bool shield = false;
	private GameObject instantiatedShield;
	
	private Vector3 hitPosition; // 0.24 0.18 0
	private Vector3 hitScale; // 1 0.7 0.5
	private float upHit = 0.1f;
	
	private Movement mov;
	
	public AudioSource[] attacks;
	public AudioSource[] hits;

	// Use this for initialization
	void Start () {
		control = (Control)(GameObject.FindGameObjectWithTag("Control").GetComponent("Control"));
		mov = gameObject.GetComponent<Movement>();
		
		hitPosition = new Vector3(0f, 0.35f, 0f);
		hitScale = new Vector3(1f, 0.7f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {		
		if(control.Attack(player) && hittingBox == null) {
			int hitDir = GetHitDir();
			//PillarPosicion si precede
			if(mov.Attack(hitDir)) {
				if (hitDir == 0) hitDir = mov.GetDirection();
				//ARREGLAR HITPOSITION
				hittingBox = (GameObject)Instantiate(hitBox, transform.position, transform.rotation);
				hittingBox.transform.parent = transform;
				
				hittingBox.transform.localScale = hitScale;
				hittingBox.transform.position +=  transform.InverseTransformDirection(hitPosition-hitDir*transform.right*0.45f);
				
				Physics.IgnoreCollision(hittingBox.collider, transform.collider);
				
				Destroy(hittingBox, mov.ATTACK_TIME+0.2f);
				int r = Random.Range(0,2);
				attacks[r].Play();
			}
			
		}
		
	}
	
	void OnTriggerStay(Collider col) {
		if(!shield) {
			if(col.gameObject.tag == "HitBox") {
				Vector3 dir = transform.position - col.transform.position + col.transform.parent.rigidbody.velocity.normalized - Physics.gravity.normalized*upHit;
				
				mov.Hit(dir);
				
				Physics.IgnoreCollision(col, collider);
				//Debug.Log ("JIT");
				
				int r = Random.Range(0,4);
				hits[r].Play();
			}
		}
	}
	
	public void setShield(bool sh) {
		shield = sh;
		
		if (shield) {
			if (instantiatedShield == null) {
				instantiatedShield = (GameObject) GameObject.Instantiate(shieldObject, transform.position, transform.rotation);
				instantiatedShield.transform.parent = transform;
				instantiatedShield.transform.localScale = new Vector3(1f,1f,1f);
			}
		}
		else {
			if (instantiatedShield) Destroy(instantiatedShield);
		}
	}
	
	public void SetPlayer(int p) {
		player = p;	
	}
	
	public void setHitPosition(Vector3 hp) {
		hitPosition = hp;	
	}
	
	public void finishAttack() {
		Destroy(hittingBox);
	}	
	
	int GetHitDir () {
		float hAxis = control.HorizontalAxis(player);
		float vAxis = control.VerticalAxis(player);
		
		if (Mathf.Abs(hAxis) < 0.1f) hAxis = 0f;
		if (Mathf.Abs(vAxis) < 0.1f) vAxis = 0f;
		Vector3 dir = new Vector3(hAxis, vAxis, 0);
		
		if (hAxis == 0f && vAxis == 0f) return 0;
		
		dir.Normalize();
			
		Vector3 aux;
		aux = Vector3.up;
		float angle = Vector3.Angle(aux, dir);
		int left = -1;
		if(Vector3.Cross(aux, dir).z < 0) left = -1;
		return left;
	}


	
}

	