/*
 * Adam Botens
 * CSCI 3005
 * Assignment 3
 * Gomoku using a 9x9 Board
 * 
 */

namespace Gomoku
{
    /// <summary>
    /// Includes PositionState, a player class, a board class 
    /// and functions for playing the game
    /// </summary>
    class GomokuGame
    {
        public enum PositionState { Empty, White, Black };
        /// <summary>
        /// Player class that holds the names and stones of players
        /// </summary>
        public class Player
        {
            public string Name { get; set; }             
            
            public PositionState Stone { get; set; }
            /// <summary>
            /// Initializes a new instance of the <see cref="Player{T}"/> class.
            /// </summary>
            /// <param name="name">Name of the player.</param>
            /// <param name="stone">Color of the player's stone.</param>
            /// <exception cref="ArgumentException"></exception>
            public Player(string name, PositionState stone)
            {
                try
                {
                    Name = name;
                    Stone = stone;
                }
                catch
                {
                    Console.WriteLine("Invalid argument.");
                    throw new ArgumentException("Invalid argument.");
                }
            }
            /// <summary>
            /// Returns string of players name and stone color.
            /// </summary>
            /// <returns>A string containing players name and stone color.</returns>
            public override string ToString()
            {
                return $"Player name: {Name}, Stone Color: {Stone}";
            }
        }
        /// <summary>
        /// Board class that holds the state of the board and provides a
        /// functions for making moves and also a function for converting colors
        /// to x's and o's.
        /// </summary>
        class Board
        {
            public PositionState[,] State { get; set; }
            /// <summary>
            /// Initializes a new instance of the <see cref="Board{T}"/> class.
            /// </summary>
            public Board()
            {
                State = new PositionState[9, 9];
                for (var row = 0; row < 9; row++)
                {
                    for (var col = 0; col < 9; col++)
                    {
                        State[row, col] = PositionState.Empty;
                    }
                }
            }
            /// <summary>
            /// A function for determining if a move is valid.
            /// </summary>
            /// <param name="row">Chosen row.</param>
            /// <param name="col">Chosen column.</param>
            /// <param name="player">The player who has current turn.</param>
            /// <returns>Returns true if the move is valid. Otherwise returns false.</returns>
            public bool MakeMove(int row, int col, Player player)
            {      
                bool success = true;
                try
                {
                    var pos = State[row, col];
                    if (pos == PositionState.Empty)
                        State[row, col] = player.Stone;
                    else
                        throw new Exception("A stone is already at that location.");
                }
                catch {
                    Console.WriteLine("Invalid Placement, try again.");
                    success = false;
                }
                return success;
            }
            /// <summary>
            /// A function for converting stone color to 'x' or 'o'.
            /// </summary>
            /// <param name="color">The color of the stone to be converted.</param>
            /// <returns>Returns a string of either " ", "x", or "o".</returns>
            public string ConvertToXandO(PositionState color)
            {
                if (color == PositionState.Empty)
                {
                    return " ";
                }
                else if (color == PositionState.White)
                {
                    return "x";
                }
                else
                    return "o";
            }
            /// <summary>
            /// Returns the state of the board.
            /// </summary>
            /// <returns>Returns a string representing the current state of the board.</returns>
            public override string ToString()
            {
                var count = 0;
                string board = "";
                board += "   -----------------\n";
                board += "   0 1 2 3 4 5 6 7 8\n";
                for (int i = 0; i < 19; i++)
                {
                    if (i % 2 == 0)
                        board += "  +-+-+-+-+-+-+-+-+-+\n";
                    else
                    {
                        //count is going too high
                        board += $"{count}:";
                        for (int j = 0; j < 9; j++)
                        {
                            board += $"|{ConvertToXandO(State[count, j])}";
                        }
                        count++;
                       board += "|\n";
                    }
                }
                board += "   0 1 2 3 4 5 6 7 8\n";
                return board;
            }
        }
        private Player _p1;
        private Player _p2;
        private Board _board;
        /// <summary>
        /// Initalizes a new instance of the <see cref="GomokuGame"/> class.
        /// </summary>
        /// <param name="p1">Player one.</param>
        /// <param name="p2">Player two.</param>
        public GomokuGame(Player p1, Player p2)
        {
            _p1 = p1;
            _p2 = p2;
            _board = new Board();
        }
        /// <summary>
        /// Handles the entire game.
        /// </summary>
        /// <returns>Once game is finished, returns whether there is a winner
        /// or a draw.</returns>
        public void Play()
        {
            var count = 0;
            while (true) {
                ChoosePlacement(_p1);
                count++;
                if(WinCheck(_p1))
                {
                    Console.WriteLine(_board);
                    Console.WriteLine($"\n------------{_p1.Name} has won!------------");
                    break;
                }
                else if(count >= 81){
                    Console.WriteLine(_board);
                    Console.WriteLine("------------The Game is a Draw!------------");
                    break;
                }
                ChoosePlacement(_p2);
                count++;
                if (WinCheck(_p2))
                {
                    Console.WriteLine(_board);
                    Console.WriteLine($"\n------------{_p2.Name} has won!------------");
                    break;
                }
                else if (count >= 81)
                {
                    Console.WriteLine(_board);
                    Console.WriteLine("------------The Game is a Draw!------------");
                    break;
                }

            }
        }
        /// <summary>
        /// Requests row and column placement from the player until they make a valid move.
        /// </summary>
        /// <param name="player">The current player.</param>
        public void ChoosePlacement(Player player)
        {
            var row = 0;
            var col = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"{player}");
                Console.WriteLine($"Stone of {player.Name} is depicted as '{_board.ConvertToXandO(player.Stone)}'");
                Console.WriteLine(_board);
                Console.Write($"{player.Name}, choose the row of your next move: ");
                _ = int.TryParse(Console.ReadLine(), out row);
                Console.Write($"{player.Name}, choose the column of your next move: ");
                _ = int.TryParse(Console.ReadLine(), out col);
                while (true)
                {
                    if (_board.MakeMove(row, col, player))
                    {           
                        break;
                    }
                    else
                    {
                        Console.Write($"\n{player.Name}, choose the row of your next move: ");
                        _ = int.TryParse(Console.ReadLine(), out row);
                        Console.Write($"{player.Name}, choose the column of your next move: ");
                        _ = int.TryParse(Console.ReadLine(), out col);
                    }

                }
                break;
            }
        }
        /// <summary>
        /// Checks if the last move made by a player results in a win.
        /// </summary>
        /// <param name="player">The current player.</param>
        /// <returns>True if the last move results in a win. False otherwise.</returns>
        public bool WinCheck(Player player)
        {
            // Return variable.
            bool win = true;
            // Used for counting number of marks in a row.
            var count = 0;
            // Check for horizontal victory.
            while (true)
            {
                for(int i =0; i < 9; i++)
                {
                    for(int j = 0; j < 9; j++)
                    {
                        if (_board.State[i, j].ToString() == player.Stone.ToString())
                        {
                            count++;
                            if(count >= 5)
                            {
                                win = true;
                                return win;
                            }
                        }
                        else
                        {
                            count = 0;
                        }
                        
                    }
                }
                // Check for vertical victory.
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (_board.State[j, i].ToString() == player.Stone.ToString())
                        {
                            count++;
                            if (count >= 5)
                            {
                                win = true;
                                return win;
                            }
                        }
                        else
                        {
                            count = 0;
                        }

                    }
                }
                // Check for upper-left to lower-right diagonal victory.
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        int k = i;
                        int l = j;
                        for (; k < 9 && l < 9; k++, l++)
                        {

                            if (_board.State[k, l].ToString() == player.Stone.ToString())
                            {
                                count++;
                                if (count >= 5)
                                {
                                    win = true;
                                    return win;
                                }
                            }
                            else
                            {
                                count = 0;
                            }
                        }

                    }
                }
                // Check for upper-right to lower-left diagonal victory
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 8; j >= 0; j--)
                    {
                        int k = i;
                        int l = j;
                        for (; k < 9 && l >= 0; k++, l--)
                        {

                            if (_board.State[k, l].ToString() == player.Stone.ToString())
                            {
                                count++;
                                if (count >= 5)
                                {
                                    win = true;
                                    return win;
                                }
                            }
                            else
                            {
                                count = 0;
                            }
                        }

                    }
                }
               
                break;
            }
            return false;
        }
        /// <summary>
        /// Uses a local function to welcome the players, and another local function to get their names.
        /// Afterwards, it starts the game by calling Play().
        /// </summary>
        static void Main()
        {
            Welcome();
            Player p1 = new(GetName(1), PositionState.Black);
            Player p2 = new(GetName(2), PositionState.White);
            GomokuGame game = new(p1, p2);
            game.Play();

            ///<summary>Requests the names of the players.</summary>
            ///<returns>Returns a string containing their names.</returns>
            string GetName(int i)
            {
                var name = "";
                while (string.IsNullOrEmpty(name))
                {
                    Console.Write($"Please enter the name of Player {i}: ");
                    name = Console.ReadLine();
                    if(name is not null)
                        name = name.Trim();
                }
                return name;
            }
            ///<summary>Displays a welcome message.</summary>
            void Welcome()
            {
                Console.WriteLine("\t\tWelcome to Gomoku!\n" +
                    "A game much like tictac toe, only with a larger game board\n" +
                    "and a requirement of 5 stones(marks) in a row, rather than 3. " +
                    "\n\n\t\t     Enjoy!\n\n\n");
            }
        }
    }
}