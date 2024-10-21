using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class NavControl : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] public Transform _moveTarget;
    [SerializeField] public Transform _lookTarget;
    [SerializeField] public float _slowdownRange = 3f;

    [Header("Movement Variables")]
    [SerializeField] public float _moveSpeed;

    [Header("Components")]
    private NavMeshAgent navAgent;
    private Animator animator;

    private bool isWalking;
    private bool isAttacking;

    [Header("System")]
    private float sqrSlowdownRange;

    #region Initialization Methods

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Turn off auto breaking
        // Note: we handle this in the script
        navAgent.autoBraking = false;

        // Set move speed to an appropriate speed
        sqrSlowdownRange = _slowdownRange * _slowdownRange;

        SetWalking(true);
    }
    #endregion

    #region Unity Callbacks

    private void Update()
    {
        float newMoveSpeed = _moveSpeed;

        if (isWalking)
        {
            navAgent.destination = _moveTarget.transform.position;
            float sqrDist = (navAgent.destination - transform.position).sqrMagnitude;

            // Set agent speed based on distance to target
            if (sqrDist < sqrSlowdownRange)
            {
                newMoveSpeed = Mathf.Lerp(_moveSpeed * 0.5f, _moveSpeed, sqrDist / sqrSlowdownRange);
            }
            else newMoveSpeed = _moveSpeed;
        }
        else
        {
            navAgent.isStopped = true; // Stop the agent when not walking
        }

        // Look at the target when attacking
        if (isAttacking)
        {
            newMoveSpeed = 0f;
            animator.speed = 1f;
            transform.LookAt(new Vector3(_lookTarget.position.x, transform.position.y, _lookTarget.position.z), transform.up);
        }

        if (isWalking)
        {
            navAgent.isStopped = false; // Resume movement
            navAgent.speed = newMoveSpeed;

            // Slowdown the animator as the player reaches their destination
            animator.speed = Mathf.Lerp(0.1f, 1f, newMoveSpeed / _moveSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            SetWalking(false);
            SetAttacking(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            SetWalking(true);
            SetAttacking(false);
        }
    }
    #endregion

    /// <summary>
    /// Sets the characters walking state.
    /// </summary>
    /// <remarks>
    /// This also updates the animator
    /// </remarks>
    private void SetWalking(bool walking)
    {
        isWalking = walking;
        animator.SetBool("IsWalking", isWalking);
    }

    /// <summary>
    /// Sets the characters attacking state.
    /// </summary>
    /// <remarks>
    /// This also updates the animator
    /// </remarks>
    private void SetAttacking(bool attacking)
    {
        isAttacking = attacking;
        animator.SetBool("IsAttacking", isAttacking);
    }
}
