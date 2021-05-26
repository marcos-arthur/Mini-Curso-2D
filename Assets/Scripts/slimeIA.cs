using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeIA : MonoBehaviour
{
    private gameController _GameController;
    private Rigidbody2D slimeRb;
    private Animator slimeAnimator;

    public float speed;
    public float timeToWalk;

    public GameObject hitBox;
    public bool isLookingLeft;

    private int h;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(gameController)) as gameController;

        slimeRb = GetComponent<Rigidbody2D>();
        slimeAnimator = GetComponent<Animator>();

        StartCoroutine("slimeWalk");
    }

    // Update is called once per frame
    void Update()
    {
        if(_GameController.currentState != gamestate.GAMEPLAY){ return; }

        if(h > 0 && isLookingLeft){
            Flip();
        }else if(h < 0 && !isLookingLeft){
            Flip();
        }

        slimeRb.velocity = new Vector2(h * speed, slimeRb.velocity.y);

        if(h != 0){
            slimeAnimator.SetBool("isWalk", true);
        }else{
            slimeAnimator.SetBool("isWalk", false);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "hitBox"){
            h = 0;
            StopCoroutine("slimeWalk");
            Destroy(hitBox);
            _GameController.playSFX(_GameController.sfxEnemyDead, 0.08f);
            slimeAnimator.SetTrigger("dead");
        }
    }

    IEnumerator slimeWalk(){
        int rand = Random.Range(0,100);
        if(rand < 33){
            h = -1;
        }
        else if (rand < 66){
            h = 0;
        }else{
            h = 1;
        }

        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("slimeWalk");
    }

    void OnDead(){
        Destroy(this.gameObject);
    }

    void Flip(){
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x * -1;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
