#pragma strict

public var posToGo : Transform;
public var speed : float = 1;


function Start () {

}

function Update () {
	if (posToGo != null) {
		rigidbody.MovePosition(Vector3.Lerp(transform.position, posToGo.position, speed*Time.deltaTime));
	}
}