using System.Collections;
using System.Collections.Generic;
using GameUtil.Network;

namespace GameService.ServerProxy {
	public class BattleScene : ServerProxy {

		public BattleScene(Connection connection) :
			base(connection) {

		}

		public override void MessageReceived(Message message) {

		}
	}
}
