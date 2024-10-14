// =======================================
// Poki SDK Wrapper for Unity WebGL
// Copyright Poki 2024
// =======================================

using UnityEngine;
using System;
using System.Text;
using System.Runtime.InteropServices;

public class PokiException : System.Exception {
	public PokiException(string message) : base(message){}
}

public class PokiUnitySDK : MonoBehaviour {
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_initPokiBridge(string instanceName);
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_gameLoadingStart();
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_gameLoadingFinished();
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_customEvent(string noun, string verb, string data);
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_shareableURL(string data);
	[DllImport("__Internal")]
	private static extern string JS_PokiSDK_getURLParam(string name);
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_gameplayStart();
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_gameplayStop();
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_commercialBreak();
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_rewardedBreak();
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_displayAd(string indentifier, string size, string top, string left);
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_destroyAd(string indentifier);
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_preInit();
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_redirect(string destination);
	[DllImport("__Internal")]
	private static extern string JS_PokiSDK_getLanguage();
	[DllImport("__Internal")]
	private static extern bool JS_PokiSDK_isAdBlocked();
	[DllImport("__Internal")]
	private static extern void JS_PokiSDK_logError(string err);

	private static PokiUnitySDK _instance;
	public static PokiUnitySDK Instance {
		get {
			if (_instance == null) {
				_instance = (PokiUnitySDK) FindObjectOfType(typeof(PokiUnitySDK));

				if (FindObjectsOfType(typeof(PokiUnitySDK)).Length > 1) {
					Debug.LogError("[Singleton] Something went really wrong " +
						" - there should never be more than 1 singleton!" +
						" Reopening the scene might fix it.");
					return _instance;
				}

				if (_instance == null) {
					GameObject singleton = new GameObject();
					_instance = singleton.AddComponent<PokiUnitySDK>();
					singleton.name = "(singleton) "+ typeof(PokiUnitySDK).ToString();

					DontDestroyOnLoad(singleton);

					Debug.Log("[Singleton] An instance of " + typeof(PokiUnitySDK) +
						" is needed in the scene, so '" + singleton +
						"' was created with DontDestroyOnLoad.");
				} else {
					Debug.Log("[Singleton] Using instance already created: " +
						_instance.gameObject.name);
				}
			}

			return _instance;
		}
	}

	public PokiUnitySDK () {}

	private bool _initializing = false;
	private bool _ready = false;

	public bool adblocked = false;
	public bool isShowingAd { get; set;}

	public delegate void SDKInitializedDelegate();
 	public SDKInitializedDelegate sdkInitializedCallback = () => {};
	public delegate void CommercialBreakDelegate();
 	public CommercialBreakDelegate commercialBreakCallBack = () => {};
	public delegate void RewardedBreakDelegate(bool withReward);
 	public RewardedBreakDelegate rewardedBreakCallBack = (withReward) => {};
	public delegate void shareableURLResolvedDelegate(string url);
 	public shareableURLResolvedDelegate shareableURLResolvedCallback;
	public delegate void shareableURLRejectedDelegate();
 	public shareableURLRejectedDelegate shareableURLRejectedCallback;

	public void init(){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: Initializing");
		if (_initializing || _ready) {
			throw new PokiException ("PokiUnitySDK is already initializing");
		}
		_initializing = true;

		// after 500ms, call ready
		Invoke("ready", 0.5f);
		#else
		if (_initializing || _ready) {
			throw new PokiException ("PokiUnitySDK is already initializing");
		}
		_initializing = true;
		JS_PokiSDK_initPokiBridge(PokiUnitySDK.Instance.name);
		#endif
	}

	public bool isAdBlocked(){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: isAdBlocked");
		return false;
		#else
		return JS_PokiSDK_isAdBlocked();
		#endif
	}

	public string getLanguage(){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: getLanguage");
		return "";
		#else
		return JS_PokiSDK_getLanguage();
		#endif
	}

