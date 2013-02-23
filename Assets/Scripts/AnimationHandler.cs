using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour {
	
	public Texture2D[] idleAnimation;
	public Texture2D[] runAnimation;
	int i = 0;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		i = (i+1)%(idleAnimation.Length*10);
		renderer.material.mainTexture = idleAnimation[i/10];
	}
}
