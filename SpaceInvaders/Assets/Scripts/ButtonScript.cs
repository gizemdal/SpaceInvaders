using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = new Color32(254, 224, 50, 255);
        buttonText.fontStyle = FontStyles.Italic;
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = new Color32(255, 255, 255, 255);
        buttonText.fontStyle = FontStyles.Normal;
    }
}
