using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject weaponHandlerObject;
    public Transform weaponTransform;
    public Animator weaponAnimator;
    public Transform playerCamera;
    public PlayerHealth pHealth;

    [Header("Attack Settings")]
    public GameObject projectilePrefab;
    public Vector3 projectileSpawnOffset = new Vector3(0, 0, 1f);
    private const float attackCooldown = 0.3f;
    private float lastAttackTime = 0f;

    [Header("Weapon Idle Animation Settings")]
    private const float weaponIdleSpeed = 3.5f;
    private const float weaponIdleDistanceX = 0.1f;
    private const float weaponIdleDistanceY = 0.1f;
    private Vector3[] weaponIdleWaypoints = new Vector3[5];
    private int weaponIdleCurrentWaypointIndex = 0;
    private const float weaponIdleDistanceCheck = 0.1375f;

   private Vector2 currentMoveInput; // Current direction of player movement

    void Start()
    {
        InitializeWeaponIdleWaypoints();
    }

    void LateUpdate()
    {
        if(PauseManager.isPaused){
            return;
        }
        if(weaponHandlerObject.active == false){
            return;
        }

        if(pHealth){
            if(pHealth.isKnockedOut == true){
                return;
            }
        }
        currentMoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetButtonDown("Fire1") && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
        }

        UpdateWeaponIdleAnimation();
    }

    private void InitializeWeaponIdleWaypoints()
    {
        //Instead of using a prebuilt animation, we handle the idle animation using waypoints. This is partly because it looks a lot better in the Unity. ( Can't easily transition with a location based animation like this using built in blend trees. )
        weaponIdleWaypoints[0] = weaponTransform.localPosition;
        weaponIdleWaypoints[1] = weaponIdleWaypoints[0] + new Vector3(-weaponIdleDistanceX, weaponIdleDistanceY, 0);
        weaponIdleWaypoints[2] = weaponIdleWaypoints[0] + new Vector3(-weaponIdleDistanceX, -weaponIdleDistanceY, 0);
        weaponIdleWaypoints[3] = weaponIdleWaypoints[0] + new Vector3(weaponIdleDistanceX, weaponIdleDistanceY, 0);
        weaponIdleWaypoints[4] = weaponIdleWaypoints[0] + new Vector3(weaponIdleDistanceX, -weaponIdleDistanceY, 0);
    }

    void Attack()
    {
        if(pHealth.ammo<1){
            return;
        }
        if (weaponAnimator != null)
            weaponAnimator.SetTrigger("anim_Attack");

        SpawnProjectile();
        if (PlayerPrefs.GetInt("PP_GodMode", 0) == 0){
            pHealth.ammo--;
        }
        pHealth.healthDisplay.UpdateDisplay(pHealth.health, pHealth.armor, pHealth.ammo);
        lastAttackTime = Time.time;
    }

    void SpawnProjectile()
    {
        if(projectilePrefab){
        // Calculate the position where the projectile should spawn
        Vector3 spawnPosition = transform.position + transform.forward * projectileSpawnOffset.z + transform.up * projectileSpawnOffset.y;

        // Instantiate the projectile
        GameObject projectileInstance = Instantiate(projectilePrefab, spawnPosition, projectilePrefab.transform.rotation);



        // Assign its forward direction
        projectileInstance.transform.forward = playerCamera.forward; //Where we are aiming
        }
    }

    /*Notes: This approach might seem kind of weird, but its partly related to how Unity handles its animator relative to location based animations.
     I wanted the animator here, similar to older Id titles, to sort of oscillate in a figure 8 pattern while the character is moving. That movement requires you to change the weapons position during the animation.
     Position based movements dont look nice in unity's blend trees. It creates this sort of jostling motion when you are just starting or ending an animation.
     Could have gotten an approach that worked with the animator, but you would have had to hardcode things like deadzones into the animator perameter calls. Ended up just being more logical to code the animation manually like this.*/
    //
    //Notes 2: In hindsight; I really went overboard with implementing this weapon stuff. Sort of did it early on in the project, before I fully realized that we were just making a basic template.
    //Still; good to have for next semester!
    //Animates the weapon model in a figure 8 animation when the player is moving.
    void UpdateWeaponIdleAnimation()
    {
        // When the player is moving
        if (currentMoveInput.magnitude > 0.1f)
        {
            // Animate the weapon based on waypoints
            weaponTransform.localPosition = Vector3.Slerp(weaponTransform.localPosition, weaponIdleWaypoints[weaponIdleCurrentWaypointIndex], weaponIdleSpeed * Time.deltaTime);

            // Check if weapon has reached the current waypoint
            if (Vector3.Distance(weaponTransform.localPosition, weaponIdleWaypoints[weaponIdleCurrentWaypointIndex]) < weaponIdleDistanceCheck)
            {
                // Move to the next waypoint
                weaponIdleCurrentWaypointIndex = (weaponIdleCurrentWaypointIndex + 1) % 5;
            }
        }
        else
        {
            // Reset the weapon position to the initial waypoint when the player is idle
            weaponTransform.localPosition = Vector3.Slerp(weaponTransform.localPosition, weaponIdleWaypoints[0], weaponIdleSpeed * Time.deltaTime);
        }
    }
}
