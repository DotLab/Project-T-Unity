using GameService.CharacterComponents;
using System;

namespace GameService.Core {
	public class User : IEquatable<User> {
		private readonly int _index;
		private readonly string _id;
		private readonly string _name;
		private readonly Character _character;
		
		public string ID { get { return _id; } }
		public string Name { get { return _name; } }
		public Character Character { get { return _character; } }
		public bool IsDM { get { return _character == null; } }

		protected User(int index, string id, string name, Character character) {
			_index = index;
			_id = id;
			_name = name;
			_character = character;
		}

		public override bool Equals(object obj) {
			return Equals(obj as User);
		}

		public override int GetHashCode() {
			return _index;
		}

		public bool Equals(User other) {
			return other != null && _index == other._index;
		}
	}
}
