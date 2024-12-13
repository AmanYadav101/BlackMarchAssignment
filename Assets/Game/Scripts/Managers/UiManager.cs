using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        public Button endTurnButton;
        private PlayerMovement _playerMovement;

        private void OnEnable()
        {
            endTurnButton.onClick.AddListener(OnEndTurnButtonPressed);
        }

        private void OnDisable()
        {
            endTurnButton.onClick.RemoveAllListeners();
        }

        void Start()
        {
            _playerMovement = FindObjectOfType<PlayerMovement>();
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

        public void HideUIOnEndTurn()
        {
            endTurnButton.gameObject.SetActive(false);
        }

        private void OnEndTurnButtonPressed()
        {
            _playerMovement.TestMethod();
        }

        public void ShowEndTurnUI()
        {
            endTurnButton.gameObject.SetActive(true);
        }
    }
}