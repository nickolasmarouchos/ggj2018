using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebController : MonoBehaviour {

    public WebNode webNodePrototype;
    public WebConnection webConnectionPrototype;
	public WebPreview webPreviewPrototype;
    public Spider spiderPrototype;
	public Canvas GameUI;
    public Text scoreText;
    public Slider spiderHealthBar;
    public ScoreDisplay scoreDisplayPrototype;
	public GameObject nodeBreakEffect;

	public float minWebDistance = 1f; // now it's changable at runtime for Unity shenanigans :3
    public float maxWebDistance = 4f; // now it's changable at runtime for Unity shenanigans :3

    public Camera mainCam;
    private List<WebNode> nodes;
    private Dictionary<WebNode, Dictionary<WebNode, WebConnection>> connections;

    private bool webBuildMode = false;
	private bool webPreviewMode = false;
	private WebNode originNode;
    private Spider spider;
	private WebPreview previewWeb;
	private WebNode previewOrigin;

    public float spiderPower = 10f;
    public float spiderPowerPerFly = 5f;
    public float webCostModifier = 1f;

    private ScoreController scoreController;
    public int maxNodeCount;
    private float flyLifeTimeModifier;
    internal GameStateManager stateManager;

    // Use this for initialization
    void Start () {
        this.mainCam = Camera.main;
        nodes = new List<WebNode>();
        connections = new Dictionary<WebNode, Dictionary<WebNode, WebConnection>>();
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

        maxNodeCount = 7;

        spider = GameObject.Instantiate<Spider>(spiderPrototype);
        spider.Init(this, nodes[0]);
        spider.transform.SetParent(transform);

        ScoreDisplay scoreDisplay = GameObject.Instantiate<ScoreDisplay>(scoreDisplayPrototype);
        scoreDisplay.textBox = scoreText;
        scoreController = new ScoreController(scoreDisplay);
        scoreDisplay.transform.SetParent(transform);
	}

    // Update is called once per frame
    void Update () {

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) ||
            Input.GetMouseButtonDown(0))
        {
            WebNode nodeHit = CheckWebNodeHit();
            if (nodeHit != null)
            {
                ResolveWebNodeHit(nodeHit);
            }
        }

		if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) ||
			Input.GetMouseButton(0))
		{
			//Update Preview
			if (webBuildMode)
				UpdateWebPreview();
		}

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) ||
            Input.GetMouseButtonUp(0))
        {
            if (webBuildMode)
            {
                WebNode nodeReleasedUpon = CheckWebNodeHit();
                if (nodeReleasedUpon == originNode)
                    TryMoveToNode(nodeReleasedUpon);
                else
                    TryExpandWeb();
            }
        }

        spiderHealthBar.value = spider.GetHealth();
    }

    private void TryMoveToNode(WebNode target)
    {
        WebNode lastNode = spider.GetLastPathNode();
        if (CheckConnection(lastNode, target))
            spider.AddToPath(target);
        DisableBuildMode();
    }

    public bool CheckConnection(WebNode origin, WebNode target)
    {
        if (origin == null || target == null)
            return false;

        if (connections.ContainsKey(origin))
            return connections[origin].ContainsKey(target);

        return false;
    }

    private void TryExpandWeb()
	{
		DisableWebPreview ();
        CreateNewNodeOnClick();
		DisableBuildMode();
    }

    public void DestroyNode(WebNode node)
    {
        foreach (Dictionary<WebNode, WebConnection> dict in connections.Values)
        {
            List<WebNode> toRemove = new List<WebNode>();
            foreach (KeyValuePair<WebNode, WebConnection> storedConnection in dict)
            {
                if (node == storedConnection.Key)
                {
                    toRemove.Add(storedConnection.Key);
                    storedConnection.Value.SetDecaying();
                }
            }
            foreach (WebNode nodeToRemove in toRemove)
            {
                dict.Remove(nodeToRemove);
            }
        }
        nodes.Remove(node);
		connections.Remove(node);
		GameObject nodeDestroy = GameObject.Instantiate<GameObject>(nodeBreakEffect);

		nodeDestroy.transform.position = node.transform.position;
        if (originNode == node)
            originNode = null;
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

	private void EnableWebPreview (WebNode origin)
	{
		previewWeb = GameObject.Instantiate<WebPreview> (webPreviewPrototype);
		previewOrigin = origin;
		UpdateWebPreview ();
	}

    private void DisableWebPreview()
	{
		previewOrigin = null;
		Destroy(previewWeb.gameObject);
	}

	private void UpdateWebPreview()
	{
		Vector3 inputLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		previewWeb.updateVisual(previewOrigin,inputLocation);
		previewWeb.transform.SetParent(transform);

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
		EnableWebPreview (origin);
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
        if (mustConnect)
        {
            if (neighbours.Count == 0)
                return;

            if (false == DeductWebCost(originNode.transform.localPosition, pos2d))
                return;

            if (originNode == null)
                return;
        }

		WebNode node = GameObject.Instantiate<WebNode>(webNodePrototype);

        node.transform.position = pos2d;
		node.transform.SetParent(transform);

		node.Init(this);
		nodes.Add(node);
        maxNodeCount = Math.Max(nodes.Count, maxNodeCount);
        if (scoreController != null)
        {
            scoreController.CreateNode(maxNodeCount);
        }

        connections.Add(node, new Dictionary<WebNode, WebConnection>());


        foreach (WebNode neighbour in neighbours)
        {
            CreateConnection(neighbour, node);
        }

	}

    private bool DeductWebCost(Vector3 origin, Vector3 target)
    {
        float cost = Vector3.Distance(origin, target) * webCostModifier;
        if (spiderPower < cost)
            return false;

        spiderPower -= cost;
        return true;
    }

    public void EatFly()
    {
        spiderPower += spiderPowerPerFly;
        scoreController.EatFly(1f, maxNodeCount);
    }

    private List<WebNode> FindNodesInRange(float minWebDistance, float maxWebDistance, Vector3 origin)
    {
        List<WebNode> neighbours = new List<WebNode>();
        foreach (WebNode node in nodes)
        {
            float distance = Vector3.Distance(origin, node.transform.localPosition);
            if (distance < minWebDistance)
            {
                neighbours.Clear();
                return neighbours;
            }
            if (distance < maxWebDistance)
                neighbours.Add(node);
        }

        return neighbours;
    }
		

	public void SendVibrationFromNode(WebNode origin, int num)
	{


		Dictionary<WebNode, WebConnection> nodeConnections = connections [origin];
		foreach (WebConnection webtoVibrate in nodeConnections.Values)
		{
			webtoVibrate.gameObject.GetComponent<Animation> ().Play ("webVibrate1");
		}
		/*
		foreach (WebNode nodeToPropogate in nodeConnections.Keys)
		{
			nodeToPropogate.Vibrate (num);
		}
		*/
	}

    private void CreateConnection(WebNode origin, WebNode target)
    {
        // visual object
        WebConnection connection = GameObject.Instantiate<WebConnection>(webConnectionPrototype);
        connection.Init(origin, target);
        connection.transform.SetParent(transform);

        // logic object
        AddConnection(origin, target, connection);
        AddConnection(target, origin, connection);
    }

    private void AddConnection (WebNode self, WebNode other, WebConnection connection)
    {
        if (!connections[self].ContainsKey(other))
            connections[self].Add(other, connection);
    }

    public float GetFlyLifeTimeModifier()
    {
        return Math.Max(0.3f, Math.Min(1f, 10f/maxNodeCount));
    }

    public void LoseGame()
    {
        stateManager.EndGame(scoreController.GetScore());
    }
}
