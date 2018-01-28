using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour {

    WebNode node;
    public GameObject[] models;
    bool isEscaping = true;

    public AudioSource audio;
    public AudioClip[] spawn;
    public AudioClip[] idle;
    public AudioClip[] death;
    public AudioClip[] escape;


	public float lifetime;
	public float life;


	// Use this for initialization
	void Start ()
    {
        life = lifetime;
        audio.Stop();
        PlayAudioSpawn();
    }

    private void PlayAudioSpawn()
    {
        audio.clip = spawn[UnityEngine.Random.Range(0, spawn.Length)];
        audio.mute = false;
        audio.loop = false;
        audio.Play();

        StartCoroutine(PlayAudioIdle());
    }

    private IEnumerator PlayAudioIdle()
    {
        yield return new WaitForSeconds(0.3f);
        audio.clip = idle[UnityEngine.Random.Range(0, idle.Length)];
        audio.mute = false;
        audio.loop = true;
        audio.Play();
    }

    private void PlayAudioDying()
    {
        audio.clip = death[UnityEngine.Random.Range(0, death.Length)];
        audio.mute = false;
        audio.loop = false;
        audio.Play();
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
        Destroy(gameObject, 1f);
        PlayAudioDying();
    }
}
