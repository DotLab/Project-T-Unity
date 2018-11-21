using GameService.Core;
using GameUtil;
using System;
using System.Collections.Generic;

namespace GameService.CharacterComponents {
	public abstract class Character : IIdentifiable {
		private readonly int _id;
		private string _name = "";
		private string _description = "";
		private Extra _belong = null;
		private readonly CharacterView _view;
		private User _controller = null;
		private bool _destroyed = false;
		private int _groupID = -1;
		private CharacterToken _token = CharacterToken.NEUTRAL;

		private int _refreshPoint = 0;
		private int _fatePoint = 0;
		private int _physicsStress = 0;
		private int _physicsStressMax = 0;
		private bool _physicsInvincible = false;
		private int _mentalStress = 0;
		private int _mentalStressMax = 0;
		private bool _mentalInvincible = false;
		private readonly List<SkillType> _skills;
		private readonly List<Aspect> _aspects;
		private readonly List<Stunt> _stunts;
		private readonly List<Extra> _extras;
		private readonly List<Consequence> _consequences;

		public int ID { get { return _id; } }
		public string Name { get { return _name; } set { _name = value; } }
		public string Description { get { return _description; } set { _description = value; } }
		public Extra Belong { get { return _belong; } }
		public CharacterView View { get { return _view; } }
		public User Controller { get { return _controller; } }
		public bool Destroyed { get { return _destroyed; } }
		public int GroupID { get { return _groupID; } }
		public CharacterToken Token { get { return _token; } set { _token = value; } }

		public int RefreshPoint { get { return _refreshPoint; } set { _refreshPoint = value; } }
		public int FatePoint { get { return _fatePoint; } set { _fatePoint = value; } }
		public int PhysicsStress { get { return _physicsStress; } set { _physicsStress = value; } }
		public int PhysicsStressMax { get { return _physicsStressMax; } set { _physicsStressMax = value; } }
		public bool PhysicsInvincible { get { return _physicsInvincible; } set { _physicsInvincible = value; } }
		public int MentalStress { get { return _mentalStress; } set { _mentalStress = value; } }
		public int MentalStressMax { get { return _mentalStressMax; } set { _mentalStressMax = value; } }
		public bool MentalInvincible { get { return _mentalInvincible; } set { _mentalInvincible = value; } }
		public List<SkillType> Skills { get { return _skills; } }
		public List<Aspect> Aspects { get { return _aspects; } }
		public List<Stunt> Stunts { get { return _stunts; } }
		public List<Extra> Extras { get { return _extras; } }
		public List<Consequence> Consequences { get { return _consequences; } }
		
		public void SetBelong(Extra belong) {
			_belong = belong;
		}

		public void SetGroupID(int val) {
			_groupID = val;
		}

		protected Character(int id, CharacterView view) {
			_id = id;
			_view = view;
			_skills = new List<SkillType>();
			_aspects = new List<Aspect>();
			_stunts = new List<Stunt>();
			_extras = new List<Extra>();
			_consequences = new List<Consequence>();
		}

		public bool IsPartyWith(Character other) {
			if (_groupID == -1) return false;
			else return _groupID == other._groupID;
		}

		public Character[] PartyMembers() {
			return CharacterManager.Instance.GetPartyMembers(this);
		}

		public void MakePartyWith(Character other) {
			CharacterManager.Instance.MakeParty(this, other);
		}

		public void LeaveParty() {
			CharacterManager.Instance.LeaveParty(this);
		}

		public void BreakParty() {
			CharacterManager.Instance.BreakParty(this);
		}

		public void MarkDestroyed() {
			_destroyed = true;
		}

		public Aspect FindAspectByID(int id) {
			foreach (var aspect in this.Aspects) {
				if (aspect.ID == id) {
					return aspect;
				}
			}
			return null;
		}

