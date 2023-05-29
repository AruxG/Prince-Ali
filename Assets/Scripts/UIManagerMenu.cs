using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Clase empleada para la gestión de la interfaz de menú
public class UIManagerMenu : MonoBehaviour
{
    //lista empleada para almacenar las puntuaciones
    public static List<int> ranking= new List<int>(5);
    //Referencias para visualizar las pantallas de créditos y ranking así como modificar el texto de puntuaciones
    [SerializeField] GameObject pantallaCreditos;
    [SerializeField] GameObject pantallaRanking;
    [SerializeField] Text textRanking;
    void Start()
    {
        //Esta línea sirve para borrar los datos almacenados
        //PlayerPrefs.DeleteAll();
        //Se llama a la carga de datos y se actualiza el texto de puntuaciones
        loadData();

        UpdateRanking();
    }
    //Método empleado para limppiar la lista de puntuaciones y cargar las almacenadas en preferencias
    public static void loadData()
    {
        ranking.Clear();
        if (PlayerPrefs.GetInt("Puntuacion1") != 0) { ranking.Add(PlayerPrefs.GetInt("Puntuacion1")); }
        if (PlayerPrefs.GetInt("Puntuacion2") != 0) { ranking.Add(PlayerPrefs.GetInt("Puntuacion2")); }
        if (PlayerPrefs.GetInt("Puntuacion3") != 0) { ranking.Add(PlayerPrefs.GetInt("Puntuacion3")); }
        if (PlayerPrefs.GetInt("Puntuacion4") != 0) { ranking.Add(PlayerPrefs.GetInt("Puntuacion4")); }
        if (PlayerPrefs.GetInt("Puntuacion5") != 0) { ranking.Add(PlayerPrefs.GetInt("Puntuacion5")); }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //método llamado por el botón que inicia el juego
    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene");
    }
    //Método llamado para mostrar la pantalla de créditos
    public void Creditos()
    {
        pantallaCreditos.SetActive(true);
    }
    //método llamado por el botón volver para desvisualizar la pantalla en que nos encontremos
    public void Volver (GameObject screen)
    {
        screen.SetActive(false);
    }
    //método llamado por el botón que cierra la aplicación
    public void Salir()
    {
        Application.Quit();
    }
    //método llamado por el botíon que muestra las puntuaciones
    public void Puntuaciones()
    {
        pantallaRanking.SetActive(true);
        UpdateRanking();
    }
    //método llamado para ordenar las puntuaciones y mostrarlas por pantalla en forma de texto
    private void UpdateRanking()
    {
        ranking.Sort(SortByScore);
        textRanking.text = "";
        for (int i = 0; i < ranking.Count && i < 5; i++)
        {
            textRanking.text += ranking[i] + "\n";
        }
    }
    //método empleado como comparador para ordenar las puntuaciones
    private int SortByScore(int p1, int p2)
    {
        return p2.CompareTo(p1);
    }
}
