#pragma strict

public var speed : float = 1;
public var intensity : float = 1;
public var offset : float = 0;

private var originalPos : Vector3;

function Start () {
	originalPos = transform.position;
}

function Update () {
	transform.position = originalPos + Vector3.up*intensity*Mathf.Sin(Time.time*speed + offset);
}