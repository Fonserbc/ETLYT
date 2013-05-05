using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {
	
	public enum IntroState {
		Studio,
		Credits,
		Video
	};
	
	private bool needsPlay = false;

	public Texture2D studio;
	private float studioTime = 3;
	public Texture2D credits;
	private float creditsTime = 4;
	
	private float time= 0;
	private IntroState state = IntroState.Studio;
	
	private Control control;
	
	private bool finished = false;
	
	
	// Use this for initialization
	void Start () {
		SetState (IntroState.Studio);
		control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		
		switch (state) {
		case IntroState.Studio:
			if (time > studioTime) SetState(IntroState.Credits);
			break;
		case IntroState.Credits:
			if (time > creditsTime) SetState(IntroState.Video);
			break;
		case IntroState.Video:
			if (control.Jump(0)) {
				Application.LoadLevel(Application.loadedLevel + 1);
			}
			if (needsPlay && !((MovieTexture)renderer.material.mainTexture).isPlaying && !audio.isPlaying) {
				
				time = -2f;
				finished = true;
			}
			break;
		}
		
		if (needsPlay) {
			if (!((MovieTexture)renderer.material.mainTexture).isPlaying) {
				((MovieTexture) renderer.material.mainTexture).Play();
				//audio.Play();
			}
			if (time > 0f && finished) Application.LoadLevel(Application.loadedLevel + 1);
		}
		else {
			if (((MovieTexture)renderer.material.mainTexture).isPlaying) {
				((MovieTexture)renderer.material.mainTexture).Stop();
				audio.Stop();
			}
		}
	}
	
	void SetState (IntroState s) {
		state = s;
		time = 0;
		
		if (s == IntroState.Video) {
			renderer.enabled = true;
			Play();
			Debug.Log("Play");
		}
	}
	
	void Play() {
		needsPlay = true;
		audio.Play();
	}
	
	void OnGUI() {
		Texture2D tex;
		Vector2 size;
		switch (state) {
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
