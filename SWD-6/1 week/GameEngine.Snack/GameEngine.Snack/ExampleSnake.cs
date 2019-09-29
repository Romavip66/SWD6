using PixelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameEngine.Snack.Extentions;

namespace GameEngine.Snack {
	public class SnakeGame: Game {
		private List snake; 
		private int foodX;
		private int foodY;
		public string Name => "Rymzhan Bekeshov csse-1604k";
		
		private int score;
		private int dir;
		private bool dead;
		private bool started;
		private ISnake snakeCharacter;
		

		static void Main(string[] args) {

			SnakeGame s = new SnakeGame();
			s.Construct(50, 50, 10, 10, 30);

			s.Start();
		}

		private struct SnakeSegment {
			public SnakeSegment(int x, int y) : this() {
				this.X = x;
				this.Y = y;
			}

			public int X {
				get;
				private set;
			}
			public int Y {
				get;
				private set;
			}
		}

		public SnakeGame() {
			AppName = "SNAKE!";
			snakeCharacter = new ExampleSnake();
		}

		public override void OnCreate() {
			Enable(Subsystem.HrText);

			Reset();
		}

		private void DrawGame() {
			Clear(Pixel.Presets.Black);

			if (started)
			DrawTextHr(new Point(15, 15), snakeCharacter.Name + " score: " + score, Pixel.Presets.Green, 2);
			else
			DrawTextHr(new Point(15, 15), "Press Enter To Start", Pixel.Presets.Green, 2);
			DrawRect(new Point(0, 0), ScreenWidth - 1, ScreenHeight - 1, Pixel.Presets.Grey);
			for (int i = 1; i < snake.Count; i++)
			Draw(snake[i].X, snake[i].Y, dead ? Pixel.Presets.Blue: Pixel.Presets.Yellow);
			Draw(snake[0].X, snake[0].Y, dead ? Pixel.Presets.Green: Pixel.Presets.Magenta);
			Draw(foodX, foodY, Pixel.Presets.Green);
		}
		
		public override void OnUpdate(float elapsed) {
			snakeCharacter.UpdateMap(GetMap());

			CheckStart();
			UpdateSnake();
			DrawGame();

			Thread.Sleep(20);
		}
		
		private void Reset() {
			snake = new List();
			for (int i = 0; i < 9; i++)
			snake.Add(new SnakeSegment(i + 20, 15));

			foodX = 30;
			foodY = 15;
			dir = 3;
			dead = false;

			Seed();
		}

		private void CheckCollision() {
			if (snake[0].X == foodX && snake[0].Y == foodY) {
				score += Math.Max(1, snake.Count / 3);
				RandomizeFood();

				snake.Add(new SnakeSegment(snake[snake.Count - 1].X, snake[snake.Count - 1].Y));
			}

			if (snake[0].X <= 0 || snake[0].X >= ScreenWidth || snake[0].Y <= 0 || snake[0].Y >= ScreenHeight - 1) dead = true;

			for (int i = 1; i < snake.Count; i++)
			if (snake[i].X == snake[0].X && snake[i].Y == snake[0].Y) dead = true;
		}

		private void CheckStart() {
			if (!started) {
				{
					Reset();
					started = true;
				}
			}
		}

		private void UpdateSnake() {

			if (dead) {
				started = false;
			}
			var direction = (SnakeDirection) dir;
			var action = snakeCharacter.GetNextDirection(direction);

			if ((action == SnakeDirection.Down && direction == SnakeDirection.Up) || (action == SnakeDirection.Up && direction == SnakeDirection.Down) || (action == SnakeDirection.Left && direction == SnakeDirection.Right) || (action == SnakeDirection.Right && direction == SnakeDirection.Left)) {
			}
			else {
				dir = (int) action;
			}
			if (snake[0].X == foodX && snake[0].Y != (foodY)) {
				if (foodY - snake[0].Y > 0) {
					dir = 2;
				}
				else {
					dir = 0;
				}
			}
			else if (snake[0].Y == foodY && snake[0].X != (foodX)) {
				if (foodX - snake[0].X > 0) {
					dir = 3;
				}
				else {
					dir = 1;
				}
			}

			if (snake[0].Y == 2 || snake[0].X == 2) {
				dir = 2;
			} else if (snake[0].Y == 47 || snake[0].X == 47) {
				dir = 1;
			}

			if (started) {
				switch (dir) {
				case 0:
					snake.Insert(0, new SnakeSegment(snake[0].X, snake[0].Y - 1));
					break;
				case 1:
					snake.Insert(0, new SnakeSegment(snake[0].X + 1, snake[0].Y));
					break;
				case 2:
					snake.Insert(0, new SnakeSegment(snake[0].X, snake[0].Y + 1));
					break;
				case 3:
					snake.Insert(0, new SnakeSegment(snake[0].X - 1, snake[0].Y));
					break;
				}

				snake.RemoveAt(snake.Count - 1);

				CheckCollision();
			}
		}
		
		private void RandomizeFood() {
			while (GetScreenPixel(foodX, foodY) != Pixel.Presets.Black) {
				foodX = Random(ScreenWidth);
				foodY = Random(ScreenHeight);
			}
		}

		{

			var screen = this.GetScreen();

			var mapBuilder = new StringBuilder();

			for (int i = 0; i < screen.GetLength(0); i++) {
				Console.WriteLine("");

				for (int j = 0; j < screen.GetLength(1); j++) {
					var pixel = screen[j, i];
					if (pixel.isWall()) mapBuilder.Append("x");

					else if (pixel.isEmpty()) mapBuilder.Append(" ");

					else if (pixel.isHead()) mapBuilder.Append("*");

					else if (pixel.isBody()) mapBuilder.Append("1");

					else if (pixel.isFood()) mapBuilder.Append("7");

					else if (pixel.isTrap()) mapBuilder.Append("6");

				}
			}

			return mapBuilder.ToString();
		}
	}

}