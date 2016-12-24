using System;
using System.IO;

namespace CryptoBase
{
    public class Elgamal : iCrypto
    {
        static int p, g, y, x;

        public Elgamal(Stream key)
        {
            SetKey(key);
        }

        public void Encrypt (Stream input, Stream output)
        {
            byte byteToCrypt;//1 байт (для криптования)

            y = Power(g, x, p);// находим некий игрек "y" - 3я часть открытого ключа 

            input.Position = 0;//читаем входной стрим с начала
            for (; input.Position < input.Length; )//и до конца
            {
                byteToCrypt = (byte)input.ReadByte();//берём один байт из стрима и пишем в byteToCrypt

                int k = Rand() % (p - 2) + 1;
                int a = Power(g, k, p);
                int b = Multiple(Power(y, k, p), byteToCrypt, p);

                output.WriteByte((byte)a);//пишем в выходной стрим 2 байта, а не 1
                output.WriteByte((byte)b);
            }
        }
        public void Decrypt (Stream input, Stream output)
        {
            byte a;
            byte b;
            byte[] bytesToDecrypt = new byte[2]; //2 байта на декрипт

            input.Position = 0;
            for (; input.Position < input.Length; )
            {
                input.Read(bytesToDecrypt, 0, 2);//читаем сразу 2 байта

                a = bytesToDecrypt[0];
                b = bytesToDecrypt[1];//заносим в переменнные

                int deM = Multiple(b, Power(a, p - 1 - x, p), p);//вычисляем оригинальное значение, исп ЗАКРЫТЫй КЛЮЧ "х"

                output.WriteByte((byte)deM);
            }
        }

        public void SetKey(Stream skey)
        {
            byte[] buf = new byte[3];
            skey.Position = 0;
            skey.Read(buf, 0, 3);
            byte[] key = new byte[3];
            key = buf;

            p = Convert.ToInt32(key[0]);
            g = Convert.ToInt32(key[1]);
            x = Convert.ToInt32(key[2]);

        }

        private int Rand()
        {
            Random random = new Random();
            return random.Next();
        }

        int Power(int a, int b, int m) // a^b mod m
        {
            int tmp = a;
            int sum = tmp;
            for (int i = 1; i < b; i++)
            {
                for (int j = 1; j < a; j++)
                {
                    sum += tmp;
                    if (sum >= m)
                        sum -= m;
                }
                tmp = sum;
            }
            return tmp;
        }


        int Multiple(int a, int b, int m) // a*b mod m
        {
            int sum = 0;
            for (int i = 0; i < b; i++)
            {
                sum += a;
                if (sum >= m)
                    sum -= m;
            }
            return sum;
        }

    }
}
