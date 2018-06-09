/* TimeManager.cs
 * Date Created: 6/09/18
 * Last Edited: 6/08/18
 * Programmer: Jack Bruce
 * Description: Slows time down. Meant for casting of spells.
 * Each Spell shall have a "
 */
using UnityEngine;

public class TimeManager : MonoBehaviour {

	public float slowdownFactor = 0.05f;
	public float slowdownLength = 2f; //can be adjusted for how long you want spell cast time to be

	void Update()
	{
		//slowly bring time back to normal
		//may not be desired, but I think it is cool ;)
		//Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime; 
		//Time.timeScale = Mathf.Clamp (Time.timeScale, 0f, 1f);

	}

	public void StartSlowDown() 
	{
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * .02f;
		Debug.Log ("Slow");
	}
		
	public void StopSlowDown()
	{
		Time.timeScale = 1;
	}

}
