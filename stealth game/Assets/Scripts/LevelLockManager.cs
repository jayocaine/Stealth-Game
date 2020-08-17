/*
 * A persistent component that manages locking and unlocking of levels.
 */

using System;
using UnityEngine;

/// <summary>
/// A component that loads, unlocks, and manages levels
/// persitently across plays
/// </summary>
public class LevelLockManager : MonoBehaviour {

    #region --PRIVATE STATIC STATE--
    /// <summary>
    /// A reference to the single instance
    /// </summary>
    private static LevelLockManager instance;

    /// <summary>
    /// Listen to this event for when the lock state of levels is loaded.  Can be added with LevelLockManager.AddListener(...)
    /// </summary>
    /// 
    private static Action onLocksLoaded;

    /// <summary>
    /// Whether the current lock state of levels has been loaded
    /// </summary>
    private static bool loaded = false;

    #endregion

    #region --INSPECTOR FIELDS--

    /// <summary>
    /// The default highest level to unlock
    /// </summary>
    [Tooltip("The default highest level to unlock")]
    public int defaultStartLevel;

    /// <summary>
    /// The scene number in build settings of the first playable level. All other levels should come after that
    /// </summary>
    [Tooltip("The scene number in build settings of the first playable level. All other levels should come after that")]
    public int firstLevelSceneNumber = 2;
    #endregion

    #region --PUBLIC STATIC PROPERTIES--
    /// <summary>
    /// Cached internal value for CurrentUnlockedLevel
    /// </summary>
    private static int currentUnlockedLevel = 0;
    /// <summary>
    /// The current highest level unlocked by the level lock manager
    /// (i.e. the highest level visited so far)
    /// </summary>
    public static int CurrentUnlockedLevel {
        get {
            return currentUnlockedLevel;
        }
    }
    #endregion

    #region --PUBLIC STATIC METHODS--

    /// <summary>
    /// Go to the next level and unlock it
    /// </summary>
    public static void Next() {
        GoToLevel(CurrentUnlockedLevel + 1);
    }

    /// <summary>
    /// Listen for lock state events (when level locks lock or unlock)
    /// </summary>
    /// <param name="listener"></param>
    public static void AddListener(Action listener) {

        //Trigger load event if we're already loaded
        if (loaded) {
            listener();
        }

        //Subscribe so we can unsubscribe or call it again if needed
        onLocksLoaded += listener;
    }

    /// <summary>
    /// Unsubscribe to lock state events (when level locks lock or unlock)
    /// </summary>
    /// <param name="listener"></param>
    public static void RemoveListener(Action listener) {
        onLocksLoaded -= listener;
    }

    /// <summary>
    /// Go to the main menu (scene 0)
    /// </summary>
    public static void MainMenu() {

        //Load scene 0
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Go to a level and unlock it
    /// </summary>
    /// <param name="levelId"></param>
    public static void GoToLevel(int levelId) {

        //Unlock next level
        currentUnlockedLevel = levelId;

        //Save current unlocked level
        PlayerPrefs.SetInt("CurrentUnlockedLevel", currentUnlockedLevel);

        //Load level
        UnityEngine.SceneManagement.SceneManager.LoadScene(instance.firstLevelSceneNumber + levelId);
    }

    #endregion

    #region --UNITY EVENT HANDLERS--
    void Start() {
        //Destroy if already loaded
        if (loaded) {
            Destroy(gameObject);
            return;
        }


        //Don't load another copy of this object
        loaded = true;

        //Keep around forever (like a singleton)
        instance = this;
        DontDestroyOnLoad(gameObject);

        //Set current level from player prefs        
        currentUnlockedLevel = PlayerPrefs.GetInt("CurrentUnlockedLevel", defaultStartLevel);

        //Trigger event
        onLocksLoaded.Invoke();
    }

    #endregion

}
