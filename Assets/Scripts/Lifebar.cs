using UnityEngine;
using System.Collections;

public class Lifebar : MonoBehaviour {

	public Texture2D[] Numbers;
	public Texture2D[] PowerUps;
	public Texture2D Portrait;
	public Texture2D[] Display;
	public Texture2D[] Arrow;
	private float life = 0;
	private int stackedPowerUp = -1;
	
	private int player = 0;
	
	private bool start = false;
	
	public GameObject Plus5; 
	
	private float angle = 0;
	Control control;
	
	
	// Use this for initialization
	void Start () {
		life = 80;
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();

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
			GUI.DrawTexture(new Rect(106, 46, 60, 60), Numbers[(centenes+1)]);
			GUI.DrawTexture(new Rect(129, 46, 60, 60), Numbers[(desenes+1)]);
			GUI.DrawTexture(new Rect(152, 46, 60, 60), Numbers[(unitats+1)]);
			GUI.DrawTexture(new Rect(25,30,100,100), Portrait);
			
			Matrix4x4 matrixBackup = GUI.matrix;
			Vector2 pivot = new Vector2(72.5f,80);
			GUIUtility.RotateAroundPivot(angle, pivot);
			GUI.DrawTexture(new Rect(62.5f, 10, 20, 20), Arrow[0]);
			GUI.matrix = matrixBackup;
			
			if (stackedPowerUp > -1) GUI.DrawTexture(new Rect(133, 94, 50, 50), PowerUps[stackedPowerUp]);
		}
		else if (player == 1) {		
		GUI.DrawTexture(new Rect(Screen.width-240, -30, 240, 240), Display[1]);
			GUI.DrawTexture(new Rect(Screen.width-255, 46, 60, 60), Numbers[(centenes+1)]);
			GUI.DrawTexture(new Rect(Screen.width-232, 46, 60, 60), Numbers[(desenes+1)]);
			GUI.DrawTexture(new Rect(Screen.width-209, 46, 60, 60), Numbers[(unitats+1)]);
			GUI.DrawTexture(new Rect(Screen.width-123,30,100,100), Portrait);
			
			Matrix4x4 matrixBackup = GUI.matrix;
			Vector2 pivot = new Vector2(Screen.width-72.5f,80);
			GUIUtility.RotateAroundPivot(angle, pivot);
			GUI.DrawTexture(new Rect(Screen.width-82.5f, 10, 20, 20), Arrow[1]);
			GUI.matrix = matrixBackup;
			
			if (stackedPowerUp > -1) GUI.DrawTexture(new Rect(Screen.width-178, 94, 50, 50), PowerUps[stackedPowerUp]);
		}
		else if (player == 2) {
			GUI.DrawTexture(new Rect(0, Screen.height-200, 240, 240), Display[0]);
			GUI.DrawTexture(new Rect(106, Screen.height-124, 60, 60), Numbers[(centenes+1)]);
			GUI.DrawTexture(new Rect(129, Screen.height-124, 60, 60), Numbers[(desenes+1)]);
			GUI.DrawTexture(new Rect(152, Screen.height-124, 60, 60), Numbers[(unitats+1)]);
			GUI.DrawTexture(new Rect(25,Screen.height-140,100,100), Portrait);
			
			Matrix4x4 matrixBackup = GUI.matrix;
			Vector2 pivot = new Vector2(72.5f,Screen.height-90);
			GUIUtility.RotateAroundPivot(angle, pivot);
			GUI.DrawTexture(new Rect(62.5f, Screen.height-160, 20, 20), Arrow[2]);
			GUI.matrix = matrixBackup;
			
			if (stackedPowerUp > -1) GUI.DrawTexture(new Rect(133, Screen.height-76, 50, 50), PowerUps[stackedPowerUp]);
		}
		else if (player == 3) {
			GUI.DrawTexture(new Rect(Screen.width-240, Screen.height-200, 240, 240), Display[1]);
			GUI.DrawTexture(new Rect(Screen.width-255, Screen.height-124, 60, 60), Numbers[(centenes+1)]);
			GUI.DrawTexture(new Rect(Screen.width-232, Screen.height-124, 60, 60), Numbers[(desenes+1)]);
			GUI.DrawTexture(new Rect(Screen.width-209, Screen.height-124, 60, 60), Numbers[(unitats+1)]);
			GUI.DrawTexture(new Rect(Screen.width-123,Screen.height-140,100,100), Portrait);

			Matrix4x4 matrixBackup = GUI.matrix;
			Vector2 pivot = new Vector2(Screen.width-72.5f,Screen.height-90);
			GUIUtility.RotateAroundPivot(angle, pivot);
			GUI.DrawTexture(new Rect(Screen.width-82.5f, Screen.height-160, 20, 20), Arrow[3]);
			GUI.matrix = matrixBackup;
			
			if (stackedPowerUp > -1) GUI.DrawTexture(new Rect(Screen.width-178, Screen.height-76, 50, 50), PowerUps[stackedPowerUp]);
		}
	}
	
	// Update is called once per frame
	void Update() {
		
		if (life <= 0) {
			//Destroy(gameObject);
			BroadcastMessage("Death");
			life = 0;
		}
		else life -= Time.deltaTime;
		//Debug.Log(centenes.ToString() + desenes.ToString() + unitats.ToString());
		
		angle = control.Slope(player);
		angle = (angle < 0)? angle + 360f : angle;
		angle = -angle+180;
		

	}
	
	//Se llama desde Movement, al cambiar a estado Hurt
	public void gotHit() {
		life -= 2;
	}
	
	//Se llama desde aqui
	public void addLife(int i) {
		life += 5;
	}
	
	public void addPowerUp(int i) {
		stackedPowerUp = i;
	}
		
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Clock") {
			life += 5;
			Instantiate(Plus5, collider.transform.position,Plus5.transform.rotation);
			Destroy(collider.gameObject);
		}
	}
	
	void SetPlayer (int p) {
		player = p;
	}
}
