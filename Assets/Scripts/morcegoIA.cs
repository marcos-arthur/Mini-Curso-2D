using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class morcegoIA : MonoBehaviour
{
    private gameController _GameController;

    private bool isFollowing;
    public bool isLookingLeft;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(gameController)) as gameController;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFollowing == true){
            transform.position = Vector3.MoveTowards(transform.position, _GameController.playerTransform.position, speed * Time.deltaTime);
        }

        if(transform.position.x < _GameController.playerTransform.position.x && isLookingLeft){
            Flip();
        }
        else if(transform.position.x > _GameController.playerTransform.position.x && !isLookingLeft){
            Flip();
        }
    }

    void OnBecameVisible() {
        isFollowing = true;
    }

    void Flip(){
        isLookingLeft = !isLookingLeft;
        float x = transform.localScale.x * -1;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
