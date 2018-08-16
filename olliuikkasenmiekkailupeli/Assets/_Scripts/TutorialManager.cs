﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public float defaultTimer, timer = 5f;
    public float defaultInputTimer, inputTimerP1 = 0.5f, inputTimerP2 = 0.5f;
    public float defaultTextTimer, textTimer;

    [Header("---Affects movement---")]
    public bool moveLock;
    public bool guardLock;
    public bool deathLock;
    public bool heightLock;
    public bool lungeLock;

    [Header("---Checks for player input---")]
    public bool P1_OK;
    public bool P2_OK;
    public bool P1Clear;
    public bool P2Clear;
    public bool canInteractText;

    [Header("---Tutorial phases---")]
    public bool tutorialNotStarted;
    public bool tutorialClear;
    
    public bool phase1, phase2, phase3, phase4, phase5, phase6, phase7, phase8;
    
    float hangingP1, hangingP2, insideP1, insideP2, heightP1, heightP2;
    
    Animator animP1, animP2;
    InputManager im;
    GameObject P1, P2;

    public static TutorialManager TM;

    void Start ()
    {
        im = FindObjectOfType<InputManager>();
        TM = this;
        tutorialNotStarted = true;

        textTimer = 1f;

        defaultTimer = timer;
        defaultTextTimer = textTimer;
        defaultInputTimer = inputTimerP1;
    }

	void Update ()
    {
        if (MainMenuController.MMC.isTutorial && GameHandler.instance.BattleStarted)
        {
            tutorialNotStarted = false;
        }

        if (!tutorialNotStarted && MainMenuController.MMC.isTutorial)
        {
            TutorialPhases();
        }

        TextInteraction();
        TutorialExit();
        TutorialClear();
    }

    void TutorialPhases ()
    {
        if (!tutorialNotStarted && MainMenuController.MMC.isTutorial)
        {
            P1 = GameObject.FindGameObjectWithTag("Player 1");
            P2 = GameObject.FindGameObjectWithTag("Player 2");

            if (moveLock)
            {
                P1.GetComponent<AlternativeMovement5>().enabled = false;
                P2.GetComponent<AlternativeMovement5>().enabled = false;
            }

            if (!moveLock)
            {
                P1.GetComponent<AlternativeMovement5>().enabled = true;
                P2.GetComponent<AlternativeMovement5>().enabled = true;
            }

            animP1 = P1.GetComponentInChildren<Animator>();
            animP2 = P2.GetComponentInChildren<Animator>();

            hangingP1 = animP1.GetFloat("Hanging");
            insideP1 = animP1.GetFloat("Inside");
            heightP1 = animP1.GetFloat("Height");

            hangingP2 = animP2.GetFloat("Hanging");
            insideP2 = animP2.GetFloat("Inside");
            heightP2 = animP2.GetFloat("Height");

            phase1 = true;

            if (heightLock)
            {
                heightP1 = 0;
                heightP2 = 0;
            }

            #region PhaseFalseStuff

            if (phase1)
            {
                moveLock = true;
                guardLock = true;
                deathLock = true;
                heightLock = true;
                lungeLock = true;
            }

            if (phase2)
            {
                phase1 = false;
            }

            if (phase3)
            {
                phase1 = false;
                phase2 = false;
                guardLock = false;
            }

            if (phase4)
            {
                phase1 = false;
                phase3 = false;
                guardLock = false;
            }

            if (phase5)
            {
                phase1 = false;
                phase4 = false;
                guardLock = false;
                heightLock = false;
            }

            if (phase6)
            {
                phase1 = false;
                phase5 = false;
                guardLock = false;
                heightLock = false;
            }

            if (phase7)
            {
                phase1 = false;
                phase6 = false;
                guardLock = false;
                heightLock = false;
                moveLock = false;
            }

            if (phase8)
            {
                phase1 = false;
                phase7 = false;
                guardLock = false;
                heightLock = false;
                moveLock = false;
                lungeLock = false;
            }
            #endregion
        }

        #region Tutorial
        if (phase1)
        {
            TutorialTextController.TTC.texts[0].SetActive(true);

            textTimer -= Time.deltaTime;

            if (textTimer < 0 && Input.GetKeyUp(KeyCode.Return) || textTimer < 0 && InputManager.IM.GetA(1) || textTimer < 0 && InputManager.IM.GetA(2))
            {
                TutorialTextController.TTC.texts[0].SetActive(false);
                TutorialTextController.TTC.texts[1].SetActive(true); //"Let’s start with doing a vertical attack (Vertical attack button) and a horizontal attack (Horizontal attack button)."
            }

            if (TutorialTextController.TTC.texts[1])
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    P1_OK = true;
                }

                if (P1_OK && Input.GetKeyDown(KeyCode.F))
                {
                    P1_OK = false;
                    P1Clear = true;
                }

                if (InputManager.IM.isKeyboardAndMouseP1 && Input.GetMouseButtonDown(1))
                {
                    P1_OK = true;
                }

                if (P1_OK && InputManager.IM.isKeyboardAndMouseP1 && Input.GetKeyDown(KeyCode.F))
                {
                    P1_OK = false;
                    P1Clear = true;
                }

                if (Input.GetKeyDown(KeyCode.RightShift))
                {
                    P2_OK = true;
                }

                if (P2_OK && Input.GetKeyDown(KeyCode.RightControl))
                {
                    P2_OK = false;
                    P2Clear = true;
                }

                if (InputManager.IM.isKeyboardAndMouseP2 && Input.GetMouseButtonDown(1))
                {
                    P2_OK = true;
                }

                if (P2_OK && InputManager.IM.isKeyboardAndMouseP2 && Input.GetKeyDown(KeyCode.F))
                {
                    P2_OK = false;
                    P2Clear = true;
                }

                if (P1Clear && P2Clear)
                {
                    timer -= Time.deltaTime;

                    if (timer <= 4.75f)
                    {
                        textTimer = defaultTextTimer;
                        phase2 = true;
                    }
                }
            }
        }

        if (phase2)
        {
            P1Clear = false;
            P2Clear = false;

            TutorialTextController.TTC.texts[1].SetActive(false); 
            TutorialTextController.TTC.texts[2].SetActive(true); //"You never seize to amaze sirs! You can also spin your sword around and do an attack from the opposite direction(Hold down attack button). This technique is called a moulinette."



            //"Note that doing a moulinette is only way to generate enough force for any proper damage, lighter attack will only wound your opponent."

            if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
            {
                inputTimerP1 -= Time.deltaTime;
            }

            else if (InputManager.IM.isKeyboardAndMouseP1 && Input.GetMouseButton(1) || InputManager.IM.isKeyboardAndMouseP1 && Input.GetKey(KeyCode.F))
            {
                inputTimerP1 -= Time.deltaTime;
            }

            else
            {
                inputTimerP1 = defaultInputTimer;
            }

            //MUISTA LAITTAA KONTROLLERIEN INPUTIT!!!

            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.RightControl))
            {
                inputTimerP2 -= Time.deltaTime;
            }

            else if (InputManager.IM.isKeyboardAndMouseP2 && Input.GetMouseButton(1) || InputManager.IM.isKeyboardAndMouseP2 && Input.GetKey(KeyCode.F))
            {
                inputTimerP2 -= Time.deltaTime;
            }

            else
            {
                inputTimerP2 = defaultInputTimer;
            }

            if (inputTimerP1 <= 0.2f)
            {
                P1_OK = true;
            }

            if (inputTimerP2 <= 0.2f)
            {
                P2_OK = true;
            }

            if (P1_OK && P2_OK)
            {
                timer -= Time.deltaTime;

                if (timer <= 4.5f)
                {
                    inputTimerP1 = defaultInputTimer;
                    inputTimerP2 = defaultInputTimer;
                    P1_OK = false;
                    P2_OK = false;
                    phase3 = true;
                }
            }
        }

        if (phase3)
        {
            //"Well done sirs! When duelling with sabres you can be attacked from two different sides: outside (the side your back is pointing) and inside (the side your chest is pointing)"

            //LAITA KONTROLLERIN INPUTIT MYÖS!!!

            if (Input.GetKeyUp(KeyCode.X) || InputManager.IM.isKeyboardAndMouseP1 && Input.GetMouseButtonDown(1))
            {
                P1_OK = true;
            }

            if (Input.GetKeyUp(KeyCode.P) || InputManager.IM.isKeyboardAndMouseP2 && Input.GetMouseButtonDown(1))
            {
                P2_OK = true;
            }

            if (P1_OK && P2_OK)
            {
                //"To block the attacks coming you have to make sure that you are guarding the correct side. The side you are guarding in indicated by the colour of your blade"

                //"You can also change between hanging(sword pointing down) and regular(sword pointing up) guard"

                //"Now please rotate through each of your guard (The guard changing button or the other guard changing button)"

                if (Input.GetKeyUp(KeyCode.C) || InputManager.IM.isKeyboardAndMouseP1 && Input.GetMouseButtonDown(2))
                {
                    P1Clear = true;
                }

                if (Input.GetKeyUp(KeyCode.I) || InputManager.IM.isKeyboardAndMouseP2 && Input.GetMouseButtonDown(2))
                {
                    P2Clear = true;
                }
            }

            if (P2Clear && P2Clear)
            {
                timer = defaultTimer;
                phase4 = true;
            }
        }

        if (phase4)
        {
            P1_OK = false;
            P2_OK = false;
            P1Clear = false;
            P2Clear = false;

            timer -= Time.deltaTime;

            if (timer > 4.5)
            {
                animP1.SetBool("forward", true);
                animP2.SetBool("forward", true);
            }

            if (timer < 2)
            {
                animP1.SetBool("forward", false);
                animP2.SetBool("forward", false);
                
                //"Very good sirs! On top of affecting the direction you're blocking guards also determine which direction your attacks will come from. Please try hitting eachother untill one of you has landed three hits");

                if (Input.GetKeyUp(KeyCode.Return) || InputManager.IM.GetA(1) || InputManager.IM.GetA(2))
                {
                    timer = defaultTimer;
                    phase5 = true;
                }
            }
        }

        if (phase5)
        {
            timer -= Time.deltaTime;

            if (!P1_OK && timer > 4.5)                                    //Players step out of striking distance
            {
                animP1.SetBool("back", true);
                animP2.SetBool("back", true);
            }

            if (!P1_OK && timer < 2)
            {
                animP1.SetBool("back", false);
                animP2.SetBool("back", false);
            }
            if (heightP1 < 0 || heightP1 > 0)
            {
                P1_OK = true;
            }

            if (heightP2 < 0 || heightP2 > 0)
            {
                P2_OK = true;
            }

            //"You can also change your sword’s height (right controller stick, mouse or  t and g for player1 and O and L for player2) to attack and protect on different bodyparts."

            if (P1_OK && P2_OK)
            {
                timer = defaultTimer;
                phase6 = true;
            }
        }

        if (phase6)
        {
            P1_OK = false;
            P2_OK = false;
            P1Clear = false;
            P2Clear = false;

            //"Absolutely splendid work sirs! Even if your guard is on correct side, it can't parry the attack if the blade isn't on the way of the attack."

            //"Now before we move on to footwork, I want to tell you a bit about what happens when your swords collide."

            //"Your sword's blade can be divided to roughly two parts in lengthwise: the strong of the blade(The half closer to your hand, used for parrying) and the weak of the blade(the half further from the hand, used for attacking)"

            //"The laws of leverage dictate that when weak of the blade hits strong the former will bounce back more"
            
            timer -= Time.deltaTime;

            if (timer < 4.5)
            {
                animP1.SetBool("forward", true);
                animP2.SetBool("forward", true);
            }

            if (timer < 2)
            {
                animP1.SetBool("forward", false);
                animP2.SetBool("forward", false);
            }

            //"How sword collisions end up are also affected by multitude of other factors such as momentum and the direction of the swings.But fun comes from figuring these things yourself.So once more, first to three!"

            if (timer < 0 && Input.GetKeyUp(KeyCode.Return) || InputManager.IM.GetA(1) || InputManager.IM.GetA(2))
            {
                phase7 = true;
            }
        }

        if (phase7)
        {
            //"Being able to swing your sword is all well and good, but you’ll soon be hit by the swings of your opponent, lest you learn how to move out of the way of those swings."

            //"Footwork basicly boils down to stepping forward(tilt left stick towards your opponent, press D or left arrow) and stepping backward(tilt left stick away from your opponent, press A or right arrow).Give me one step in either direction."

            if (animP1.GetBool("forward") || animP1.GetBool("back"))
            {
                P1_OK = true;
            }

            if (animP2.GetBool("forward") || animP2.GetBool("back"))
            {
                P2_OK = true;
            }

            if (P1_OK && P2_OK)
            {
                P1_OK = false;
                P2_OK = false;
                phase8 = true;
            }
        }

        if (phase8)
        {
            //"Now I shall teach you some advanced footwork. First show me a leap backwards(Double tap leftstick away from opponent)!"

            if (animP1.GetBool("Jumped"))
            {
                P1_OK = true;
            }

            if (animP2.GetBool("Jumped"))
            {
                P2_OK = true;
            }

            //"Clearly you’re no peasants!  And now for for the last and propably the most important move show me a Lunge(Double tap leftstick towards opponent)"

            if (P1_OK && animP1.GetBool("Lunged"))
            {
                P1Clear = true;
            }

            if (P2_OK && animP2.GetBool("Lunged"))
            {
                P2Clear = true;
            }

            if (P1Clear && P2Clear)
            {
                //"You inbred bastards! You can leave your lunged position by moving forward(move forward) or backward(move backward)"

                //"In most cases you want to start preparing your swing just before lunging. Also don’t let yourself get too close to opponent, lest your fight will detoriate to a common brawl!"

                //"The ideal distance from your opponent is from where you can hit your opponent by lunging but can’t be reached when standing in normally. This is called being at measure."

                //"I hope you swift deaths."

                tutorialClear = true;        //Move to duel mode
            }
        }

        #endregion
    }

    void TutorialExit ()     //IF PLAYER EXITS TUTORIAL, SET BOOLEANS TO FALSE!!!
    {
        if (SceneManager.GetActiveScene().name != "TutorialScene")
        {
            tutorialNotStarted = true;
            phase1 = false;
            moveLock = false;
            guardLock = false;
            deathLock = false;
            heightLock = false;
            lungeLock = false;
            tutorialClear = false;
        }
    }

    void TutorialClear()
    {
        if (tutorialClear)
        {
            SceneManager.LoadScene(3);
        }
    }

    void TextInteraction()
    {
        if (canInteractText)
        {
            
        }

        if (!canInteractText)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = defaultTimer;
                canInteractText = true;
            }
        }
    }
}