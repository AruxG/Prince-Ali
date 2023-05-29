using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Clase empleada para la gestión del giroscopio
public static class DeviceRotation
{
    //variable que indica si se ha inicializado
    public static bool gyroInitialized = false;
    //Método que devuelve si el dispositivo soporta giroscopio
    public static bool HasGyroscope
    {
        get
        {
            return SystemInfo.supportsGyroscope;
        }
    }
    //método empleado para iniciar el giroscopio si no lo estaba y, si tiene giroscopio, leer su orientación
    public static Quaternion Get()
    {
        if (!gyroInitialized)
        {
            InitGyro();
        }
        return HasGyroscope 
            ? ReadGyroscopeRotation() 
            : Quaternion.identity;
    }
    //método llamado para inicializar el giroscopio. Se habilita y fija el intervalo de lectura o actualización
    private static void InitGyro()
    {
        if (HasGyroscope)
        {
            Input.gyro.enabled = true;
            Input.gyro.updateInterval = 0.0017f;
        }
        gyroInitialized = true;
    }
    //método que devuelve la orientación del giroscopio, transformada al sistema deseado
    private static Quaternion ReadGyroscopeRotation()
    {
        return new Quaternion(1,1,1,1)//Velocidad de giro
            * Input.gyro.attitude
            * new Quaternion(1,1,-1,-1);//Rotation Fix
    }
}
