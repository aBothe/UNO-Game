﻿
namespace Uno
{
	public enum UnoMessage : byte
	{
		DrawCardFromStack=1,
		PressUno,
		SkipRound,
		PutCard,

		ActionNotAllowed = 0,

		YouAreNext
	}
}
