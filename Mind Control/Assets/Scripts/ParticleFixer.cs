using UnityEngine;
using System.Collections;

public class ParticleFixer : MonoBehaviour {

	public string LayerName = "ParticleLayer";

	// Use this for initialization
	void Start () 
	{
		GetComponent<ParticleSystem> ().GetComponent<Renderer> ().sortingLayerName = LayerName;
	}
	

}
