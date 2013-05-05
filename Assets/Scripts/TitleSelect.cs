using UnityEngine;
using System.Collections;

public class TitleSelect : MonoBehaviour {
	
	private Control control;
	
	public Renderer play;
	public Renderer instructions;
	
	public Renderer tutorialBack;
	
	public Texture2D loreTex;
	public Texture2D wiimote;
	public Texture2D nunchuck;
	
	private bool up = true;
	private float waitTime = 3f;
	
	private bool onTuto = false;
	private bool lore = true;
	
	// Use this for initialization
	void Start () {
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		play.material.color = Color.white;
		instructions.material.color = Color.gray;
	}
	
	// Update is called once per frame
	void Update () {
		if (!onTuto) {
			float vAxis = VerticalAxis();
			
			if (Mathf.Abs(vAxis) > 0.1f) {
				if (vAxis < 0f) {
					play.material.color = Color.gray;
					instructions.material.color = Color.white;
					up = false;
				}
				if (vAxis > 0f) {
					play.material.color = Color.white;
					instructions.material.color = Color.gray;
					up = true;
				}
			}
			
			if (waitTime > 0f) waitTime -= Time.deltaTime;
			else if (control.Accept()) {
				if (up) Application.LoadLevel(Application.loadedLevel + 1);
				else SetTuto(true);
			}
		}
		else {
			if (waitTime > 0f) waitTime -= Time.deltaTime;
			else if (control.Accept()) {
				if (lore) {
					lore = false;
					waitTime = 0.5f;
				}
				else SetTuto(false);
			}
		}
	}
			
	float VerticalAxis () {
		float axis = 0f;
		int ps = control.players;
		for (int i = 0; i < ps; ++i) {
			axis += control.VerticalAxis(i);
		}
		
		return axis/(float)ps;
	}
	
	void SetTuto(bool b) {
		onTuto = b;
		tutorialBack.enabled = b;
		waitTime = 0.5f;
		lore = b;
	}
	
	void OnGUI() {
		if (onTuto) {
			Vector2 size;
			if (lore) {
				size = new Vector2(Mathf.Min(loreTex.width, Screen.width), Mathf.Min(loreTex.height, Screen.height));
				GUI.DrawTexture(new Rect(Screen.width/2 - size.x/2, Screen.height/2 - size.y/2, size.x, size.y), loreTex);
			}
			else {
				size = new Vector2(Mathf.Min(wiimote.width, Screen.width*3/4), Mathf.Min(wiimote.height, Screen.height));
				GUI.DrawTexture(new Rect(Screen.width/4 - size.x/2, Screen.height/2 - size.y/2, size.x, size.y), wiimote);
				GUI.DrawTexture(new Rect(Screen.width*3/4 - size.x/2, Screen.height/2 - size.y/2, size.x, size.y), nunchuck);
			}
		}
	}
}
