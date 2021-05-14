using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    private Camera cam;
    public Transform playerTransform;

    public float speedCam;
    public Transform limiteCamEsq, limiteCamDir, limiteCamBaix, limiteCamSup;

    [Header ("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip sfxJump;
    public AudioClip sfxAtack;
    public AudioClip sfxCoin;
    public AudioClip sfxEnemyDead;
    public AudioClip sfxDamage;
    public AudioClip[] sfxStep;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {
        float posCamX = playerTransform.position.x;
        float posCamY = playerTransform.position.y;

        if(cam.transform.position.x < limiteCamEsq.position.x && playerTransform.position.x < limiteCamEsq.position.x){
            posCamX = limiteCamEsq.position.x;
        }
        else if(cam.transform.position.x > limiteCamDir.position.x && playerTransform.position.x > limiteCamDir.position.x){
            posCamX = limiteCamDir.position.x;

        }

        if(cam.transform.position.y < limiteCamBaix.position.y && playerTransform.position.y < limiteCamBaix.position.y){
            posCamY = limiteCamBaix.position.y;
        }
        else if(cam.transform.position.y > limiteCamSup.position.y && playerTransform.position.y > limiteCamSup.position.y){
            posCamY = limiteCamSup.position.y;

        }

        Vector3 posCam = new Vector3(posCamX, posCamY, cam.transform.position.z);

        cam.transform.position = Vector3.Lerp(cam.transform.position, posCam, speedCam * Time.deltaTime);
    }

    public void playSFX(AudioClip sfxClip, float volume){
        sfxSource.PlayOneShot(sfxClip, volume);
    }
}
