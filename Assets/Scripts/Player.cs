using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHolaCountChange))]
    int HolaCount = 0;
    void HandleMovement()
    {
        if(isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement= new Vector3(moveHorizontal*0.1f,moveVertical*0.1f,0);
            transform.position = transform.position + movement;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Sending Hola to server");
            Hola();
        }
        if (isServer && transform.position.y > 50)
        {
            TooHigh();
        }
    }
    public override void OnStartServer()
    {
        Debug.Log("player has been spawned on the server");
    }
    [Command]
    void Hola()
    {
        Debug.Log("Received Hola from client!");
        HolaCount += 1;
        ReplyHola();
    }
    [TargetRpc]
    void ReplyHola()
    {
        Debug.Log("Received Hola from server");
    }
    [ClientRpc]
    void TooHigh()
    {
        Debug.Log("Too High!");
    }
    void OnHolaCountChange(int oldCount, int newCount)
    {
        Debug.Log($"we had {oldCount} holas but now we have {newCount} holas");
    }
}
