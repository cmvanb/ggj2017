﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject CrystalPrefab;

    public GameObject PlayerPrefab;

    public GameObject LevelObject;

    private SpawnPoints spawnPoints;
    
	// Unity callbacks.
	void Start ()
    {
        spawnPoints = LevelObject.GetComponent<SpawnPoints>();

        // TODO: Decide how many players are playing.
        StartGame(4);
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

            playerInfo.AssignIndex();

            int playerIndex = playerInfo.PlayerIndex;

            Debug.Assert(playerIndex == i, "playerIndex is " + playerIndex + ", expected " + i);

            // Set spawn point and spawn player.
            newPlayer.GetComponent<PlayerRespawn>().respawnPoint = spawnPointsToUse[i];
        }
    }

    public void EndGame()
    {
        // dont know yet
    }
}
