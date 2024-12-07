using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Managers
{
    /// <summary>
    /// UIManager is used for Handling the UI components.
    /// Functions in this class can be used to manipulate the behaviour of the
    /// UI elements based on the condition needed.  
    /// </summary>
    public class UiManager : MonoBehaviour
    {
        public TextMeshProUGUI waitForYourTurnText;

        void Start()
        {
            waitForYourTurnText.gameObject.SetActive(false);
        }
        
        public IEnumerator WaitForTurn()
        {
            waitForYourTurnText.gameObject.SetActive(true);
            waitForYourTurnText.text = "Wait For Your Turn...";
            yield return new WaitForSeconds(2f);
            waitForYourTurnText.gameObject.SetActive(false);
        }

        public IEnumerator Moving()
        {
            waitForYourTurnText.gameObject.SetActive(true);
            waitForYourTurnText.fontSize = 100;
            waitForYourTurnText.text = "Moving to the target location...";
            yield return new WaitForSeconds(1.5f);
            waitForYourTurnText.gameObject.SetActive(false);
            waitForYourTurnText.fontSize = 145;
        }
        public IEnumerator NoPathFound()
        {
            waitForYourTurnText.gameObject.SetActive(true);
            waitForYourTurnText.fontSize = 100;
            waitForYourTurnText.text = "No Possible Path to Move to the target location...";
            yield return new WaitForSeconds(1.5f);
            waitForYourTurnText.gameObject.SetActive(false);
            waitForYourTurnText.fontSize = 145;
        }
        public IEnumerator ObstacleTile()
        {
            waitForYourTurnText.gameObject.SetActive(true);
            waitForYourTurnText.fontSize = 100;
            waitForYourTurnText.text = "There is an obstacle at this location...";
            yield return new WaitForSeconds(1.5f);
            waitForYourTurnText.gameObject.SetActive(false);
            waitForYourTurnText.fontSize = 145;
        }
    }
}