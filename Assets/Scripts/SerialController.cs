using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;


public class SerialController : MonoBehaviour
{

    public float speed = 5f;
    SerialPort serialPort;
    bool portOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serialPort = new SerialPort("COM11", 9600);
        try
        {
            serialPort.Open();
            portOpen = true;
            Debug.Log("Puerto serial abierto");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al abrir el puerto serial: " + ex.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (portOpen)
        {
            try
            {
                string[] data = serialPort.ReadLine().Trim().Split('|');
                float x = float.Parse(data[0]);
                float z = float.Parse(data[1]);

                Debug.Log($"Datos recibidos: x={x}, z={z}");

                Vector3 movement = new Vector3(x, 0, z) * speed * Time.deltaTime;
                this.transform.Translate(movement);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al leer datos del puerto serial: " + ex.Message);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colision detectada con: " + other.name);
        serialPort.WriteLine("1");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Salida de colision con: " + other.name);
        serialPort.WriteLine("0");
    }
}

