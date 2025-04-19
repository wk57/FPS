using System.IO;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class PdfManager : MonoBehaviour
{
    public Texture2D texture2d;
    public string apiUrl = "https://api.pdf.co/v1/pdf/convert/from/image"; 
    public string apiKey = "ukkiofox@hotmail.com_Ubp6fobH3usOvCNRqyPLl5ZCGj2VlqRWnvnQZmBjePbd2nE1WMxkfYDOjeosl6H6"; 

    public void CreatePdf()
    {
        StartCoroutine(SendTextureApi(texture2d));
    }

    private IEnumerator SendTextureApi(Texture2D textura)
    {
        // Convierte textura a PNG y luego a Base64
        byte[] imageData = textura.EncodeToPNG();
        string base64Image = System.Convert.ToBase64String(imageData);

        // Cuerpo JSON para la API
        string jsonBody = $"{{\"name\": \"GeneratedPDF.pdf\", \"images\": [\"{base64Image}\"]}}";

        // Configuración del POST
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-api-key", apiKey);

        Debug.Log("Enviando solicitud a la API");

        // Envío de solicitud
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Solicitud exitosa");
            Debug.Log($"Respuesta: {request.downloadHandler.text}");

            RespuestaPdf(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"Paila: {request.error}");
            Debug.LogError($"Detalles del Error: {request.downloadHandler.text}");
        }
    }

    private void RespuestaPdf(string respuesta)
    {
        Debug.Log($"Respuesta de la API: {respuesta}");
        PDFResponse jsonResponse = JsonUtility.FromJson<PDFResponse>(respuesta);

        if (!string.IsNullOrEmpty(jsonResponse.url))
        {
            Debug.Log($"URL del PDF generado: {jsonResponse.url}");
            StartCoroutine(DownloadPDF(jsonResponse.url));
        }
        else
        {
            Debug.LogError("No se obtuvo la URL del PDF en la respuesta.");
        }
    }

    private IEnumerator DownloadPDF(string pdfUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(pdfUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "GeneratedPDF.pdf");
            File.WriteAllBytes(filePath, request.downloadHandler.data);
            Debug.Log($"PDF descargado en: {filePath}");
        }
        else
        {
            Debug.LogError($"Error al descargar el PDF: {request.error}");
        }
    }

    [System.Serializable]
    public class PDFResponse
    {
        public string url;
    }
}
