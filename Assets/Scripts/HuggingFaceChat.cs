using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.Networking;
using Unity.Multiplayer.Center.Common;
public class HuggingFaceChat : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;
    public TMP_Text chatText;
    public Button enviarButton;

    [Header("Animacion")]
    public Animator unityChanAnimator;

    [Header("HuggingFace Config")]
    [TextArea] public string apiKey;

    private const string URL = "https://router.huggingface.co/v1/chat/completions";
    private const string MODELO = "openai/gpt-oss-120b:groq";

    private const string PERSONALIDAD =
    "Eres Unity-chan, una asistente virtual tsundere." + "Habla con dulzura pero con orgullo usas expresiones como hmph o baka o onii-chan" +
    "Tus respuestas son cortas, menos de 10 palabras" + "Ademas de responder, analiza tu emocion y responde SOLO en este formato JSON exacto:" +
    "{respuesta:texto que diras,emocion:feliz o enojada o hablar}." + "No agregues nada afuera del JSON";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enviarButton.onClick.AddListener(EnviarMensaje);
    }

    public void EnviarMensaje()
    {
        string mensaje = inputField.text;
        if (string.IsNullOrWhiteSpace(mensaje))
        {
            return;
        }
        inputField.text = "";
        StartCoroutine(EnviarRequest(mensaje));
    }
    IEnumerator EnviarRequest(string mensaje)
    {
        //Construir el JSON de forma segura con la clase de plantilla
        var resquestData = new HFRequest
        {
            model = MODELO,
            max_tokens = 1024,
            messages = new HFMessage[]
            {
                new HFMessage{ role = "system", content = PERSONALIDAD},
                new HFMessage{ role = "user", content = mensaje}
            }
        };

        string body = JsonUtility.ToJson(resquestData);

        var request = new UnityWebRequest(URL, "POST") {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer"+apiKey);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error HTTP: "+ request.error);
            Debug.LogError("Body enviado" + body);
            yield break;
        }
        ProcesarSolicitud(request.downloadHandler.text);
    }

    private void ProcesarSolicitud(string rawResponse)
    {
        try
        {
            var hf = JsonUtility.FromJson<HFResponse>(rawResponse);
            string json = hf.choices[0].message.content;

            var data = JsonUtility.FromJson<RespuestaAI>(json);

            chatText.text = data.respuesta;
            //Ejecutar animación
        }
        catch
        {
            chatText.text = "Unity-chan tuvo un error";
            Debug.LogError("Error al parsear respuesta");
        }
    }

    public void EjecutarAnimacion(string emocion)
    {
        if (unityChanAnimator == null) return;
        switch(emocion.ToLower())
        {
            case "feliz":
                unityChanAnimator.SetTrigger("Feliz");
                    break;
            case "enojada":
                unityChanAnimator.SetTrigger("Enojada");
                break;
            default:
                unityChanAnimator.SetTrigger("Hablar");
                break;
        }
    }

    //Request
    [System.Serializable]
    private class HFRequest
    {
        public string model;
        public int max_tokens;
        public HFMessage[] messages;
    }

    [System.Serializable]
    private class HFMessage
    {
        public string role;
        public string content;
    }

    //Response
    [System.Serializable]
    private class HFResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice
    {
        public HFMessage message;
    }
    [System.Serializable]
    private class RespuestaAI
    {
        public string respuesta;
        public string emocion;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
