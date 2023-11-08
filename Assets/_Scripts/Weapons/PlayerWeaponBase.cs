using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponBase : MonoBehaviour
{
    [Header("Weapon Base")]
    [SerializeField] protected List<AnimationClip> _attackAnimList = new();

    protected PlayerStates _playerStatesScript;
    protected Animator _animator;
    protected InputManager _inputManager;
    protected Transform _playerTransform;


    protected virtual void Awake()
    {
        _inputManager = new InputManager();

        Initialization();
    }

    protected virtual void Start()
    {
        
    }

    private void Initialization()
    {
        _playerStatesScript = transform.root.gameObject.GetComponent<PlayerStates>();
        _animator = gameObject.GetComponentInChildren<Animator>(); 
        _playerTransform = transform.root;
    }

    protected virtual bool IsActionAuth(PlayerStates.States[] blockingActionStates)
    {
        foreach (PlayerStates.States state in blockingActionStates)
        {
            if (state == _playerStatesScript.State)
            {
                return false;
            }
        }

        return true;
    }
}
