using GameUtil.Network.Streamable;
using System;

namespace GameUtil {
	public enum BattleMapDirection {
		POSITIVE_ROW = 1,
		POSITIVE_COL = 2,
		NEGATIVE_ROW = 4,
		NEGATIVE_COL = 8
	}

	public enum CharacterAction {
		CREATE_ASPECT = 1,
		ATTACK = 2,
		HINDER = 4
	}

	public enum CharacterToken {
		PLAYER,
		FRIENDLY,
		NEUTRAL,
		NEUTRAL_HOSTILE,
		HOSTILE
	}

	public enum CheckResult {
		FAIL = 0,
		TIE = 1,
		SUCCEED = 2,
		SUCCEED_WITH_STYLE = 3
	}

	public struct Vec2 : IStreamable {
		public float X, Y;

		public Vec2(float x, float y) {
			X = x; Y = y;
		}

		public void ReadFrom(IDataInputStream stream) {
			X = stream.ReadSingle();
			Y = stream.ReadSingle();
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteSingle(X);
			stream.WriteSingle(Y);
		}
	}

	public struct Vec3 : IStreamable {
		public float X, Y, Z;

		public Vec3(float x, float y, float z) {
			X = x; Y = y; Z = z;
		}

		public void ReadFrom(IDataInputStream stream) {
			X = stream.ReadSingle();
			Y = stream.ReadSingle();
			Z = stream.ReadSingle();
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteSingle(X);
			stream.WriteSingle(Y);
			stream.WriteSingle(Z);
		}
	}

	public struct Vec4 : IStreamable {
		public float X, Y, Z, W;

		public Vec4(float x, float y, float z, float w) {
			X = x; Y = y; Z = z; W = w;
		}

		public void ReadFrom(IDataInputStream stream) {
			X = stream.ReadSingle();
			Y = stream.ReadSingle();
			Z = stream.ReadSingle();
			W = stream.ReadSingle();
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteSingle(X);
			stream.WriteSingle(Y);
			stream.WriteSingle(Z);
			stream.WriteSingle(W);
		}
	}

	public struct Range : IStreamable {
		public bool lowOpen;
		public float low;

		public bool highOpen;
		public float high;

		public Range(float greaterEqual, float less) {
			lowOpen = false;
			highOpen = true;
			low = greaterEqual;
			high = less;
		}

		public bool InRange(float num) {
			bool ret = true;
			ret &= num > low || (!lowOpen && num == low);
			ret &= num < high || (!highOpen && num == high);
			return ret;
		}

		public bool OutOfRange(float num) {
			return !this.InRange(num);
		}

		public void ReadFrom(IDataInputStream stream) {
			lowOpen = stream.ReadBoolean();
			low = stream.ReadSingle();
			highOpen = stream.ReadBoolean();
			high = stream.ReadSingle();
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteBoolean(lowOpen);
			stream.WriteSingle(low);
			stream.WriteBoolean(highOpen);
			stream.WriteSingle(high);
		}
	}

	public struct T2DID : IStreamable {
		public Guid textureID;
		public int spriteID;
		
		public void ReadFrom(IDataInputStream stream) {
			textureID = InputStreamHelper.ReadGuid(stream);
			spriteID = stream.ReadInt32();
		}

		public void WriteTo(IDataOutputStream stream) {
			OutputStreamHelper.WriteGuid(stream, textureID);
			stream.WriteInt32(spriteID);
		}
	}

	public sealed class CharacterView : IStreamable {
		public T2DID avatar;
		public T2DID portrait;
		public T2DID isometric;

		public void ReadFrom(IDataInputStream stream) {
			avatar.ReadFrom(stream);
			portrait.ReadFrom(stream);
			isometric.ReadFrom(stream);
		}

		public void WriteTo(IDataOutputStream stream) {
			avatar.WriteTo(stream);
			portrait.WriteTo(stream);
			isometric.WriteTo(stream);
		}
	}

	public struct Layout : IStreamable {
		public static readonly Layout INIT = new Layout(new Vec2(0, 0), 0, new Vec2(1, 1));

		public Vec2 pos;
		public float rot;
		public Vec2 sca;

		public Layout(Vec2 pos, float rot, Vec2 sca) {
			this.pos = pos;
			this.rot = rot;
			this.sca = sca;
		}

		public void ReadFrom(IDataInputStream stream) {
			pos.ReadFrom(stream);
			rot = stream.ReadSingle();
			sca.ReadFrom(stream);
		}

		public void WriteTo(IDataOutputStream stream) {
			pos.WriteTo(stream);
			stream.WriteSingle(rot);
			sca.WriteTo(stream);
		}
	}

	public struct GridPos : IStreamable, IEquatable<GridPos> {
		public int row;
		public int col;
		public bool highland;

		public bool Equals(GridPos other) {
			return row == other.row && col == other.col && highland == other.highland;
		}
		
		public override int GetHashCode() {
			int hash = ((row << 8) & 0xFF00) | (col & 0xFF);
			return highland ? (hash ^ 0xFFFF) & 0xFFFF : hash;
		}
		
		public void ReadFrom(IDataInputStream stream) {
			row = stream.ReadInt32();
			col = stream.ReadInt32();
			highland = stream.ReadBoolean();
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(row);
			stream.WriteInt32(col);
			stream.WriteBoolean(highland);
		}
	}

	public struct CameraSFX : IStreamable {
		public enum SFXType {
			Shake
		}
		
		public SFXType effect;

		public CameraSFX(SFXType effect) {
			this.effect = effect;
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteByte((byte)effect);
		}

		public void ReadFrom(IDataInputStream stream) {
			effect = (SFXType)stream.ReadByte();
		}
	}

	public struct PortraitSFX : IStreamable {
		public enum SFXType {
			Shake, Flash
		}
		
		public SFXType effect;
		public int loopTime;

		public PortraitSFX(SFXType effect, int loopTime) {
			this.effect = effect;
			this.loopTime = loopTime;
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteByte((byte)effect);
			stream.WriteInt32(loopTime);
		}

		public void ReadFrom(IDataInputStream stream) {
			effect = (SFXType)stream.ReadByte();
			loopTime = stream.ReadInt32();
		}
	}

	public struct PortraitStyle : IStreamable {
		public static readonly PortraitStyle INIT = new PortraitStyle(0);
		
		public int emotion;

		public PortraitStyle(int emotion) {
			this.emotion = emotion;
		}

		public void ReadFrom(IDataInputStream stream) {
			emotion = stream.ReadInt32();
		}

		public void WriteTo(IDataOutputStream stream) {
			stream.WriteInt32(emotion);
		}
	}
}
