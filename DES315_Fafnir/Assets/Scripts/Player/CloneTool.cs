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

		//Check if the current command is an ACTION command
		if (clone.ComList[clone.ComPos].type == PCom_t.P_ACTION && cooldownTimer <= 0f)
		{
			// Set the tool hitbox to be active
			toolSprite.GetComponent<SpriteRenderer>().color = hitColour;
			toolSprite.GetComponent<BoxCollider2D>().size = Vector2.one;
			// Start the hit frame timer
			hitTimer = hitActive;

			GameObject hitbutton = GetClickedButton();
            if (hitbutton == null)
			{
				return;
			}
		}
	}

    /*
        |-----------------------------------------------|
        |   FUNCTION NOT WORKING AS OF THIS MOMENT!!!!  |
        |-----------------------------------------------|

        Returns null if no button found!

        Check position of every button and run distance function on their position to the player (this works for the tool since the tool is centered on the player)

        If the distance is less than 5 [radius of the tool's reach] then we can add it to a list of buttons to process

        Get X/Y distance from all buttons in the list, then grab the hypotenuse

        Find angle between tool and currently checked button, angle A is the target angle, B is always 90* and C is unessecary to calculate since angle A satisfies the
        angule we are aiming to later check

        To find the angle of A we get the Y distance and plug that into a COS function [when converted into radians and offset by 90* since the tool is off by 90* to the player]

            |\
            | \
            |  \
            | A	\
            |___/\
            |	  \
            |	   \
            |		\
       \ /	|		 \
        Y	|		  \
        |	|		   \
            |			\
            |			 \
            |			 /\
            |----|		/  \
            | B	 |		| C	\
            |____|______|____\

                   \ /
                    X
                   / \

        Once we find the angle A has been found we should have the proper angle between the checked button and the tool
        Next we find which angle fits the most compared to the tool's angle with some margin of error, however some rooms should be placed on potential null hits

        Margin of error should be 15* positive and negative from the current determined angle [this is due to testing with the tool and click points on a square]
        if the angle is satisfied then the button MUST be pressed by the clone and will return the button instance from the function, otherwise if not then we check the next button

        Should all buttons be checked and no angle satisfies the conditions then it should be assumed the player did not hit any button on the action command
     */
    GameObject GetClickedButton()
	{
        List<GameObject> buttoncheck = new List<GameObject>();					//Buttons within the tool distance
        GameObject[] Buttons = GameObject.FindGameObjectsWithTag("Clickable");	//All buttons in scene

		foreach (GameObject button in Buttons)
		{
			Vector3 bpos = button.transform.position;

			//Add to buttoncheck if the button is within distance
			if (Vector3.Distance(bpos, transform.parent.position) < DistanceMax)
			{
				buttoncheck.Add(button);
			}
		}

		//Angle check
		for (int x = 0; x < buttoncheck.Count; x++)
		{
			float disty = buttoncheck[x].transform.position.y - transform.parent.position.y;
            //float distx = buttoncheck[x].transform.position.x - transform.parent.position.x;
            //float hypo = Mathf.Sqrt((distx * distx) + (disty * disty));
            //float angle = Mathf.Cos(disty);
            float distangle = 2f * Mathf.PI / disty;
            float distTotal = (Mathf.Cos(distangle * Mathf.Deg2Rad) * 180) + 90;   //Convert input to radians since unity expects it as so [for some fucking reason] then adjust to degree's

            if (clone.ComList[clone.ComPos].angl > distTotal - AngleMargin && clone.ComList[clone.ComPos].angl < distTotal + AngleMargin)
			{
                return buttoncheck[x];
			}
		}
        return null;
	}
}
