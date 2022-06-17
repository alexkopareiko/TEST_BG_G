using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShieldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool shieldActive = false;
    private float timeToWaitForShield = 2f;
    private float buttonTimePressed = 0f;

    private void Update() {
        if(shieldActive) {
            buttonTimePressed += Time.deltaTime;
            if(buttonTimePressed >= timeToWaitForShield) {
                DisableShieldButton();
            }
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        shieldActive = true;
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        shieldActive = false;
    }

    private void DisableShieldButton()
    {
        GetComponent<Button>().interactable = false;
        buttonTimePressed = 0f;
        shieldActive = false;
        EnableButton();
    }

    private void EnableButton() {
        GetComponent<Button>().interactable = true;
    }
}
