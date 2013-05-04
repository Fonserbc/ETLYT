using UnityEngine;
using System.Collections;

public class PowerUpHandler : MonoBehaviour {
	
	private int stack = -1;
	private float timeLeft = 0;
	private bool isActive = false;
	private int activePowerUp = -1;
	private int player = 0;

	public void stackPowerUp(int i) {
		stack = i;	
	}
	
	public void usePowerUp() {
		if (stack != -1) {
			if (isActive) terminatePowerUp();
			activePowerUp = stack;
			stack = -1;
			timeLeft = 10;
			isActive = true;
			Movement m = GetComponent<Movement>();
			PlayerHitBoxControl phb = GetComponent<PlayerHitBoxControl>();
			switch(activePowerUp) {
			case 0:
				phb.setShield(true);
				break;	
			case 1:
				m.jumpForce *= 3;
				m.maxSpeed *= 2;
//				m.acceleration *=2;
//				m.maxAirSpeed *=2;
				m.airAcceleration *=2;
				Debug.Log ("Estic super saltant");
				break;
			case 2:
				m.maxSpeed *= 2;
				m.acceleration *=2;
				m.maxAirSpeed *=1.5f;
			//	m.airAcceleration *=2;
				Debug.Log ("Estic super corrents");
				break;
			case 3:
				rigidbody.useGravity = false;
				Debug.Log ("No tinc gravetat");
				break;
			default:
				break;
			}
		}
	}
	
	void terminatePowerUp() {
		isActive = false;
		timeLeft = 0;
		Movement m = GetComponent<Movement>();
		PlayerHitBoxControl pHB = GetComponent<PlayerHitBoxControl>();
		switch(activePowerUp) {
			case 0:
				pHB.setShield(false);
				break;
			case 1:
				m.jumpForce /= 3;
				m.maxSpeed /= 2;
//				m.acceleration /=2;
//				m.maxAirSpeed /=2;
				m.airAcceleration /=2;
				break;
			case 2:
				m.maxSpeed /= 2;
				m.acceleration /=2;
				m.maxAirSpeed /=1.5f;
//				m.airAcceleration /=2;
				break;
			case 3:
				rigidbody.useGravity = true;
				break;
			default:
				break;
		}
		activePowerUp = -1;
	}
	
	void HUDActualization() {
		Lifebar LB = GameObject.FindGameObjectWithTag("Control").GetComponent<Lifebar>();
		Debug.Log(stack);
		LB.addPowerUp(stack);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(timeLeft > 0) {
			timeLeft -= Time.deltaTime;
			if (timeLeft < 0) {
				terminatePowerUp();
			}
		}
		Control c = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		if (c.AbilityPowerUp(player)) usePowerUp();
		HUDActualization();
	}
	
	void SetPlayer (int p) {
		player = p;
	}
}
