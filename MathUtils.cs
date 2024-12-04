using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Cache;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AsymCryptoLab3
{
    public static class MathUtils
    {

        public static BigInteger Mod(BigInteger a, BigInteger n)
        {
            if (a < 0)
            {
                a = -a * (n - 1);
            }

            BigInteger res = a % n;
            return res;
        }
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            if (b == 0) return a;
            return GCD(b, Mod(a, b));
        }
        public static BigInteger LCM(BigInteger a, BigInteger b)
        {
            return (a * b) / GCD(a, b);
        }
        public static bool CheckStrongPrime(BigInteger n, BigInteger x)
        {
            if (x >= n || x <= 1) throw new Exception("Bad x: its must be in range");
            BigInteger s, t;
            s = 0;
            t = n - 1;

            while (t % 2 == 0)
            {
                t >>= 1;
                s++;
            }

            BigInteger y = BigInteger.ModPow(x, t, n);

            if ((y == 1) || (y == n - 1))
            {
                return true;
            }

            for (int i = 1; i < s; i++)
            {
                y = BigInteger.ModPow(y, 2, n);
                if (y == n - 1) return true;
            }

            return false;

        }
        
        public static BigInteger ExtendedGCD(BigInteger x, BigInteger n, out BigInteger xReverse)
        {
            xReverse = 0;

            BigInteger rPrev = x;
            BigInteger r = n;
            BigInteger q = 0;

            BigInteger u1 = 1;
            BigInteger u2 = 0;
            BigInteger u3 = 0;

            BigInteger v1 = 0;
            BigInteger v2 = 1;
            BigInteger v3 = 0;

            while (true)
            {
                (q, BigInteger rNext) = BigInteger.DivRem(rPrev, r);

                u3 = u1 - (u2 * q);
                v3 = v1 - (v2 * q);

                if (rNext == 0) break;

                // Preparing for the next iteration
                rPrev = r;
                r = rNext;
                u1 = u2;
                u2 = u3;
                v1 = v2;
                v2 = v3;
            }

            if (r == 1) xReverse = Mod(u2, n);

            return r;
        }

        public static BigInteger GenerateRandomPrime(int bitLength)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                while (true)
                {
                    byte[] bytes = new byte[bitLength / 8];
                    rng.GetBytes(bytes);

                    bytes[bytes.Length - 1] |= 0x80;

                    BigInteger candidate = new BigInteger(bytes);
                    if (candidate < 0)
                    {
                        candidate = -candidate;
                    }

                    if (candidate.IsEven)
                    {
                        candidate++;
                    }

                    if (candidate > 1 && PrimeTest(candidate))
                    {
                        return candidate;
                    }
                }
            }
        }

        public static bool PrimeTest(BigInteger n, int k = 1) // к - ітерації, не чіпай - хата сгорить
        {
            if (n < 0) n = -n;
            if ((n == 2) || (n == 3) || (n == 5)) return true;
            if ((n % 2 == 0) || (n == 1)) return false;

            BigInteger x = 1;
            for (int i = 0; i < k; i++)
            {
                Random rand = new Random();
                x = RandomBigInteger(3, n / 2);
                if (MathUtils.GCD(x, n) > 1) return false;
                if (MathUtils.CheckStrongPrime(n, x)) return true;
            }

            return false;

        }

        //діапазон
        public static BigInteger RandomBigInteger(BigInteger min, BigInteger max)
        {
            if (min >= max)
            {
                throw new ArgumentException("min cannot be greater than or equal to max");
            }

            byte[] maxBytes = (max - 1).ToByteArray();
            byte[] bytes = new byte[maxBytes.Length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            BigInteger value = new BigInteger(bytes);
            BigInteger range = max - min;
            value = (BigInteger.Abs(value) % range) + min;
            return value;
        }

        public static BigInteger CongruencesSolver(List<BigInteger> ys, List<BigInteger> mods)
        {
            BigInteger N = 1;
            BigInteger X = 0;

            foreach (var n in mods)
            {
                N *= n;
            }

            for (int i = 0; i < ys.Count; i++)
            {
                BigInteger N_i = N / mods[i];
                BigInteger N_iReverse = 0;
                ExtendedGCD(N_i, mods[i], out N_iReverse);
                X += N_i * N_iReverse * ys[i];
            }

            return Mod(X, N);

        }


    }
}
