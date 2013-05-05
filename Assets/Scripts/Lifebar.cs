using UnityEngine;
using System.Collections;

public class Lifebar : MonoBehaviour {

	public Texture2D[] Numbers;
	public Texture2D[] PowerUps;
	public Texture2D Portrait;
	public Texture2D[] Display;
	public Texture2D Arrow;
	private float life = 0;
	private int stackedPowerUp = -1;
	
	private int player = 0;
	
	// Use this for initialization
	void Start () {
		life = 100;
	}
	
	void OnGUI() {
	//	GUI.Label(new Rect(10, 10, 180, 45), "Tiempo: " + life);
		int centenes = ((int)life%1000)/100;
		int desenes = ((int)life%100)/10;
		int unitats = (int)life%10;
		if (centenes == 0) centenes = -1;
		if (desenes == 0 && centenes == -1) desenes = -1;
		if (unitats == 0 && desenes == -1) unitats = -1;
		
		if (player == 0) {
			GUI.DrawTexture(new Rect(0, -30, 240, 240), Display[0]);
			GUI.DrawTexture(new Rect(106, 46, 60, 60), Numbers[centenes+1]);
			GUI.DrawTexture(new Rect(129, 46, 60, 60), Numbers[desenes+1]);
			GUI.DrawTexture(new Rect(152, 46, 60, 60), Numbers[unitats+1]);
			GUI.DrawTexture(new Rect(25,30,100,100), Portrait);
			//GUI.DrawTexture(new Rect(30,30,100,100), Arrow);
			
			if (stackedPowerUp > -1) GUI.DrawTexture(new Rect(133, 94, 50, 50), PowerUps[stackedPowerUp]);
		}
		else if (player == 1) {		
		GUI.DrawTexture(new Rect(Screen.width-240, -30, 240, 240), Display[1]);
			GUI.DrawTexture(new Rect(Screen.width-255, 46, 60, 60), Numbers[centenes+1]);
			GUI.DrawTexture(new Rect(Screen.width-232, 46, 60, 60), Numbers[desenes+1]);
			GUI.DrawTexture(new Rect(Screen.width-209, 46, 60, 60), Numbers[unitats+1]);
			GUI.DrawTexture(new Rect(Screen.width-123,30,100,100), Portrait);
			//GUI.DrawTexture(new Rect(30,30,100,100), Arrow);
			
			if (stackedPowerUp > -1) GUI.DrawTexture(new Rect(Screen.width-178, 94, 50, 50), PowerUps[stackedPowerUp]);
		}
		else if (player == 2) {
			GUI.DrawTexture(new Rect(0, Screen.height-200, 240, 240), Display[0]);
			GUI.DrawTexture(new Rect(106, Screen.height-124, 60, 60), Numbers[centenes+1]);
			GUI.DrawTexture(new Rect(129, Screen.height-124, 60, 60), Numbers[desenes+1]);
			GUI.DrawTexture(new Rect(152, Screen.height-124, 60, 60), Numbers[unitats+1]);
			GUI.DrawTexture(new Rect(25,Screen.height-140,100,100), Portrait);
			//GUI.DrawTexture(new Rect(30,30,100,100), Arrow);
			
			if (stackedPowerUp > -1) GUI.DrawTexture(new Rect(133, Screen.height-76, 50, 50), PowerUps[stackedPowerUp]);
		}
		else if (player == 3) {
			GUI.DrawTexture(new Rect(Screen.width-240, Screen.height-200, 240, 240), Display[1]);
			GUI.DrawTexture(new Rect(Screen.width-255, Screen.height-124, 60, 60), Numbers[centenes+1]);
			GUI.DrawTexture(new Rect(Screen.width-232, Screen.height-124, 60, 60), Numbers[desenes+1]);
			GUI.DrawTexture(new Rect(Screen.width-209, Screen.height-124, 60, 60), Numbers[unitats+1]);
			GUI.DrawTexture(new Rect(Screen.width-123,Screen.height-140,100,100), Portrait);
			//GUI.DrawTexture(new Rect(30,30,100,100), Arrow);
			
			if (stackedPowerUp > -1) GUI.DrawTexture(new Rect(Screen.width-178, Screen.height-76, 50, 50), PowerUps[stackedPowerUp]);
		}
	}
	
	// Update is called once per frame
	void Update() {
		life -= Time.deltaTime;
		if (life <= 0) BroadcastMessage("Death");
		//Debug.Log(centenes.ToString() + desenes.ToString() + unitats.ToString());
	}
	
	public void gotHit() {
		life -= 10;
	}
	
	public void addLife(int i) {
		life += 5;
	}
	
	public void addPowerUp(int i) {
		stackedPowerUp = i;
	}
		
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Clock") {
			life += 5;
			Destroy(collider.gameObject);
		}
	}
	
	void SetPlayer (int p) {
		player = p;
	}
}
