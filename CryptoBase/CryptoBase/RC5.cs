using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CryptoBase
{  
    public class RC5 : iCrypto
    {
        const int W = 64; // половина длины блока в битах. Возможные значения 16, 32 и 64.
        const int R = 12; // число раундов. Возможные значения 0…255.
        const UInt64 PW = 0xB7E151628AED2A6B; // 64-битная константа  Pw = Odd((f - 1) * 2^W;
        const UInt64 QW = 0x9E3779B97F4A7C15;// 64-битная константа  Qw = Odd((e - 2) * 2^W; Odd() - округление до ближайшего нечетного целого.
        UInt64[] L_Key; // массив слов для секретного ключа пользователя
        UInt64[] S_Table; // таблица расширенных ключей
        int t_sizeOfTable; // размер таблицы
        int b_BytesOfKey; // число байтов в ключе
        int u_byteOfW; // кол-во байтов в одном машинном слове
        int c_lenOfL; // размер массива слов L

        public RC5(Stream skey)
        {
            UInt64 x, y;
            int i, j, n;

            byte[] buf = new byte[255];
            skey.Position = 0;
            skey.Read(buf, 0, 255);
            byte[] key = new byte[buf.Length];// до 255 байт
            key = buf;

            //Этап 1. Генерация констант - заданы
            // Этап 2. Разбиение ключа на слова
            u_byteOfW = W >> 3;//64>>3 = 8 - байт в машинном слове
            b_BytesOfKey = key.Length;// размер ключа в байтах
            c_lenOfL = b_BytesOfKey % u_byteOfW > 0 ? (b_BytesOfKey / u_byteOfW + 1) : (b_BytesOfKey / u_byteOfW); //размер массива слов для ключа         
            L_Key = new UInt64[c_lenOfL];//место под "масссив" ключа

            for (i = b_BytesOfKey - 1; i >= 0; i--)
                L_Key[i / u_byteOfW] = ROL(L_Key[i / u_byteOfW], 8) + key[i];//заполняем таблицу слов ключа

            //Этап 3. Построение таблицы расширенных ключей, построение таблицы расширенных ключей S[0]..S[2(R + 1)]
            t_sizeOfTable = 2 * (R + 1);
            S_Table = new UInt64[t_sizeOfTable];
            S_Table[0] = PW;
            for (i = 1; i < t_sizeOfTable; i++)
                S_Table[i] = S_Table[i - 1] + QW;

            // Этап 4. Перемешивание
            x = y = 0; i = j = 0;
            n = 3 * Math.Max(t_sizeOfTable, c_lenOfL);

            for (int k = 0; k < n; k++)
            {
                x = S_Table[i] = ROL((S_Table[i] + x + y), 3);
                y = L_Key[j] = ROL((L_Key[j] + x + y), (int)(x + y));
                i = (i + 1) % t_sizeOfTable;
                j = (j + 1) % c_lenOfL;
            }
        }


        private UInt64 ROL(UInt64 a, int offset)//циклический сдвиг битов влево
        {
            UInt64 r1, r2;
            r1 = a << offset;
            r2 = a >> (W - offset);
            return (r1 | r2);
        }
        private UInt64 ROR(UInt64 a, int offset)//циклический сдвиг битов вправо
        {
            UInt64 r1, r2;
            r1 = a >> offset;
            r2 = a << (W - offset);
            return (r1 | r2);

        }
        private static UInt64 BytesToUInt64(byte[] b, int p)
        {
            UInt64 r = 0;
            for (int i = p + 7; i > p; i--)
            {
                r |= (UInt64)b[i]; 
                r <<= 8;
            }
            r |= (UInt64)b[p];
            return r;
        }
        private static void UInt64ToBytes(UInt64 a, byte[] b, int p)
        {
            for (int i = 0; i < 7; i++)
            {
                b[p + i] = (byte)(a & 0xFF);
                a >>= 8;
            }
            b[p + 7] = (byte)(a & 0xFF);
        }

        public byte[] CipherBlock(byte[] buf)//криптование 16байтного блока
        {
            UInt64 a = BytesToUInt64(buf, 0);
            UInt64 b = BytesToUInt64(buf, 8);

            a = a + S_Table[0];
            b = b + S_Table[1];

            for (int i = 1; i < R + 1; i++)
            {
                a = ROL((a ^ b), (int)b) + S_Table[2 * i];
                b = ROL((b ^ a), (int)a) + S_Table[2 * i + 1];
            }

            UInt64ToBytes(a, buf, 0);
            UInt64ToBytes(b, buf, 8);

            return buf;
        }

 
        public byte[] DecipherBlock(byte[] buf)
        {
            UInt64 a = BytesToUInt64(buf, 0);
            UInt64 b = BytesToUInt64(buf, 8);

            for (int i = R; i > 0; i--)
            {
                b = ROR((b - S_Table[2 * i + 1]), (int)a) ^ a;
                a = ROR((a - S_Table[2 * i]), (int)b) ^ b;
            }

            b = b - S_Table[1];
            a = a - S_Table[0];

            UInt64ToBytes(a, buf, 0);
            UInt64ToBytes(b, buf, 8);

            return buf;
        }

        public void Encrypt(Stream input, Stream output)
        {
            int readed;
            byte[] bufIn = new byte[16];//создаём буферы под криптование

            
            input.Position = 0;//читаем входной стрим с начала
            while ((readed = input.Read(bufIn, 0, 16)) != 0)//пока есть стрим?
            {
                for (; readed < 16; readed++)//если прочитано меньше 16 байт
                {
                    bufIn[readed] = 0;//дополняем нулями оставшиеся байты до нужного 16 байтного блока 
                }
                bufIn = CipherBlock(bufIn);//криптим 
                output.Write(bufIn, 0, bufIn.Length);//пишем в выходной стрим
                bufIn = new byte[16];//обнуляем буфер                    
            }
        }

        public void Decrypt(Stream input, Stream output)
        {
            int readed;
            byte[] bufIn = new byte[16];//создаём буферы под криптование

            input.Position = 0;//читаем входной стрим с начала
            while ((readed = input.Read(bufIn, 0, 16)) != 0)//пока есть стрим
            {
                bufIn = DecipherBlock(bufIn);//декриптим 
                output.Write(bufIn, 0, bufIn.Length);//пишем в выходной стрим
                bufIn = new byte[16];//обнуляем буфер                    
            }
        }
    }
}
