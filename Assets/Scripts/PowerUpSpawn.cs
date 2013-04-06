using UnityEngine;
using System.Collections;

public class PowerUpSpawn : MonoBehaviour {
	
	private float time;
	private Transform[] SpawnPoints;
	public GameObject PowerUpType1;
	public GameObject PowerUpType2;
	public Texture2D[] PowerUps;
	
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
		if (time >= 4f) {
			time -= 4f;
			int pos = Random.Range(0, 5);
			SphereGizmos sG = SpawnPoints[pos].GetComponent<SphereGizmos>();
			if (!sG.isInstantiated() && !SpawnPoints[pos].GetComponent<SpawnPrevention>().isSomeoneThere()) {
				sG.is_instanced(true);
				if (pos < 3) {
					GameObject c = (GameObject)Instantiate(PowerUpType1, SpawnPoints[pos].position, SpawnPoints[pos].rotation);
					c.transform.GetChild(0).renderer.material.mainTexture = PowerUps[pos];
					c.transform.GetChild(1).renderer.material.mainTexture = PowerUps[pos];
					ClockGestor rC = c.GetComponent<ClockGestor>();
					SimpleIDHandler iDH = c.GetComponent<SimpleIDHandler>();
					iDH.setID(pos);
					Debug.Log(iDH.getID ());
					rC.setSpawner(SpawnPoints[pos]);
				}
				else {		
					GameObject c = (GameObject)Instantiate(PowerUpType2, SpawnPoints[pos].position, SpawnPoints[pos].rotation);
					c.transform.GetChild(0).renderer.material.mainTexture = PowerUps[pos];
					c.transform.GetChild(1).renderer.material.mainTexture = PowerUps[pos];
					ClockGestor rC = c.GetComponent<ClockGestor>();
					SimpleIDHandler iDH = c.GetComponent<SimpleIDHandler>();
					iDH.setID(pos);
					Debug.Log(iDH.getID ());
					rC.setSpawner(SpawnPoints[pos]);
				}
			}
		}
	}
}