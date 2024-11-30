using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public int maxDepthCharges = 10;
    [SerializeField] private Transform depthChargeSpawnPoint;
    [SerializeField] private Health health;
    [SerializeField] private ExplosionEffect explosionEffect;
    [SerializeField] private TextMeshProUGUI depthChargesText;

    public static ShipController Instance;

    private int currentDepthCharges;
    private bool canShoot = true;
    private Coroutine disableShootingCoroutine;
    private float remainingDisableTime = 0f;

    private Rigidbody2D _playerRigidBody2D;
    private Animator _animator;

    private float horizontalInput;
    private ControlMapping playerInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        playerInput = new ControlMapping();
        _playerRigidBody2D = GetComponent<Rigidbody2D>();
        currentDepthCharges = maxDepthCharges;
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.HorizontalMovement.performed += HorizontalMovementOnperformed;
        playerInput.Player.HorizontalMovement.canceled += HorizontalMovementOncanceled;
        playerInput.Player.Fire.performed += OnFirePerformed;
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
        playerInput.Player.HorizontalMovement.performed -= HorizontalMovementOnperformed;
        playerInput.Player.HorizontalMovement.canceled -= HorizontalMovementOncanceled;
        playerInput.Player.Fire.performed -= OnFirePerformed;
    }

    private void FixedUpdate()
    {
        _playerRigidBody2D.velocity = new Vector2(speed * horizontalInput, 0f);
        UpdateDepthChargesText();
    }

    private void HorizontalMovementOncanceled(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    private void HorizontalMovementOnperformed(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (canShoot && currentDepthCharges > 0)
        {
            LaunchDepthCharge();
        }
    }

    private void LaunchDepthCharge()
    {
        if (depthChargeSpawnPoint != null)
        {
            GameObject depthCharge = PoolManager.Instance.GetDepthCharge();
            depthCharge.transform.SetPositionAndRotation(depthChargeSpawnPoint.position, Quaternion.identity);
            currentDepthCharges--;
        }
    }

    private void UpdateDepthChargesText()
    {
        if (depthChargesText != null)
        {
            depthChargesText.text = $"{currentDepthCharges} / {maxDepthCharges}";
        }
    }

    public void TakeDamage(int points)
    {
        health.TakeDamage(points);
    }

    public void ToggleSpeed()
    {
        if (Mathf.Approximately(speed, 5f))
        {
            speed /= 2f;
        }
        else
        {
            speed = 5f;
        }
    }

    public void DisableShooting(float duration)
    {
        if (disableShootingCoroutine != null)
        {
            StopCoroutine(disableShootingCoroutine);
            remainingDisableTime = duration;
        }
        else
        {
            remainingDisableTime = duration;
        }
        
        disableShootingCoroutine = StartCoroutine(DisableShootingCoroutine());
    }

    public void OnDeath()
    {
        print("HER");
        playerInput.Disable();

        int playerLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("SunkShip");
        Physics2D.IgnoreLayerCollision(playerLayer, LayerMask.NameToLayer("Mine"), true);

        // Explode and sink the ship
        StartCoroutine(ExplodeAndSink());
    }

    public void HandleExplosionEnd()
    {
        print("OVDJE");
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = 5f;
        rigidbody.mass = 10;
        rigidbody.AddForce(Vector2.down);
    }

    private IEnumerator DisableShootingCoroutine()
    {
        canShoot = false;
        while (remainingDisableTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingDisableTime -= 1f;
        }
        canShoot = true;
        disableShootingCoroutine = null;
    }

    private IEnumerator ExplodeAndSink()
    {
        // Trigger explosion effect
        if (explosionEffect != null)
        {
            explosionEffect.Explode();
        }

        // Wait for explosion animation to complete
        yield return new WaitForSeconds(2f); // Adjust this duration as needed based on the explosion animation

        // Make the ship sink
     //   _playerRigidBody2D.gravityScale = 1f;
     //   _playerRigidBody2D.drag = 2f;

     //   while (true)
     //   { 
        //    _playerRigidBody2D.AddForce(Vector2.down * 10f);
          //  yield return new WaitForSeconds(0.1f);
      //  }
    }

    public void RefillDepthCharges()
    {
        currentDepthCharges = maxDepthCharges;
    }

    public void ForceLaunchDepthCharge()
    {
        if (currentDepthCharges > 0)
        {
            LaunchDepthCharge();
        }
    }
}
