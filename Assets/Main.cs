using System.Collections;
using System.Collections.Generic;
using GameUtil.Network;
using GameService.ServerProxy;
using Futilef;
using UnityEngine;

public class Main : MonoBehaviour {

	GameServer server;
	GpController storySceneGpC;

	void OnEnable() {
		storySceneGpC = new GpController();
		var connection = new NetworkfConnection();
		server = new GameServer(connection);
		
	}
	
	// Update is called once per frame
	void Update() {
		server.FlushServerMessage();
		if (storySceneGpC != null) storySceneGpC.Update(Time.deltaTime);
	}

	void OnDisable() {
		storySceneGpC.Dispose();
	}
}