	public void gameLoadingStart(){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: gameLoadingStart");
		#else
		JS_PokiSDK_gameLoadingStart();
		#endif
	}

	public void gameLoadingFinished (){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: gameLoadingFinished");
		#else
		JS_PokiSDK_gameLoadingFinished();
		#endif
	}

	public void customEvent (string eventNoun, string eventVerb, ScriptableObject eventData){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: customEvent");
		#else
		JS_PokiSDK_customEvent(eventNoun, eventVerb, JsonUtility.ToJson(eventData ,true));
		#endif
	}

	public void shareableURL (ScriptableObject urlParams){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: shareableURL");
		#else
		JS_PokiSDK_shareableURL(JsonUtility.ToJson(urlParams, true));
		#endif
	}

	public string getURLParam (string name){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: getURLParam "+name);
		return "";
		#else
		return JS_PokiSDK_getURLParam(name);
		#endif
	}

	public void gameplayStart() {
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: gameplayStart");
		#else
		JS_PokiSDK_gameplayStart();
		#endif
	}

	public void gameplayStop() {
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: gameplayStop");
		#else
		JS_PokiSDK_gameplayStop();
		#endif
	}

	public void commercialBreak() {
		isShowingAd = true;
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: commercialBreak");
		commercialBreakCompleted();
		#else
		if(!_ready || adblocked){
			commercialBreakCompleted();
			return;
		}
		JS_PokiSDK_commercialBreak();
		#endif
	}

	public void rewardedBreak(){
		isShowingAd = true;
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: rewardedBreak");
		rewardedBreakCompleted("true");
		#else
		if(adblocked){
			rewardedBreakCompleted("false");
			return;
		}
		JS_PokiSDK_rewardedBreak();
		#endif
	}

	public void displayAd(string identifier, string size, string top, string left){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: displayAd with identifier:"+identifier+", size:"+size+" top:"+top+" left:"+left);
		#else
		string[] availableSizes = {"970x250", "300x250", "728x90", "160x600", "320x50"};
		bool allowSize = Array.Exists(availableSizes, element => element == size);

		 if(identifier.Length<3){
			throw new PokiException ("PokiUnitySDK identifier must be at least 3 characters");
		 }
		 if(!allowSize){
			throw new PokiException ("PokiUnitySDK displayAd size:"+size+" is currently not supported");
		 }
		if((top.Contains("%") || top.Contains("px")) == false || (left.Contains("%") || left.Contains("px")) == false){
			throw new PokiException ("PokiUnitySDK displayAd unsupported top or left syntax, please use format '10px' or '10%'");
		}

		JS_PokiSDK_displayAd(identifier, size, top, left);
		#endif
	}

	public void destroyAd(string identifier){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: destroyAd with identifier:"+identifier);
		#else
		JS_PokiSDK_destroyAd(identifier);
		#endif
	}

	public void logError(string error){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: logError");
		#else
		JS_PokiSDK_logError(error);
		#endif
	}

	public void ready(){
		_initializing = false;
		_ready = true;
		sdkInitializedCallback();
	}

	public void adblock(){
		adblocked = true;
		_initializing = false;
		_ready = true;
		sdkInitializedCallback();
	}

	public void commercialBreakCompleted(){
		isShowingAd = false;
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: commercialBreak completed");
		#else
		commercialBreakCallBack();
		#endif
	}

	public void rewardedBreakCompleted(string withReward){
		isShowingAd = false;
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: rewardedBreak completed, received reward:"+withReward);
		#else
		rewardedBreakCallBack((withReward == "true"));
		#endif
	}

	public void shareableURLResolved(string url){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: shareableURL resolved, received url:"+url);
		#else
		shareableURLResolvedCallback(url);
		#endif
	}

	public void shareableURLRejected(){
		#if UNITY_EDITOR
		Debug.Log("PokiUnitySDK: shareableURL rejected");
		#else
		shareableURLRejectedCallback();
		#endif
	}
}
