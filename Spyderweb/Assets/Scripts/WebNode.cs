using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WebNode : MonoBehaviour {

	public Fly flyPrototype;
	public GameObject flyBarPrototype;
    public float timerMin = 2f;
    public float timerMax = 20f;

    WebController controller;
	float flyTimer;
	float flyTimerTotal;

	Fly trappedFly;
	GameObject flyBar;
    private bool pauseFlySpawn = false;

    // Use this for initialization
    void Start () {
		StartFlyTimer ();
	}

    public void Init(WebController controller)
    {
        this.controller = controller;
    }

	void StartFlyTimer()
	{
        if (HasFly() == false)
        {

            flyTimer = UnityEngine.Random.Range(timerMin * 6f/controller.maxNodeCount, timerMax * 6f/controller.maxNodeCount);
			flyTimerTotal = flyTimer;
		}
	}

    // Update is called once per frame
    void Update () {
        if (!pauseFlySpawn)
        {
            flyTimer -= Time.deltaTime;

            if (HasFly())
            {
                flyBar.gameObject.GetComponent<Slider>().value = trappedFly.life / trappedFly.lifetime;
            }
            if (flyTimer < 0 && !HasFly())
            {
                AddFly();
            }
        }
	}

    internal bool HasFly()
    {
        return trappedFly != null;
    }

    void AddFly()
	{
		trappedFly = GameObject.Instantiate<Fly>(flyPrototype);
		trappedFly.transform.parent = transform;
		trappedFly.transform.localPosition = new Vector3 (0, 0, 0);
		trappedFly.Init(this, controller.GetFlyLifeTimeModifier());

		flyBar = GameObject.Instantiate<GameObject> (flyBarPrototype);
		flyBar.transform.parent = transform;

		Vector3 screenPos = this.controller.mainCam.WorldToScreenPoint(this.transform.position);
		flyBar.transform.position = new Vector3(screenPos.x,screenPos.y, 0f);
		flyBar.transform.parent = this.controller.GameUI.transform;
	}

    public void StartMovingToTile()
    {
        if (trappedFly)
            trappedFly.StopEscaping();
        pauseFlySpawn = true;
    }

    public void LeaveTile()
    {
        pauseFlySpawn = false;
    }

    internal void StartEating()
    {
        trappedFly.StartEating();
		Destroy (flyBar.gameObject);
    }

    internal void FinishEating()
    {
        trappedFly.Die();
    }

    internal void EatFly()
    {
        RemoveFly();
        controller.EatFly();
        StartFlyTimer();
    }

	public void EscapeFly()
	{
        RemoveFly();
        controller.DestroyNode(this);
		Destroy (this.gameObject);
	}

    void RemoveFly ()
    {
        Destroy(trappedFly.gameObject);
        Destroy(flyBar.gameObject);
        trappedFly = null;
        flyBar = null;
    }
}
