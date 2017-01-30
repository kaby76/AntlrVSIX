namespace org.antlr.codebuff.validation
{
    using System.Linq;
	using MurmurHash = Antlr4.Runtime.Misc.MurmurHash;


	public class FeatureVectorAsObject
	{
		public readonly int[] features;
		public readonly FeatureMetaData[] featureMetaData;

		public FeatureVectorAsObject(int[] features, FeatureMetaData[] featureMetaData)
		{
			this.features = features;
			this.featureMetaData = featureMetaData;
		}

		public override int GetHashCode()
		{
			int hash = MurmurHash.Initialize();
			int n = 0;
			for (int i = 0; i < features.Length - 3; i++)
			{ // don't include INFO
				if (featureMetaData != null && featureMetaData[i] == FeatureMetaData.UNUSED)
				{
					continue;
				}
				n++;
				int feature = features[i];
				hash = MurmurHash.Update(hash, feature);
			}
			return MurmurHash.Finish(hash, n);
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			else if (!(obj is FeatureVectorAsObject))
			{
				return false;
			}
			FeatureVectorAsObject other = (FeatureVectorAsObject) obj;
			for (int i = 0; i < features.Length - 3; i++)
			{ // don't include INFO
				if (featureMetaData != null && featureMetaData[i] == FeatureMetaData.UNUSED)
				{
					continue;
				}
				if (features[i] != other.features[i])
				{
					return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
		    return string.Join(" ", features);
		}
	}

}