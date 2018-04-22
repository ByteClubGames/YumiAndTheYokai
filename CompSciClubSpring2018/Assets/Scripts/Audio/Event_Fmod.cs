
/* =======================================================================================================
 * 
 * Description: Handles Fmod Events inside of Unity. Also creates a UI inside of the Unity Editor 
 * under the inspector tab
 * 
 * Unity includes a feature called Platform Dependent Compilation (#if UNITY_EDITOR).
 * This consists of some preprocessor directives that let you partition
 * your scripts to compile and execute a section of code exclusively for one of the supported platforms.
 * 
 * You can run this code within the Unity Editor,
 * so you can compile the code specifically for your target platform and test it in the Editor!
*  ======================================================================================================= 
*/


using UnityEngine;
#if UNITY_EDITOR //#define directive for calling Unity Editor scripts from your game code.
using UnityEditor;
#endif

public class Event_Fmod : MonoBehaviour
{
    //If you use the attribute [EventRef] on any string property you are using to hold the path of an event

    // The two lines of code below creates the UI in Unity
    // UI contains the Script, Event Path, and Event Properties
    [FMODUnity.EventRef]
    public string fmodEventName;

    FMOD.Studio.EventInstance fmodEventInstance;

    void Awake()
    {
        if (fmodEventName != null)
        {
            fmodEventInstance = FMODUnity.RuntimeManager.CreateInstance(fmodEventName);
            if (fmodEventInstance.isValid())
            {
                fmodEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            }
#if UNITY_EDITOR
            EditorApplication.pauseStateChanged += HandlePauseState;
#endif
        }
    }

#if UNITY_EDITOR
    // Pause the fmod instance when the unity editor pauses
    void HandlePauseState(PauseState state)
    {
        //Debug.Log(state);
        Pause(state == PauseState.Paused);
    }
#endif

    public void Pause(bool pause)
    {
        if (fmodEventInstance.isValid())
        {
            fmodEventInstance.setPaused(pause);
            FMODUnity.RuntimeManager.StudioSystem.update();
        }
    }

    void OnEnable()
    {
        if (fmodEventInstance.isValid())
        {
            fmodEventInstance.start();
        }
    }
}
