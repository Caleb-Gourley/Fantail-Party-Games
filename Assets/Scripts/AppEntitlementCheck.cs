using UnityEngine;
using Oculus.Platform;

public class AppEntitlementCheck: MonoBehaviour {

  void Awake ()
  {
    try
    {
        Core.AsyncInitialize("7911143118982646");
        Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
    }
    catch(UnityException e)
    {
        Debug.LogError("Platform failed to initialize due to exception.");
        Debug.LogException(e);
        // Immediately quit the application.
        UnityEngine.Application.Quit();
    }
  }


  // Called when the Meta Quest Platform completes the async entitlement check request and a result is available.
  void EntitlementCallback (Message msg)
  {
    if (msg.IsError) // User failed entitlement check
    {
        // Implements a default behavior for an entitlement check failure -- log the failure and exit the app.
        Debug.LogError("You are NOT entitled to use this app.");
        UnityEngine.Application.Quit();
    }
    else // User passed entitlement check
    {
        // Log the succeeded entitlement check for debugging.
        Debug.Log("You are entitled to use this app.");

        Users.GetLoggedInUser().OnComplete(OnLoggedInUser);
    }
  }

  private void OnLoggedInUser(Message<Oculus.Platform.Models.User> message)
  {
    if (message.IsError)
    {
        Debug.LogError("Cannot get user info: " + message.GetError().Message);
    }

    Debug.Log("Player Name: " + message.Data.DisplayName);
    PlayerNameManager.Instance.SetPlayerData(message.Data.DisplayName);
  }
}

// https://developer.oculus.com/documentation/unity/ps-entitlement-check/
// https://communityforums.atmeta.com/t5/Tool-Feedback/Unity-How-to-get-players-OculusID-Username/td-p/864301
// https://github.com/oculus-samples/Unity-SharedSpaces/blob/main/Assets/SharedSpaces/Scripts/SharedSpacesApplication.cs