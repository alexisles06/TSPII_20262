using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class EventUI : MonoBehaviour
{
    public List<GameObject> listaInstrucciones;
    public List<string> mensajesInstrucciones;
    public TextMeshProUGUI TextMeshProUGUI;
    public int currentIndex = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Actualizas la visibilidad de paneles
        UpdateVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Metodos para cambiar de escena
    public void ChangeSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    
    public void ChangeSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    //Metodo para actualizar la visibilidad de paneles
    private void UpdateVisibility()
    {
        for (int i = 0; i < listaInstrucciones.Count; i++) 
        {
            //Solo el panel en el indice actual esta activo
            listaInstrucciones[i].SetActive(i==currentIndex);
        }
    }

    //Metodo para cambiar paneles
    public void CycleObject(int direction)
    {
        //Incrementa o decrementa el indice y se reinicia
        currentIndex = (currentIndex + direction + listaInstrucciones.Count) % listaInstrucciones.Count;

        UpdateVisibility();
    }

    //Metodo para cambiar mensajes
    public void CycleText(int direction)
    {
        //Incrementa o decrementa el indice y se reinicia
        currentIndex = (currentIndex + direction + mensajesInstrucciones.Count) % mensajesInstrucciones.Count;

        UpdateText();
    }

    //Metodo para actualizar la visibilidad de mensajes
    private void UpdateText()
    {
        if(mensajesInstrucciones.Count > 0 && TextMeshProUGUI != null)
        {
            TextMeshProUGUI.text = mensajesInstrucciones[currentIndex];
        }
    }

    //Metodo para salir de la aplicación
    public void ExitGame()
    {
        //donde vas a hacer la impresión del mensaje
        Debug.Log("Va a salir");
        Application.Quit();
        Debug.Log("Ya salió");
    }

}
