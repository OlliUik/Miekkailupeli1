﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // UPDATED 30.07.2018 

    [SerializeField]
    GameObject mainMenu, settingsMenu, creditsMenu;

    Animator anim;
    Animator characterAnim;

    [SerializeField] Menu activeMenu;
    [SerializeField] GameObject activeNode;

    [SerializeField]
    float timer = 0.5f, defaultTimer;

    float hor, ver, dampTime = 0.5f;
    int index;
    public bool canInteract;

    enum Menu { MainMenu, Settings, Credits, };
    [SerializeField] GameObject mainmenuCharacter;
    [SerializeField] GameObject[] mainmenuNodes, settingsNodes;
    [SerializeField] GameObject[] mainmenuHighlights, settingsHighlights;

    [SerializeField] Slider[] volumeSliders;

    void Start()
    {
        foreach (Slider slider in volumeSliders)
        {
            slider.value = 0.1f;
        }

        mainMenu.gameObject.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
        creditsMenu.gameObject.SetActive(false);

        DisableHighlights(Menu.MainMenu);
        DisableHighlights(Menu.Settings);
        mainmenuHighlights[index].SetActive(true);

        anim = Camera.main.GetComponent<Animator>();
        characterAnim = mainmenuCharacter.GetComponent<Animator>();

        anim.SetBool("MainMenu", true);
        anim.SetBool("SettingsMenu", false);
        anim.SetBool("CreditsMenu", false);

        activeMenu = Menu.MainMenu;
        activeNode = mainmenuNodes[0];
        defaultTimer = timer;

        volumeSliders[0].value = AudioManager.instance.musicVolumePercent;
        volumeSliders[1].value = AudioManager.instance.sfxVolumePercent;
    }

    void Update()
    {
        GetInput();
        ChangeNode();
        ButtonLogic();

        if(anim.GetBool("CreditsMenu") == true && canInteract)
        {
            if (Input.anyKeyDown)
                Back(2);
        }

        // DELETE AFTER TESTING
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AchievementManager.instance.AddProgressToAchievement("Pacifist run", 50);
        }

    }

    void DisableHighlights(Menu menu)
    {
        if(menu == Menu.MainMenu)
        {
            for (int i = 0; i < mainmenuHighlights.Length; i++)
            {
                mainmenuHighlights[i].SetActive(false);
            }
        }
        else if(menu == Menu.Settings)
        {
            for (int i = 0; i < settingsHighlights.Length; i++)
            {
                settingsHighlights[i].SetActive(false);
            }
        }
    }

    private void ChangeNode()
    {
        if (!canInteract)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = defaultTimer;
                canInteract = true;
            }
        }

        if (activeMenu == Menu.MainMenu)
        {
            if (ver >= .5f && canInteract)
            {
                ToggleDown();
            }
            else if (ver <= -0.5f && canInteract)
            {
                ToggleUp();
            }

            if(activeNode == mainmenuNodes[0])
                characterAnim.SetFloat("Blend", 1, dampTime, Time.deltaTime);
            
            else if(activeNode == mainmenuNodes[1])
                characterAnim.SetFloat("Blend", 0.35f, dampTime, Time.deltaTime);
            
            else if (activeNode == mainmenuNodes[2])
                characterAnim.SetFloat("Blend", -0.35f, dampTime, Time.deltaTime);
            
            else if (activeNode == mainmenuNodes[3])
                characterAnim.SetFloat("Blend", -1, dampTime, Time.deltaTime);

        }
        else if (activeMenu == Menu.Settings)
        {
            if (ver >= .5f && canInteract)
            {
                ToggleDown();
            }
            else if(ver <= -.5f && canInteract)
            {
                ToggleUp();
            }
        }
    }

    void ToggleUp()
    {
        index++;

        if(activeMenu == Menu.MainMenu)
        { 
            if (index > mainmenuNodes.Length - 1)
            {
                index = 0;
            }
            canInteract = false;

            DisableHighlights(Menu.MainMenu);
            mainmenuHighlights[index].SetActive(true);
        }

        if(activeMenu == Menu.Settings)
        {
            if (index > settingsNodes.Length - 1)
            {
                index = 0;
            }
            canInteract = false;

            DisableHighlights(Menu.Settings);
            settingsHighlights[index].SetActive(true);
        }

        AudioManager.instance.PlaySoundeffect("uuf");

    }

    void ToggleDown()
    {
        index--;

        if(activeMenu == Menu.MainMenu)
        {
            if(index < 0)
            {
                index = mainmenuNodes.Length - 1;
            }
            canInteract = false;

            DisableHighlights(Menu.MainMenu);
            mainmenuHighlights[index].SetActive(true);
        }

        if(activeMenu == Menu.Settings)
        {
            if (index < 0)
            {
                index = settingsNodes.Length - 1;
            }
            canInteract = false;

            DisableHighlights(Menu.Settings);
            settingsHighlights[index].SetActive(true);
        }

        AudioManager.instance.PlaySoundeffect("uuf");

    }

    private void ButtonLogic()
    {
        if (activeMenu == Menu.MainMenu) // mainMenu
        {
            activeNode = mainmenuNodes[index];

            if (!InputManager.IM.isOnlyKeyboard) // controller
            {
                if ((InputManager.IM.P1_A || InputManager.IM.P2_A || Input.GetKeyDown(KeyCode.Return)) && canInteract)
                {
                    if (activeNode == mainmenuNodes[0])
                    {
                        // START
                        Debug.Log("Start");
                        AudioManager.instance.PlaySoundeffect("uuf");
                        LevelChanger.instance.FadeToNextLevel();
                        canInteract = false;
                    }
                    else if (activeNode == mainmenuNodes[1])
                    {
                        // OPTIONS
                        Debug.Log("Options");
                        Options();
                        canInteract = false;
                    }
                    else if (activeNode == mainmenuNodes[2])
                    {
                        // CREDITS
                        Debug.Log("Credits");
                        Credits();
                        canInteract = false;
                    }
                    else if (activeNode == mainmenuNodes[3])
                    {
                        // EXIT
                        Application.Quit();
                        canInteract = false;
                    }
                }
            }
            else // keyboard
            {
                if (Input.GetKeyDown(KeyCode.Return) && canInteract)
                {
                    if (activeNode == mainmenuNodes[0])
                    {
                        // START
                        Debug.Log("START");
                        LevelChanger.instance.FadeToNextLevel();
                        canInteract = false;
                    }
                    else if (activeNode == mainmenuNodes[1])
                    {
                        // OPTIONS
                        Debug.Log("Options");
                        Options();
                        canInteract = false;
                    }
                    else if (activeNode == mainmenuNodes[2])
                    {
                        // CREDITS
                        Debug.Log("Credits");
                        Credits();
                        canInteract = false;
                    }
                    else if (activeNode == mainmenuNodes[3])
                    {
                        // EXIT
                        Application.Quit();
                    }
                }
            }
        }
        else if (activeMenu == Menu.Settings) // settings
        {
            activeNode = settingsNodes[index];

            if (activeNode == settingsNodes[0]) // volume
            {
                if(hor >= .5f && volumeSliders[0].value < 1 && canInteract)
                {
                    AudioManager.instance.AddVolume(0);
                    canInteract = false;
                    volumeSliders[index].value = AudioManager.instance.musicVolumePercent;
                }
                if(hor <= -.5 && volumeSliders[0].value > 0 && canInteract)
                {
                    AudioManager.instance.LessVolume(0);
                    canInteract = false;
                    volumeSliders[index].value = AudioManager.instance.musicVolumePercent;
                }
            }
            else if(activeNode == settingsNodes[1]) // sfx
            {
                if (hor >= .5f && volumeSliders[1].value < 1 && canInteract)
                {
                    AudioManager.instance.AddVolume(1);
                    canInteract = false;
                    volumeSliders[index].value = AudioManager.instance.sfxVolumePercent;
                }
                if (hor <= -.5f && volumeSliders[1].value > 0 && canInteract)
                {
                    AudioManager.instance.LessVolume(1);
                    canInteract = false;
                    volumeSliders[index].value = AudioManager.instance.sfxVolumePercent;
                }
            }
            else if (activeNode == settingsNodes[2])
            {
                if ((InputManager.IM.P1_A || InputManager.IM.P2_A || Input.GetKeyDown(KeyCode.Return)) && canInteract)
                {
                    Back(1);
                }
            }
        }
    }

    public void Options()
    {
        index = 0;
        timer = 1f;

        Invoke("DisableMain", 1);
        settingsMenu.gameObject.SetActive(true);

        activeMenu = Menu.Settings;

        anim.SetBool("SettingsMenu", true);
        anim.SetBool("MainMenu", false);

        DisableHighlights(Menu.Settings);
        settingsHighlights[index].SetActive(true);

        canInteract = false;
    }

    public void Back(int num)
    {
        index = num;
        timer = 1f;

        Invoke("DisableSettings", 1);
        Invoke("DisableCredits", 1);
        mainMenu.gameObject.SetActive(true);

        activeMenu = Menu.MainMenu;

        DisableHighlights(Menu.MainMenu);
        mainmenuHighlights[index].SetActive(true);
        activeNode = mainmenuNodes[index];

        anim.SetBool("MainMenu", true);
        anim.SetBool("SettingsMenu", false);
        anim.SetBool("CreditsMenu", false);
        canInteract = false;
    }

    public void Credits()
    {
        timer = 1f;

        Invoke("DisableMain", 1);
        creditsMenu.gameObject.SetActive(true);

        activeMenu = Menu.Credits;

        anim.SetBool("MainMenu", false);
        anim.SetBool("SettingsMenu", false);
        anim.SetBool("CreditsMenu", true);
        Debug.Log("credits");
        canInteract = false;
    }

    void DisableMain()
    {
        mainMenu.gameObject.SetActive(false);
    }

    void DisableSettings()
    {
        settingsMenu.gameObject.SetActive(false);
    }

    void DisableCredits()
    {
        creditsMenu.gameObject.SetActive(false);
    }

    void GetInput()
    {
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");
    }


    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    public void SetSfxVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }

}
