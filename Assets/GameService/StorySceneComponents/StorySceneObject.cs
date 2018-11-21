using System.Collections;
using System.Collections.Generic;
using GameUtil.Network;
using GameUtil;
using GameService.Core;
using GameService.CharacterComponents;

namespace GameService.StorySceneComponents {
    public class SceneObject : IIdentifiable {
		protected readonly Character _characterRef;
		protected Layout _layout = Layout.INIT;
		protected PortraitStyle _style = PortraitStyle.INIT;
		protected PortraitSFX _effect;

		public int ID { get { return _characterRef.ID; } }
		public string Name { get { return _characterRef.Name; } set { _characterRef.Name = value; } }
		public string Description { get { return _characterRef.Description; } set { _characterRef.Description = value; } }
		public Character CharacterRef { get { return _characterRef; } }
		public Layout Layout { get { return _layout; } }
		public PortraitStyle Style { get { return _style; } }
		public PortraitSFX Effect { get { return _effect; } }

		public SceneObject(Character character) {
			_characterRef = character;
		}

		public virtual void ApplyEffect(PortraitSFX effect) {
			_effect = effect;
			
		}

		public virtual void MoveTo(Layout layout) {
			_layout = layout;
			
		}

		public virtual void ChangeStyle(PortraitStyle style) {
			_style = style;
			
		}
		
		public virtual void Interact() {
            
		}
	}
}