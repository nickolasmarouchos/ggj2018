using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebController : MonoBehaviour {

    public WebNode webNodePrototype;
    public WebConnection webConnectionPrototype;
		public Canvas GameUI;

	public float minWebDistance = 1f; // now it's changable at runtime for Unity shenanigans :3
    public float maxWebDistance = 4f; // now it's changable at runtime for Unity shenanigans :3

    public Camera mainCam;
    private List<WebNode> nodes;
    private Dictionary<WebNode, List<WebNode>> connections;

    private bool webBuildMode = false;
    private WebNode originNode;
    private object neighbours;

    // Use this for initialization
    void Start () {
        this.mainCam = Camera.main;
        nodes = new List<WebNode>();
        connections = new Dictionary<WebNode, List<WebNode>>();
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
            WebNode origin = CheckWebNodeHit();
            if (origin != null)
            {
                ResolveWebNodeHit(origin);
            }
        }

        if (webBuildMode)
        {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
                Input.GetMouseButtonUp(0))
            {
                CreateNewNodeOnClick();
                DisableBuildMode();
            }
        }
    }

    private void EnableBuildMode(WebNode origin)
    {
        webBuildMode = true;
        originNode = origin;
    }

    private void DisableBuildMode()
    {
        webBuildMode = false;
        originNode = null;
    }

    private WebNode CheckWebNodeHit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider != null) { 
            WebNode node = hit.collider.GetComponent<WebNode>();
            if (node != null) { 
                return node;
            }
        }
		return null;
    }

    private void ResolveWebNodeHit(WebNode origin)
    {
        EnableBuildMode(origin);
    }

    private void CreateNewNodeOnClick()
    {
		Vector3 screenPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
		CreateNewNode (screenPos, originNode, true);
    }

    private void CreateNewNode(float x, float y)
    {
        CreateNewNode(new Vector3(x, y, 0f));
    }

	private void CreateNewNode(Vector3 pos, WebNode origin = null, bool mustConnect = false)
	{
        Vector3 pos2d = new Vector3(pos.x, pos.y, 0f);
        List<WebNode> neighbours = FindNodesInRange(minWebDistance, maxWebDistance, pos2d);
        if (neighbours.Count == 0 && mustConnect)
            return;

		WebNode node = GameObject.Instantiate<WebNode>(webNodePrototype);

        node.transform.position = pos2d;
		node.transform.parent = transform;

		node.Init(this);
		nodes.Add(node);
        connections.Add(node, new List<WebNode>());

        foreach (WebNode neighbour in neighbours)
        {
            CreateConnection(neighbour, node);
        }
	}

    private void CreateConnection(WebNode origin, WebNode target)
    {
        // visual object
        WebConnection connection = GameObject.Instantiate<WebConnection>(webConnectionPrototype);
        connection.Init(origin, target);

        // logic object
        AddConnection(origin, target);
        AddConnection(target, origin);
    }

    private void AddConnection (WebNode self, WebNode other)
    {
        if (connections[self].Contains(other))
            connections[self].Add(other);
    }

    private List<WebNode> FindNodesInRange(float minWebDistance, float maxWebDistance, Vector3 origin)
    {
        List<WebNode> neighbours = new List<WebNode>();
        foreach (WebNode node in nodes)
        {
            float distance = Vector3.Distance(origin, node.transform.localPosition);
            if (distance > minWebDistance && distance < maxWebDistance)
                neighbours.Add(node);
        }

        return neighbours;
    }
}
