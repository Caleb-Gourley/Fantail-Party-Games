using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Unity.Netcode;
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
        pushToStart.button = this.gameObject;
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
            StartButtonPressedOnRpc();
            isPressed = true;
        }
        else if(isPressed)
        {
            StartButtonPressedOffRpc();
            isPressed = false;
        }
    }

    [Rpc(SendTo.Server)]
    public void StartButtonPressedOnRpc()
    {
        pushToStart.ButtonPushedOn();
    }

    [Rpc(SendTo.Server)]
    public void StartButtonPressedOffRpc()
    {
        pushToStart.ButtonPushedOff();
    }

    [ContextMenu("Start Bomb Spawn")]
    void temp()
    {
        OnButtonPressed();
    }
}
