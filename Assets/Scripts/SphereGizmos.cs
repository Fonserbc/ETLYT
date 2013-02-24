using UnityEngine;
using System.Collections;

public class SphereGizmos : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
}
