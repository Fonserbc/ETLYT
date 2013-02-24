using UnityEngine;
using System.Collections;

public class SphereGizmos : MonoBehaviour {
	
	private bool instantiated = false;
	
	void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
	
	public void is_instanced(bool inst) {
		instantiated = inst;
	}
	
	public bool isInstantiated() {
		return instantiated;
	}
}
