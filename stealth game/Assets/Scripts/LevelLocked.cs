/*
 * A persistent component that manages locking and unlocking of levels.
 */

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Trigger events when/if the level specified is locked or unlocked
/// </summary>
public class LevelLocked : MonoBehaviour
{

    #region -- INSEPCTOR FIELDS --

    /// <summary>
    /// Called when the specific level number is locked
    /// </summary>
    [Tooltip("Called when the specific level number in locked")]
    public UnityEvent onLocked;

    /// <summary>
    /// Called when the specific level number in unlocked
    /// </summary>
    [Tooltip("Called when the specific level number in unlocked")]
    public UnityEvent onUnlocked;

    /// <summary>
    /// The level number
    /// </summary>
    [Tooltip("The level number")]
    public int levelNumber = 0;
    #endregion

    #region -- UNITY EVENT HANDLERS --
    public void Awake() {

        //Listen for lock loaded event
        LevelLockManager.AddListener(OnLocksLoaded);
    }
    #endregion

    #region -- LOCK EVENT HANDLERS --

    /// <summary>
    /// Handles lock loaded event from LevelLockManager
    /// </summary>
    void OnLocksLoaded() {

        //Pick lock or unlock event depending on if we've passed the level specified or not and invoke it
        UnityEvent e = levelNumber > LevelLockManager.CurrentUnlockedLevel ? onLocked : onUnlocked;
        e.Invoke();
    }
    #endregion

    #region -- PUBLIC METHODS --
    /// <summary>
    /// Load the level specified using the Level Lock Manager
    /// </summary>
    public void Load() {
        LevelLockManager.GoToLevel(levelNumber);
    }
    #endregion
}
