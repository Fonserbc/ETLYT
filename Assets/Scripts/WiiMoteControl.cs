using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class WiiMoteControl : MonoBehaviour {
	
	[DllImport ("UniWii")]
	private static extern void wiimote_start();

	[DllImport ("UniWii")]
	private static extern void wiimote_stop();

	[DllImport ("UniWii")]
	public static extern int wiimote_count();
	[DllImport ("UniWii")]
	public static extern bool wiimote_isExpansionPortEnabled( int which );
	[DllImport ("UniWii")]
	public static extern double wiimote_getBatteryLevel( int which );
	
	[DllImport ("UniWii")]
	public static extern byte wiimote_getAccX(int which);
	[DllImport ("UniWii")]
	public static extern byte wiimote_getAccY(int which);
	[DllImport ("UniWii")]
	public static extern byte wiimote_getAccZ(int which);

	[DllImport ("UniWii")]
	public static extern float wiimote_getPitch(int which);
	
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonA(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonB(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonUp(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonLeft(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonRight(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonDown(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButton1(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButton2(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonPlus(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonMinus(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonHome(int which);
	
	[DllImport ("UniWii")]
	public static extern byte wiimote_getNunchuckStickX(int which);
	[DllImport ("UniWii")]
	public static extern byte wiimote_getNunchuckStickY(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonNunchuckC(int which);
	[DllImport ("UniWii")]
	public static extern bool wiimote_getButtonNunchuckZ(int which);
	
	/**
	 * Variables
	 */
	
	//WIIMOTE
	public float sensitivity = 1.0f;
	public float pitchFudge = 30.0f;
	public float maxDeltaPitch = 30.0f;
	
	public int wiimoteCount;

	public float[] pitch;
	public float[] oldPitch;
	public float[] deltaPitch;
	
	// Use this for initialization
	void Start () {
		wiimote_start();
		pitch = new float[2];
		oldPitch = new float[2];
		deltaPitch = new float[2];
	}
	
	void FixedUpdate() {
		wiimoteCount = wiimote_count();
		
		if (wiimoteCount > 0) {
			for (int i = 0; i < min(wiimoteCount,2); ++i) {
				float newPitch = wiimote_getPitch(i) + pitchFudge;
				deltaPitch[i] = 0;
				
				if (!float.IsNaN(newPitch)) {
					pitch[i] = newPitch;
					deltaPitch[i] = pitch[i] - oldPitch[i];
					deltaPitch[i] *= sensitivity;
					if (deltaPitch[i] > 0) deltaPitch[i] = min(maxDeltaPitch, deltaPitch[i]);
					else deltaPitch[i] = max(-maxDeltaPitch, deltaPitch[i]);
					oldPitch[i] = pitch[i];
				}
			}
		}
	}
	
	void OnApplicationQuit() {
		wiimote_stop();
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawLine(Vector3.zero, Physics.gravity);
	}
	
	int min(int x, int y) {
		return (x < y)? x : y;
	}
	
	float min(float x, float y) {
		return (x < y)? x : y;
	}
	
	float max(float x, float y) {
		return (x > y)? x : y;
	}
}

