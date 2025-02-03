using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CloneTool : Tool_Swing
{
	private CloneAI clone;
	const float DistanceMax = 5.0f;
	const float AngleMargin = 15.0f;

    private void Start()
    {
        // Initialise the tool as inactive
        toolSprite = gameObject.GetComponentInChildren<SpriteRenderer>().gameObject;    
        toolSprite.GetComponent<SpriteRenderer>().color = ogColour;
        toolSprite.GetComponent<BoxCollider2D>().size = Vector2.zero;
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
        //Set speed proportional to duration
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, clone.ComList[clone.ComPos].angl, (clone.ComList[clone.ComPos].angl * clone.ComList[clone.ComPos].dur) * Time.deltaTime);

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		if (clone.ComList[clone.ComPos].type == PCom_t.P_ACTION) SetToolActive();
    }
}
