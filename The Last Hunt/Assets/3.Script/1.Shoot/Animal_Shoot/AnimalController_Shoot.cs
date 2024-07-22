using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.Events;
using UnityEngine.Assertions;
using UnityEngine.UI;
//using UnityEditor;

public enum PlayerAlertStage
{
    Peaceful,
    Intrigued,
    Alerted
}

public class AnimalController_Shoot : MonoBehaviour
{
    // State arrays
    [SerializeField] public Idle_State[] idleStates;
    [SerializeField] private Movement_State[] movementStates;
    [SerializeField] private AI_State[] attackingStates;
    [SerializeField] private AI_State[] deathStates;

    [SerializeField] public string species = "N/A";

    [SerializeField] public AI_Stats stats;

    // How far from origin this animal will wander
    [SerializeField] private float wanderDistance = 10f;
    public float MaxDistance
    {
        get
        {
            return wanderDistance;
        }
        set
        {
            wanderDistance = value;
        }
    }

    // Chance of attack
    [SerializeField] private int dominance = 1;
    [SerializeField] private int originalDominance = 0;

    // Awareness -> Sensing predator || Scent -> Sensing prey
    [SerializeField] private float awareness = 30f;
    [SerializeField] private float scent = 30f;
    [SerializeField] private float originalScent = 0f;
    [SerializeField] private bool isAnimalInRange;

    [SerializeField] private PlayerController_Shoot player;
    private float playerAwareness = 200f;
    [SerializeField] [Range(0, 360)] private float fovAngle;
    [SerializeField] private PlayerAlertStage playerAlertStage;
    [SerializeField] [Range(0, 100)] private float playerAlertLevel;
    [SerializeField] private bool isPlayerInRange;
    [SerializeField] private bool isPlayerLocated;
    private bool isHitByArrow;

    // Basic stats
    [SerializeField] private float stamina = 10f;
    [SerializeField] private float attack = 10f;
    [SerializeField] private float attackSpeed = 0.5f;
    [SerializeField] private float health = 5f;
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private Slider healthUI;

    // Chance of attack (0~100)
    [SerializeField] private float aggression = 0f;
    [SerializeField] private float originalAggression = 0f;

    // Will attack same species
    [SerializeField] private bool isTerritoral = false;

    // Can't be detected by other animals
    [SerializeField] private bool isStealthy = false;

    // Can't leave original territorial zone
    [SerializeField] private bool constrainedToZone = false;

    // Won't attack these species
    [SerializeField] private string[] peacefulTowards;

    private static List<AnimalController_Shoot> allAnimals = new List<AnimalController_Shoot>();
    public static List<AnimalController_Shoot> AnimalControllers
    {
        get
        {
            return allAnimals;
        }
    }

    // Stopping distance to target
    private const float stopDistance = 1f;

    // Will animal rotate to match terrain (layer should be "Terrain")
    [SerializeField] private bool matchSurfaceRotation = false;
    [SerializeField] private float surfaceRotationSpeed = 2f;

    // Log changes to animal in console
    [SerializeField] private bool logChanges = false;

    // Will gizmos be drawn in editor
    [SerializeField] private bool showGizmos = false;
    [SerializeField] private bool drawWanderRange = true;
    [SerializeField] private bool drawScentRange = true;
    [SerializeField] private bool drawAwarenessRange = true;

    // UI Controller
    [SerializeField] private UIController_Shoot ui;

    public UnityEvent deathEvent;
    public UnityEvent attackEvent;
    public UnityEvent idleEvent;
    public UnityEvent moveEvent;

    private Color distanceColor = new Color(0f, 0f, 205f);
    private Color awarenessColor = new Color(1f, 0f, 1f, 1f);
    private Color scentColor = new Color(1f, 0f, 0f, 1f);

    private Animator animator;
    private CharacterController characterController;
    private NavMeshAgent navMeshAgent;
    private Vector3 origin;

    private int totalIdleStateWeight;

    private bool useNavMesh = false;
    private Vector3 targetLocation = Vector3.zero;

    private float turnSpeed = 0f;

    public enum WanderState
    {
        Idle,
        Wander,
        Chase,
        Evade,
        Attack,
        Dead
    }

    private float attackTimer = 0;

    private float minStaminaForAggression
    {
        get
        {
            return stats.stamina * 0.9f;
        }
    }
    private float minStaminaForEvade
    {
        get
        {
            return stats.stamina * 0.1f;
        }
    }

