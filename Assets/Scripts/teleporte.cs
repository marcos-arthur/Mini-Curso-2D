using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporte : MonoBehaviour
{
    private gameController _GameController;
    
    public Transform pontoSaida;
    public Transform posCamera;

    public Transform limiteCamEsq, limiteCamDir, limiteCamBaix, limiteCamSup;

    public musicaFase novaMusica;
    
    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(gameController)) as gameController;
        
    }

   void OnTriggerEnter2D(Collider2D col) {
       if(col.gameObject.tag == "Player"){
           _GameController.trocarMusica(musicaFase.CAVERNA);
           col.transform.position = pontoSaida.position;
           Camera.main.transform.position = posCamera.position;

           _GameController.limiteCamBaix = limiteCamBaix;
           _GameController.limiteCamDir = limiteCamDir;
           _GameController.limiteCamEsq = limiteCamEsq;
           _GameController.limiteCamSup = limiteCamSup;
       }
   }
}
