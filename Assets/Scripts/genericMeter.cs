using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class genericMeter : MonoBehaviour {

	public RectTransform meter { get; private set; }
	public float maxSize = 290f;
	
	// Use this for initialization
	void Start () {
		meter = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setMeter(float percent) {
		meter.offsetMax = new Vector2 (-maxSize + (Mathf.Clamp01(percent) * maxSize), meter.offsetMax.y);
	}

	public void setNeedle(float percent) {
		meter.localRotation = Quaternion.Euler (0f, 0f, 60f - (120 * Mathf.Clamp01 (percent)));
	}
}
