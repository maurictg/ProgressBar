using System;

public class ProgressBar
    {
        public int Blocks = 20,
            Max = 100,
            Min = 0,
            PositionX,
            PositionY;

        long prevTicks;
        long averageTicks;
        long updates = 0;
        long totalTicks = 0;

        string Message;
        char Symbol = '=';

        private int progress;
        public int Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                PreUpdate();
            }
        }

        System.Timers.Timer timer = new System.Timers.Timer(500);


        public ProgressBar(int max = 100, int min = 0, string message = "Loading...", int blocks = 20)
        {
            Max = max;
            Min = min;
            Message = message;
            Blocks = blocks;
            prevTicks = 0;
            timer.Start();
            timer.Elapsed += delegate { Update(); };
        }

        private void PreUpdate()
        {
            updates++;
            long ticks = DateTime.UtcNow.Ticks;
            //Algoritme 

            prevTicks = (prevTicks < 1) ? ticks : prevTicks;

            long elapsedTicks = ticks - prevTicks;
            totalTicks += elapsedTicks;
            averageTicks = (totalTicks + elapsedTicks) / updates;
        }

        private void Update()
        {
            decimal c = (decimal)progress;
            decimal t = (decimal)Max;
            decimal percentage = (c / t) * 100;
            int blocks = Convert.ToInt32((c/t)*Blocks);

            long ticks = DateTimeOffset.UtcNow.Ticks;

            long ticksNeeded = averageTicks * Max;
            long ticksLeft = ticksNeeded - totalTicks;
            TimeSpan timeLeft = TimeSpan.FromTicks(ticksLeft);
            //hoe convert je deze naar x minuten en x seconde, of wanneer het minder dan een minuut is naar x seconde

            int hours = (timeLeft.TotalMinutes > 60) ? timeLeft.Hours : 0; 
            int minutes = (timeLeft.TotalSeconds > 60) ? timeLeft.Minutes : 0;
            int seconds = timeLeft.Seconds;

            string timestring = $"{hours.ToString().PadLeft(hours.ToString().Length,'0')}:{minutes.ToString().PadLeft(minutes.ToString().Length, '0')}:{seconds.ToString().PadLeft(seconds.ToString().Length, '0')}";

            Console.SetCursorPosition(PositionX, PositionY);

            string progressBar = $"{Message} [{new string(Symbol, blocks).PadRight(Blocks,' ')}] ({percentage.ToString("0.0")}%) [{timestring} left]";
            Console.WriteLine(progressBar);

            prevTicks = ticks;

            if (progress >= Max)
            {
                timer.Stop();
                Console.WriteLine("Done!");
            }
                
        }
    }