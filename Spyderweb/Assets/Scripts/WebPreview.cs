using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebPreview : MonoBehaviour {

    public Transform line;

    private WebNode origin;
	private Vector3 target;
    private bool isDecaying = false;


    // Use this for initialization
    void Start () {
		
	}

	public void updateVisual(WebNode origin, Vector3 target)
	{
		this.origin = origin;
		this.target = target;

        if (origin == null ||
            target == null)
            line.transform.eulerAngles = Vector3.up;

//		float length = Vector3.Distance(origin.gameObject.transform.localPosition, target);
		float xDelta = origin.gameObject.transform.localPosition.x - target.x;
		float yDelta = origin.gameObject.transform.localPosition.y - target.y;
		float length = (float) Math.Sqrt (Math.Pow (xDelta, 2) + Math.Pow (yDelta, 2));

		line.localPosition = new Vector3 (origin.transform.localPosition.x / 2 + target.x / 2,
			origin.transform.localPosition.y / 2 + target.y / 2, 0f);

		line.localScale = new Vector3(length,line.localScale.y,line.localScale.z);
		
//		line.localPosition = new Vector3(line.localPosition.x,			line.localPosition.z,			length * 0.5f);

		float opp = origin.transform.position.y - target.y;
		float hyp = length;
		double soh = (double) (opp / hyp);
		//line.transform.localRotation = Math.Asin (soh);

		Debug.Log (xDelta + "    " + yDelta + "    " + soh);
		if (xDelta < 0 || yDelta < 0)
			soh = -soh;
		if (xDelta > 0 && yDelta < 0)
			soh = -soh;
		float angle = (float) Math.Asin (soh) * Mathf.Rad2Deg;

		//Debug.Log (angle);

		line.transform.eulerAngles = new Vector3(0, 0, angle);

	}

}
