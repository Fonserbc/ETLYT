using UnityEngine;
using System.Collections;

public class ClockSpawn : MonoBehaviour {
	
	private float time;
	private Transform[] SpawnPoints;
	public GameObject Clock;
	// Use this for initialization
	void Start () {
		SpawnPoints = new Transform[transform.GetChildCount()];
		for (int i = 0; i < SpawnPoints.Length; ++i) {
			SpawnPoints[i] = transform.GetChild(i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time >= 5) {
			time -= 5;
			int pos = Random.Range(0, SpawnPoints.Length-1);
			Instantiate (Clock, SpawnPoints[pos].position, SpawnPoints[pos].rotation);
		}
	}
}
