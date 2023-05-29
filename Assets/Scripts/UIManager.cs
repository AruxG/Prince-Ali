using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//clase empleada para el manejo de la interfaz in game
public class UIManager : MonoBehaviour
{
    //Referencias a las pantallas de pausa, victoria y derrota, así como el texto que muestra la puntuación final en caso de victoria
    [SerializeField]
    GameObject pantallaPausa;
    [SerializeField]
    GameObject pantallaVictoria;
    [SerializeField]
    Text puntuacionText;
    [SerializeField]
    GameObject pantallaDerrota;
    //referencia al botón que pausa el juego
    [SerializeField]
    Button pauseButton;
    //referencias empleadas para calcular y pintar el tiempo restante
    [SerializeField]
    float Contrareloj = 60;
    public Text crUI;
    //booleano que indica que el juego se ha pausado
    bool paused;
    //Variables usadas para contabilizar las lámparas recogidas y mostrarlas por pantalla
    int LampCont = 0;
    public Image[] lampsOff;
    public Sprite lampOn;
    //Variables usadas para contabilizar los anillos recogidos y mostrarlos por pantalla
    int RingCont = 0;
    public Image[] ringOff;
    public Sprite ringOn;
    //entero empleado comom puntuación final
    int puntuacion;
    //booleano que indica que se ha finalizado el juego
    bool fin;
    // Start is called before the first frame update
    void Start()
    {
        //Inicializamos el juego como no pausado
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        //mientras no se haya acabado el juego y el contrarreloj supere cero, este irá reduciéndose
        if (Contrareloj > 0 &&!fin)
        {
            Contrareloj -= Time.deltaTime;
        }
        //si baja de cero, implica que hemos perdido y se llama al método pertinente
        else if(Contrareloj<=0)
        {
            Derrota();
        }
        //se convierte el float a formato timesopan para poder mostrarlo por pantalla
        var ts = TimeSpan.FromSeconds(Contrareloj);
        crUI.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }
    //método empleado para incrementar el número de lámparas y actualizar la UI
    public void incrementLamps()
    {
        LampCont++;
        lampsOff[LampCont - 1].sprite = lampOn;
        //Si conseguimos las cuatro lámparaws ganamos
        if (LampCont == 4)
        {
            Victoria();
        }
    }
    //método empleado para incrementar el número de anillos y actualizar la UI
    public void incrementRing()
    {
        RingCont++;
        ringOff[RingCont - 1].sprite = ringOn;
        //si conseguimos los tres anillos ganamos
        if (RingCont == 3)
        {
            Victoria();
        }
    }
    //método llamado por el botón de pausa para detener el juego
    public void pauseGame()
    {
        //si estaba pausado lo despasamos y ocultamos la pantalal de pausa
        if (paused)
        {
            Time.timeScale = 1;
            pauseButton.interactable = true;
            pantallaPausa.SetActive(false);
            paused = false;
        }
        else//Si no lo estaba lo pausamos y mostramos la pantalla de pausa
        {
            Time.timeScale = 0;
            paused = true;
            pauseButton.interactable = false;
            pantallaPausa.SetActive(true);
        }
    }
    //método empleado cuando el jugador consigue todas las lámparas o anillos
    public void Victoria()
    {
        //Se detiene el tiempo de juego, y se indica que se ha acabado
        Time.timeScale = 0;
        fin = true;
        //Se calcula la puntuacióna acorde al tiempo restante, y el número de lámparas y anillos recogidos
        puntuacion = (int)Contrareloj * 10;
        puntuacion += LampCont * 10;
        puntuacion += RingCont * 15;
        //Se deshabilita el botón de pausa y se pinta la puntuación final
        pauseButton.interactable = false;
        puntuacionText.text = "Puntuación: " + puntuacion;
        //Se carga la información de puntuaciones, se añade la nueva y se ordena el ranking, para actualizar la información guardada
        UIManagerMenu.loadData();
        UIManagerMenu.ranking.Add(puntuacion);
        UIManagerMenu.ranking.Sort();
        PlayerPrefs.SetInt("Puntuacion1", UIManagerMenu.ranking[0]);
        for(int i=1; i< UIManagerMenu.ranking.Count; i++)
        {
            PlayerPrefs.SetInt("Puntuacion"+ (i+1) , UIManagerMenu.ranking[i]);
        }
        //Debemos realizar un guardado de player preferences para que se fijen los cambios
        PlayerPrefs.Save();
        //y mostramos la pantalla de victoria
        pantallaVictoria.SetActive(true);
    }
    //método llamado cuando se nos agota el tiempo de juego
    public void Derrota()
    {
        //se detiene el tiempo, se indica que se ha acabado ys e muestra la pantalla de derrota
        Time.timeScale = 0;
        fin = true;
        pauseButton.interactable = false;
        pantallaDerrota.SetActive(true);
    }
    //método llamado por el botón para volver a cargar el juego
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //método llamado por el botón para volver al menú inicial
    public void Menu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
