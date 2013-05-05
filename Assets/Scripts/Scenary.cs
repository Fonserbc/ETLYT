using UnityEngine;
using System.Collections;

public class Scenary : MonoBehaviour {
	
	public Vector3[] initPosition;
	public AudioSource selection;
	
	private bool started = false;
	
	private float time = 1.5f;
	private float time2 = 0;
	
	private int numPlayers;
	private int cont = 1;
	
	// Use this for initialization
	void Start () {
		selection.Play();
		BattleInformer bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();
		numPlayers = bi.getNumPlayers();
		time2 = time/numPlayers;//debugar
		Debug.Log(time2);
	}
	
	void Update() {
		if(!started){
			time2 -= Time.deltaTime;
			if(time2 <= 0) {
				time2 += time/numPlayers;
				BattleInformer bi = GameObject.FindGameObjectWithTag("BattleInformer").GetComponent<BattleInformer>();
				bi.initPlayers(initPosition[cont],cont);
				++cont;
				if(cont == numPlayers) {
					started = true;
					bi.initFight();
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
	

}
