using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class PokiManager : MonoBehaviour
{
    //singleton to accesss from other clasess
    public static PokiManager instance;
    // bool to prevent repeat gameplay events
    public bool gameplayOnGoing;
    // bool to prevent restart the sdk
    public bool pokiSDKIsReady;

    [Header("Buttons")] public CanvasGroup buttonPanel;
    [Header("Audio example")] public AudioMixerGroup audioMasterMixer;
    [Header("Info")] 
    public int coins;

    public TextMeshProUGUI coinsText;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!pokiSDKIsReady)
        {
            // delegate to start the events
            PokiUnitySDK.Instance.sdkInitializedCallback += SetPokiEvents;
            LogManager.instance.AddLogMessage("Starting...");
            //prevent touch the button mean the sdk is not ready
            buttonPanel.interactable = false;
            //start the sdk
            PokiUnitySDK.Instance.init();
            //to maintain the manager through the scenes 
            DontDestroyOnLoad(gameObject);
            pokiSDKIsReady = true;
        }
       
        coinsText.text = "x" + coins;
    }

    private void SetMuteAudio(bool mute)
    {
        if (mute)
        {
            audioMasterMixer.audioMixer.SetFloat("MasterVolume", -9000f);
        }
        else
        {
            audioMasterMixer.audioMixer.SetFloat("MasterVolume", 0);
        }
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SetPokiEvents()
    {
        LogManager.instance.AddLogMessage("Poki sdk is ready");
        buttonPanel.interactable = true;
    }


    public void GameplayStart()
    {
        if (!gameplayOnGoing)
        {
            PokiUnitySDK.Instance.gameplayStart();
            LogManager.instance.AddLogMessage("Gameplay start!!! :D");
            gameplayOnGoing = true;
        }
        
    }
    public void GameplayStop()
    {
        if (gameplayOnGoing)
        {
            PokiUnitySDK.Instance.gameplayStop();
            LogManager.instance.AddLogMessage("Gameplay stop!!! :c");
            gameplayOnGoing = false;
        }
        
    }

    public void CommercialBreak()
    {
        LogManager.instance.AddLogMessage("Commercial break intent");
        PokiUnitySDK.Instance.commercialBreakCallBack = CompleteCommercialBreak;
        PokiUnitySDK.Instance.commercialBreak();
        SetMuteAudio(true);
        SetPause(true);
    }

    private void CompleteCommercialBreak()
    {
        LogManager.instance.AddLogMessage("back from commercial break");
        SetMuteAudio(false);
        SetPause(false);
    }

    public void RewardedAd()
    {
        LogManager.instance.AddLogMessage("Reward break intent");
        PokiUnitySDK.Instance.rewardedBreakCallBack = CompleteRewardedAd;
        PokiUnitySDK.Instance.rewardedBreak();
        SetMuteAudio(true);
    }

    private void CompleteRewardedAd(bool reward)
    {
        SetMuteAudio(false);
        SetPause(false);
        if (reward)
        {
            LogManager.instance.AddLogMessage("back from reward break, with reward yay!");
            coins += 100;
            coinsText.text = "x" + coins;
        }
        else
        {
            LogManager.instance.AddLogMessage("back from reward break ;c");
        }
        
    }
   
}
