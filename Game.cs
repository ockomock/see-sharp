using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Chess
{
    class Game
    {
        private Player p1, p2, activePlayer;
        Board board;
        int mode;
        int turnCounter;
        private FileSystemWatcher watcher;
        private System.IO.FileInfo file;

        public Game(int gameMode, ref Board b, int turn = 1)
        {
            createFileWatcher();
            file = new System.IO.FileInfo("board.xml");

            board = b;
            mode = gameMode;
            turnCounter = turn;

            switch (gameMode)
            {
                case 1:
                    p1 = new HumanPlayer(Color.WHITE, this);
                    p2 = new AIPlayer(Color.BLACK, this);
                    break;
                case 2:
                    p1 = new HumanPlayer(Color.WHITE, this);
                    p2 = new HumanPlayer(Color.BLACK, this);
                    break;
                case 3:
                    p1 = new AIPlayer(Color.WHITE, this);
                    p2 = new AIPlayer(Color.BLACK, this);                   
                    break;
            }

            if (turnCounter == 1)
                activePlayer = p1;
            else if (turnCounter == -1)
                activePlayer = p2;
        }

        public void changeTurn(Board board)
        {
           
            if (board.validKings())
            {
                // Is it checkmate?
                if (board.checkMate(activePlayer.getColor() == Color.BLACK ? Color.WHITE : Color.BLACK))
                {
                    Console.WriteLine("Check mate! :)");
                    activePlayer = null;
                }
                else
                {
                    turnCounter *= -1;

                    if (turnCounter == 1)
                        activePlayer = p1;
                    else
                        activePlayer = p2;
                }
            }
            else
            {
                activePlayer = null;
            }
            // Save game state to file
            stopWatcher();
            board.saveToFile("board.xml", mode, turnCounter);
            startWatcher();
        }

        public void handleClick(Point grid, ref Board b, ref Graphics g)
        {
            if (activePlayer != null)
            {
                activePlayer.handleClick(grid, ref b, ref g);
                if (mode == 1 && activePlayer != null)
                    activePlayer.performMove(ref b);
            }
            b.Draw(g);            
        }      

        public void performActivePlayerMove(Board b, Graphics g)
        {       
            if (activePlayer != null)
            {
                activePlayer.performMove(ref b);
            }
            b.Draw(g);
        }      
        
        public void createFileWatcher()
        {
            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher();
            watcher.Path = ".";
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastWrite;              
            // Only watch text files.
            watcher.Filter = "*.xml";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        public void startWatcher()
        {
            file = new System.IO.FileInfo(".");
            watcher.EnableRaisingEvents = true;
        }

        public void stopWatcher()
        {
            watcher.EnableRaisingEvents = false;
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            System.IO.FileInfo tmp = new System.IO.FileInfo("board.xml");

            // This will make sure the file only gets loaded when the saving is done
            if (tmp.LastWriteTime != file.LastWriteTime)
            {
                // Specify what is done when a file is changed, created, or deleted.
                Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

                int c = 0;
                //Thread.Sleep(1000); // Needed to not start loading when reading to the file!!!
                board.loadFromFile("board.xml", ref c);
                file = new System.IO.FileInfo(".");
            }
        }
    }
}
