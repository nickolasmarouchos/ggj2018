using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour {

    public WebNode webNodePrototype;

    Camera mainCam;
    List<WebNode> nodes;

	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        nodes = new List<WebNode>();
    }
	
	// Update is called once per frame
	void Update () {

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            screenPos.z = 0f;
            RaycastHit2D hit = Physics2D.Raycast(screenPos, -Vector2.up);
            if (hit.collider != null)
            {
                Debug.Log("hit object " + hit.collider.gameObject.name);
            }
            
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
            Input.GetMouseButtonUp(0))
		{
            WebNode node = GameObject.Instantiate<WebNode>(webNodePrototype);
            Vector3 screenPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            node.transform.position = new Vector3 (screenPos.x, screenPos.y, 0f);
            node.transform.parent = transform;

            node.Init(this);
		}
	
	}





}
