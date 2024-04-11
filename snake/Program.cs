using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class Program
    {

        static int width = 60;
        static int height = 25;
        static int score = 0;
        static int delay = 100;
        static bool gameOver = false;
        static Random random = new Random();
        static string playerName = ""; // Variable para almacenar el nombre del jugador


        static List<int[]> snake = new List<int[]>();
        static List<int[]> mines = new List<int[]>(); // Lista de minas
        static int[] food = new int[2];

        static int dx = 1; // Dirección X inicial
        static int dy = 0; // Dirección Y inicial

        static ConsoleColor originalForegroundColor; // Almacena el color de primer plano original
        static ConsoleColor originalBackgroundColor; // Almacena el color de fondo original

        static void Main(string[] args)
        {
            int totalWidth = width + 10; // Ajusta según sea necesario

            // Verificamos si el ancho total requerido es mayor que el ancho actual de la consola
            if (totalWidth > Console.WindowWidth)
            {
                // Si es mayor, ajustamos el ancho de la consola
                Console.WindowWidth = totalWidth;
            }

            Console.Title = "Snake Game";
            Console.CursorVisible = false;
            Console.SetWindowSize(width + 1, height + 2);
            Console.SetBufferSize(width + 1, height + 2);
            originalForegroundColor = Console.ForegroundColor;
            originalBackgroundColor = Console.BackgroundColor;

            Console.WriteLine("Ingrese su nombre:");
            playerName = Console.ReadLine();
            char playAgainInput;
           
                StartGame();
               
           
        }
        static void StartGame()
        {
            Console.Clear(); // Limpiar la pantalla antes de iniciar el juego

            // Calcular la posición X para el nombre del jugador
            int playerNameX = Console.WindowWidth - playerName.Length - 50;

            


            int totalWidth = width + 10; // Ajusta según sea necesario

            // Verificamos si el ancho total requerido es mayor que el ancho actual de la consola
            if (totalWidth > Console.WindowWidth)
            {
                // Si es mayor, ajustamos el ancho de la consola
                Console.WindowWidth = totalWidth;
            }
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.Clear();

         

            //        int scoreX = width + 2; // Posición X del puntaje
            //        int scoreY = height / 2; // Posición Y del puntaje
            //         // Verificamos si la posición X del puntaje excede el ancho de la consola y ajustamos si es necesario

            //        if (scoreX >= Console.WindowWidth)
            //{
            //    scoreX = Console.WindowWidth - 10; // Ajusta según la longitud del puntaje y la posición deseada
            //}

            //Console.SetCursorPosition(scoreX, scoreY);


            Console.WriteLine();

            Console.WriteLine("===================================");
            Console.WriteLine("     BIENVENIDO A SNAKE     ");
            Console.WriteLine("        "+ playerName);
           Console.WriteLine("===================================");
            Console.WriteLine();


            string snakeArt = @"

           /^\/^\
         _|__|  O|
\/     /~     \_/ \
 \____|__________/  \
        \_______      \
                `\     \                 \
                  |     |                  \
                 /      /                    \
                /     /                       \\
              /      /                         \ \
             /     /                            \  \
           /     /             _----_            \   \
          /     /           _-~      ~-_         |   |
         (      (        _-~    _--_    ~-_     _/   |
          \      ~-____-~    _-~    ~-_    ~-_-~    /
            ~-_           _-~          ~-_       _-~
               ~--______-~                ~-___-~ ";
            Console.WriteLine(snakeArt);

            Console.WriteLine();






            Console.Write("Presiona Enter para Jugar");
            //Console.SetCursorPosition(width / 2 - 5, height / 2);
         
            Console.ReadKey();
            Console.Clear();


            DrawScore(); // Dibujar el puntaje inicial
           
            DrawGameArea();
            DrawBorder();
            DrawFood();
            DrawSnake();
            DrawMines(); // Dibujar las minas

            Thread inputThread = new Thread(ReadInput);
            inputThread.Start();

            while (!gameOver)
            {
                MoveSnake();
               NomUser();
               
                if (IsEatingFood())
                {
                    score++;
                    DrawFood();
                    DrawScore(); // Actualiza el puntaje
                }
                Thread.Sleep(delay);
            }
            Console.SetCursorPosition(width / 2 - 5, height / 2);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("Game Over!");
            Console.SetCursorPosition(width / 2 - 8, height / 2 + 1);
            Console.Write($"Score: {score}");
            Console.SetCursorPosition(0, height + 1);

        }
        static void NomUser()
        {

            // Almacenar el color de primer plano y de fondo original
            ConsoleColor previousForegroundColor = Console.ForegroundColor;
            ConsoleColor previousBackgroundColor = Console.BackgroundColor;

            // Establecer el color de primer plano y de fondo para el puntaje
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;

            // Calcular la posición X para el nombre del jugador
            int playerNameX = Console.WindowWidth - playerName.Length -50;

            // Dibujar el nombre del jugador
            Console.SetCursorPosition(playerNameX, 0);
            Console.ForegroundColor = ConsoleColor.Black; //COLOR DEL NOMBRE
        
            Console.Write(playerName);
            // Establecer el color de primer plano y de fondo para el puntaje



            // Restaurar el color de primer plano y de fondo original
            Console.ForegroundColor = previousForegroundColor;
            Console.BackgroundColor = previousBackgroundColor;
        }





        static void DrawScore()
        {
           
                // Almacenar el color de primer plano y de fondo original
                ConsoleColor previousForegroundColor = Console.ForegroundColor;
                ConsoleColor previousBackgroundColor = Console.BackgroundColor;

                // Establecer el color de primer plano y de fondo para el puntaje
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Cyan;

                // Establecer la posición donde se dibujará el puntaje
                int scoreX = width + 2; // Ajusta según tus necesidades
                int scoreY = height / 2; // Ajusta según tus necesidades o puedes elegir otra posición

                // Verificar si scoreX excede el ancho de la consola y ajustar si es necesario
                if (scoreX >= Console.WindowWidth)
                {
                    scoreX = Console.WindowWidth - 10; // Ajusta según la longitud del puntaje y la posición deseada
                }

                // Limpiar la línea donde se mostrará el puntaje
                Console.SetCursorPosition(scoreX, scoreY);
                Console.Write(new string(' ', 10)); // Limpia 10 espacios para asegurarse de borrar el puntaje anterior

                // Dibujar el puntaje actualizado
                Console.SetCursorPosition(scoreX, scoreY);
                Console.Write($"Score: {score}");

                // Restaurar el color de primer plano y de fondo original
                Console.ForegroundColor = previousForegroundColor;
                Console.BackgroundColor = previousBackgroundColor;
        }

        static void DrawGameArea()
        {
         

            // Dibuja el fondo dentro del área de juego
            Console.BackgroundColor = ConsoleColor.Yellow;
            for (int y = 1; y <= height; y++)
            {
                for (int x = 1; x <= width; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }

        
            // Dibuja los bordes
            for (int i = 0; i < width + 2; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("▓");
                Console.SetCursorPosition(i, height + 1);
                Console.Write("▓");
            }

            for (int i = 1; i < height + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("▓");
                Console.SetCursorPosition(width + 1, i);
                Console.Write("▓");
            }
        }
        static void DrawBorder()
        {

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.BackgroundColor = ConsoleColor.Yellow;

            // Draw horizontal borders
            for (int i = 0; i < width + 2; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("▓");
                Console.SetCursorPosition(i, height + 1);
                Console.Write("▓");
            }

            // Draw vertical borders
            for (int i = 1; i < height + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("▓");
                Console.SetCursorPosition(width + 1, i);
                Console.Write("▓");
            }
        }

        static void DrawFood()
        {
            food[0] = random.Next(1, width);
            food[1] = random.Next(1, height);

            Console.SetCursorPosition(food[0], food[1]);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("$");
        }

        static void DrawSnake()
        {
            snake.Clear();
            snake.Add(new int[] { width / 2, height / 2 });
            Console.SetCursorPosition(snake[0][0], snake[0][1]);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("▀");
        }
        static void DrawMines()
        {
            // Dibujar 5 minas aleatorias
            for (int i = 0; i < 15; i++)
            {
                int[] mine = { random.Next(1, width), random.Next(1, height) };
                mines.Add(mine);
                Console.SetCursorPosition(mine[0], mine[1]);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("X");
            }
        }
        static void MoveSnake()
        {
            int[] newHead = { snake[0][0] + dx, snake[0][1] + dy };

            // Verificar si la nueva cabeza colisiona con el cuerpo de la serpiente
            for (int i = 1; i < snake.Count; i++)
            {
                if (newHead[0] == snake[i][0] && newHead[1] == snake[i][1])
                {
                    gameOver = true;
                    return;
                }
            }
            // Verificar si la nueva cabeza colisiona con una mina
            foreach (var mine in mines)
            {
                if (newHead[0] == mine[0] && newHead[1] == mine[1])
                {
                    gameOver = true;
                    return;
                }
            }


            if (newHead[0] <= 0 || newHead[0] >= width + 1 || newHead[1] <= 0 || newHead[1] >= height + 1)
            {
                gameOver = true;
                return;
            }

            Console.SetCursorPosition(snake[snake.Count - 1][0], snake[snake.Count - 1][1]);
            Console.Write(" ");
            snake.RemoveAt(snake.Count - 1);

            snake.Insert(0, newHead);
            Console.SetCursorPosition(newHead[0], newHead[1]);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("▀");
        }

        static bool IsEatingFood()
        {
            if (snake[0][0] == food[0] && snake[0][1] == food[1])
            {
                snake.Add(new int[] { food[0], food[1] });
                return true;
            }
            return false;
        }

        static void ReadInput()
        {
            while (!gameOver)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        dx = 0;
                        dy = -1;
                        break;
                    case ConsoleKey.DownArrow:
                        dx = 0;
                        dy = 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        dx = -1;
                        dy = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        dx = 1;
                        dy = 0;
                        break;
                    default:
                        gameOver = true;
                        break;
                }
            }
        }
    }
}

