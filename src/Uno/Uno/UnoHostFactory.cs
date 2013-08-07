using Uno.Game;

namespace Uno
{
	public class UnoHostFactory : GameHostFactory
	{
		public readonly static UnoHostFactory Instance = new UnoHostFactory();

		public GameHost Create()
		{
			return new UnoHost();
		}

		public GameConnection CreateConnection()
		{
			return new UnoGameConnection();
		}
	}
}
