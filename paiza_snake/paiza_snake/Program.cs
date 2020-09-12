using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace paiza_snake
{
    class Program
    {
        private static int commandLineCount = 1;
        private static string GetLine(string[] cmd)
        {
            var returnValue = String.Empty;
#if DEBUG
            returnValue = cmd[commandLineCount].Replace("\r", "").Replace("\n", "");
            commandLineCount++;
#else
        returnValue = System.Console.ReadLine();
#endif
            return returnValue;
        }
        /// <summary>
        /// 方向
        /// </summary>
        private enum Direction
        {
            North,
            East,
            South,
            West,
        }
        static void Main(string[] args)
        {
            var cmd = System.Environment.GetCommandLineArgs();
            var line = GetLine(cmd).Split(' ');

            int h = int.Parse(line[0]);
            int w = int.Parse(line[1]);
            int currentY = int.Parse(line[2]);
            int currentX = int.Parse(line[3]);
            int actionCount = int.Parse(line[4]);

            //マップの展開
            var map = CreateMap(cmd, w, h);
            //初期値のマーク
            map[currentX, currentY] = "*";

            //方向転換分の移動
            var currentDirection = Direction.North;
            int totalTime = 0;
            bool isStop = false;
            for (int i = 0; i < actionCount; i++)
            {
                line = GetLine(cmd).Split(' ');
                int nextTime = int.Parse(line[0]);

                while (totalTime < nextTime)
                {
                    if (!Move(map, currentDirection, ref totalTime, ref currentX, ref currentY, ref isStop))
                    {
                        if (isStop) break;
                        continue;
                    }
                }

                if (isStop) break;

                //方向転換
                string nextDirection = line[1];
                currentDirection = ChangeDirection(currentDirection, nextDirection == "R");

            }

            //方向転換終了後の直進
            while (!isStop)
            {
                if (!Move(map, currentDirection, ref totalTime, ref currentX, ref currentY, ref isStop))
                {
                    break;
                }
            }

            //出力
            for (int y = 0; y < h; y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < w; x++)
                {
                    sb.Append(map[x, y]);
                }
                System.Console.WriteLine(sb.ToString());
            }
        }
        /// <summary>
        /// マップ作製
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private static string[,] CreateMap(string[] cmd, int w, int h)
        {
            var map = new string[w, h];

            //マップの展開
            for (int y = 0; y < h; y++)
            {
                var splitValue = GetLine(cmd).ToCharArray();
                for (int x = 0; x < w; x++)
                {
                    map[x, y] = splitValue[x].ToString();
                }
            }

            return map;
        }
        /// <summary>
        /// /移動
        /// </summary>
        /// <param name="map"></param>
        /// <param name="currentDirection"></param>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        /// <returns></returns>
        private static bool Move(string[,] map, Direction currentDirection, ref int totalTime, ref int currentX, ref int currentY, ref bool isStop)
        {
            if (totalTime == 100)
            {
                isStop = true;
                return false;
            }
            totalTime++;

            int moveX = 0;
            int moveY = 0;

            //動く先の座標を特定
            switch (currentDirection)
            {
                case Direction.North: moveY = -1; break;
                case Direction.East: moveX = 1; break;
                case Direction.South: moveY = 1; break;
                case Direction.West: moveX = -1; break;
            }

            int tempDestinationX = currentX + moveX;
            int tempDestinationY = currentY + moveY;

            //移動不可なマスのチェック
            if (tempDestinationX < 0
                || tempDestinationY < 0
                || tempDestinationX == map.GetLength(0)
                || tempDestinationY == map.GetLength(1))
            {
                isStop = true;
                return false;
            }

            if (map[tempDestinationX, tempDestinationY] == "#")
            {
                return false;
            }
            if (map[tempDestinationX, tempDestinationY] == "*")
            {
                isStop = true;
                return false;
            }

            //移動
            map[tempDestinationX, tempDestinationY] = "*";
            currentX = tempDestinationX;
            currentY = tempDestinationY;


            return true;
        }
        /// <summary>
        /// 方向転換
        /// </summary>
        /// <param name="currentDirection"></param>
        /// <param name="isMoveRight"></param>
        /// <returns></returns>
        private static Direction ChangeDirection(Direction currentDirection, bool isMoveRight)
        {
            var tempDirection = currentDirection;

            switch (currentDirection)
            {
                case Direction.North:
                    if (isMoveRight)
                    {
                        tempDirection = Direction.East;
                    }
                    else
                    {
                        tempDirection = Direction.West;
                    }
                    break;
                case Direction.East:
                    if (isMoveRight)
                    {
                        tempDirection = Direction.South;
                    }
                    else
                    {
                        tempDirection = Direction.North;
                    }
                    break;
                case Direction.South:
                    if (isMoveRight)
                    {
                        tempDirection = Direction.West;
                    }
                    else
                    {
                        tempDirection = Direction.East;
                    }
                    break;
                case Direction.West:
                    if (isMoveRight)
                    {
                        tempDirection = Direction.North;
                    }
                    else
                    {
                        tempDirection = Direction.South;
                    }
                    break;
            }

            return tempDirection;
        }
    }
}
