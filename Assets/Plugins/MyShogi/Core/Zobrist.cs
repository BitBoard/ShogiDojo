﻿using System;

namespace MyShogi.Model.Shogi.Core
{
    // 局面のhash keyを求めるときに用いるZobrist key
    public static class Zobrist
    {
        public static HASH_KEY Zero;                          // ゼロ(==0)
        public static HASH_KEY Side;                          // 手番(==1)
        private static HASH_KEY[,] psq = new HASH_KEY[Square.NB_PLUS1.ToInt(),Piece.NB.ToInt()];	// 駒pcが盤上sqに配置されているときのZobrist Key
	    private static HASH_KEY[,] hand = new HASH_KEY[Color.NB.ToInt(),Piece.HAND_NB.ToInt()];	// c側の手駒prが一枚増えるごとにこれを加算するZobristKey

        /// <summary>
        /// sqの升にpcがあるときのZobrist Key
        /// </summary>
        /// <param name="sq"></param>
        /// <param name="pc"></param>
        /// <returns></returns>
        public static HASH_KEY Psq(Square sq , Piece pc)
        {
            return psq[sq.ToInt(), pc.ToInt()];
        }

        /// <summary>
        /// c側の手駒hand_pcがあるときのZobrist Key
        /// </summary>
        /// <param name="c"></param>
        /// <param name="hand_pc"></param>
        /// <returns></returns>
        public static HASH_KEY Hand(Color c , Piece hand_pc)
        {
            return hand[c.ToInt(), hand_pc.ToInt()];
        }

        // static constructorで初期化するの、筋が良くないのでは…。
        /*
        static Zobrist()
        {
            Init();
        }
        */

        /// <summary>
        /// 上のテーブルを初期化する
        /// これは起動時に自動的に行われる
        /// </summary>
        public static void Init()
        {
            var rng = new PRNG(20151225); // 開発開始日 == 電王トーナメント2015,最終日

            // 手番としてbit0を用いる。それ以外はbit0を使わない。これをxorではなく加算して行ってもbit0は汚されない。
            SET_HASH(ref Side, 1, 0, 0, 0);
            SET_HASH(ref Zero, 0, 0, 0, 0);

            // 64bit hash keyは256bit hash keyの下位64bitという解釈をすることで、256bitと64bitのときとでhash keyの下位64bitは合致するようにしておく。
            // これは定跡DBなどで使うときにこの性質が欲しいからである。
            // またpc==NO_PIECEのときは0であることを保証したいのでSET_HASHしない。
            // psqは、C++の規約上、事前にゼロであることは保証される。
            for (Piece pc = Piece.ZERO + 1; pc < Piece.NB; ++ pc)
                for (Square sq = Square.ZERO; sq < Square.NB; ++ sq)
                    {
                        var r0 = rng.Rand() & ~1UL;
                        var r1 = rng.Rand();
                        var r2 = rng.Rand();
                        var r3 = rng.Rand();
                        SET_HASH(ref psq[sq.ToInt(),pc.ToInt()], r0, r1, r2, r3);
                    }

            // またpr==NO_PIECEのときは0であることを保証したいのでSET_HASHしない。
            foreach (var c in All.IntColors())
                for (Piece pr = Piece.ZERO + 1; pr < Piece.HAND_NB; ++pr)
                    {
                        var r0 = rng.Rand() & ~1UL;
                        var r1 = rng.Rand();
                        var r2 = rng.Rand();
                        var r3 = rng.Rand();
                        SET_HASH(ref hand[c , pr.ToInt()], r0, r1, r2, r3);
                    }
        }

        /// <summary>
        /// HASH_KEYに乱数を代入する
        /// </summary>
        /// <param name="h"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        private static void SET_HASH(ref HASH_KEY h , UInt64 a,UInt64 b,UInt64 c , UInt64 d)
        {
            h.p.Set(a,b);
 
            // 残り128bitは使用しない
            // 128bitで足りないなら、もしかしたら使うかも
        }
    }
}
