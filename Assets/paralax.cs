using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralax : MonoBehaviour {

  public float offset = 20.0f;
  public GameObject basedOn;
  private Vector3 pos;
  private float width;


	// Use this for initialization
	void Start () {
		pos = transform.position;
    //width = transform.rect.width;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (basedOn.transform.position.x, 0, 0);
	}



}
