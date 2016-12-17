using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMouse : MonoBehaviour {

    public float speed = 1.5f;
    private Vector3 target;
    private bool moving = false;

    void Start () {
      target = transform.position;
    }

    void Update () {
      if (Input.GetMouseButtonDown(0) && !moving) {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;
        moving = true;
      }
      transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
      if( transform.position == target){
        moving = false;
      }
    }
}
