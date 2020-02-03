using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionTypes
{
	Idle,
	Walk,
	Strike
}

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
	public static Vector2[] cardinalDirections =
	{
		Vector2.up,
		Vector2.right,
		Vector2.down,
		Vector2.left
	};

	public GameObject pickaxe;
	public AudioSource footsteps;

	[HideInInspector]
	public List<Collectible> gears = new List<Collectible>();
	[HideInInspector]
	public int playerID;
	[HideInInspector]
	public int score;
	[HideInInspector]
	public float speed = 300;

	private Vector2 currentFacing;
    private Vector2 moveDirection;
	private Rigidbody2D rigid;
	private Animator animator;
	private Coroutine currentAction;
	private SpriteRenderer spriteRenderer;
	private CircleCollider2D circleCollider;
	private PlayerLightController playerLightController;

	private bool cancelUpdate;
	private int strikeDamage = 6;
	private int startingHealth = 24;
	private int health;
	private float timeToStrike = 0.5f;
	private float timeToStrikeRecover = 0.14f;
	private float strikeStunDuration = 0.33f;
	private float animSpeedStore;

	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		circleCollider = GetComponent<CircleCollider2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		playerLightController = GetComponentInChildren<PlayerLightController>();
		playerLightController.SetSpotlightLevel(8);
	}

	private void Start()
	{
		if (transform.position.x < 0)
		{
			currentFacing = Vector2.right;
		}
		else
		{
			currentFacing = Vector2.left;
		}

		SetAnimation(ActionTypes.Idle);
	}

	void Update()
	{
		if (cancelUpdate)
		{
			return;
		}

        if (Input.GetJoystickNames().Length < 2)
        {
            if (InputManager.Instance.KFire)
            {
				if (currentAction != null)
				{
					StopCoroutine(currentAction);
				}

				currentAction = StartCoroutine(Act(ActionTypes.Strike, timeToStrike, timeToStrikeRecover));
                return;
            }

            moveDirection = new Vector2(InputManager.Instance.Horizontal, InputManager.Instance.Vertical);
            rigid.velocity = moveDirection * speed;

            if (InputManager.Instance.Horizontal != 0 || InputManager.Instance.Vertical != 0)
            {
                if (!footsteps.isPlaying)
                {
                    footsteps.Play();
                }
            }
        }
        else
        {
            rigid.velocity = moveDirection * speed;
        }

		playerLightController.UpdateLights(currentFacing);
		SetFacing(moveDirection);        
	}

	public void Reset(Vector2 resetPos)
	{
		cancelUpdate = false;
		circleCollider.enabled = true;
		transform.position = new Vector3(resetPos.x, resetPos.y, transform.position.z);
		health = startingHealth;
		gears.Clear();
		score = 0;
	}

	public void GoalReached()
	{
		currentFacing = Vector2.down;
		SetAnimation(ActionTypes.Walk);
		GameManager.instance.cumulativeScores[playerID] += score;
		StopAllInteractions();
		gears.Clear();
	}

	public void StopAllInteractions()
	{
		rigid.velocity = Vector2.zero;
		circleCollider.enabled = false;
		cancelUpdate = true;
	}

	private void SetFacing(Vector2 currentDirection)
	{
		if (currentDirection == Vector2.zero)
		{
			SetAnimation(ActionTypes.Idle);
			return;
		}

		Vector2 closestDirection = cardinalDirections[0];

		for (int i = 0; i < cardinalDirections.Length - 1; i++)
		{
			for (int o = i + 1; o < cardinalDirections.Length; o++)
			{
				if (Vector2.Distance(currentDirection, cardinalDirections[o]) < Vector2.Distance(currentDirection, closestDirection))
				{
					closestDirection = cardinalDirections[o];
				}
			}
		}

		currentFacing = closestDirection;
		SetAnimation(ActionTypes.Walk);
	}

	public void SetAnimation(ActionTypes action)
	{
		if (currentFacing == Vector2.up)
		{
			if (action == ActionTypes.Idle)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "BackIdle");
			}
			else if (action == ActionTypes.Walk)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "BackWalk");
			}
			else if (action == ActionTypes.Strike)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "BackAttack", -1, 0);
			}
		}
		else if (currentFacing == Vector2.right)
		{
			Vector3 scale = animator.transform.localScale;
			scale.x = Mathf.Abs(scale.x);
			animator.transform.localScale = scale;

			if (action == ActionTypes.Idle)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "SideIdle");
			}
			else if (action == ActionTypes.Walk)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "SideWalk");
			}
			else if (action == ActionTypes.Strike)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "SideAttack", -1, 0);
			}
		}
		else if (currentFacing == Vector2.down)
		{
			if (action == ActionTypes.Idle)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "FrontIdle");
			}
			else if (action == ActionTypes.Walk)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "FrontWalk");
			}
			else if (action == ActionTypes.Strike)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "FrontAttack", -1, 0);
			}
		}
		else if (currentFacing == Vector2.left)
		{
			Vector3 scale = animator.transform.localScale;
			scale.x = -Mathf.Abs(scale.x);
			animator.transform.localScale = scale;

			if (action == ActionTypes.Idle)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "SideIdle");
			}
			else if (action == ActionTypes.Walk)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "SideWalk");
			}
			else if (action == ActionTypes.Strike)
			{
				animator.Play(ProjectReferences.instance.GetColorForPlayerID(playerID) + "SideAttack", -1, 0);
			}
		}
	}

	public IEnumerator Act(ActionTypes action, float timeToAct = 0, float timeToRecover = 0)
	{
		if (action == ActionTypes.Strike)
		{
			footsteps.Stop();
			SetAnimation(action);
			rigid.velocity = Vector2.zero;
			cancelUpdate = true;
		}

		yield return new WaitForSeconds(timeToAct);

		if (action == ActionTypes.Strike)
		{
			pickaxe.transform.localPosition = currentFacing;
			pickaxe.SetActive(true);
		}

		yield return new WaitForSeconds(timeToRecover);

		if (action == ActionTypes.Strike)
		{
			pickaxe.SetActive(false);
			cancelUpdate = false;
		}
	}

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();

        if (!footsteps.isPlaying)
        {
            footsteps.Play();
        }
    }

    void OnStrike()
    {
		if (currentAction != null)
		{
			StopCoroutine(currentAction);
		}

		currentAction = StartCoroutine(Act(ActionTypes.Strike, timeToStrike, timeToStrikeRecover));
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.tag == "Pickaxe" && other.gameObject != pickaxe)
		{
			StartCoroutine(TakeDamage(strikeDamage, strikeStunDuration));
		}
	}

	public IEnumerator TakeDamage(int damage, float stunDuration = 0)
	{
		health -= damage;
		animator.Play("FlashRed", -1, 0);

		if (currentAction != null)
		{
			StopCoroutine(currentAction);
		}

		cancelUpdate = true;

		if (health > 0)
		{
			yield return new WaitForSeconds(stunDuration);
			cancelUpdate = false;
		}
		else
		{
			health = 0;

			yield return new WaitForSeconds(0.13f);

			Instantiate(ProjectReferences.instance.deathEffect, transform.position, Quaternion.identity, null);

			for (int i = 0; i < gears.Count; i++)
			{
				gears[i].gameObject.SetActive(true);
				gears[i].transform.position = transform.position;
				gears[i].StartCoroutine(gears[i].Fling());
			}

			gameObject.SetActive(false);
		}
	}
}