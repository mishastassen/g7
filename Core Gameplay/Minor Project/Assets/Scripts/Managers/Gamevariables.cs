using UnityEngine;
using System.Collections;

public class Gamevariables : MonoBehaviour {

	public static float timer;
	public static string currentLevel;
	public static string returnLevel;
	public static float alarmPercent;
	public static int minigameDifficulty;

	public static bool magicPackage;

	// Variables for AI: Determines strength guards
	public static int playersDeathCount;
	public static int guardsDeathCount;

	// reset also the death counts for AI
	// actions in previous levels have no influence on the guards strength
	public static void SetCurrentLevel(string levelName) {
		currentLevel = levelName;
		playersDeathCount = 0;
		guardsDeathCount = 0;
	}
}
