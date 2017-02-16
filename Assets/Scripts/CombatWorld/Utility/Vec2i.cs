using System.Collections;
using System.Collections.Generic;

namespace CombatWorld.Utility
{
	public class Vec2i
	{
		public int x { get; private set; }
		public int y { get; private set; }

		public Vec2i(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public Vec2i Get()
		{
			return this;
		}

		public static Vec2i operator +(Vec2i left, Vec2i right)
		{
			return new Vec2i(left.x + right.x, left.y + right.y);
		}

		public static Vec2i operator -(Vec2i left, Vec2i right)
		{
			return new Vec2i(left.x - right.x, left.y - right.y);
		}

		public override string ToString()
		{
			return "{" + x + "," + y + "}";
		}

		#region Needed for use in dictionaries etc.
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != typeof(Vec2i))
				return false;
			Vec2i other = (Vec2i)obj;
			return x == other.x && y == other.y;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return x.GetHashCode() ^ y.GetHashCode();
			}
		}
		#endregion
	}
}