﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerGameObjects
{
    public GameObject PlayerObject;

    public GameObject CrystalObject;
}

public class GameManager : MonoBehaviour
{
    // Public vars.
    public GameObject CrystalPrefab;

    public GameObject PlayerPrefab;

	public GameObject LevelObject;

    public PlayerGameObjects[] ActivePlayersGameObjects = new PlayerGameObjects[4];

    // Private vars.
    private SpawnPoints spawnPoints;

    private StatsManager statsManager;

    // Unity callbacks.
    void Start()
    {
        spawnPoints = LevelObject.GetComponent<SpawnPoints>();

		statsManager = GetComponent<StatsManager>();

        // TODO: Decide how many players are playing.
        StartGame(4);
        initStatsManager();

    }

    // Public methods.
    public void StartGame(int numberOfPlayers)
    {
        GameObject[] spawnPointsToUse =
            numberOfPlayers == 2 ? spawnPoints.twoPlayers :
            numberOfPlayers == 3 ? spawnPoints.threePlayers :
            numberOfPlayers == 4 ? spawnPoints.fourPlayers :
            null;

        for (int i = 0; i < numberOfPlayers; ++i)
        {
            // Spawn.
            GameObject newCrystal = GameObject.Instantiate(CrystalPrefab);

            GameObject newPlayer = GameObject.Instantiate(PlayerPrefab);

            // Position and parent.
            newCrystal.transform.position = spawnPointsToUse[i].transform.position;

            newCrystal.transform.parent = spawnPointsToUse[i].transform;

            // Set indices.
            newCrystal.GetComponent<CrystalInfo>().PlayerIndex = i;

            PlayerInfo playerInfo = newPlayer.GetComponent<PlayerInfo>();

            playerInfo.PlayerIndex = i;

            // Set spawn point and spawn player.
            PlayerRespawn playerRespawn = newPlayer.GetComponent<PlayerRespawn>();

            playerRespawn.RespawnPoint = spawnPointsToUse[i];

            playerRespawn.Respawn();

            // Listen for battery charge drained event (loss condition).
            Battery newBattery = newCrystal.GetComponent<Battery>();

            newBattery.ChargeDrainedEvent += OnChargeDrained;

            // Keep track of active players.
            ActivePlayersGameObjects[i].PlayerObject = newPlayer;
            ActivePlayersGameObjects[i].CrystalObject = newCrystal;
        }
    }

	private void initStatsManager()
	{
		print("init");
		statsManager.Initialise();
	}

    public void EndGame()
    {
        // dont know yet
    }

    // Private methods.
    private void OnChargeDrained(object sender, CrystalInfo crystalInfo)
    {
        PermaKill(crystalInfo.PlayerIndex);
    }

    private void PermaKill(int playerIndex)
    {
        ActivePlayersGameObjects[playerIndex].PlayerObject.SetActive(false);
        ActivePlayersGameObjects[playerIndex].CrystalObject.SetActive(false);
    }
}
