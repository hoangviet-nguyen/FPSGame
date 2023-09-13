using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuUILogic : MonoBehaviour
    {
        private const string LevelSelectorName = "LevelSelector";
        private const string StartButtonName = "StartButton";
        private const string EditCartButtonName = "OptionButton";
        private const string QuitButtonName = "QuitButton";

        private UIDocument _mainMenuUIDocument;

        private void OnEnable()
        {
            _mainMenuUIDocument = GetComponent<UIDocument>();
            if (_mainMenuUIDocument == null)
            {
                Debug.LogError("MainMenuUILogic: UIDocument is null");
                return;
            }
            else
            {
                Debug.Log("MainMenuUILogic: UIDocument opened");
            }

            _mainMenuUIDocument.rootVisualElement.Q<Button>(StartButtonName).clicked += () =>
            {
                Debug.Log("StartButton clicked");
                int sceneIndex = _mainMenuUIDocument.rootVisualElement.Q<DropdownField>(LevelSelectorName).index + 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
            };

            _mainMenuUIDocument.rootVisualElement.Q<Button>(EditCartButtonName).clicked += () =>
            {
                Debug.Log("EditButton clicked");
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            };

            _mainMenuUIDocument.rootVisualElement.Q<Button>(QuitButtonName).clicked += () =>
            {
                Debug.Log("QuitButton clicked");
            };
        }
    }
}