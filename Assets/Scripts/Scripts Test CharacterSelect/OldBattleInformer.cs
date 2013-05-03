using UnityEngine;
using System.Collections;

public class OldBattleInformer : MonoBehaviour {
	
	private int playerCount;
	private GameObject[] players; //personaje escogido
	private float[] life;
	
	private bool start = false;
	
	public void setPlayer(GameObject p, int i) {
		players[i] = p;
	}
	
	public void setLife(float f, int i) {
		life[i] = f;
	}
	
	public void startBattle() {
		for (int i=0; i<=playerCount-1; i++) {
			Instantiate(players[i], new Vector3(), players[i].transform.rotation);
		}
		start = true;	
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(transform.gameObject);
		Control control = GameObject.FindGameObjectWithTag("Control").GetComponent<Control>();
		playerCount = WiiMoteControl.wiimote_count();
		players = new GameObject[playerCount];
	}
	
	// Update is called once per frame
	void Update () {
		if(!start) {
			
		}	
	}
}
