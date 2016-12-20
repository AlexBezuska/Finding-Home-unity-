/*

The MIT License (MIT)

Copyright (c) 2015 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Yarn.Unity.Example {
  public class PlayerCharacter : MonoBehaviour {

    public float minPosition = -5.3f;
    public float maxPosition = 5.3f;

    public float moveSpeed = 1.0f;

    public float interactionRadius = 2.0f;

    public float movementFromButtons {get;set;}

    // Draw the range at which we'll start talking to people.
    void OnDrawGizmosSelected() {
      Gizmos.color = Color.blue;

      // Flatten the sphere into a disk, which looks nicer in 2D games
      Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1,1,0));

      // Need to draw at position zero because we set position in the line above
      Gizmos.DrawWireSphere(Vector3.zero, interactionRadius);
    }


    public float speed = 1.5f;
    private Vector3 target;
    private Vector3 positionLastFrame;
    private Animator animator;
    private SpriteRenderer mySpriteRenderer;
    private ParticleSystem snailTrail;
    private Color debugLineColor = new Color(1F, 1F, 1F, 0.01F);

    void Start() {
      animator = GetComponent<Animator>();
      target = transform.position;
      positionLastFrame = transform.position;
      mySpriteRenderer = GetComponent<SpriteRenderer>();
      snailTrail = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }



    // Update is called once per frame
    void Update () {

      if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true) {
        // For animator state machine
        animator.SetBool("moving", false);
        target = transform.position;
        return;
      }


      if (Input.GetMouseButton(0)) {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;
      }

      if (transform.position != target){
      //Debug.Log(transform.position);
        //Debug.Log(target);

        // direction to face
        if (target.x < transform.position.x){
          mySpriteRenderer.flipX = true;
        } else {
          mySpriteRenderer.flipX = false;
        }
        // For animator state machine
        animator.SetBool("moving", true);
        snailTrail.Play();

      } else{
        // For animator state machine
        animator.SetBool("moving", false);
        snailTrail.Pause();
      }
      positionLastFrame = transform.position;



    //  DrawLine(transform.position, target, debugLineColor, 0.2f);

      transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

      if (Input.GetMouseButtonDown(0)) {
        Debug.Log("click");
        RaycastHit2D hit = Physics2D.Raycast(target, -Vector2.up);
        if (hit && hit.transform.gameObject.GetComponent("NPC")) {
          CheckForNearbyNPC ();
        }
      }


    }



    public void CheckForNearbyNPC ()
    {
      // Find all DialogueParticipants, and filter them to
      // those that have a Yarn start node and are in range;
      // then start a conversation with the first one
      var allParticipants = new List<NPC> (FindObjectsOfType<NPC> ());
      var target = allParticipants.Find (delegate (NPC p) {
        return string.IsNullOrEmpty (p.talkToNode) == false && // has a conversation node?
        (p.transform.position - this.transform.position)// is in range?
        .magnitude <= interactionRadius;
        });
        if (target != null) {
          // Kick off the dialogue at this node.
          FindObjectOfType<DialogueRunner> ().StartDialogue (target.talkToNode);
        }
      }


      void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
      {
          GameObject myLine = new GameObject();
          myLine.transform.position = start;
          myLine.AddComponent<LineRenderer>();
          LineRenderer lr = myLine.GetComponent<LineRenderer>();
          lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
          lr.SetColors(color, color);
          lr.SetWidth(0.1f, 0.1f);
          lr.SetPosition(0, start);
          lr.SetPosition(1, end);
          GameObject.Destroy(myLine, duration);
      }


    }

  }
