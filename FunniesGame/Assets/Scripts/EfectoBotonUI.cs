using UnityEngine;
using UnityEngine.EventSystems;

public class EfectoBotonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Vector3 escalaOriginal;
    public float multiplicadorTamano = 1.1f;

    void Awake()
    {
        escalaOriginal = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AgrandarBoton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AchicarBoton();
    }

    public void OnSelect(BaseEventData eventData)
    {
        AgrandarBoton();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        AchicarBoton();
    }

    private void AgrandarBoton()
    {
        transform.localScale = escalaOriginal * multiplicadorTamano;
    }

    private void AchicarBoton()
    {
        transform.localScale = escalaOriginal;
    }
}
