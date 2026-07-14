using UnityEngine;
using System.IO;

public class CamaraMiniaturas : MonoBehaviour
{
    public KeyCode teclaCaptura = KeyCode.F12;
    public int multiplicadorResolucion = 2;

    void Update()
    {
        if (Input.GetKeyDown(teclaCaptura))
        {
            TomarFoto();
        }
    }

    private void TomarFoto()
    {
        string carpeta = Application.dataPath + "/CapturasNiveles";
        
        if (!Directory.Exists(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }

        string nombreArchivo = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string rutaCompleta = carpeta + "/" + nombreArchivo;

        ScreenCapture.CaptureScreenshot(rutaCompleta, multiplicadorResolucion);
        Debug.Log("Foto guardada en: " + rutaCompleta);
    }
}
