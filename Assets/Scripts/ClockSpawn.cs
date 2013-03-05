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
		if (time >= 3) {
			time -= 3;
			int pos = Random.Range(0, SpawnPoints.Length-1);
			SphereGizmos sG = SpawnPoints[pos].GetComponent<SphereGizmos>();
			if (!sG.isInstantiated()) {
				sG.is_instanced(true);
				GameObject c = (GameObject)Instantiate(Clock, SpawnPoints[pos].position, SpawnPoints[pos].rotation);
				ClockGestor rC = c.GetComponent<ClockGestor>();
				Debug.Log (rC == null);
				rC.setSpawner(SpawnPoints[pos]);
			}
		}
	}
}