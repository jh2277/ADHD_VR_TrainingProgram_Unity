using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendCharacter : MonoBehaviour
{
    public int characterID; 

    private void Start()
    {
        if (PlayerPrefs.HasKey("C_Data"))
        {
            int selectedCharacterID = PlayerPrefs.GetInt("C_Data");
            
            if (characterID == selectedCharacterID)
            {
                Debug.Log("Activating character: " + characterID);
                gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Deactivating character: " + characterID);
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("No character data found.");
        }
    }
}

