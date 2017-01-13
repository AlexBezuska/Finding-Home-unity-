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
    public static PlayerCharacter instance;
    public float interactionRadius = 1.5f;
    void OnDrawGizmosSelected() {
      Gizmos.color = Color.blue;
      Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1,1,0));
      Gizmos.DrawWireSphere(Vector3.zero, interactionRadius);
    }

    public bool canTalk = true;

    void Start() {

      instance = this;

    }

    void Update () {

      if (Input.GetMouseButtonDown(0)  && canTalk){
        //Debug.Log("clicked");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if(hit.collider != null){
          if(hit.transform.tag == "click-target"){
            //Debug.Log("clicked npc");
            CheckForNearbyNPC ();
          }
        }
      }

    }

    public void CheckForNearbyNPC (){
      /* Find all DialogueParticipants, and filter them to
      those that have a Yarn start node and are in range;
      then start a conversation with the first one */
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
    }
  }