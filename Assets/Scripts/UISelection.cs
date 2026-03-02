using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//Librerias opcionales
using System.Collections;
using System.Collections.Generic;
using System;


public class UISelection : MonoBehaviour
{
    public static bool gazedAt;
    [SerializeField]
    float fillTime = 5f;
    public Image radialImage;
    public UnityEvent onFillComplete; //Evento genérico que pasa cuando termina la carga


    //Proceso asícrono
    private Coroutine fillCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gazedAt = false;
        radialImage.fillAmount = 0;
    }
    public void OnPointerEnter()
    {
        gazedAt = true;

        if(fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(FillRadial());
    }

    public void OnPointerExit()
    {
        gazedAt = false;
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
        radialImage.fillAmount = 0f;
    }

    private IEnumerator FillRadial()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fillTime) 
        {
            if (!gazedAt)
            {
                yield break;
            }
            elapsedTime += Time.deltaTime;
            radialImage.fillAmount = Mathf.Clamp01(elapsedTime/fillTime);

            yield return null;
        }
        // Evento a ejecutar
        onFillComplete?.Invoke();

        Console.WriteLine(gazedAt ? "Verdadero" : "Falso");
    }

    
}
