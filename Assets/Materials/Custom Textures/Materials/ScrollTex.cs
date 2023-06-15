using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTex : MonoBehaviour {

	public float ScrollX = 0.5f;
	public float ScrollY = 0.5f;
	public int spinSpeed = 360;

	// Update is called once per frame
	void Start () {
		float OffsetX = Time.time * ScrollX;
		float OffsetY = Time.time * ScrollY;
		 transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
	}
}
