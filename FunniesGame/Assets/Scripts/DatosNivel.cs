using UnityEngine;

[CreateAssetMenu(fileName = "NuevoNivel", menuName = "Custom/Datos de Nivel")]
public class DatosNivel : ScriptableObject
{
    public string nombreNivel;
    public string escenaNombre;
    public Sprite fotoNivel;
    [TextArea(3, 5)] public string descripcionNivel;
}
