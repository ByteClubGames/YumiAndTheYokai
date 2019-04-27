using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEnabler : MonoBehaviour
{
    private string activeCharacter;
    public GameObject CharacterYumi;
    private GameObject CharacterYokai;
    private YumiMovement yumiMovementScript;
    private YokaiMovement yokaiMovementScript;
    private YokaiSwitcher characterSwitcher;
    private SpellCasting spellcaster;
    private VisibilityController invisibleObjects;

    public struct CharStatus
    {
        private bool onGround;
        private bool onRightSlope;
        private bool onLeftSlope;
        private bool onRightSlope_Steep;
        private bool onLeftSlope_Steep;
        private bool onLowRoof;
        private bool onRightSlide;
        private bool onLeftSlide;
        private bool onRightWall;
        private bool onLeftWall;

        /// <summary>
        /// Struct initializer.
        /// </summary>
        /// <param name="flag">Bool flag; Should be initialized as false.</param>
        public CharStatus(bool flag)
        {
            onGround = flag;
            onLowRoof = flag;
            onRightSlope = flag;
            onLeftSlope = flag;
            onRightSlope_Steep = flag;
            onLeftSlope_Steep = flag;
            onRightSlide = flag;
            onLeftSlide = flag;
            onRightWall = flag;
            onLeftWall = flag;
        }

        public void setOnGround(bool flag)
        {
            onGround = flag;
        }

        public void setOnLowRoof(bool flag)
        {
            onLowRoof = flag;
        }

        public void setOnRightSlope(bool flag)
        {
            onRightSlope = flag;
        }

        public void setOnLeftSlope(bool flag)
        {
            onLeftSlope = flag;
        }

        public void setOnRightSlope_Steep(bool flag)
        {
            onRightSlope = flag;
        }

        public void setOnLeftSlope_Steep(bool flag)
        {
            onLeftSlope = flag;
        }

        public void setOnRightSlide(bool flag)
        {
            onRightSlide = flag;
        }

        public void setOnLeftSlide(bool flag)
        {
            onLeftSlide = flag;
        }

        public void setOnLeftWall(bool flag)
        {
            onLeftWall = flag;
        }

        public void setOnRightWall(bool flag)
        {
            onLeftWall = flag;
        }
    }
    public CharStatus activeCharacterStatus;

    // Start is called before the first frame update
    void Start()
    {
        activeCharacter = "Yumi";
        activeCharacterStatus = new CharStatus(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        //GetCharacterStatus();
    }


    public string GetActiveCharacter()
    {
        return activeCharacter;
    }


    #region Control Requests
    public void RequestJump()
    {

    }


    public void RequestCancelJump()
    {

    }


    public void RequestLeft()
    {

    }


    public void RequestRight()
    {

    }


    public void RequestYokai()
    {
        if(activeCharacter == "Yumi")
        {
            activeCharacter = "Yokai";
            characterSwitcher.SpawnYokai();
            invisibleObjects.SetInvisible();
            
        }
        else
        {
            activeCharacter = "Yumi";
            characterSwitcher.DeleteYokai(GameObject.Find("Yokai(Clone)"));
            invisibleObjects.SetInvisible();
        }
    }


    public void RequestEarth()
    {
        if (activeCharacter == "Yumi")
        {
            if (spellcaster.GetSpellStatus())
            {
                spellcaster.DestroySpellSpawner();
            }
            spellcaster.CallEarthSpell();
        }
    }


    public void RequestIce()
    {
        if (activeCharacter == "Yumi")
        {
            if (spellcaster.GetSpellStatus())
            {
                spellcaster.DestroySpellSpawner();
            }
            spellcaster.CallIceSpell();
        }
    }


    public void RequestWind()
    {
        if (activeCharacter == "Yumi")
        {
            if (spellcaster.GetSpellStatus())
            {
                spellcaster.DestroySpellSpawner();
            }
            spellcaster.CallWindSpell();
        }
    }


    public void RequestCancelSpells()
    {
        if (activeCharacter == "Yumi")
        {
            spellcaster.DestroySpellSpawner();
        }
        else
        {
            activeCharacter = "Yumi";
            characterSwitcher.DeleteYokai(GameObject.Find("Yokai(Clone)"));
            invisibleObjects.SetInvisible();
        }
    }
    #endregion
}
