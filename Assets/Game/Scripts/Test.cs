using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IPointerClickHandler
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime, Space.World);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("aahahahah");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Sol t�klama alg�land�!");
            // T�klama olay� ger�ekle�ti�inde yap�lacak i�lemleri buraya ekleyebilirsiniz.
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Sa� t�klama alg�land�!");
            // T�klama olay� ger�ekle�ti�inde yap�lacak i�lemleri buraya ekleyebilirsiniz.
        }
    }
}
