using System.Collections;
using System.Collections.Generic;
using GameUtil.Network;
using GameService.Core;
using Futilef;

namespace GameService.ServerProxy {
    public class StoryScene : ServerProxy {
        private GpController _gpc;

        public StoryScene(Connection connection, GpController gpc) :
            base(connection) {
            _gpc = gpc;
        }

        public override void MessageReceived(Message message) {
            // 1
            
        }

        
    }
    

}