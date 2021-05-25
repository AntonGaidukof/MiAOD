namespace RabinKarp
{
	public class HashData
	{
		public long Power { get; set; }
		public long Hash { get; set; }

		public HashData( long hash, long power )
		{
			Hash = hash;
			Power = power;
		}
	}
}