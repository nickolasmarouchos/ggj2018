using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour {

    public WebNode webNodePrototype;

    Camera mainCam;
    List<WebNode> nodes;
    bool webBuildMode = false;

	private float minWebDistance = 1f;

	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
        nodes = new List<WebNode>();
		InitialSetup ();
    }

	private void InitialSetup()
	{
		CreateNewNode (0f, 0f);
		CreateNewNode (2f, 0f);
		CreateNewNode (1f, 2f);
		CreateNewNode (-1f, 2f);
		CreateNewNode (-2f, 0f);
		CreateNewNode (-1f, -2f);
		CreateNewNode (1f, -2f);
	}

	// Update is called once per frame
	void Update () {

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            Input.GetMouseButtonDown(0))
        {
            if (CheckWebNodeHit())
            {
                ResolveWebNodeHit();
            }
        }

        if (webBuildMode)
        {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
                Input.GetMouseButtonUp(0))
            {
				CreateNewNodeOnClick();
                webBuildMode = false;
            }
        }
    }

    private bool CheckWebNodeHit()
    {
        Vector3 screenPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        //RaycastHit2D hit = Physics2D.Raycast(screenPos, -Vector2.up);
		Debug.Log(Physics.Raycast(screenPos, Vector3.forward));
        if (Physics.Raycast(screenPos, Vector3.forward))
        {
			//Debug.Log("hit object");
			return true;
        }
		return false;
    }

    private void ResolveWebNodeHit()
    {
        webBuildMode = true;
    }

	private void CreateNewNodeOnClick()
    {
		Vector3 screenPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
		CreateNewNode (screenPos.x, screenPos.y);
    }

	private void CreateNewNode(float posX, float posY)
	{
		WebNode node = GameObject.Instantiate<WebNode>(webNodePrototype);

		node.transform.position = new Vector3(posX, posY, 0f);
		node.transform.parent = transform;

		node.Init(this);
		nodes.Add(node);
	}
}
