using System;
using GameService.Core;

namespace GameService.CharacterComponents {
	public enum PersistenceType {
		Fixed = 0,
		Common = 1,
		Temporary = 2
	}

	public class Aspect : IIdentifiable {
		protected readonly int _id;
		protected string _name = "";
		protected string _description = "";
		protected PersistenceType _persistenceType = PersistenceType.Common;
		protected Character _benefiter = null;
		protected int _benefitTimes = 0;

		public Aspect() {
			
		}

		public int ID { get { return _id; } }

		public string Name { get { return _name; } set { _name = value; } }
		public string Description { get { return _description; } set { _description = value; } }
		public PersistenceType PersistenceType { get { return _persistenceType; } set { _persistenceType = value; } }
		public Character Benefiter { get { return _benefiter; } set { _benefiter = value; } }
		public int BenefitTimes { get { return _benefitTimes; } set { _benefitTimes = value; } }
	}

	public sealed class Consequence : Aspect {
		private int _counteractLevel = 0;
		private int _actualDamage = 0;
		private bool _mentalDamage = false;

		public int CounteractLevel { get { return _counteractLevel; } set { _counteractLevel = value; } }
		public int ActualDamage { get { return _actualDamage; } set { _actualDamage = value; } }
		public bool MentalDamage { get { return _mentalDamage; } set { _mentalDamage = value; } }

		public Consequence() {
		}
	}
}
