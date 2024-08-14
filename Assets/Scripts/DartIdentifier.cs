using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartIdentifier : MonoBehaviour
{
    public string playerDartName;

    public void SetPlayerName(string name)
    {
        playerDartName = name;
        Debug.Log("Player's name is " + playerDartName);
    }
}
