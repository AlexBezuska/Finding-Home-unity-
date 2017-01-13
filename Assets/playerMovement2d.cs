using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;

public class playerMovement2d : MonoBehaviour {

    public float speed = 1.5f;
    private Vector3 target;
    private Animator animator;
    private SpriteRenderer mySpriteRenderer;
    private ParticleSystem snailTrail;
    //UnityEngine.AI.NavMeshAgent agent;
    float waitAfterTalk = 1f;

    void Start() {
      //agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
      animator = GetComponent<Animator>();
      target = transform.position;
      mySpriteRenderer = GetComponent<SpriteRenderer>();
      snailTrail = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    void Update () {

      if(PlayerCharacter.instance.canTalk == false){
        waitAfterTalk -= Time.deltaTime;
        //Debug.Log(waitAfterTalk);
      }

      if(waitAfterTalk < 0){
        PlayerCharacter.instance.canTalk = true;
        waitAfterTalk = 1f;
      }

      if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true) {
        // For animator state machine
        animator.SetBool("moving", false);
        target = transform.position;
        snailTrail.emissionRate = 0;
        PlayerCharacter.instance.canTalk = false;
        return;
      }

      if (Input.GetMouseButton(0) && PlayerCharacter.instance.canTalk) {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;
      }

      if (transform.position != target){
        // direction to face
        if (target.x < transform.position.x){
          mySpriteRenderer.flipX = true;
        } else {
          mySpriteRenderer.flipX = false;
        }
        // For animator state machine
        animator.SetBool("moving", true);
        snailTrail.emissionRate = 20;

      } else{
        // For animator state machine
        animator.SetBool("moving", false);
        snailTrail.emissionRate = 0;
      }

      transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
  }