using UnityEngine;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;


public class nugetPdf : MonoBehaviour
{
    public Texture2D textureToPdf;

    public void CreatePDFFromTexture()
    {
       // textura a byte 
        byte[] textureBytes = textureToPdf.EncodeToPNG();

        //crear el pdf
        Document doc = new Document();
        string pdfPath = Path.Combine(Application.persistentDataPath, "output.pdf");

        try
        {
            PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));
            doc.Open();

            //de byte a itext 
            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(textureBytes);
            pdfImage.ScaleToFit(doc.PageSize.Width - 50, doc.PageSize.Height - 50);
            pdfImage.Alignment = Element.ALIGN_CENTER;

            //carga la textura
            doc.Add(pdfImage);

            Debug.Log("guardado en " + pdfPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error " + e.Message);
        }
        finally
        {
            doc.Close();
        }
    }
}
