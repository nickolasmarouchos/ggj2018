﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebConnection : MonoBehaviour {
	public WebNode node1;
	public WebNode node2;
    private WebNode origin;
    private WebNode target;

    // Use this for initialization
    void Start () {
		
	}
	
    internal void Init(WebNode origin, WebNode target)
    {
        this.origin = origin;
        this.target = target;

        transform.localPosition = origin.transform.localPosition;
    }

	// Update is called once per frame
	void Update () {
		
	}

}
