public class WinCondition
{
	public bool DoCheck ()
	{
		// Main Win Condition
		// if the current gameAction is Move
		// and gameAction.player.CurrentWorker.CurrentTile is Level 2 and the player moves to a tile with Level 3
		GameAction currAction = GameActionController.CurrentGameAction;
		if ( currAction != null )
		{
			if ( currAction.playerAction.ActionText == "Move" )
			{
				if ( currAction.prevTile.Level == Tile.Levels.Two && currAction.currTile.Level == Tile.Levels.Three )
				{
					PlayerManager.WinningPlayer = currAction.player;
					GameManager.SwitchScene ( "Post Match" );
				}
			}
		}

		// Alternate Win Condition(s)
		// (1) If a players worker is unable to move, they lose
		// (2) If a players worker is unable to build, they lose
		return false;
	}
}