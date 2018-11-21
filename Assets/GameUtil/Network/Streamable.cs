using System;

namespace GameUtil.Network.Streamable {
	public interface IDataOutputStream {
		void WriteBoolean(Boolean val);
		void WriteString(String val);
		void WriteByte(Byte val);
		void WriteInt16(Int16 val);
		void WriteInt32(Int32 val);
		void WriteSingle(Single val);
	}

	public interface IDataInputStream {
		Boolean ReadBoolean();
		String ReadString();
		Byte ReadByte();
		Int16 ReadInt16();
		Int32 ReadInt32();
		Single ReadSingle();
	}

	public interface IStreamable {
		void WriteTo(IDataOutputStream stream);
		void ReadFrom(IDataInputStream stream);
	}

	public static class OutputStreamHelper {
		public static void WriteGuid(IDataOutputStream stream, Guid guid) {
			byte[] bs = guid.ToByteArray();
			stream.WriteByte((byte)bs.Length);
			foreach (var b in bs) {
				stream.WriteByte(b);
			}
		}
	}

	public static class InputStreamHelper {
		public static Guid ReadGuid(IDataInputStream stream) {
			int length = stream.ReadByte();
			byte[] bs = new byte[length];
			for (int i = 0; i < length; ++i) {
				bs[i] = stream.ReadByte();
			}
			return new Guid(bs);
		}
	}

	public struct Describable : IStreamable {
		public string name;
		public string description;
		
		public void ReadFrom(IDataInputStream stream) {
			name = stream.ReadString();
			description = stream.ReadString();
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteString(name);
			stream.WriteString(description);
		}
	}

	public struct SkillTypeDescription : IStreamable {
		public int id;
		public string name;

		public void ReadFrom(IDataInputStream stream) {
			id = stream.ReadInt32();
			name = stream.ReadString();
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(id);
			stream.WriteString(name);
		}
	}

	public struct CharacterPropertyDescription : IStreamable {
		public int propertyID;
		public Describable describable;

		public void ReadFrom(IDataInputStream stream) {
			propertyID = stream.ReadInt32();
			describable.ReadFrom(stream);
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(propertyID);
			describable.WriteTo(stream);
		}
	}
	
	public struct GridObjectData : IStreamable {
		public struct ActableObjectData : IStreamable {
			public int actionPoint;
			public int actionPointMax;
			public bool movable;
			public int movePoint;

			public void ReadFrom(IDataInputStream stream) {
				actionPoint = stream.ReadInt32();
				actionPointMax = stream.ReadInt32();
				movable = stream.ReadBoolean();
				movePoint = stream.ReadInt32();
			}

			public void WriteTo(IDataOutputStream stream) {
				stream.WriteInt32(actionPoint);
				stream.WriteInt32(actionPointMax);
				stream.WriteBoolean(movable);
				stream.WriteInt32(movePoint);
			}
		}

		public string id;
		public int row;
		public int col;
		public bool obstacle;
		public bool highland;
		public int stagnate;
		public BattleMapDirection direction;
		public bool actable;
		public ActableObjectData actableObjData;

		public void ReadFrom(IDataInputStream stream) {
			id = stream.ReadString();
			row = stream.ReadInt32();
			col = stream.ReadInt32();
			obstacle = stream.ReadBoolean();
			highland = stream.ReadBoolean();
			stagnate = stream.ReadInt32();
			direction = (BattleMapDirection)stream.ReadByte();
			actable = stream.ReadBoolean();
			if (actable) {
				actableObjData.ReadFrom(stream);
			}
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteString(id);
			stream.WriteInt32(row);
			stream.WriteInt32(col);
			stream.WriteBoolean(obstacle);
			stream.WriteBoolean(highland);
			stream.WriteInt32(stagnate);
			stream.WriteByte((byte)direction);
			stream.WriteBoolean(actable);
			if (actable) {
				actableObjData.WriteTo(stream);
			}
		}
	}

	public struct LadderObjectData : IStreamable {
		public string id;
		public int row;
		public int col;
		public int stagnate;
		public BattleMapDirection direction;

		public void ReadFrom(IDataInputStream stream) {
			id = stream.ReadString();
			row = stream.ReadInt32();
			col = stream.ReadInt32();
			stagnate = stream.ReadInt32();
			direction = (BattleMapDirection)stream.ReadByte();
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteString(id);
			stream.WriteInt32(row);
			stream.WriteInt32(col);
			stream.WriteInt32(stagnate);
			stream.WriteByte((byte)direction);
		}
	}
}