    public WanderState CurrentState;
    private AnimalController_Shoot primaryPrey;
    private AnimalController_Shoot primaryPredator;
    private AnimalController_Shoot target_ani;
    private float moveSpeed = 0f;
    private float attackReach = 2f;
    private bool isForcedUpdate = false;
    private Vector3 startPosition;
    private Vector3 wanderTarget;
    private float idleUpdateTime;

    // Gizmo control -> Show range of movement, detection, etc
    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
        {
            return;
        }
        // Draw circle of radius (wanderDistance)
        if (drawWanderRange)
        {
            Gizmos.color = distanceColor;
            Gizmos.DrawWireSphere(origin == Vector3.zero ? transform.position : origin, wanderDistance);
            Vector3 IconWander = new Vector3(transform.position.x, transform.position.y + wanderDistance, transform.position.z);
            Gizmos.DrawIcon(IconWander, "ICON-Wander", true);
        }
        // Draw circle of radius (awareness)
        if (drawAwarenessRange)
        {
            Gizmos.color = awarenessColor;
            Gizmos.DrawWireSphere(transform.position, awareness);
            Vector3 IconAwareness = new Vector3(transform.position.x, transform.position.y + awareness, transform.position.z);
            Gizmos.DrawIcon(IconAwareness, "ICON-Awareness", true);
        }
        // Draw circle of radius (scent)
        if (drawScentRange)
        {
            Gizmos.color = scentColor;
            Gizmos.DrawWireSphere(transform.position, scent);
            Vector3 IconScent = new Vector3(transform.position.x, transform.position.y + scent, transform.position.z);
            Gizmos.DrawIcon(IconScent, "ICON-Scent", true);
        }

        if (!Application.isPlaying)
        {
            return;
        }

