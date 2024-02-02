﻿namespace MyShogi.Model.Shogi.Core
{
    public static class Initializer
    {
        /// <summary>
        /// 起動時の初期化一式
        /// static constructorで初期化したくないのでここでまとめて初期化する
        /// </summary>
        public static void Init()
        {
            Zobrist.Init();
            Bitboard.Init();
        }
    }
}
