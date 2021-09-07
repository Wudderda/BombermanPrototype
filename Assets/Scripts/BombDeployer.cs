using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to deploy bombs to the map.
/// </summary>
public class BombDeployer : MonoBehaviour, PowerUpChangeListener
{
    [SerializeField] private PowerUpManager manager;
    [SerializeField] private Button placeBombButton;

    private const float CHAR_OFFSET = 0.5f;
    private const float BOMB_OFFSET = 0.795f;

    private  WaitForSecondsRealtime bombTimer = new WaitForSecondsRealtime(1.5f);
    private  WaitForSecondsRealtime explosionDelay = new WaitForSecondsRealtime(0.5f);

    private int maxDeployableBombCount = 2;
    private int firedBombCount = 0;
    private int explosionRange = 2;

    private ObjectPoolManager objectPoolManager;

    // Start is called before the first frame update
    void Start()
    {
        placeBombButton.onClick.AddListener(() => StartCoroutine(PlaceBomb()));
        objectPoolManager = ObjectPoolManager.Instance;
        manager.AddListener(this);
    }

    /// <summary>
    /// Places a bomb to the player's snapped position.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlaceBomb()
    {
        if (maxDeployableBombCount > firedBombCount)
        {
            // Place a bomb in player's position
            Component bomb = objectPoolManager.GetObject<Transform>(ObjectPoolEnum.BOMB);
            bomb.transform.position = MapUtils.SnapToCenter(transform.position + Vector3.up * CHAR_OFFSET) +
                                      Vector3.down * BOMB_OFFSET;
            firedBombCount++;

            // Wait for bomb timer
            yield return bombTimer;

            // Make explosion
            List<Animator> explosionEffects = MakeExplosion(bomb.transform.position);

            // Put the bomb back to the pool
            objectPoolManager.PutObject(ObjectPoolEnum.BOMB, bomb);
            firedBombCount--;

            // Wait for explosion delay
            yield return explosionDelay;

            // Put the effects back to the pool
            foreach (var explosionEffect in explosionEffects)
            {
                objectPoolManager.PutObject(ObjectPoolEnum.EXPLOSION_EFFECT, explosionEffect);
            }
        }
    }

    /// <summary>
    /// Makes the explosion in specified position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private List<Animator> MakeExplosion(Vector3 position)
    {
        var explosionReach = FireRaysForTiles(position + Vector3.up * BOMB_OFFSET);
        FireRaysForEnemies(position + Vector3.up * BOMB_OFFSET, explosionReach);

        return SpawnExplosions(position, explosionReach);
    }

    /// <summary>
    /// Fire rays in four main directions.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private (int, int, int, int) FireRaysForTiles(Vector3 position) =>
        (FireRayForTile(position, Vector2.up), FireRayForTile(position, Vector2.down),
            FireRayForTile(position, Vector2.right), FireRayForTile(position, Vector2.left));

    /// <summary>
    /// Fire rays for the tile objects and destroy them.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private int FireRayForTile(Vector3 position, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, explosionRange, 1 << 6 | 1 << 7);
        if (hit.collider != null)
        {
            Vector2 hitDistance = (hit.collider.transform.position - position) * direction;
            int result = hitDistance.x != 0
                ? Mathf.RoundToInt(Mathf.Abs(hitDistance.x))
                : Mathf.RoundToInt(Mathf.Abs(hitDistance.y));
            if (hit.collider.gameObject.layer == 6)
            {
                objectPoolManager.PutObject(ObjectPoolEnum.BRICK, hit.collider.transform);
                return result;
            }
            else
            {
                return result - 1;
            }
        }

        return explosionRange;
    }

    /// <summary>
    /// Fire rays for enemies in four main directions.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="explosionReach"></param>
    private void FireRaysForEnemies(Vector3 position, (int up, int down, int right, int left) explosionReach)
    {
        FireRayForEnemy(position, Vector2.up, explosionReach.up);
        FireRayForEnemy(position, Vector2.down, explosionReach.down);
        FireRayForEnemy(position, Vector2.right, explosionReach.right);
        FireRayForEnemy(position, Vector2.left, explosionReach.left);
    }

    /// <summary>
    /// Fires ray for an enemy for a single direction.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="reach"></param>
    private void FireRayForEnemy(Vector3 position, Vector2 direction, int reach)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, reach, 1 << 8);
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Enemy>().Die();
        }
    }

    /// <summary>
    /// Spawns the explosion effects in four main directions for given range.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="explosionReach"></param>
    /// <returns></returns>
    private List<Animator> SpawnExplosions(Vector2 position, (int up, int down, int right, int left) explosionReach)
    {
        var activeExplosions =
            new List<Animator>(explosionReach.up + explosionReach.down + explosionReach.right + explosionReach.left);

        for (int i = 1; i <= explosionReach.up; i++)
        {
            SpawnAndAddExplosion(position + i * Vector2.up, activeExplosions);
        }

        for (int i = 1; i <= explosionReach.down; i++)
        {
            SpawnAndAddExplosion(position + i * Vector2.down, activeExplosions);
        }

        for (int i = 1; i <= explosionReach.right; i++)
        {
            SpawnAndAddExplosion(position + i * Vector2.right, activeExplosions);
        }

        for (int i = 1; i <= explosionReach.left; i++)
        {
            SpawnAndAddExplosion(position + i * Vector2.left, activeExplosions);
        }

        return activeExplosions;
    }

    /// <summary>
    /// Spawn an explosion for the given direction.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="explosionAnimations"></param>
    private void SpawnAndAddExplosion(Vector2 position, List<Animator> explosionAnimations)
    {
        Animator explosionEffect = objectPoolManager.GetObject<Animator>(ObjectPoolEnum.EXPLOSION_EFFECT);
        explosionEffect.transform.position = position;
        explosionEffect.SetTrigger("explode");
        explosionAnimations.Add(explosionEffect);
    }

    public void PowerUpChanged(PowerUpController powerUpController)
    {
        explosionRange = powerUpController.BombRange;
        maxDeployableBombCount = powerUpController.BombCount;
    }
}