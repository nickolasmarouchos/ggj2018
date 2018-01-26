using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour {

    public WebNode webNodePrototype;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
            Input.GetMouseButtonUp(0))
		{
            WebNode node = GameObject.Instantiate<WebNode>(webNodePrototype);
            node.transform.parent = transform;
            //node.transform.localPosition = Input.mousePosition;


		}
	
	}





}
