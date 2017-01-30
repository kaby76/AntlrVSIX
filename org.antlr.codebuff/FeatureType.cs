using System.Collections.Generic;

namespace org.antlr.codebuff
{

	public sealed class FeatureType
	{
		public static readonly FeatureType TOKEN = new FeatureType("TOKEN", InnerEnum.TOKEN, 12);
		public static readonly FeatureType RULE = new FeatureType("RULE", InnerEnum.RULE, 14);
		public static readonly FeatureType INT = new FeatureType("INT", InnerEnum.INT, 12);
		public static readonly FeatureType BOOL = new FeatureType("BOOL", InnerEnum.BOOL, 5); // bool can be -1 meaning don't know
		public static readonly FeatureType INFO_FILE = new FeatureType("INFO_FILE", InnerEnum.INFO_FILE, 15);
		public static readonly FeatureType INFO_LINE = new FeatureType("INFO_LINE", InnerEnum.INFO_LINE, 4);
		public static readonly FeatureType INFO_CHARPOS = new FeatureType("INFO_CHARPOS", InnerEnum.INFO_CHARPOS, 4);
		public static readonly FeatureType UNUSED = new FeatureType("UNUSED", InnerEnum.UNUSED, 0);

		private static readonly IList<FeatureType> valueList = new List<FeatureType>();

		static FeatureType()
		{
			valueList.Add(TOKEN);
			valueList.Add(RULE);
			valueList.Add(INT);
			valueList.Add(BOOL);
			valueList.Add(INFO_FILE);
			valueList.Add(INFO_LINE);
			valueList.Add(INFO_CHARPOS);
			valueList.Add(UNUSED);
		}

		public enum InnerEnum
		{
			TOKEN,
			RULE,
			INT,
			BOOL,
			INFO_FILE,
			INFO_LINE,
			INFO_CHARPOS,
			UNUSED
		}

		private readonly string nameValue;
		private readonly int ordinalValue;
		private readonly InnerEnum innerEnumValue;
		private static int nextOrdinal = 0;
		public int displayWidth;

		internal FeatureType(string name, InnerEnum innerEnum, int displayWidth)
		{
			this.displayWidth = displayWidth;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

		public static IList<FeatureType> values()
		{
			return valueList;
		}

		public InnerEnum InnerEnumValue()
		{
			return innerEnumValue;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public override string ToString()
		{
			return nameValue;
		}

		public static FeatureType valueOf(string name)
		{
			foreach (FeatureType enumInstance in FeatureType.values())
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}