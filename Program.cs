using System;
using System.Threading.Tasks;

public class Pradbird
{
    static int jump_value = 5;
    static int pipe_position = 1;
    static game_state state = game_state.playing;
    static char pipe = '░';

    static bool[,] pipe_exsistence = new bool[10,10];

    static float total_score = 0f;

    static async Task Main()
    {
        
        Pradbird game = new Pradbird(); // New instance of a game
        pipe_exsistence = new bool[Console.BufferWidth, Console.BufferHeight];

        for (;;)
        {
            Console.Clear();
            await game.game_loop();
            await game.gravity();
            if (state == game_state.over)
                break;
        }

        Console.SetCursorPosition(0, Console.CursorTop + 1);
        Console.WriteLine("Press X to quit");

        if (Console.ReadKey(true).Key == ConsoleKey.X)
        {
            Environment.Exit(0); // Safe exit
        }
    }

    async Task game_loop()
    {
        Console.SetCursorPosition(0, 0);
        string score_msg = "Score : " + (int)total_score;
        Console.Write(score_msg);
        for (int i = score_msg.Length; i < Console.BufferWidth; i++)
        {
            Console.Write('═');
        }

        draw_bird();

        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)
            jump_value -= 2;

        draw_pipes(); // Draw multiple pipes

        pipe_position += 2;

        Console.SetCursorPosition(0, Console.BufferHeight);
        for (int i = 0; i < Console.BufferWidth; i++)
        {
            Console.Write('═');
        }

        await Task.Delay(50);
    }

    async Task gravity()
    {
        if (jump_value < Console.BufferHeight)
            jump_value++;

        if (jump_value >= Console.BufferHeight)
            quit("You hit the floor.");

        await Task.Delay(150);
    }

    void draw_bird()
    {
        if (jump_value == 0)
            quit("You hit the roof.");

        Console.SetCursorPosition(2, jump_value);

        if (pipe_exsistence[2, jump_value] == true)
            quit("You hit the block!");

        Console.Write("🐦");
    }

    void quit(string message)
    {
        Console.Clear();
        Console.Write("\n\n\n\n\n");

        string s = "Oops.. Game Over! | You died!";
        Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
        Console.WriteLine(s);

        Console.SetCursorPosition((Console.WindowWidth - message.Length) / 2, Console.CursorTop);
        Console.WriteLine(message);

        state = game_state.over;
    }

    void draw_pipes()
    {
        Random random = new Random();
        int pipeCount = 2;
        int minPipeSpacing = 10;

        int top = random.Next(0, Console.BufferHeight);
        for (int i = 0; i < pipeCount; i++)
        {
            
            int xPosition = Console.BufferWidth - pipe_position - i * (minPipeSpacing + 1);
            
            Console.SetCursorPosition(xPosition, top);
            pipe_exsistence[xPosition, top] = true;
            pipe_exsistence[xPosition - 1, top] = false;

            Console.Write(pipe);

            if(xPosition <= 1)
                pipe_position = 1;
        }
        total_score += 0.3f;
    }

    void halt() // for debugging use!!
    {
        for (; ; ) ;
    }
}