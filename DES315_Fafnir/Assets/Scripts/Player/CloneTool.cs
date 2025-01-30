using UnityEngine;

public class CloneTool : Tool_Swing
{
	private CloneAI clone;
	private const float AngleSpeed = 960;
    private void Start()
    {
		clone = GetComponentInParent<CloneAI>();
    }
    private void Update()
	{
		//Update tool rotation and activation
		HandleTool();
		// Run any active timers
		RunTimer();
	}

	protected override void HandleTool()
	{
		float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, clone.ComList[clone.ComPos].angl, AngleSpeed * Time.deltaTime);

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		// If the user is using the tool
		if (Input.GetButtonDown("Activate") && cooldownTimer <= 0f)
		{
			// Set the tool hitbox to be active
			toolSprite.GetComponent<SpriteRenderer>().color = hitColour;
			toolSprite.GetComponent<BoxCollider2D>().size = Vector2.one;
			// Start the hit frame timer
			hitTimer = hitActive;
		}
	}
}
