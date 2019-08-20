using System;
using System.Diagnostics;

namespace ProgressBar
{
    class ProgressBar
    {
        public int BlocksCount = 50,
            Max, Min,
            PositionX, PositionY;

        public char Symbol = '\u25A0';
        private int progress;
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                Update();
            }
        }

        private int lastProgress;
        private decimal average;
        private Stopwatch stopwatch;

        public ProgressBar(int max = 100, int min = 0, int blocksCount = 50, int positionX = 0, int positionY = 0)
        {
            Max = max;
            Min = min;
            BlocksCount = blocksCount;
            PositionX = positionX;
            PositionY = positionY;

            lastProgress = min;
            stopwatch = Stopwatch.StartNew();
        }

        private void Update(bool done = false, bool success = true)
        {
            var percentage = (decimal)progress / Max * 100;
            int blocks = (int)((decimal)progress / Max * BlocksCount);

            if (stopwatch.ElapsedMilliseconds >= 1000 || average == 0)
            {
                average = (average + (decimal)(progress - lastProgress) / stopwatch.ElapsedMilliseconds) / 2;
                lastProgress = progress;
                stopwatch.Restart();
            }
            TimeSpan timeLeft = TimeSpan.FromMilliseconds((int)((Max - progress) / average));

            string progressBar = $"[{new string(Symbol, blocks).PadRight(BlocksCount, ' ')}] ({percentage.ToString("0.0")}%) [{string.Format("{0:h\\:m\\:ss}", timeLeft)}]";
            Console.SetCursorPosition(PositionX, PositionY);
            Console.Write(progressBar);

            if (done)
            {
                Console.Write(" [");
                Console.ForegroundColor = (success) ? ConsoleColor.Green : ConsoleColor.Red;
                Console.Write((success) ? "Done" : "Failed");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("]\n");
            }
            else
                Console.Write("\n");
        }

        public void Done() =>
            Update(true);
        public void Fail() =>
            Update(true, false);
    }
}
