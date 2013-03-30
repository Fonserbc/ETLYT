using UnityEngine;
using System.Collections;

public class PlayerHitBoxControl : MonoBehaviour {
	public GameObject hitBox;
	private GameObject hittingBox;
	private Control control;
	private int player = 0;
	private bool shield = false;
	
	public Vector3 hitPosition;
	


	// Use this for initialization
	void Start () {
		control = (Control)(GameObject.FindGameObjectWithTag("Control").GetComponent("Control"));	
	}
	
	// Update is called once per frame
	void Update () {
		if(control.Attack(player) && hittingBox == null) {
			int hitDir = GetHitDir();
			//PillarPosicion si precede
			if(hitDir != 0) {
				//ARREGLAR HITPOSITION
				hittingBox = (GameObject)Instantiate(hitBox, transform.position+hitPosition*hitDir, transform.rotation);
				Physics.IgnoreCollision(hittingBox.collider, transform.collider);
				hittingBox.transform.parent = transform;

				
				
				 Movement mov = GetComponent<Movement>();
				 mov.Attack();			 
				 
			}
			
		}
		
	}
	
	void OnTriggerEnter(Collider col) {
		if(!shield) {
			if(col.gameObject.tag == "HitBox") {
				Vector3 dir = transform.position - col.transform.position;
				
				Movement mov = GetComponent<Movement>();
				mov.Hit(dir);
				Debug.Log ("JIT");
			}
		}
	}
	
	public void setShield(bool sh) {
		shield = sh;
	}
	
	public void setPlayer(int p) {
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
		if(Vector3.Cross(aux, dir).z < 0) left = 1;
		return left;
	}


	
}

	