using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_touches_photo : MonoBehaviour {
   private SpriteRenderer sprite;

  void Start( ) {
    sprite = transform.GetComponentInParent<SpriteRenderer>();
    sprite.enabled = false;
  }

  void OnCollisionEnter2D (Collision2D col) {
    Debug.Log("enter " + col.gameObject.name);
    if (col.gameObject.name == "Player") {
      sprite.enabled = true;
    }
  }

  void OnCollisionExit2D (Collision2D col) {
    Debug.Log("exit " + col.gameObject.name);
    if (col.gameObject.name == "Player") {
      sprite.enabled = false;
    }
  }


}
