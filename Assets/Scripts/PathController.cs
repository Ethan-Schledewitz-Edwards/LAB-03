using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] public string _keyCode = "1";
    [SerializeField] public bool _stopOnCollide = false;

    [Header("Movement Variables")]
    [SerializeField] public float _moveSpeed;
    [SerializeField] public float _rotSpeed;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private PathManager _pathManager;

    // System
    List<Waypoint> path;
    Waypoint curTarget;
    private bool isWalking;

    private void Start()
    {
        path = _pathManager.Path;
        if (path != null && path.Count > 0)
        {
            curTarget = path[0];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            SetWalking(!isWalking);
        }

        if (isWalking)
        {

            RotateTowardsTarget();
            MoveForward();
        }
    }

    private void RotateTowardsTarget()
    {
        float stepSize = _rotSpeed * Time.deltaTime;

        Vector3 targerDir = curTarget.Pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targerDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    private void MoveForward()
    {
        float stepSize = _moveSpeed * Time.deltaTime;
        float distanceToTarget = Vector3.Distance(transform.position, curTarget.Pos);

        if(distanceToTarget < stepSize)
        {
            return;
        }

        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        curTarget = _pathManager.GetNextTarget();

        // Make character enter idle on touch
        if (_stopOnCollide)
            SetWalking(false);
    }

    /// <summary>
    /// Sets the characters walking state.
    /// </summary>
    /// <remarks>
    /// This also updates the animator
    /// </remarks>
    private void SetWalking(bool walking)
    {
        isWalking = walking;
        _animator.SetBool("IsWalking", isWalking);
    }
}
