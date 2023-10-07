using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace UI
{
    public class MainMenuUILogic : MonoBehaviour
    {
        private const string LoadoutSelectorName = "LoadoutSelector";
        private const string LevelSelectorName = "LevelSelector";
        private const string DifficultySelectorName = "DifficultySelector";
        private const string WaveLengthSelectorName = "LengthSelector";
        private const string StartButtonName = "StartButton";
        private const string OptionsButtonName = "OptionButton";
        private const string QuitButtonName = "QuitButton";

        private UIDocument _mainMenuUIDocument;

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.None;
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
                
                GameValues.Difficulty = _mainMenuUIDocument.rootVisualElement.Q<DropdownField>(DifficultySelectorName)
                    .index + 1;
                GameValues.WaveLength = _mainMenuUIDocument.rootVisualElement.Q<DropdownField>(WaveLengthSelectorName).index + 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
            };

            _mainMenuUIDocument.rootVisualElement.Q<Button>(OptionsButtonName).clicked += () =>
            {
                Debug.Log("EditButton clicked");
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            };

            _mainMenuUIDocument.rootVisualElement.Q<Button>(QuitButtonName).clicked += () =>
            {
                Debug.Log("QuitButton clicked");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else 
                Application.Quit();
#endif
            };
        }
    }
}