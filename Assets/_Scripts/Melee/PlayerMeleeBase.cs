using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerMeleeBase : MonoBehaviour
{
    [Header("Melee Base")]
    [SerializeField] protected List<AnimationClip> _attackAnimList = new List<AnimationClip>();

    protected PlayerStates _playerStatesScript;
    protected Animator _meleeAnimator;
    protected InputManager _inputManager;
    protected Transform _playerTransform;


    protected virtual void Awake()
    {
        _inputManager = new InputManager();
    }

    protected virtual void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        _playerStatesScript = transform.root.gameObject.GetComponent<PlayerStates>();
        _meleeAnimator = gameObject.GetComponent<Animator>(); 
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
