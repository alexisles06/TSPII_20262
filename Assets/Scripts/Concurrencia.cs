using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//para los hilos
using System.Threading;
using System.Threading.Tasks;

public class Concurrencia : MonoBehaviour
{
    [Header("Activa los metodos")]
    public bool useSincrono;
    public bool useThread;
    public bool useTask;
    public bool useCoroutine;

    [Header("Esfera a mover")]
    public Transform sincronoSphere;
    public Transform threadSphere;
    public Transform taskSphere;
    public Transform coroutineSphere;
    public Transform mainCube;

    //Acciones a ejecutar en el hilo secundario

    private Queue<Action> mainThreadActions = new Queue<Action>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(useSincrono) MoveSincrono();
        if (useThread) MoveWithThread();
        if (useTask) MoveWithTask();
        if (useCoroutine) StartCoroutine(MoveWithCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        //Siempre gira el cubo de referencia
        mainCube.Rotate(Vector3.up, 50 * Time.deltaTime);

        //Ejecuta las acciones en el hilo principal
        lock (mainThreadActions)
        {
            while (mainThreadActions.Count > 0) { mainThreadActions.Dequeue().Invoke(); }
        }
    }

    //Metodos para herramientas de concurrencia
    void MoveSincrono()
    {
        for (int i = 0; i <= 100; i++)
        {
            sincronoSphere.position += Vector3.right * 0.05f;
        }
        Thread.Sleep(50);
    }

    //Movimiento cin hilo secundario
    void MoveWithThread()
    {
        new Thread(() =>
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50);
                lock (mainThreadActions)
                {
                    mainThreadActions.Enqueue(() => {threadSphere.position += Vector3.right * 0.05f; });
                }
            }
        }).Start();
    }

    //Metodos con task asincrona
    async void MoveWithTask()
    {
        await Task.Run(() => 
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(50);

                lock (mainThreadActions) { mainThreadActions.Enqueue(() => { taskSphere.position += Vector3.right * 0.05f; }); }
            }
        });
    }

    //Corrutina
    IEnumerator MoveWithCoroutine()
    {
        for (int i = 0; i <= 100; i++)
        {
            coroutineSphere.position += Vector3.right * 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
