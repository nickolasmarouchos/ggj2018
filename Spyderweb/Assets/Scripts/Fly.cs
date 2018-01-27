using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

    WebNode node;
    public GameObject[] models;
    bool isEscaping = true;

	public float lifetime;
	public float life;


	// Use this for initialization
	void Start () {
		life = lifetime;	
	}

	public void Init(WebNode node, float lifetimeModifier)
	{
		this.node = node;
        lifetime *= lifetimeModifier;
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
        foreach (GameObject model in models)
            model.SetActive(false);
        isEscaping = false;
    }

    public void Die()
    {
        //Debug.Log("Fly dead");
        node.EatFly();
    }

    internal void Init(WebNode webNode, object flyLifeTimeModifier)
    {
        throw new NotImplementedException();
    }
}
