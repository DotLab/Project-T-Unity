using GameService.Core;
using GameUtil;
using System;
using System.Collections.Generic;

namespace GameService.CharacterComponents {
	public sealed class Stunt : IIdentifiable {
		private readonly int _id;
		private string _name = "";
		private string _description = "";

		public Stunt(string name, string description) {
			_name = name;
			_description = description;
		}

		public int ID { get { return _id; } }
		public string Name { get { return _name; } set { _name = value; } }
		public string Description { get { return _description; } set { _description = value; } }
		
	}
}