		public Stunt FindStuntByID(int id) {
			if (this.Stunts != null) {
				foreach (var stunt in this.Stunts) {
					if (stunt.ID == id) {
						return stunt;
					}
				}
			}
			return null;
		}

		public Extra FindExtraByID(int id) {
			if (this.Extras != null) {
				foreach (var extra in this.Extras) {
					if (extra.ID == id) {
						return extra;
					}
				}
			}
			return null;
		}

		public Consequence FindConsequenceByID(int id) {
			if (this.Consequences != null) {
				foreach (var consequence in this.Consequences) {
					if (consequence.ID == id) {
						return consequence;
					}
				}
			}
			return null;
		}
	}

	public sealed class CharacterManager {
		private sealed class CharacterGroup {
			private static int _autoIncrement = 0;

			private readonly int _thisNumber = 0;
			private readonly List<Character> _characters = new List<Character>();

			public int ID { get { return _thisNumber; } }
			public List<Character> Characters { get { return _characters; } }
			public bool Empty { get { return _characters.Count <= 0; } }

			public CharacterGroup() {
				_thisNumber = _autoIncrement++;
			}
		}

		private static readonly CharacterManager _instance = new CharacterManager();
		public static CharacterManager Instance { get { return _instance; } }

		private int _incrementalIDNum = 0;

		private readonly IdentifiedObjectList<Character> _savingNPCharacters = new IdentifiedObjectList<Character>();
		private readonly IdentifiedObjectList<Character> _playerCharacters = new IdentifiedObjectList<Character>();

		private readonly Dictionary<int, CharacterGroup> _characterGroups = new Dictionary<int, CharacterGroup>();

		public IdentifiedObjectList<Character> SavingNPCharacters { get { return _savingNPCharacters; } }
		public IdentifiedObjectList<Character> PlayerCharacters { get { return _playerCharacters; } }

		private CharacterManager() {
			
		}

		public Character[][] GetAllParties() {
			var list = new List<Character[]>();
			foreach (var characterGroup in _characterGroups) {
				if (!characterGroup.Value.Empty) {
					list.Add(characterGroup.Value.Characters.ToArray());
				}
			}
			return list.ToArray();
		}
		
		public Character[] GetPartyMembers(Character character) {
			if (character.GroupID == -1) return new Character[0];
			else return _characterGroups[character.GroupID].Characters.ToArray();
		}

		public void MakeParty(Character character, Character partyMember) {
			if (character == partyMember) return;
			if (character.GroupID == partyMember.GroupID && character.GroupID != -1) return;
			if (character.GroupID != -1) LeaveParty(character);
			if (partyMember.GroupID != -1) {
				_characterGroups[partyMember.GroupID].Characters.Add(character);
				character.SetGroupID(partyMember.GroupID);
			} else {
				bool hasEmptyGroup = false;
				foreach (var group in _characterGroups) {
					if (group.Value.Empty) {
						group.Value.Characters.Add(character);
						group.Value.Characters.Add(partyMember);
						character.SetGroupID(group.Key);
						partyMember.SetGroupID(group.Key);
						hasEmptyGroup = true;
						break;
					}
				}
				if (!hasEmptyGroup) {
					var newGroup = new CharacterGroup();
					newGroup.Characters.Add(character);
					newGroup.Characters.Add(partyMember);
					character.SetGroupID(newGroup.ID);
					partyMember.SetGroupID(newGroup.ID);
					_characterGroups.Add(newGroup.ID, newGroup);
				}
			}
		}

		public void LeaveParty(Character character) {
			if (character.GroupID == -1) return;
			_characterGroups[character.GroupID].Characters.Remove(character);
			character.SetGroupID(-1);
		}

		public void BreakParty(Character partyMember) {
			if (partyMember.GroupID == -1) return;
			var group = _characterGroups[partyMember.GroupID];
			foreach (var member in group.Characters) {
				member.SetGroupID(-1);
			}
			group.Characters.Clear();
		}
		
	}
}
