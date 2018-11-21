﻿using GameUtil.Network.Streamable;

namespace GameUtil.Network.ClientMessages {
	public sealed class ClientInitMessage : Message {
		public const int MESSAGE_TYPE = 1;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}

	public sealed class RequestResourceSynchronizationMessage : Message {
		public const int MESSAGE_TYPE = 2;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}

	public sealed class UserDecisionMessage : Message {
		public const int MESSAGE_TYPE = 30;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int selectionIndex;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(selectionIndex);
		}

		public override void ReadFrom(IDataInputStream stream) {
			selectionIndex = stream.ReadInt32();
		}
	}

	public sealed class BroadcastChatMessage : Message {
		public const int MESSAGE_TYPE = 30;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string text;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(text);
		}

		public override void ReadFrom(IDataInputStream stream) {
			text = stream.ReadString();
		}
	}

	public sealed class PrivateChatMessage : Message {
		public const int MESSAGE_TYPE = 30;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string receiverID;
		public string text;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(receiverID);
			stream.WriteString(text);
		}

		public override void ReadFrom(IDataInputStream stream) {
			receiverID = stream.ReadString();
			text = stream.ReadString();
		}
	}
	
	public sealed class CampaignDoNextActionMessage : Message {
		public const int MESSAGE_TYPE = 784;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}

	public sealed class CampaignJumpToMessage : Message {
		public const int MESSAGE_TYPE = 7484;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string campaignName;

		public override void ReadFrom(IDataInputStream stream) {
			campaignName = stream.ReadString();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(campaignName);
		}
	}

	public sealed class CampaignPlayShotMessage : Message {
		public const int MESSAGE_TYPE = 7884;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string shotName;

		public override void ReadFrom(IDataInputStream stream) {
			shotName = stream.ReadString();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(shotName);
		}
	}
	
	public sealed class CheckerPassiveSkillSelectedMessage : Message {
		public const int MESSAGE_TYPE = 5;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int skillTypeID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(skillTypeID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			skillTypeID = stream.ReadInt32();
		}
	}

	public sealed class CheckerPassiveStuntSelectedMessage : Message {
		public const int MESSAGE_TYPE = 7;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int stuntID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(stuntID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			stuntID = stream.ReadInt32();
		}
	}

	public sealed class CheckerAspectSelectedMessage : Message {
		public const int MESSAGE_TYPE = 6;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int aspectOwnerID;
		public int aspectID;
		public bool isConsequence;
		public bool reroll;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(aspectOwnerID);
			stream.WriteInt32(aspectID);
			stream.WriteBoolean(isConsequence);
			stream.WriteBoolean(reroll);
		}

		public override void ReadFrom(IDataInputStream stream) {
			aspectOwnerID = stream.ReadInt32();
			aspectID = stream.ReadInt32();
			isConsequence = stream.ReadBoolean();
			reroll = stream.ReadBoolean();
		}
	}

	public sealed class CheckerAspectSelectionOverMessage : Message {
		public const int MESSAGE_TYPE = 18;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void WriteTo(IDataOutputStream stream) { }
		public override void ReadFrom(IDataInputStream stream) { }
	}

	public sealed class CheckerSetSkipSelectAspectMessage : Message {
		public const int MESSAGE_TYPE = 17;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public bool val;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteBoolean(val);
		}

		public override void ReadFrom(IDataInputStream stream) {
			val = stream.ReadBoolean();
		}
	}

	public sealed class CheckerGetInitiativeCanUseSkillMessage : Message {
		public const int MESSAGE_TYPE = 26;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int initiativeID;
		public int skillTypeID;
		public CharacterAction action;

		public override void ReadFrom(IDataInputStream stream) {
			initiativeID = stream.ReadInt32();
			skillTypeID = stream.ReadInt32();
			action = (CharacterAction)stream.ReadByte();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(initiativeID);
			stream.WriteInt32(skillTypeID);
			stream.WriteByte((byte)action);
		}
	}

	public sealed class CheckerGetInitiativeCanUseStuntMessage : Message {
		public const int MESSAGE_TYPE = 39;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int initiativeID;
		public int stuntID;
		public CharacterAction action;

		public override void ReadFrom(IDataInputStream stream) {
			initiativeID = stream.ReadInt32();
			stuntID = stream.ReadInt32();
			action = (CharacterAction)stream.ReadByte();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(initiativeID);
			stream.WriteInt32(stuntID);
			stream.WriteByte((byte)action);
		}
	}

	public sealed class CheckerGetPassiveUsableActionListMessage : Message {
		public const int MESSAGE_TYPE = 27;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}

	public sealed class CheckerGetStuntTargetValidityMessage : Message {
		public const int MESSAGE_TYPE = 31;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int initiativeID;
		public int targetID;
		public int stuntID;
		public CharacterAction action;

		public override void ReadFrom(IDataInputStream stream) {
			initiativeID = stream.ReadInt32();
			targetID = stream.ReadInt32();
			stuntID = stream.ReadInt32();
			action = (CharacterAction)stream.ReadByte();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(initiativeID);
			stream.WriteInt32(targetID);
			stream.WriteInt32(stuntID);
			stream.WriteByte((byte)action);
		}
	}

	public sealed class GetCharacterDataMessage : Message {
		public const int MESSAGE_TYPE = 8;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public enum DataType {
			INFO,
			ASPECTS,
			STUNTS,
			EXTRAS,
			CONSEQUENCES,
			FATEPOINT,
			STRESS,
			GROUP
		}

		public int characterID;
		public DataType dataType;

		public override void ReadFrom(IDataInputStream stream) {
			characterID = stream.ReadInt32();
			dataType = (DataType)stream.ReadByte();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(characterID);
			stream.WriteByte((byte)dataType);
		}
	}

	public sealed class GetAspectDataMessage : Message {
		public const int MESSAGE_TYPE = 9;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int characterID;
		public int aspectID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(characterID);
			stream.WriteInt32(aspectID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			characterID = stream.ReadInt32();
			aspectID = stream.ReadInt32();
		}
	}

	public sealed class GetConsequenceDataMessage : Message {
		public const int MESSAGE_TYPE = 10;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int characterID;
		public int consequenceID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(characterID);
			stream.WriteInt32(consequenceID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			characterID = stream.ReadInt32();
			consequenceID = stream.ReadInt32();
		}
	}

	public sealed class GetSkillDataMessage : Message {
		public const int MESSAGE_TYPE = 11;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int characterID;
		public int skillTypeID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(characterID);
			stream.WriteInt32(skillTypeID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			characterID = stream.ReadInt32();
			skillTypeID = stream.ReadInt32();
		}
	}

	public sealed class GetStuntDataMessage : Message {
		public const int MESSAGE_TYPE = 12;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int characterID;
		public int stuntID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(characterID);
			stream.WriteInt32(stuntID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			characterID = stream.ReadInt32();
			stuntID = stream.ReadInt32();
		}
	}

	public sealed class GetExtraDataMessage : Message {
		public const int MESSAGE_TYPE = 13;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int characterID;
		public int extraID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(characterID);
			stream.WriteInt32(extraID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			characterID = stream.ReadInt32();
			extraID = stream.ReadInt32();
		}
	}

	public sealed class GetDirectResistSkillsMessage : Message {
		public const int MESSAGE_TYPE = 14;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int initiativeSkillTypeID;
		public CharacterAction actionType;

		public override void ReadFrom(IDataInputStream stream) {
			initiativeSkillTypeID = stream.ReadInt32();
			actionType = (CharacterAction)stream.ReadByte();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(initiativeSkillTypeID);
			stream.WriteByte((byte)actionType);
		}
	}

	public sealed class GetDirectResistableStuntsMessage : Message {
		public const int MESSAGE_TYPE = 34;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int initiativeSkillTypeID;
		public int passiveCharacterID;
		public CharacterAction actionType;

		public override void ReadFrom(IDataInputStream stream) {
			initiativeSkillTypeID = stream.ReadInt32();
			passiveCharacterID = stream.ReadInt32();
			actionType = (CharacterAction)stream.ReadByte();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(initiativeSkillTypeID);
			stream.WriteInt32(passiveCharacterID);
			stream.WriteByte((byte)actionType);
		}
	}

	public sealed class GetSkillTypeListMessage : Message {
		public const int MESSAGE_TYPE = 15;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}
	
	public sealed class StorySceneAddPlayerToStage : Message {
		public const int MESSAGE_TYPE = 77;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string playerID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(playerID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			playerID = stream.ReadString();
		}
	}

	public sealed class StorySceneRemovePlayerFromStage : Message {
		public const int MESSAGE_TYPE = 88;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string playerID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(playerID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			playerID = stream.ReadString();
		}
	}

	public sealed class StorySceneAllowPlayerActingMessage : Message {
		public const int MESSAGE_TYPE = 3;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string playerID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(playerID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			playerID = stream.ReadString();
		}
	}
	
	public sealed class StorySceneStopPlayerActingMessage : Message {
		public const int MESSAGE_TYPE = 473;
		public override int MessageType { get { return MESSAGE_TYPE; } }
		
		public override void WriteTo(IDataOutputStream stream) { }
		public override void ReadFrom(IDataInputStream stream) { }
	}
	
	public sealed class StorySceneInvestigateObjectMessage : Message {
		public const int MESSAGE_TYPE = 331;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int objID;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(objID);
		}

		public override void ReadFrom(IDataInputStream stream) {
			objID = stream.ReadInt32();
		}
	}

	public sealed class StorySceneSelectSelectionMessage : Message {
		public const int MESSAGE_TYPE = 354;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int index;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(index);
		}

		public override void ReadFrom(IDataInputStream stream) {
			index = stream.ReadInt32();
		}
	}

	public sealed class StorySceneObjectDoActionMessage : Message {
		public const int MESSAGE_TYPE = 21;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int objID;
		public int skillTypeOrStuntID;
		public bool isStunt;
		public CharacterAction action;
		public int[] targetsID;

		public override void ReadFrom(IDataInputStream stream) {
			objID = stream.ReadInt32();
			skillTypeOrStuntID = stream.ReadInt32();
			isStunt = stream.ReadBoolean();
			action = (CharacterAction)stream.ReadByte();
			int length = stream.ReadInt32();
			targetsID = new int[length];
			for (int i = 0; i < length; ++i) {
				targetsID[i] = stream.ReadInt32();
			}
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(objID);
			stream.WriteInt32(skillTypeOrStuntID);
			stream.WriteBoolean(isStunt);
			stream.WriteByte((byte)action);
			stream.WriteInt32(targetsID.Length);
			foreach (var target in targetsID) {
				stream.WriteInt32(target);
			}
		}
	}

	public sealed class StorySceneObjectUseStuntDirectlyMessage : Message {
		public const int MESSAGE_TYPE = 22;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public int objID;
		public int stuntID;
		public int[] targetsID;

		public override void ReadFrom(IDataInputStream stream) {
			objID = stream.ReadInt32();
			stuntID = stream.ReadInt32();
			int length = stream.ReadInt32();
			targetsID = new int[length];
			for (int i = 0; i < length; ++i) {
				targetsID[i] = stream.ReadInt32();
			}
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(objID);
			stream.WriteInt32(stuntID);
			stream.WriteInt32(targetsID.Length);
			foreach (var target in targetsID) {
				stream.WriteInt32(target);
			}
		}
	}

	public sealed class StoryDialogSendTextMessage : Message {
		public const int MESSAGE_TYPE = 378;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string text;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(text);
		}

		public override void ReadFrom(IDataInputStream stream) {
			text = stream.ReadString();
		}
	}

	public sealed class StoryDialogSendPrivateTextMessage : Message {
		public const int MESSAGE_TYPE = 366;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string text;

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(text);
		}

		public override void ReadFrom(IDataInputStream stream) {
			text = stream.ReadString();
		}
	}

	public sealed class StoryDialogSendPortraitEmotionMessage : Message {
		public const int MESSAGE_TYPE = 543;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public PortraitStyle style;
		public PortraitSFX sfx;

		public override void WriteTo(IDataOutputStream stream) {
			style.WriteTo(stream);
			sfx.WriteTo(stream);
		}

		public override void ReadFrom(IDataInputStream stream) {
			style.ReadFrom(stream);
			sfx.ReadFrom(stream);
		}
	}

	public sealed class BattleSceneGetGridObjectDataMessage : Message {
		public const int MESSAGE_TYPE = 24;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string objID;

		public override void ReadFrom(IDataInputStream stream) {
			objID = stream.ReadString();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(objID);
		}
	}

	public sealed class BattleSceneGetLadderObjectDataMessage : Message {
		public const int MESSAGE_TYPE = 25;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string objID;

		public override void ReadFrom(IDataInputStream stream) {
			objID = stream.ReadString();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(objID);
		}
	}

	public sealed class BattleSceneGetObjectCanExtraMoveMessage : Message {
		public const int MESSAGE_TYPE = 28;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}

	public sealed class BattleSceneObjectTakeExtraMovePointMessage : Message {
		public const int MESSAGE_TYPE = 23;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}

	public sealed class BattleSceneGetMovePathInfoMessage : Message {
		public const int MESSAGE_TYPE = 19;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void WriteTo(IDataOutputStream stream) { }
		public override void ReadFrom(IDataInputStream stream) { }
	}

	public sealed class BattleSceneActableObjectMoveMessage : Message {
		public const int MESSAGE_TYPE = 20;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public GridPos dst;

		public override void WriteTo(IDataOutputStream stream) {
			dst.WriteTo(stream);
		}

		public override void ReadFrom(IDataInputStream stream) {
			dst.ReadFrom(stream);
		}
	}

	public sealed class BattleSceneActableObjectDoActionMessage : Message {
		public const int MESSAGE_TYPE = 21;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string skillTypeOrStuntID;
		public bool isStunt;
		public CharacterAction action;
		public GridPos dstCenter;
		public string[] targetsID;

		public override void ReadFrom(IDataInputStream stream) {
			skillTypeOrStuntID = stream.ReadString();
			isStunt = stream.ReadBoolean();
			action = (CharacterAction)stream.ReadByte();
			dstCenter.ReadFrom(stream);
			int length = stream.ReadInt32();
			targetsID = new string[length];
			for (int i = 0; i < length; ++i) {
				targetsID[i] = stream.ReadString();
			}
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(skillTypeOrStuntID);
			stream.WriteBoolean(isStunt);
			stream.WriteByte((byte)action);
			dstCenter.WriteTo(stream);
			stream.WriteInt32(targetsID.Length);
			foreach (var target in targetsID) {
				stream.WriteString(target);
			}
		}
	}

	public sealed class BattleSceneActableObjectUseStuntDirectlyMessage : Message {
		public const int MESSAGE_TYPE = 22;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string stuntID;
		public GridPos dstCenter;
		public string[] targetsID;

		public override void ReadFrom(IDataInputStream stream) {
			stuntID = stream.ReadString();
			dstCenter.ReadFrom(stream);
			int length = stream.ReadInt32();
			targetsID = new string[length];
			for (int i = 0; i < length; ++i) {
				targetsID[i] = stream.ReadString();
			}
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(stuntID);
			dstCenter.WriteTo(stream);
			stream.WriteInt32(targetsID.Length);
			foreach (var target in targetsID) {
				stream.WriteString(target);
			}
		}
	}

	public sealed class BattleSceneGetActionAffectableAreasMessage : Message {
		public const int MESSAGE_TYPE = 32;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public string skillTypeOrStuntID;
		public bool isStunt;

		public override void ReadFrom(IDataInputStream stream) {
			skillTypeOrStuntID = stream.ReadString();
			isStunt = stream.ReadBoolean();
		}

		public override void WriteTo(IDataOutputStream stream) {
			stream.WriteString(skillTypeOrStuntID);
			stream.WriteBoolean(isStunt);
		}
	}

	public sealed class BattleSceneTurnOverMessage : Message {
		public const int MESSAGE_TYPE = 29;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}

	public sealed class GetAllPartyListMessage : Message {
		public const int MESSAGE_TYPE = 37;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}

	public sealed class GetPlayerCharacterMessage : Message {
		public const int MESSAGE_TYPE = 38;
		public override int MessageType { get { return MESSAGE_TYPE; } }

		public override void ReadFrom(IDataInputStream stream) { }
		public override void WriteTo(IDataOutputStream stream) { }
	}
}
