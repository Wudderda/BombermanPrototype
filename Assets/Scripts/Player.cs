using System.Collections.Generic;
using UnityEngine;

public class Player : PowerUpManager, LevelChangeListener
{
    [SerializeField] private LayerMask rangePowerUpLayerMask;
    [SerializeField] private LayerMask countPowerUpLayerMask;
    [SerializeField] private LayerMask doorLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private int initialBombRange;
    [SerializeField] private int initialBombCount;
    [SerializeField] private float speed;
    [SerializeField] private BombDeployer bombDeployer;

    private Animator playerAnimator;
    private Joystick joystick;
    private List<PowerUpChangeListener> listeners = new List<PowerUpChangeListener>();
    private Vector3 moveInput = Vector3.zero;
    private PowerUpController powerUpController;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        joystick = FindObjectOfType<Joystick>();
        powerUpController = new PowerUpController(initialBombRange, initialBombCount);
        GameManager.Instance.AddListener(this);
        NotifyPowerUpChange();
    }

    // Update is called once per frame
    void Update()
    {
        // Move character with joystick input
        moveInput.Set(joystick.Horizontal, joystick.Vertical, 0);
        AnimateDirection(moveInput);
        transform.position += moveInput * Time.deltaTime * speed;
        spriteRenderer.flipX = moveInput.x > 0;
    }

    /// <summary>
    /// Animates the given direction.
    /// </summary>
    /// <param name="direction"></param>
    private void AnimateDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            playerAnimator.SetFloat("speedX", direction.x);
            playerAnimator.SetFloat("speedY", 0);
        }
        else
        {
            playerAnimator.SetFloat("speedX", 0);
            playerAnimator.SetFloat("speedY", direction.y);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (1 << col.gameObject.layer == countPowerUpLayerMask)
        {
            // Upgrade power and notify listeners
            powerUpController.IncreasePowerLevel(PowerUpType.BOMB_COUNT);
            ObjectPoolManager.Instance.PutObject(ObjectPoolEnum.BOMB_COUNT_POWERUP, col.transform);
            NotifyPowerUpChange();
        }
        else if (1 << col.gameObject.layer == rangePowerUpLayerMask)
        {
            // Upgrade power and notify listeners
            powerUpController.IncreasePowerLevel(PowerUpType.BOMB_RANGE);
            ObjectPoolManager.Instance.PutObject(ObjectPoolEnum.BOMB_RANGE_POWERUP, col.transform);
            NotifyPowerUpChange();
        }
        else if (1 << col.gameObject.layer == doorLayerMask)
        {
            // Proceed to the next level
            ObjectPoolManager.Instance.PutObject(ObjectPoolEnum.DOOR, col.transform);
            GameManager.Instance.LoadNextLevel();
        }
        else if (1 << col.gameObject.layer == enemyLayerMask)
        {
            // Die
            Die();
        }
    }

    private void NotifyPowerUpChange()
    {
        foreach (PowerUpChangeListener listener in listeners)
        {
            listener.PowerUpChanged(powerUpController);
        }
    }

    private void Die()
    {
        GameManager.Instance.GameOver();
        powerUpController.ResetValues(initialBombRange, initialBombCount);
        NotifyPowerUpChange();
    }

    public void LevelChanged(LevelData newLevel)
    {
        ResetPosition();
    }

    private void ResetPosition()
    {
        transform.position = MapUtils.GetWorldPosition(new CellIndex(0, 0));
    }

    public override void AddListener(PowerUpChangeListener listener)
    {
        listeners.Add(listener);
    }

    public override void RemoveListener(PowerUpChangeListener listener)
    {
        listeners.Remove(listener);
    }
}