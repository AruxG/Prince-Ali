using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase empleada para la gestión del movimiento del jugador
public class CharacterMovement : MonoBehaviour
{
    //campos que almacenan su velocidad, su velocidad en aceleración, su velocidad de giro y velocidad mínima
    [SerializeField]
    private float speed=10;
    [SerializeField]
    private float MaxSpeed=20;
    [SerializeField]
    private float rotationSpeed = 10;
    private float MinSpeed = 10;
    //sonido empleado al recoger un objeto y referencia al controlador de UI
    AudioSource pickSound;
    UIManager uiManager;
    //variable empleada para calcular la rotación que se ha producido
    [SerializeField]
    Vector3 anguloRotacion;
    //variable con la orientación inicial
    [SerializeField]
    Vector3 initialOrientation;
    //referencia al controlador del jugador
    private CharacterController rb;
    // Start is called before the first frame update

    void Start()
    {
        //Se habilita el paso de tiempo, se cogen las referencias, se habilita el giroscopio y se coge la oreintación inicial en eulers
        Time.timeScale = 1;
        uiManager = FindObjectOfType<UIManager>();
        rb = gameObject.GetComponent<CharacterController>();
        pickSound = gameObject.GetComponent<AudioSource>();
        Input.gyro.enabled = true;
        initialOrientation = DeviceRotation.Get().eulerAngles;
    }
    //Enumerator empleado para aumentar la velocidad de movimiento durante dos segundos
    IEnumerator Boost()
    {
        speed = MaxSpeed;
        yield return new WaitForSeconds(2);
        speed= MinSpeed;
    }
    // Update is called once per frame
    void Update()
    {
       //Se fija un movimiento constante adelante y arriba a las velocidades indicadaas
        rb.Move(transform.forward*speed*Time.deltaTime);
        rb.Move(transform.up*3*Time.deltaTime);
        //si se realiza un movimiento positivo en el eje y de una cierta magnitud se realiza la corrutina de aceleración
        if (Input.acceleration.y > 1)
        {
            StartCoroutine(Boost());
        }

        //se hace la diferencia con respecto a la orientación inicial para hacerlo con la orientación del teléfono y simplemente alinear
        Vector3 aux = DeviceRotation.Get().eulerAngles - initialOrientation;
        //Se transportan para asegurar el valor de los ángulos
        Quaternion deviceRotation = Quaternion.Euler(new Vector3((aux.x + 180) % 360, (aux.y + 180) % 360, (aux.z + 180) % 360));
        //y se traduce el quaternion a eulers y se almacena
        anguloRotacion = deviceRotation.eulerAngles;
        //Para usar los giros del teléfono como rotaciones
        //para cada eje se comprueba la diferencia de grados para determinar si se gira o no y en qué sentido, y se fija un float acorde a ello
        //Rotación en Y
        float fixedY = 0;
        
        if(deviceRotation.eulerAngles.y > 20 && deviceRotation.eulerAngles.y < 120)
        {
            fixedY = -rotationSpeed;
        }else if(deviceRotation.eulerAngles.y < 340 && deviceRotation.eulerAngles.y > 240)
        {
            fixedY = rotationSpeed;
        }
        else if(deviceRotation.eulerAngles.y < 20 && deviceRotation.eulerAngles.y > 0 || deviceRotation.eulerAngles.y < 360 && deviceRotation.eulerAngles.y > 340)
        {
            fixedY = 0;
        }
        
        //Rotación en X
        float fixedX = 0;
        if (deviceRotation.eulerAngles.x > 20 && deviceRotation.eulerAngles.x < 120)
        {
            fixedX = -rotationSpeed * 0.7f;
        }
        else if (deviceRotation.eulerAngles.x < 340 && deviceRotation.eulerAngles.x > 240)
        {
            fixedX = rotationSpeed * 0.7f;
        }
        else if(deviceRotation.eulerAngles.x < 20 && deviceRotation.eulerAngles.x > 0 || deviceRotation.eulerAngles.x < 360 && deviceRotation.eulerAngles.x > 340)
        {
            fixedX = 0;
        }
        //Rotación en Z
        float fixedZ = 0;
        
        if (deviceRotation.eulerAngles.z > 15 && deviceRotation.eulerAngles.z < 120)
        {
            fixedZ = rotationSpeed * 0.7f;
        }
        else if (deviceRotation.eulerAngles.z < 345 && deviceRotation.eulerAngles.z > 240)
        {
            fixedZ = -rotationSpeed * 0.7f;
        }
        else
        {
            fixedZ = 0;
        }
        //Se rota acorde a esos floats, ordenándolos para el sitema apaisado
        transform.Rotate(fixedZ, fixedX, -fixedY);
        
        //límites empleados en x y z para evitar que el jugador se desoriente o de la vuelta
        float limiteX, limiteZ;
        //X
        if(transform.rotation.eulerAngles.x<330 && transform.rotation.eulerAngles.x>270)
        {
            limiteX = 330;
        }else if (transform.rotation.eulerAngles.x > 30 && transform.rotation.eulerAngles.x< 90)
        {
            limiteX = 30;
        }
        else
        {
            limiteX = transform.rotation.eulerAngles.x;
        }

        //Z
        if (transform.rotation.eulerAngles.z < 330 && transform.rotation.eulerAngles.z > 270)
        {
            limiteZ = 330;
        }
        else if (transform.rotation.eulerAngles.z > 30 && transform.rotation.eulerAngles.z < 90)
        {
            limiteZ = 30;
        }
        else
        {
            limiteZ = transform.rotation.eulerAngles.z;
        }
        //Se aplican dichos limites a la rotacion, respetando la rotación respecto al eje y
        transform.rotation = Quaternion.Euler(limiteX, transform.rotation.eulerAngles.y, limiteZ);
        
    }
    //método llamado para la detección de colisiones con objetos
    void OnCollisionEnter(Collision other)
    {
        //Se comprueba si el otro objeto tiene un tag válido, y de ser así suena el sonido de recoger, se destruye el objeto 
        //y se incrementa su contador pertinente
        if (other.collider.tag== "Lamp" || other.collider.tag=="Ring")
        {
            pickSound.Play();
            Destroy(other.collider.gameObject);
            if (other.collider.tag == "Lamp")
            {
                uiManager.incrementLamps();
            }
            else
            {
                uiManager.incrementRing();
            }
        }
    }
}
