using System.Collections;
using System.Collections.Generic;
using GameUtil.Network;

namespace GameService.ServerProxy {
    public abstract class ServerProxy : IMessageReceiver {
		protected readonly Connection _connection;

		protected ServerProxy(Connection connection) {
			_connection = connection;
		}

		public abstract void MessageReceived(Message message);
    }

    public class GameServer : ServerProxy {

        public GameServer(Connection connection) :
            base(connection) {
            
        }

        public override void MessageReceived(Message message) {

        }

        public void FlushServerMessage() {
            _connection.FlushReceivingBuffer();
        }
    }

}
