
namespace Uno
{
	public enum UnoMessage : byte
	{
		DrawCardFromStack=1,
		PressUno,
		SkipRound,
		PutCard,

		ActionNotAllowed = 0,

		YouAreNext,
		/// <summary>
		/// Will be sent out just for telling the winner's name
		/// </summary>
		GameFinished,
	}
}
