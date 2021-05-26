using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private gameController _GameController;

    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private SpriteRenderer playerSr;

    public float speed;
    public float jumpForce;

    public bool isLookingLeft;

    public Transform groundCheck;
    private bool isGrounded;
    private bool isAtack;

    public Transform mao;
    public GameObject hitBoxPrefab;

    public Color hitColor;
    public Color noHitColor;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSr = GetComponent<SpriteRenderer>();

        _GameController = FindObjectOfType(typeof(gameController)) as gameController;
        _GameController.playerTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool("isGrounded", isGrounded);

        if(_GameController.currentState != gamestate.GAMEPLAY){
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            playerAnimator.SetInteger("h", 0);
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        
        if(isAtack && isGrounded){
            h = 0;
        }

        if(h > 0 && isLookingLeft){
            Flip();
        }else if(h < 0 && !isLookingLeft){
            Flip();
        }

        float speedY = playerRb.velocity.y;

        if(Input.GetButtonDown("Jump") && isGrounded){
            _GameController.playSFX(_GameController.sfxJump, 0.5f);
            playerRb.AddForce(new Vector2(0, jumpForce));
        }

        if(Input.GetButtonDown("Fire1") && !isAtack){
            _GameController.playSFX(_GameController.sfxAtack, 0.5f);

            isAtack = true;
            playerAnimator.SetTrigger("atack");
        }

        playerRb.velocity = new Vector2(h * speed, speedY);


        playerAnimator.SetInteger("h", (int) h);
        playerAnimator.SetFloat("speedY", speedY);
        playerAnimator.SetBool("isAtack", isAtack);
    }

    void FixedUpdate() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Coletavel"){
            _GameController.playSFX(_GameController.sfxCoin, 0.5f);
            _GameController.getCoin();
            Destroy(col.gameObject);
        }
        else if(col.gameObject.tag == "Damage"){
            _GameController.getHit();
            if(_GameController.vida > 0){
                StartCoroutine("damageController");
            }            
        }
        else if(col.gameObject.tag == "abismo"){
            _GameController.playSFX(_GameController.sfxDamage, 0.5f);
            _GameController.vida = 0;
            _GameController.heartController();
            _GameController.painelGameOver.SetActive(true);
            _GameController.currentState = gamestate.GAMEOVER;
            _GameController.trocarMusica(musicaFase.GAMEOVER);
        }
        else if(col.gameObject.tag == "flag"){
            _GameController.theEnd();
        }
    }

    void Flip(){
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x * -1;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    void onEndAtack(){
        isAtack = false;
    }

    void hitBoxAtack(){
        GameObject hitBoxTemp = Instantiate(hitBoxPrefab, mao.position, transform.localRotation);
        Destroy(hitBoxTemp, 0.2f);
    }

    void footStep(){
        _GameController.playSFX(_GameController.sfxStep[Random.Range(0,_GameController.sfxStep.Length)], 0.5f);
    }

    IEnumerator damageController(){
        _GameController.playSFX(_GameController.sfxDamage, 0.3f);
        this.gameObject.layer = LayerMask.NameToLayer("Invencivel");
        playerSr.color = hitColor;
        yield return new WaitForSeconds(0.2f);
        playerSr.color = noHitColor;
        for(int i = 0; i < 3; i++){
            playerSr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerSr.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        playerSr.color = Color.white;
    }
}