        // Draw target position (Position = sphere, Distance = line)
        if (useNavMesh)
        {
            if (navMeshAgent.remainingDistance > 1f)
            {
                Gizmos.DrawSphere(navMeshAgent.destination + new Vector3(0f, 0.1f, 0f), 0.2f);
                Gizmos.DrawLine(transform.position, navMeshAgent.destination);
            }
        }
        else
        {
            if (targetLocation != Vector3.zero)
            {
                Gizmos.DrawSphere(targetLocation + new Vector3(0f, 0.1f, 0f), 0.2f);
                Gizmos.DrawLine(transform.position, targetLocation);
            }
        }
    }

    private void Awake()
    {
        isHitByArrow = false;
        // Error if no stat to script
        if (!stats)
        {
            Debug.LogError(string.Format("No stats attached to {0} wander script.", gameObject.name));
            enabled = false;
            return;
        }

        TryGetComponent(out animator);
        var runtimeController = animator.runtimeAnimatorController;

        // Modify HashSet object to contain all elements in specified collection / in itself / both
        // Add all parameeter selection elements in animator to animatorParameters
        if (animator)
        {
            animatorParameters.UnionWith(animator.parameters.Select(p => p.name));
        }

        // Error messages for when changes are made to animal
        if (logChanges)
        {
            if (runtimeController == null)
            {
                Debug.LogError(string.Format("No animator controller for {0}", gameObject.name));
                enabled = false;
                return;
            }
            if (animator.avatar == null)
            {
                Debug.LogError(string.Format("No avatar for {0}", gameObject.name));
                enabled = false;
                return;
            }
            if (animator.hasRootMotion == true)
            {
                Debug.LogError(string.Format("Root motion applied for {0}", gameObject.name));
                animator.applyRootMotion = false;
            }
            if (idleStates.Length == 0 || movementStates.Length == 0)
            {
                Debug.LogError(string.Format("No idle/movement states for {0}", gameObject.name));
                enabled = false;
                return;
            }
            if (idleStates.Length > 0)
            {
                for (int i = 0; i < idleStates.Length; i++)
                {
                    if (idleStates[i].animationBool == "")
                    {
                        Debug.LogError(string.Format("{0} has " + movementStates.Length + "movement states -> Each state should have an animation boolean."));
                        enabled = false;
                        return;
                    }
                }
            }
            if (movementStates.Length > 0)
            {
                for (int i = 0; i < movementStates.Length; i++)
                {
                    if (movementStates[i].animationBool == "")
                    {
                        Debug.LogError(string.Format("{0} has " + movementStates.Length + "movement states -> Each state should have an animation boolean."));
                    }
                    if (movementStates[i].moveSpeed <= 0)
                    {
                        Debug.LogError(string.Format("Movement state with a movement speed of 0 or less for {0}", gameObject.name));
                        enabled = false;
                        return;
                    }
                    if (movementStates[i].turnSpeed <= 0)
                    {
                        Debug.LogError(string.Format("Movement state with a turn speed of 0 or less for {0}", gameObject.name));
                        enabled = false;
                        return;
                    }
                }
            }
            if (attackingStates.Length == 0)
            {
                Debug.Log(string.Format("{0} is unable to attack.", gameObject.name));
            }
            if (attackingStates.Length > 0)
            {
                for (int i = 0; i < attackingStates.Length; i++)
                {
                    if (attackingStates[i].animationBool == "")
                    {
                        Debug.LogError(string.Format("No attacking states for {0}", gameObject.name));
                        enabled = false;
                        return;
                    }
                }
            }
            if (stats == null)
            {
                Debug.LogError(string.Format("No stats for {0}", gameObject.name));
                enabled = false;
                return;
            }
            if (animator)
            {
                foreach (var item in AllStates)
                {
                    if (!animatorParameters.Contains(item.animationBool))
                    {
                        Debug.LogError(string.Format("{0} doesn't contain {1}", gameObject.name, item.animationBool));
                        enabled = false;
                        return;
                    }
                }
            }
        }

        // Add stateWeight to total weight for each state in idle state array
        foreach (Idle_State state in idleStates)
        {
            totalIdleStateWeight += state.stateWeight;
        }

        // Initialize
        origin = transform.position;
        animator.applyRootMotion = false;
        TryGetComponent(out characterController);
        TryGetComponent(out navMeshAgent);
        healthUI = GetComponentInChildren<Slider>();

        // Assign stats to variables
        maxHealth = stats.health;
        health = maxHealth;
        stamina = stats.stamina;
        attackSpeed = stats.attackSpeed;
        originalDominance = stats.dominance;
        dominance = originalDominance;
        originalAggression = stats.aggression;
        aggression = originalAggression;
        originalScent = scent;
        scent = originalScent;
        isTerritoral = stats.isTerritorial;
        isStealthy = stats.isStealthy;

        // Alert state
        playerAlertStage = PlayerAlertStage.Peaceful;
        playerAlertLevel = 0f;

        // Add stoppingDistance to target
        if (navMeshAgent)
        {
            useNavMesh = true;
            navMeshAgent.stoppingDistance = stopDistance;
        }

        // Set rotation if animal will match surface rotation & animal has child(ren)
        if (matchSurfaceRotation && transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.AddComponent<SurfaceRotation_Shoot>().SetRotationSpeed(surfaceRotationSpeed);
        }
    }

    // HashSet = Dictionary - value for each key = Collection of keys
    // Can test whether or not a value is part of the set
    readonly HashSet<string> animatorParameters = new HashSet<string>();

    // Interface to use Foreach in a collection of states
    private IEnumerable<AI_State> AllStates
    {
        get
        {
            foreach (var item in idleStates)
            {
                yield return item;
            }
            foreach (var item in movementStates)
            {
                yield return item;
            }
            foreach (var item in attackingStates)
            {
                yield return item;
            }
            foreach (var item in deathStates)
            {
                yield return item;
            }
        }
    }

    // Add animal to list when enabled
    private void OnEnable()
    {
        allAnimals.Add(this);
    }
    // Remove from list when disabled & stop all coroutines
    private void OnDisable()
    {
        allAnimals.Remove(this);
        StopAllCoroutines();
    }

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(RandomStartDelay());
    }

    private bool started = false;

    private void Update()
    {
        if (!started)
        {
            return;
        }
        // If update is forced -> Update AI & Reset
        if (isForcedUpdate)
        {
            UpdateAI();
            isForcedUpdate = false;
        }

        // If attacking ->
        // If there is no target animal or target animal is dead : Update AI
        // If target_ani == previous -> Log error for same target
        // Afterwards -> Add deltaTime to attackTimer
        if (CurrentState == WanderState.Attack)
        {
            if (!target_ani || target_ani.CurrentState == WanderState.Dead)
            {
                UpdateAI();
                var previous = target_ani;
                if (previous && previous == target_ani)
                {
                    Debug.LogError(string.Format("Target was same {0}", previous.gameObject.name));
                }
            }
            attackTimer += Time.deltaTime;
        }

        // If attackTimer exceeds attackSpeed -> Reset attackTimer by reducing by attackSpeed
        // If target animal exists : Target animal takes damage || If target animal dies : Update AI
        // If target player exists : Target player takes damage || If target player dies : Update AI
        if (attackTimer > attackSpeed)
        {
            attackTimer -= attackSpeed;
            if (target_ani)
            {
                target_ani.TakeDamage(attack);
            }
            if (target_ani.CurrentState == WanderState.Dead)
            {
                UpdateAI();
            }
        }

        // If player is within animal detection range -> Update player alert state
        isPlayerInRange = false;
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, awareness);
        foreach (Collider c in playersInRange)
        {
            if (c.CompareTag("Player"))
            {
                float fovOrientationAngle = Vector3.Angle(transform.forward, c.transform.position - transform.position);
                if (Mathf.Abs(fovOrientationAngle) < fovAngle / 2)
                {
                    isPlayerInRange = true;
                }
                break;
            }
        }
        // If player location is detected -> Update evasion route
        isPlayerLocated = false;
        Collider[] playerLocations = Physics.OverlapSphere(transform.position, playerAwareness);
        foreach (Collider c in playerLocations)
        {
            if (c.CompareTag("Player"))
            {
                isPlayerLocated = true;
                break;
            }
        }

        var position = transform.position;
        var targetPosition = position;
        UpdatePlayerAlert(isPlayerInRange, isPlayerLocated, position, targetPosition);
        UpdateHealth();
        // Switch case for all AI states
        switch (CurrentState)
        {
            // Attack : Face the target animal/player
            case WanderState.Attack:
                FaceDirection((target_ani.transform.position - position).normalized);
                targetPosition = position;
                break;
            // Chase : See below
            case WanderState.Chase:
                // If there is no primaryPrey or primaryPrey is dead -> Null primaryPrey & set state to idle
                if (!primaryPrey || primaryPrey.CurrentState == WanderState.Dead)
                {
                    primaryPrey = null;
                    SetState(WanderState.Idle);
                    goto case WanderState.Idle;
                }
                // If primaryPrey location is invalid -> Set state to idle & Update AI
                targetPosition = primaryPrey.transform.position;
                ValidatePosition(ref targetPosition);
                if (!IsValidLocation(targetPosition))
                {
                    SetState(WanderState.Idle);
                    targetPosition = position;
                    UpdateAI();
                    break;
                }
                FaceDirection((targetPosition - position).normalized);
                // Stamina drains per time passed & if stamina is zero -> Update AI
                stamina -= Time.deltaTime;
                if (stamina <= 0)
                {
                    UpdateAI();
                }
                break;
            // Evade : See below
            case WanderState.Evade:
                // Target position is current position + projection of current position from primaryPredator/player position normal to y-axis (on to the xz plane)
                if (isPlayerLocated)
                {
                    targetPosition = position + Vector3.ProjectOnPlane(position - player.transform.position, Vector3.up);
                }
                else if (isAnimalInRange)
                {
                    targetPosition = position + Vector3.ProjectOnPlane(position - primaryPredator.transform.position, Vector3.up);
                }
                if (!IsValidLocation(targetPosition))
                {
                    targetPosition = startPosition;
                }
                ValidatePosition(ref targetPosition);
                FaceDirection((targetPosition - position).normalized);
                stamina -= Time.deltaTime;
                if (stamina <= 0)
                {
                    UpdateAI();
                }
                break;
            // Wander : See below
            case WanderState.Wander:
                // Stamina is filled during wander
                stamina = Mathf.MoveTowards(stamina, stats.stamina, Time.deltaTime);
                targetPosition = wanderTarget;
                Debug.DrawLine(position, targetPosition, Color.red);
                FaceDirection((targetPosition - position).normalized);
                // If animal reaches wander target -> Set state to idle & Update AI
                var displacementFromTarget = Vector3.ProjectOnPlane(targetPosition - transform.position, Vector3.up);
                if (displacementFromTarget.magnitude < stopDistance)
                {
                    SetState(WanderState.Idle);
                    UpdateAI();
                }
                break;
            // Idle : See below
            case WanderState.Idle:
                // Stamina is filled during wander
                stamina = Mathf.MoveTowards(stamina, stats.stamina, Time.deltaTime);
                // If animal reaches idle time limit -> Set state to wander & Update AI
                if (Time.time >= idleUpdateTime)
                {
                    SetState(WanderState.Wander);
                    UpdateAI();
                }
                break;
        }
        // If navMeshAgent exits -> Apply values to navMeshAgent
        if (navMeshAgent)
        {
            navMeshAgent.destination = targetPosition;
            navMeshAgent.speed = moveSpeed;
            navMeshAgent.angularSpeed = turnSpeed;
        }
        // If no navMeshAgent -> Apply simpleMove to characterController
        else
        {
            characterController.SimpleMove(moveSpeed * Vector3.ProjectOnPlane(targetPosition - position, Vector3.up).normalized);
        }
    }

    // Method to update health bar
    private void UpdateHealth()
    {
        healthUI.maxValue = maxHealth;
        healthUI.value = health;
    }

    // Update player alert levels
    private void UpdatePlayerAlert(bool isPlayerInRange, bool isPlayerLocated, Vector3 position, Vector3 targetPosition)
    {
        if (CurrentState == WanderState.Dead)
        {
            return;
        }
        switch (playerAlertStage)
        {
            case PlayerAlertStage.Peaceful:
                if (isPlayerInRange)
                {
                    playerAlertStage = PlayerAlertStage.Intrigued;
                    Debug.Log(string.Format("Player sensed by {0} : Intrigued", gameObject.name));
                }
                break;
            case PlayerAlertStage.Intrigued:
                if (isPlayerInRange)
                {
                    playerAlertLevel += 0.01f * stats.sensitivity;
                    if (playerAlertLevel >= 100)
                    {
                        playerAlertStage = PlayerAlertStage.Alerted;
                        Debug.Log(string.Format("{0} alerted by Player : ALERTED", gameObject.name));
                    }
                }
                else
                {
                    playerAlertLevel -= 0.02f;
                    if (playerAlertLevel <= 0)
                    {
                        playerAlertStage = PlayerAlertStage.Peaceful;
                        Debug.Log(string.Format("Player NOT sensed by {0} : Peaceful", gameObject.name));
                    }
                }
                break;
            case PlayerAlertStage.Alerted:
                if (isPlayerLocated)
                {
                    SetState(WanderState.Evade);
                    if (!isPlayerInRange)
                    {
                        playerAlertLevel -= 0.02f;
                        if (playerAlertLevel <= 90)
                        {
                            playerAlertStage = PlayerAlertStage.Intrigued;
                            Debug.Log(string.Format("Player NOT sensed by {0} : Intrigued", gameObject.name));
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    // Method on if and when arrow hits animal
    private void OnCollisionEnter(Collision collision)
    {
        var position = transform.position;
        var targetPosition = position;
        if (CurrentState == WanderState.Dead)
        {
            return;
        }
        else if (collision.transform.CompareTag("Arrow"))
        {
            isHitByArrow = true;
            StartCoroutine(FleeArrow_co(isPlayerLocated, position, targetPosition));
        }
    }

    // Coroutine to flee arrow
    private IEnumerator FleeArrow_co(bool isPlayerLocated, Vector3 position, Vector3 targetPosition)
    {
        if (isPlayerLocated)
        {
            Run();
            targetPosition = position + Vector3.ProjectOnPlane(position - player.transform.position, Vector3.up);
        }
        if (!IsValidLocation(targetPosition))
        {
            targetPosition = startPosition;
        }
        FaceDirection((targetPosition - position).normalized);
        ValidatePosition(ref targetPosition);
        stamina -= Time.deltaTime;
        if (stamina <= 0)
        {
            UpdateAI();
        }
        yield return new WaitForSeconds(10f);
    }

    // Method to face animal in direction of action
    private void FaceDirection(Vector3 facePosition)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Vector3.RotateTowards(transform.forward, facePosition, turnSpeed * Time.deltaTime * Mathf.Deg2Rad, 0f), Vector3.up), Vector3.up);
    }
    // Method to take damage when attacked -> Reduce health & Call die method when health <= 0
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(string.Format("{0} Health : {1}", gameObject.name, health));
        if (health <= 0)
        {
            Die();
        }
    }
    // Method when dead -> Set state to dead
    public void Die()
    {
        SetState(WanderState.Dead);
    }

    // Method to update AI
    private void UpdateAI()
    {
        // Error message if animal is dead
        if (CurrentState == WanderState.Dead)
        {
            Debug.Log("Animal is already dead -> Cannot update AI");
            return;
        }

        var position = transform.position;
        primaryPredator = null;
        if (awareness > 0)
        {
            // Set detection range if awareness > 0
            var closestDistance = awareness;
            // If there are animals
            if (allAnimals.Count > 0)
            {
                // Check chasing status of all animals
                foreach (var chaser in allAnimals)
                {
                    // If the primaryPrey or target animal of chaser is not this animal
                    if (chaser.primaryPrey != this && chaser.target_ani != this)
                    {
                        continue;
                    }
                    // If chaser is dead
                    if (chaser.CurrentState == WanderState.Dead)
                    {
                        continue;
                    }
                    // If chaser is not targeting & is stealthy OR dominance is lower OR outside of detection range
                    var distance = Vector3.Distance(position, chaser.transform.position);
                    if ((chaser.target_ani != this && chaser.isStealthy) || chaser.dominance <= this.dominance || distance > closestDistance)
                    {
                        continue;
                    }
                    closestDistance = distance;
                    primaryPredator = chaser;
                }
            }
        }

        var isSameTarget = false;
        // If primaryPrey exists
        if (primaryPrey)
        {
            // If primaryPrey is dead -> Null
            if (primaryPrey.CurrentState == WanderState.Dead)
            {
                primaryPrey = null;
            }
            else
            {
                // If distance exceeds detection range -> Null / is same target
                var distanceToPrey = Vector3.Distance(position, primaryPrey.transform.position);
                if (distanceToPrey > scent)
                {
                    primaryPrey = null;
                }
                else
                {
                    isSameTarget = true;
                }
            }
        }
        // If primaryPrey doesn't exist
        if (!primaryPrey)
        {
            primaryPrey = null;
            if (dominance >= 0 && attackingStates.Length > 0)
            {
                var aggPer = aggression * 0.01f;
                aggPer *= aggPer;
                var closestDistance = scent;
                // Check all potentialPrey in animal list
                foreach (var potentialPrey in allAnimals)
                {
                    if (potentialPrey.CurrentState == WanderState.Dead)
                    {
                        Debug.LogError(string.Format("Dead animal : {0}", potentialPrey.gameObject.name));
                    }
                    // If this animal is the potentialPrey OR is of the same species (w/0 territorial) OR dominance is higher OR is stealthy -> Ignore
                    if (potentialPrey == this || (potentialPrey.species == species && !isTerritoral) || potentialPrey.dominance > dominance || potentialPrey.isStealthy)
                    {
                        continue;
                    }
                    // If animal species is part of peaceful list -> Ignore
                    if (peacefulTowards.Contains(potentialPrey.species))
                    {
                        continue;
                    }
                    // If Random value is bigger than aggPer -> Ignore
                    if (Random.Range(0f, 0.99999f) >= aggPer)
                    {
                        continue;
                    }
                    // If prey is in invalid location -> Ignore
                    var preyPosition = potentialPrey.transform.position;
                    if (!IsValidLocation(preyPosition))
                    {
                        continue;
                    }

                    // If distance exceeds detection range -> Ignore
                    var distance = Vector3.Distance(position, preyPosition);
                    if (distance > closestDistance)
                    {
                        continue;
                    }
                    if (logChanges)
                    {
                        Debug.Log(string.Format("[{0}] found [{1}] => Chasing", gameObject.name, potentialPrey.gameObject.name));
                    }
                    closestDistance = distance;
                    primaryPrey = potentialPrey;
                }
            }
        }
        
        // Set aggressive state
        var isAggressive = false;
        if (primaryPrey)
        {
            if ((isSameTarget && stamina > 0) || stamina > minStaminaForAggression)
            {
                isAggressive = true;
            }
            else
            {
                primaryPrey = null;
            }
        }
        // Set defensive state
        var isDefensive = false;
        if (primaryPredator && !isAggressive)
        {
            if (stamina > minStaminaForEvade)
            {
                isDefensive = true;
            }
        }

        // Determine if prey/player is in range
        var updateTargetAI = false;
        var isPreyInRange = isAggressive && Vector3.Distance(position, primaryPrey.transform.position) < CalcAttackRange(primaryPrey);
        var isPredatorInRange = isDefensive && Vector3.Distance(position, primaryPredator.transform.position) < CalcAttackRange(primaryPredator);

        if (isPredatorInRange)
        {
            target_ani = primaryPredator;
            isAnimalInRange = true;
        }
        else if (isPreyInRange)
        {
            target_ani = primaryPrey;
            if (!target_ani.target_ani == this)
            {
                updateTargetAI = true;
            }
        }
        else
        {
            target_ani = null;
        }

        // Dettermine attack status -> Set state
        var isAttacking = attackingStates.Length > 0 && (isPreyInRange || isPredatorInRange);
        if (isAttacking)
        {
            SetState(WanderState.Attack);
        }
        else if (isAggressive)
        {
            SetState(WanderState.Chase);
        }
        else if (isDefensive)
        {
            SetState(WanderState.Evade);
        }
        else if (CurrentState != WanderState.Idle && CurrentState != WanderState.Wander)
        {
            SetState(WanderState.Idle);
        }
        if (isAttacking && updateTargetAI)
        {
            target_ani.isForcedUpdate = true;
        }
    }

    // Method to determine location validity
    private bool IsValidLocation(Vector3 targetPosition)
    {
        if (!constrainedToZone)
        {
            return true;
        }
        var distanceFromStart = Vector3.Distance(startPosition, targetPosition);
        var isInZone = distanceFromStart < wanderDistance;
        return isInZone;
    }

    // Method to calculate attack range for animals
    private float CalcAttackRange(AnimalController_Shoot other)
    {
        var thisRange = navMeshAgent ? navMeshAgent.radius : characterController.radius;
        var otherRange = other.navMeshAgent ? other.navMeshAgent.radius : other.characterController.radius;
        return attackReach + thisRange + otherRange;
    }

    // Method to set state
    public void SetState(WanderState state)
    {
        var previousState = CurrentState;
        if (previousState == WanderState.Dead)
        {
            Debug.LogError("Animal is dead -> Cannot set state");
            return;
        }

        // Add if(state != previousState)?
        CurrentState = state;
        // Switch case for each state
        switch (CurrentState)
        {
            case WanderState.Idle:
                Idle();
                break;
            case WanderState.Chase:
                Chase();
                break;
            case WanderState.Evade:
                Evade();
                break;
            case WanderState.Attack:
                Attack();
                break;
            case WanderState.Dead:
                Dead();
                break;
            case WanderState.Wander:
                Wander();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Method to clear animation bool values
    private void ClearAnimatorBools()
    {
        foreach (var item in idleStates)
        {
            TrySetBool(item.animationBool, false);
        }
        foreach (var item in movementStates)
        {
            TrySetBool(item.animationBool, false);
        }
        foreach (var item in attackingStates)
        {
            TrySetBool(item.animationBool, false);
        }
        foreach (var item in deathStates)
        {
            TrySetBool(item.animationBool, false);
        }
    }
    // Method to set animation bools
    private void TrySetBool(string parameterName, bool value)
    {
        if (!string.IsNullOrEmpty(parameterName))
        {
            if (logChanges || animatorParameters.Contains(parameterName))
            {
                animator.SetBool(parameterName, value);
            }
        }
    }

    // Death method : Clear bools -> Set death animation -> Invoke death event -> Finish navMeshAgent (destination = current location) -> Disable 
    private void Dead()
    {
        healthUI.value = 0;
        ClearAnimatorBools();
        if (deathStates.Length > 0)
        {
            TrySetBool(deathStates[Random.Range(0, deathStates.Length)].animationBool, true);
        }
        if (isHitByArrow)
        {
            ui.AddKill(species, stats.value);
        }
        deathEvent.Invoke();
        if (navMeshAgent && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.destination = transform.position;
        }
        enabled = false;
        Destroy(gameObject, 5f);
    }
    // Attack method : Set turn speed -> Clear bools -> Set attack animation -> Invoke attack event
    private void Attack()
    {
        turnSpeed = 120f;
        ClearAnimatorBools();
        var attackState = Random.Range(0, attackingStates.Length);
        TrySetBool(attackingStates[attackState].animationBool, true);
        attackEvent.Invoke();
    }
    // Evade method : Call run method -> Invoke move event
    private void Evade()
    {
        Run();
        moveEvent.Invoke();
    }
    // Chase method : Call run method -> Invoke move event
    private void Chase()
    {
        Run();
        moveEvent.Invoke();
    }
    // Run method : Set max movement state -> Assert moveState is not null (Checks reference) -> Clear bools -> Set animation bool
    private void Run()
    {
        Movement_State moveState = null;
        var maxSpeed = 0f;
        foreach (var state in movementStates)
        {
            var stateSpeed = state.moveSpeed;
            if (stateSpeed > maxSpeed)
            {
                moveState = state;
                maxSpeed = stateSpeed;
            }
        }
        Assert.IsNotNull(moveState, string.Format("No movement states in {0}'s wander script", gameObject.name));
        turnSpeed = moveState.turnSpeed;
        moveSpeed = maxSpeed;
        ClearAnimatorBools();
        TrySetBool(moveState.animationBool, true);
    } 
    // Walk method : Set min movement state -> Assert moveState is not null (Checks reference) -> Clear bools -> Set animation bool 
    private void Walk()
    {
        Movement_State moveState = null;
        var minSpeed = float.MaxValue;
        foreach (var state in movementStates)
        {
            var stateSpeed = state.moveSpeed;
            if (stateSpeed < minSpeed)
            {
                moveState = state;
                minSpeed = stateSpeed;
            }
        }
        Assert.IsNotNull(moveState, string.Format("No movement states in {0}'s wander script", gameObject.name));
        turnSpeed = moveState.turnSpeed;
        moveSpeed = minSpeed;
        ClearAnimatorBools();
        TrySetBool(moveState.animationBool, true);
    }
    // Idle method : Null primaryPrey & player -> Randomly set idle state weight -> Add state weight to current weight
    // If current weight exceeds target weight -> Start idleUpdateTime -> Clear bools -> Set animation bool -> Stop movement
    private void Idle()
    {
        primaryPrey = null;
        var targetWeight = Random.Range(0, totalIdleStateWeight);
        var curWeight = 0;
        foreach (var idleState in idleStates)
        {
            curWeight += idleState.stateWeight;
            if (targetWeight > curWeight)
            {
                continue;
            }
            idleUpdateTime = Time.time + Random.Range(idleState.minStateTime, idleState.maxStateTime);
            ClearAnimatorBools();
            TrySetBool(idleState.animationBool, true);
            moveSpeed = 0f;
            break;
        }
        idleEvent.Invoke();
    }
    // Wander method : Null primaryPrey & player -> Randomly set wander target within range -> Validate position -> Set target position -> Call walk method
    private void Wander()
    {
        primaryPrey = null;
        var rand = Random.insideUnitSphere * wanderDistance;
        var targetPosition = startPosition + rand;
        ValidatePosition(ref targetPosition);
        wanderTarget = targetPosition;
        Walk();
    }

    // Method to validate position for navMesh
    private void ValidatePosition(ref Vector3 targetPosition)
    {
        if (navMeshAgent)
        {
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(targetPosition, out hit, Mathf.Infinity, 1 << NavMesh.GetAreaFromName("Walkable")))
            {
                Debug.LogError("Unable to sample nav mesh -> Add Nav Mesh Layer with name [Walkable]");
                enabled = false;
                return;
            }
            targetPosition = hit.position;
        }
    }

    // Coroutine to randomly delay start
    private IEnumerator RandomStartDelay()
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        started = true;
        StartCoroutine(ConstantTicking(Random.Range(0.7f, 1f)));
    }
    // Coroutine to constantly update AI per random delay (Set within RandomStartDelay coroutine)
    private IEnumerator ConstantTicking(float delay)
    {
        while (true)
        {
            UpdateAI();
            yield return new WaitForSeconds(delay);
        }
    }

    // Delete all set stats and replace with default
    public void BasicSetup()
    {
        Movement_State walking = new Movement_State();
        Movement_State running = new Movement_State();
        Idle_State idle = new Idle_State();
        AI_State attacking = new AI_State();
        AI_State death = new AI_State();

        walking.stateName = "Walking";
        walking.animationBool = "isWalking";
        running.stateName = "Running";
        running.animationBool = "isRunning";
        movementStates = new Movement_State[2];
        movementStates[0] = walking;
        movementStates[1] = running;

        idle.stateName = "Idle";
        idle.animationBool = "isIdling";
        idleStates = new Idle_State[1];
        idleStates[0] = idle;

        attacking.stateName = "Attacking";
        attacking.animationBool = "isAttacking";
        attackingStates = new AI_State[1];
        attackingStates[0] = attacking;

        death.stateName = "Dead";
        death.animationBool = "isDead";
        deathStates = new AI_State[1];
        deathStates[0] = death;
    }
}
