using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebConnection : MonoBehaviour {

    public Transform line;

    public WebNode node1;
	public WebNode node2;

    private WebNode origin;
    private WebNode target;
    private bool isDecaying = false;


    // Use this for initialization
    void Start () {
		
	}

    internal void Init(WebNode origin, WebNode target)
    {
        this.origin = origin;
        this.target = target;

        float length = Vector3.Distance(origin.gameObject.transform.localPosition, target.gameObject.transform.localPosition);

        transform.localPosition = origin.transform.localPosition;

        transform.LookAt(target.transform);
        line.localScale = new Vector3(line.localScale.x,
                                      line.localScale.y,
                                      length);
        line.localPosition = new Vector3(line.localPosition.x,
                                         line.localPosition.z,
                                         length * 0.5f);
    }

    public void SetDecaying()
    {
        isDecaying = true;
    }

    // Update is called once per frame
    void Update () {
		if (isDecaying)
        {
            AnimateDecay();
        }
	}

    void AnimateDecay()
    {
        Destroy(gameObject);
    }
}
