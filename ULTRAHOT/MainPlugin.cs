using System;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ULTRAHOT {
	[BepInPlugin(GUID,Name,Version)]
	public class MainPlugin : BaseUnityPlugin {

		internal const string GUID = "io.github.TeamDoodz." + Name;
		internal const string Name = "ULTRAHOT";
		internal const string Version = "1.0.0";

		internal static ManualLogSource logger;

		float velocityMax = 16f;
		float timeMin = 0.05f;
		float timeMax = 1.25f;
		float timeMoveSpeed = 5f;

		void Awake() {
			logger = Logger;
			GetConfig();
		}
		void Update() {
			if(Input.GetKeyDown(KeyCode.H)) logger.LogInfo($"Current scene: {SceneManager.GetActiveScene().name} Is level: {IsLevel()}");
			if(IsLevel()) {
				Rigidbody rigidbody = PlayerTracker.Instance.GetRigidbody();
				AssistController.Instance.gameSpeed = Mathf.MoveTowards(AssistController.Instance.gameSpeed, Mathf.Clamp(rigidbody.velocity.magnitude / velocityMax, timeMin, timeMax), Time.deltaTime * timeMoveSpeed);
				if(Input.GetKeyDown(KeyCode.Z)) logger.LogInfo($"Player velocity: {rigidbody.velocity.magnitude}");
			}
		}

		bool IsLevel() {
			string scene = SceneManager.GetActiveScene().name;
			return scene == "Endless" || scene.StartsWith("Level");
		}

		void GetConfig() {
			logger.LogMessage("If you don't like a particular part of the mod, you can edit the configs.");
			velocityMax = Config.Bind("General", nameof(velocityMax), velocityMax, "The speed the player must move for time to be at 1x speed.").Value;
			timeMin = Config.Bind("General", nameof(timeMin), timeMin, "The lower bound limit of how much this mod can change the time by. Don't set this to 0!").Value;
			timeMax = Config.Bind("General", nameof(timeMax), timeMax, "The upper bound limit of how much this mod can change the time by.").Value;
			timeMoveSpeed = Config.Bind("General", nameof(timeMoveSpeed), timeMoveSpeed, "Speed that time transitions from one value to another. Set this to a high value for instant time changes.").Value;
		}
	}
}
