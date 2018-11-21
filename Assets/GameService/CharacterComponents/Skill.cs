using GameService.Core;
using GameUtil;
using System;
using System.Collections.Generic;

namespace GameService.CharacterComponents {
	public sealed class SkillType : IEquatable<SkillType> {
		private static readonly Dictionary<string, SkillType> skillTypes = new Dictionary<string, SkillType>();
		public static Dictionary<string, SkillType> SkillTypes { get { return skillTypes; } }

		private readonly int _id;
		private readonly string _name;
		private int _level;
		
		public int ID { get { return _id; } }
		public string Name { get { return _name; } }
		public int Level { get { return _level; } }

		private SkillType(int id, string name) {
			_id = id;
			_name = name;
		}

		public bool Equals(SkillType other) {
			return other != null && _id == other._id;
		}

		public override bool Equals(object obj) {
			return Equals(obj as SkillType);
		}

		public override int GetHashCode() {
			return _id.GetHashCode();
		}
	}
}
