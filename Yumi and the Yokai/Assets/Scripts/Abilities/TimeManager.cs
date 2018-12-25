/* TimeManager.cs
 * Date Created: 6/09/18
 * Last Edited: 6/08/18
 * Programmer: Jack Bruce and Evan Gierst
 * Description: Slows time down. Meant for casting of spells.
 * Each SpellSpawner shall have this script attached.
 * slowdownFactor and slowdownLength can be adjusted acordingly.
 */
using UnityEngine;

public class TimeManager : MonoBehaviour {

	public float slowdownFactor = 0.05f;
	public float slowdownLength = 2f; //can be adjusted for how long you want spell cast time to be
    private bool resuming = false;
    private float lastScale = 1f; // Detection of competing TimeManagers

	void Update()
	{
        if (Time.timeScale < lastScale) // If another timemanager slowed time, then cancel.
        {
            lastScale = 1f;
            return;
        }
        else if (resuming)
        {
            //slowly bring time back to normal
            //may not be desired, but I think it is cool ;)
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            lastScale = Time.timeScale;
        }
	}

	public void StartSlowDown() 
	{
        resuming = false;
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * .02f;
		Debug.Log ("Slow");
        lastScale = Time.timeScale;
    }
		
	public void StopSlowDown()
	{
        resuming = true;
		//Time.timeScale = 1;
	}

}
