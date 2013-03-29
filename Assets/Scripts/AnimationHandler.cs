using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour {
	
	public Texture2D[] airAnimation;
	public Texture2D[] attackAnimation;	
	public Texture2D[] hurtAnimation;
	public Texture2D[] idleAnimation;	
	public Texture2D[] jumpAnimation;
	public Texture2D[] runAnimation;	
	public Texture2D[] slideAnimation;
	public Texture2D[] wallAnimation;
	
	private int m_step = 0;
	private float m_stepStatus = 0;
	private float m_animationSpeed = 0;
	private Movement.PlayerState m_actualState;	
	private int m_direction = -1;
	
	public void setAnimation (Movement.PlayerState newState, float animationSpeed) {
		m_actualState = newState;
		m_step = 0;
		m_stepStatus = 0;
		m_animationSpeed = animationSpeed;
	}
	
	public void setDirection(int dir) {
		m_direction = dir;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) setAnimation(Movement.PlayerState.Jump, 0.1f);
		m_stepStatus = m_stepStatus + Time.deltaTime;
		switch (m_actualState) {

		case Movement.PlayerState.Air:
			if(m_stepStatus >= m_animationSpeed) {
				m_stepStatus %= m_animationSpeed;
				m_step = (++m_step)%airAnimation.Length;
			} 
			renderer.material.mainTexture = airAnimation[m_step];
			break;
		case Movement.PlayerState.Attack:
			if(m_stepStatus >= m_animationSpeed) {
				m_stepStatus %= m_animationSpeed;
				m_step = (++m_step)%attackAnimation.Length;
			} 
			renderer.material.mainTexture = attackAnimation[m_step];
			break;
		case Movement.PlayerState.Hurt:
			if(m_stepStatus >= m_animationSpeed) {
				m_stepStatus %= m_animationSpeed;
				m_step = (++m_step)%hurtAnimation.Length;
			} 
			renderer.material.mainTexture = hurtAnimation[m_step];
			break;
		case Movement.PlayerState.Idle:
			if(m_stepStatus >= m_animationSpeed) {
				m_stepStatus %= m_animationSpeed;
				m_step = (++m_step)%idleAnimation.Length;
			} 
			renderer.material.mainTexture = idleAnimation[m_step];
			break;
		case Movement.PlayerState.Jump:
			if(m_stepStatus >= m_animationSpeed) {
				m_stepStatus %= m_animationSpeed;
				if (m_step <= 5) m_step = (++m_step)%jumpAnimation.Length;
				else if (m_step == 6) m_step = 7;
				else m_step = 6;
			} 
			renderer.material.mainTexture = jumpAnimation[m_step];
			break;
		case Movement.PlayerState.Run:
			if(m_stepStatus >= m_animationSpeed) {
				m_stepStatus %= m_animationSpeed;
				m_step = (++m_step)%runAnimation.Length;
			} 
			renderer.material.mainTexture = runAnimation[m_step];
			break;
		case Movement.PlayerState.Slide:
			if(m_stepStatus >= m_animationSpeed) {
				m_stepStatus %= m_animationSpeed;
				m_step = (++m_step)%slideAnimation.Length;
			} 
			renderer.material.mainTexture = slideAnimation[m_step];
			break;
		case Movement.PlayerState.Wall:
			if(m_stepStatus >= m_animationSpeed) {
				m_stepStatus %= m_animationSpeed;
				m_step = (++m_step)%wallAnimation.Length;
			} 
			renderer.material.mainTexture = wallAnimation[m_step];
			break;
		default:
			break;
		}
		renderer.material.mainTextureScale = new Vector2((float)m_direction, 1f);
	}
	
	Movement.PlayerState getAnimationState() {
		return m_actualState;	
	}
	
	void setAnimationSpeed(float speed) {
		m_animationSpeed = speed;
	}
	
}
