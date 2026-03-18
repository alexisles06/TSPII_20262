using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Flight : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 100f;
    public Transform cameraTransform;
    public Vector2 movementInput;

    //Control de iteraciones
    public int turbulenceIterations = 1000000;

    //Lista de vectores de posición calculados
    private List<Vector3> turbulenceForces = new List<Vector3>();

    //Metodo para mover la nave
    public void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraTransform == null)
        {
            Debug.LogError("No hay camara asignada");
            return; 
        }

        //Actividad 1: Proceso pesado que consume recursos
        SimulateTurbulence();

        //Mover la nave de forma lineal

        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        //Mover la nave en rotacion
        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, yaw, 0);
    }

    public void SimulateTurbulence()
    {
        turbulenceForces.Clear();
        //Repeticiones
        for (int i = 0; i < turbulenceIterations; i++)
        {
            Vector3 force = new Vector3(
                Mathf.PerlinNoise(i * 0.0001f, Time.time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.0002f, Time.time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.0003f, Time.time) * 2 - 1
            );
            turbulenceForces.Add(force);
        }
    }
}
