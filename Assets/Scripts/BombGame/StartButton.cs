using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public PokeInteractable pokeInteractable;
    public PushToStart pushToStart;
    public bool isPressed = false;
    public bool isBeingPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        pushToStart = FindObjectOfType<PushToStart>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pokeInteractable.State == InteractableState.Select && !isBeingPressed)
        {
            isBeingPressed = true;
            OnButtonPressed();
        }

        if(pokeInteractable.State == InteractableState.Normal)
        {
            isBeingPressed = false;
        }
    }

    private void OnButtonPressed()
    {
        if(!isPressed)
        {
            pushToStart.ButtonPushedOn();
            isPressed = true;
        }
        else if(isPressed)
        {
            pushToStart.ButtonPushedOff();
            isPressed = false;
        }
    }

    [ContextMenu("Start Bomb Spawn")]
    void temp()
    {
        OnButtonPressed();
    }
}
