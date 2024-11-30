using UnityEngine;

public class InputManager : MonoBehaviour
{
    private ControlMapping playerInput;

    public static InputManager Instance { get; private set; } = null;

    public ControlMapping GetControlMapping()
    {
        return playerInput;
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerInput = new ControlMapping();
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
