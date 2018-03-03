using UnityEngine;
using UnityEngine.UI;

public class ClickedOnFromKeyboard : MonoBehaviour
{
    [SerializeField]
    private KeyCode onClickKey;

    private Button button;

	void Awake () {
        button = GetComponent<Button>();
    }
	
	// if the player presses Escape, it will be like pressing on the exit button
	void Update () {
        if (Input.GetKeyDown(onClickKey) && button.interactable)
        {
            button.onClick.Invoke();
        }
    }
}
