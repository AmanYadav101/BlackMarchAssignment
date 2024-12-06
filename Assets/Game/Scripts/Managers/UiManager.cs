using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Managers
{
    public class UiManager:MonoBehaviour
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

    }
}