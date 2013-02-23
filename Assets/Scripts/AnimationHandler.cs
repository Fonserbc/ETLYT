using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour {
	
	public Texture2D[] idleAnimation;
	public Texture2D[] runAnimation;
	int m_step = 0;
	int m_stepStatus = 0;
	int m_animationSpeed = 0;
//	enum animation = idle;	
	
	void setAnimation () {
//		m_animation = animation
		m_step = 0;
		m_speed = animationSpeed;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (animation) {
		case idle:
			m_stepStatus = m_stepStatus + m_animationSpeed;

			if(m_stepStatus >= 100) {
				m_stepStatus %= 100;
				m_step = (++m_step)%idleAnimation.Length;
			} 
			renderer.material.mainTexture = idleAnimation[m_step];
			break;
		case run:
			break;
		default:
			break;
		}
	}
}
