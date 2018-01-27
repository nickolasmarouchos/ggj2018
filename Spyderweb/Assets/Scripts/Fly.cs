using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

	WebNode node;

	public float lifetime;
	public float life;


	// Use this for initialization
	void Start () {
		lifetime = 1.0f;
		life = lifetime;	
	}

	public void Init(WebNode node)
	{
		this.node = node;
	}

	// Update is called once per frame
	void Update () {
		life -= Time.deltaTime;
		if (life < 0)
			Escape ();
	}

	void Escape()
	{
		this.node.RemoveFly ();
		Destroy (this);
	}
}
