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
	public static extern float wiimote_getIrX(int which);
	[DllImport ("UniWii")]
	public static extern float wiimote_getIrY(int which);
	[DllImport ("UniWii")]
	public static extern float wiimote_getRoll(int which);
	[DllImport ("UniWii")]
	public static extern float wiimote_getPitch(int which);
	[DllImport ("UniWii")]
	public static extern float wiimote_getYaw(int which);
	
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
	
	// Use this for initialization
	void Start () {
		wiimote_start();
	}

	
	void OnApplicationQuit() {
		wiimote_stop();
	}
}

