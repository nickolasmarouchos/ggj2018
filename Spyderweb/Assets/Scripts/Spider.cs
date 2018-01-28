using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spider : MonoBehaviour {

    public float moveSpeed = 0.1f;
    public float eatDuration = 1f;
	public GameObject blood;

	public GameObject eatingBarPrototype;
	GameObject eatingBar;

    WebController controller;
    WebNode currentNode;
    List<WebNode> path;

    bool isMoving = false;
    bool isEating = false;
    DateTime eatStart;
    bool isDying = false;

	// Use this for initialization
	void Start () {
        path = new List<WebNode>();

    }

    internal void Init(WebController controller, WebNode node)
    {
        this.controller = controller;
        currentNode = node;
        currentNode.StartMovingToTile();
        transform.localPosition = node.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (!isMoving && currentNode.HasFly())
        {
            if (isEating)
                ContinueEating();
            else
                StartEating(currentNode);
        }
        else if (path.Count > 0)
        {
            if (!isMoving)
                StartMoving();
            else
                MoveTo(path[0]);

            if (path.Count > 0 && currentNode == path[0])
                path.RemoveAt(0);
        }
    }

    private void StartEating(WebNode node)
    {
        node.StartEating();
        isEating = true;
        eatStart = DateTime.Now;

		this.gameObject.GetComponent<Animation>().Play("SpiderEat");

		eatingBar = GameObject.Instantiate<GameObject> (eatingBarPrototype);
		eatingBar.transform.parent = transform;

		Vector3 screenPos = this.controller.mainCam.WorldToScreenPoint(this.transform.position);
		eatingBar.transform.position = new Vector3(screenPos.x,screenPos.y, 0f);
		eatingBar.transform.parent = this.controller.GameUI.transform;
    }

    private void ContinueEating()
	{
		if ((DateTime.Now - eatStart).TotalSeconds > eatDuration) 
		{
			FinishEating ();
		}
		eatingBar.gameObject.GetComponent<Slider>().value = (float) ((DateTime.Now - eatStart).TotalSeconds / eatDuration);
    }

    private void FinishEating()
    {
		//Debug.Log("Finish Eating");
		this.gameObject.GetComponent<Animation>().Play("SpiderIdle");
        currentNode.FinishEating();
		isEating = false;
		blood.SetActive (false);
		blood.SetActive (true);
		Destroy (eatingBar.gameObject);
    }

    private void StartMoving()
    {
        if (controller.CheckConnection(currentNode, path[0]))
        {
            //Debug.Log("Start Moving: Possible");
            isMoving = true;
            path[0].StartMovingToTile();
            currentNode.LeaveTile();
			this.gameObject.GetComponent<Animation>().Play("SpiderWalk");

        }
        else
            path.Clear();
    }

    private void MoveTo(WebNode dest)
    {
        float distance = Vector3.Distance(dest.transform.localPosition, transform.localPosition);
        if (distance < 0.005f)
        {
            currentNode = dest;
            isMoving = false;

			this.gameObject.GetComponent<Animation>().Play("SpiderIdle");
            return;
        }

        Vector3 direction = Vector3.Normalize(dest.transform.localPosition - transform.localPosition);
        transform.Translate(Math.Min(distance, moveSpeed) * direction);
    }

    internal WebNode GetLastPathNode()
    {
        if (path.Count == 0)
            return currentNode;
        else
            return path[path.Count - 1];
    }

    internal void AddToPath(WebNode node)
    {
        path.Add(node);
    }
}
