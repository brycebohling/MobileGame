using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input Reader")]
public class InputReaderSO : ScriptableObject, GameInput.IPlayerActions
{
    public event UnityAction<Vector2> MovementEvent;
    public event UnityAction FireEvent;
    public event UnityAction DashEvent;
    public event UnityAction InteractEvent;

    GameInput gameInput;

    
    private void OnEnable() 
    {
        if (gameInput == null)
		{
			gameInput = new GameInput();
			gameInput.Player.SetCallbacks(this);
		}

		EnableGameplayInput();
    }

    private void OnDisable() 
    {
        DisableAllInput();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        FireEvent?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        DashEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractEvent?.Invoke();
        }
    }

    public void EnableGameplayInput()
	{
		gameInput.Player.Enable();
	}

    public void DisableAllInput()
	{
		gameInput.Player.Disable();
    }
}
