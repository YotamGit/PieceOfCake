using UnityEngine;
using UnityEngine.UI;

public class ClickedOnFromKeyboard : MonoBehaviour
{
    [SerializeField]
    private KeyCode onClickKey;

    private Button button;

	void Awake ()
    {
        button = GetComponent<Button>();
    }
	
	// if the player presses certein key, it will be like activating a specified button
	void Update ()
    {
        if (Input.GetKeyDown(onClickKey) && button.interactable && button.enabled)
        {
            button.onClick.Invoke();
        }
    }
}
