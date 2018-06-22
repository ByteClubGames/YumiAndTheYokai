/* AbilityUse.cs
 * Date Created: 3/10/18
 * Last Edited: 3/10/18
 * Programmer: Jack Bruce
 * Description: Probably will delete this script. Using as a model
 * for "IceSpellUse.cs"
 */

using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine;

public class AbilityUse : MonoBehaviour {

    public GameObject fireballPrefab;
    private FireBall fba;
    private Stopwatch abilityCooldownTimer;
    private Button button;
    private Image fillImage;

    public void OnAbilityUse(GameObject btn)
    {
        //if ability is not on cool down use it
        fillImage = btn.transform.GetChild(0).gameObject.GetComponent<Image>();
        UnityEngine.Debug.Log(btn.transform.GetChild(0).gameObject.name);
        button = btn.GetComponent<Button>();
        // button.interactable = false;     // Hunter commented out because it's weird on master 
        fillImage.fillAmount = 1;
        abilityCooldownTimer = new Stopwatch();
        abilityCooldownTimer.Start();
        fba = new FireBall();

        GameObject go = Instantiate<GameObject>(fireballPrefab);

        StartCoroutine(SpinImage());

    }

    private IEnumerator SpinImage() 
    {
        UnityEngine.Debug.Log(fba.AbilityCoolDown);
        while (abilityCooldownTimer.IsRunning && abilityCooldownTimer.Elapsed.TotalSeconds < fba.AbilityCoolDown)
        {
            UnityEngine.Debug.Log(fillImage.fillAmount);
            fillImage.fillAmount = ((float)abilityCooldownTimer.Elapsed.TotalSeconds / fba.AbilityCoolDown);
            yield return null;
        }
        fillImage.fillAmount = 0;
        // button.interactable = true;      // Hunter commented out because it's weird on master  
        abilityCooldownTimer.Stop();
        abilityCooldownTimer.Reset();

        yield return null;
    }

}
