using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour {
	
	public Texture2D[] idleAnimation;
	public Texture2D[] runAnimation;
	int m_step = 0;
	float m_stepStatus = 0;
	float m_animationSpeed = 0;
	Movement.PlayerState m_actualState;	
	
	void setAnimation (Movement.PlayerState newState, float animationSpeed) {
		m_actualState = newState;
		m_step = 0;
		m_animationSpeed = animationSpeed;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (m_actualState) {
		case Movement.PlayerState.Idle:
			m_stepStatus = m_stepStatus + Time.deltaTime;
			if(m_stepStatus >= m_animationSpeed) {
				m_stepStatus %= m_animationSpeed;
				m_step = (++m_step)%idleAnimation.Length;
			} 
			renderer.material.mainTexture = idleAnimation[m_step];
			break;
		case Movement.PlayerState.Run:
			break;
		default:
			break;
		}
	}
}
