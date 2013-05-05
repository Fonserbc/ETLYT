using UnityEngine;
using System.Collections;

public class Scenary : MonoBehaviour {
	
	public Vector3[] initPosition;
	public AudioSource selection;
	
	private bool started = false;
	
	private float time = 1.5f;
	private float time2 = 0;
	
	private int numPlayers;
	private int cont = 0;
	BattleInformer bi;
	
	private bool finished = false;
	public Texture2D finish;
	
	private float timefinish = 3.0f;
	
	// Use this for initialization
	void Start () {
		selection.Play();
		bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();
		numPlayers = bi.getNumPlayers();
	}
	
	void Update() {
		if(!started){
			time2 -= Time.deltaTime;
			if(time2 <= 0) {
				time2 += time/numPlayers;
				bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();
				bi.initPlayers(initPosition[cont],cont);
				++cont;
				if(cont == numPlayers) {
					started = true;
					bi.initFight();
				}
			}
		} else {
			if(!finished) {
				numPlayers = bi.getPlayersInGame();
				if(numPlayers == 1) {
					finished = true;
					Time.timeScale = 0.5f;
				}
			} else {
				timefinish -= 2*Time.deltaTime;
				if(timefinish <= 0) {
					Destroy (GameObject.FindGameObjectWithTag("BattleInformer"));
					Destroy (GameObject.FindGameObjectWithTag("Control"));
					Application.LoadLevel("Character Select");
				}
			}
				
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		for(int i = 0; i < initPosition.Length; ++i) {
			Gizmos.DrawWireCube(initPosition[i],Vector3.one);
		}
	}
	
	void OnGUI() {
		if(finished) GUI.DrawTexture(new Rect((Screen.width/2)-200,(Screen.height/2) -200, 400, 400), finish);
	}
	

}
