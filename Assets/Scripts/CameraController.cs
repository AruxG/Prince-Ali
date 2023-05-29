using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Clase empleada para rotar la cámara alrededor del mundo en el menú inicial
public class CameraController : MonoBehaviour
{
    //velocidad de giro
    [SerializeField]
    float speedPosition = 10;
    //punto en torno al que gira
    [SerializeField]
    GameObject target;


    // Update is called once per frame
    void Update()
    {
        //se gira alrededor del target con respecto al eje y a la velocidad indicada
        transform.RotateAround(target.transform.position, new Vector3 (0,1,0),1*speedPosition);
    }
}