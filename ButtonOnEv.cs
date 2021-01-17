
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonOnEv : MonoBehaviour,IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        FindObjectOfType<AudioController>().PlaySound("button");
    }

}
