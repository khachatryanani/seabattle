using System;

namespace SeaBattleGame
{
    static class Program
    {
        static void Main()
        {
            User user = new User("Player 1");
            Poseidon poseidon = new Poseidon("Player 2");

            GameManager game = new GameManager(user, poseidon);
            game.Run();
        }
    }
}
