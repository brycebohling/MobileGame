using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStates))]
public class PlayerDash : PlayerBase
{
    [Header("Blocking States")]
    public PlayerStates.States[] BlockingActionStates;

    [Header("Input")]
    [SerializeField] InputReaderSO inputReaderSO;

    [Header("Dash")]
    [SerializeField] float dashSpeedMultiplayer;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    [Header("Animations")]
    [SerializeField] AnimationClip dashAnim;

    [Header("MMFeedbacks")]
    [SerializeField] MMF_Player dashFeedBackPlayer;
    
    bool isActive;
    float dashingTimer;
    float dashCooldownTimer;
    Vector2 velocityBeforeDash;


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        dashCooldownTimer = dashCooldown;
    }

    private void Update()
    {
        if (!IsActionAuth(BlockingActionStates))
        {
            if (isActive)
            {
                OnActionCancel();
            }
        }

        ProcessCooldowns();
    }

    protected override void OnEnable()
    {
        inputReaderSO.DashEvent += DashPressed;
    }

    protected override void OnDisable()
    {
        inputReaderSO.DashEvent -= DashPressed;
    }

    private void DashPressed()
    {
        if (!IsActionAuth(BlockingActionStates) || dashCooldownTimer < dashCooldown) return;
        
        OnActionActivate();
    }

    protected override void OnActionActivate()
    {
        _statesScript.State = PlayerStates.States.Dashing;
        velocityBeforeDash = _rb.velocity;
        isActive = true;
        dashCooldownTimer = 0;

        _rb.AddForce(velocityBeforeDash * dashSpeedMultiplayer, ForceMode2D.Impulse);

        StartAnimation(_animator, dashAnim);

        dashFeedBackPlayer?.PlayFeedbacks();
    }

    protected override void OnActionDeactivate()
    {
        _statesScript.State = PlayerStates.States.Idle;
        isActive = false;
        dashingTimer = 0;
        _rb.velocity = Vector2.zero;
        
        StopAnimation(_animator);
    }

    protected override void OnActionCancel()
    {
        isActive = false;
        dashingTimer = 0;
    }

    protected override void ProcessCooldowns()
    {
        if (isActive)
        {
            dashingTimer += Time.deltaTime;
            if (dashingTimer >= dashDuration)
            {
                OnActionDeactivate();   
            }

        } else
        {
            dashCooldownTimer += Time.deltaTime;
        }
    }
}
