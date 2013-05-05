using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {
	
	public enum IntroState {
		Detecting,
		Studio,
		Credits,
		Video
	};

	public Texture2D studio;
	private float studioTime = 4;
	public Texture2D credits;
	private float creditsTime = 4;
	public Texture2D cantDetect;
	public Texture2D noFriends;
	
	public Material whiteMaterial;
	public GameObject background;
	
	private float time= 0;
	private IntroState state = IntroState.Detecting;
	
	private Control control;
	
	private bool finished = false;
	private int count = 0;
	
	
	// Use this for initialization
	void Start () {
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		SetState(IntroState.Detecting);
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		
		count = WiiMoteControl.wiimote_count();	
	
		switch (state) {
		case IntroState.Detecting:
			if (WiiMoteControl.wiimote_count() > 1) SetState(IntroState.Studio);
			break;
		case IntroState.Studio:
			if (time > studioTime) SetState(IntroState.Credits);
			break;
		case IntroState.Credits:
			if (time > creditsTime) SetState(IntroState.Video);
			break;
		case IntroState.Video:
			if (control.Accept()) {
				Application.LoadLevel(Application.loadedLevel + 1);
			}
			if (!((MovieTexture)renderer.material.mainTexture).isPlaying && !audio.isPlaying) {
				Application.LoadLevel(Application.loadedLevel + 1);
			}
			break;
		}
	}
	
	void SetState (IntroState s) {
		state = s;
		time = 0;
		
		if (s == IntroState.Studio) {
			background.renderer.material = whiteMaterial;
		}
		
		if (s == IntroState.Video) {
			renderer.enabled = true;
			Play();
		}
	}
	
	void Play() {
		if (!((MovieTexture)renderer.material.mainTexture).isPlaying) {
			((MovieTexture) renderer.material.mainTexture).Play();
			//audio.Play();
		}		
		audio.Play();
	}
	
	void OnGUI() {
		Texture2D tex;
		Vector2 size;
		switch (state) {
			case IntroState.Detecting:
				if (count == 0) tex = cantDetect;
				else tex = noFriends;
				break;
			case IntroState.Studio:
				tex = studio;
				break;
			case IntroState.Credits:
				tex = credits;
				break;
			default:
				tex = null;
				break;
		}		
		
		if (tex != null) {
			size = new Vector2(Mathf.Min(tex.width, Screen.width), Mathf.Min(tex.height, Screen.height));
			GUI.DrawTexture(new Rect(Screen.width/2 - size.x/2, Screen.height/2 - size.y/2, size.x, size.y), tex);
		}
	}
	
	void SetPlayer (int p) {
		var player = p;
	}
}
