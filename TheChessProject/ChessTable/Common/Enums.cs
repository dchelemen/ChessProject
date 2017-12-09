namespace ChessTable.Common
{
	public enum FigureType
	{
		NO_FIGURE = 0,
		KING,
		QUEEN,
		ROOK,
		BISHOP,
		KNIGHT,
		PAWN,
		EN_PASSANT_PAWN,
		MOVED_ROOK,
		MOVED_KING,
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------

	public enum TableHighLight
	{
		NONE,
		BLUE,
		RED
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------

	public enum Colors
	{
		WHITE,
		BLACK,
		RED,
		GREEN,
		BLUE,
		NO_COLOR
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------

	public enum Player
	{
		PLAYER_ONE,
		PLAYER_TWO,
		NO_PLAYER
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------

	public enum Algorithm
	{
		HUMAN,
		RANDOM,
		ALPHA_BETA,
		ALPHA_BETA_RANDOM,
		ALPHA_BETA_WITH_WEIGHT,
		ALPHA_BETA_RANDOM_WITH_WEIGHT
	}

	//-----------------------------------------------------------------------------------------------------------------------------------------

	public enum GameType
	{
		STANDARD_GAME,
		CUSTOM_GAME
	}
}
