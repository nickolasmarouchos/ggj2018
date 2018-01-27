using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

	WebNode node;
    bool isEscaping = true;

	public float lifetime;
	public float life;


	// Use this for initialization
	void Start () {
		lifetime = 5.0f;
		life = lifetime;	
	}

	public void Init(WebNode node)
	{
		this.node = node;
	}

	// Update is called once per frame
	void Update () {
        if (isEscaping)
    		life -= Time.deltaTime;
		if (life < 0)
			Escape ();
	}

	void Escape()
	{
		node.EscapeFly ();
    }

    internal void StopEscaping()
    {
       // Debug.Log("Stop Escaping");
        isEscaping = false;
    }

    public void StartEating()
    {
        //Debug.Log("Is being eaten");
        isEscaping = false;
    }

    public void Die()
    {
        //Debug.Log("Fly dead");
        node.EatFly();
    }
}
