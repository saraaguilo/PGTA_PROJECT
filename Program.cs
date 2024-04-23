using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;
using OurClasses;


class Program
{
    static void Main()
    {

        byte[] fileBytes = ReadFile();

        List<byte[]> Messages = GetCat48Messages(fileBytes);

        for (int i = 0; i < Messages.Count; i++)
        {
            List<byte> fspec = Fspec(Messages[i]);

            byte[] data = Data(Messages[i], fspec.Count);

            if (i == 0)
            {
                Console.WriteLine(string.Join(",", fspec));

                Console.WriteLine(string.Join(",", data));

                Console.WriteLine(string.Join(",", Messages[i]));
            }

            break;

            classifyparams(fspec, data);
        }
    }

    static byte[] Data(byte[] message, int Lenfspec)
    {
        byte[] data = new byte[message.Length - Lenfspec -3];

        Array.Copy(message, Lenfspec + 3, data, 0, message.Length - Lenfspec - 3);

        return data;
    }

    static List<byte[]> GetCat48Messages(byte[] fileBytes)
    {
        List<byte[]> listabyte = new List<byte[]>();
        int i = 0;
        int contador = getdeclen(fileBytes[1], fileBytes[2]);
        //int contador = fileBytes[2];

        while (i < fileBytes.Length)
        {
            byte[] array = new byte[contador];
            for (int j = 0; j < array.Length; j++)
            {
                array[j] = fileBytes[i];
                i++;
            }
            listabyte.Add(array);
            if (i + 2 < fileBytes.Length)
            {
                contador = getdeclen(fileBytes[i + 1], fileBytes[i + 2]);
                //contador = fileBytes[i + 2];
            }
        }
        return listabyte;
    }

    static int getdeclen(byte l1, byte l2)
    {
        int Length = (l1 << 8) | l2;
        return Length;
    }

    static byte[] ReadFile()
    {
        byte[] fileBytes = File.ReadAllBytes(@"C:\Users\polro\Desktop\PGTA\230502-est-080001_BCN_60MN_08_09.ast");
        return fileBytes;
    }

    static List<byte> Fspec(byte[] message)
    {
        List<byte> fspec = new List<byte>();
        int i = 3;
        while(i != 0)
        {
            fspec.Add(message[i]);
            if ((message[i] & 0b00000001) == 1) { i++; }
            else { i = 0; }
        }
        return fspec;
    }

    static void classifyparams(List<byte> fspec, byte[] data)
    {
        Decoder48 decoder48 = new Decoder48();
        if (1 == ((fspec[0] >> 7) & 0b00000001))
        {
            byte SAC = data[0];
            byte SIC = data[1];
            Array.Copy(data, 2, data, 0, data.Length -2);
        }
        if (1 == ((fspec[0] >> 6) & 0b00000001))
        {
            byte[] data_item_140 = new byte[3]; 
            Array.Copy(data, 0, data_item_140, 0, 3);
            TimeSpan TimeofDay = decoder48.decodeTimeDay(data_item_140);
            Array.Copy(data, 3, data, 0, data.Length - 3);
        }
        if (1 == ((fspec[0] >> 5) & 0b00000001))
        {
            List<byte> data_item_020 = new List<byte>;
            int i = 0;
            while (i != -1)
            {
                data_item_020.Add(data[i]);
                if ((data[i] & 0b00000001) == 1) { i++; }
                else { i = -1; }
            }
            var result020 = decoder48.decode020(data_item_020);
            Array.Copy(data, data_item_020.Count, data, 0, data.Length - data_item_020.Count);
        }
        if (1 == ((fspec[0] >> 4) & 0b00000001))
        {
            byte[] data_item_040 = new byte[4];
            Array.Copy(data, 0, data_item_040, 0, 4);
            var result040 = decoder48.decode040(data_item_040);
            float rho = result040[0];
            float theta = result040[1];
            Array.Copy(data, 4, data, 0, data.Length - 4);
        }
        if (1 == ((fspec[0] >> 3) & 0b00000001))
        {
            byte[] data_item_070 = new byte[2];
            Array.Copy(data, 0, data_item_070, 0, 2);
            var result070 = decoder48.decode070(data_item_070);
            Array.Copy(data, 2, data, 0, data.Length - 2);

        }
        if (1 == ((fspec[0] >> 2) & 0b00000001))
        {
            byte[] data_item_090 = new byte[2];
            Array.Copy(data, 0, data_item_090, 0, 2);
            var result090 = decoder48.decode090(data_item_090);
            Array.Copy(data, 2, data, 0, data.Length - 2);
        }
        if (1 == ((fspec[0] >> 1) & 0b00000001))
        {
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((data[0] & (1 << i)) != 0)
                {
                    count++;
                }
            }
            byte[] data_item_130 = new byte[count];
            Array.Copy(data, 0, data_item_130, 0, count);
            var result130 = decoder48.decode090(data_item_130);
            Array.Copy(data, count, data, 0, data.Length - count);
        }
        if (1 == ((fspec[0] & 0b00000001)))
        {
            if (1 == ((fspec[1] >> 7) & 0b00000001))
            {
                byte[] data_item_220 = new byte[3];
                Array.Copy(data, 0, data_item_220, 0, 3);
                var result220 = decoder48.decode220(data_item_220);
                Array.Copy(data, 3, data, 0, data.Length - 3);
            }
            if (1 == ((fspec[1] >> 6) & 0b00000001))
            {
                byte[] data_item_240 = new byte[6];
                Array.Copy(data, 0, data_item_240, 0, 6);
                var result240 = decoder48.decode240(data_item_240);
                Array.Copy(data, 6, data, 0, data.Length - 6);
            }
            if (1 == ((fspec[1] >> 5) & 0b00000001))
            {
                int len = data[0]*8+1
                byte[] data_item_250 = new byte[len];
                Array.Copy(data, 0, data_item_250, 0, len);
                var result250 = decoder48.decode240(data_item_250);
                Array.Copy(data, len, data, 0, data.Length - len);
            }
            if (1 == ((fspec[1] >> 4) & 0b00000001))
            {
                byte[] data_item_161 = new byte[2];
                Array.Copy(data, 0, data_item_161, 0, 2);
                var result161 = decoder48.tracknumberDecoding(data_item_161);
                Array.Copy(data, 2, data, 0, data.Length - 2);
            }
            if (1 == ((fspec[1] >> 3) & 0b00000001))
            {
                byte[] data_item_042 = new byte[4];
                Array.Copy(data, 0, data_item_042, 0, 4);
                var result042 = decoder48.decode042(data_item_042);
                Array.Copy(data, 4, data, 0, data.Length - 4);
            }
            if (1 == ((fspec[1] >> 2) & 0b00000001))
            {
                byte[] data_item_200 = new byte[4];
                Array.Copy(data, 0, data_item_200, 0, 4);
                var result0200 = decoder48.decode200(data_item_200);
                Array.Copy(data, 4, data, 0, data.Length - 4);
            }
            if (1 == ((fspec[1] >> 1) & 0b00000001))
            {
                List<byte> data_item_170 = new List<byte>;
                int i = 0;
                while (i != -1)
                {
                    data_item_170.Add(data[i]);
                    if ((data[i] & 0b00000001) == 1) { i++; }
                    else { i = -1; }
                }
                var result170 = decoder48.decode170(data_item_170);
                Array.Copy(data, data_item_170.Count, data, 0, data.Length - data_item_170.Count);
            }
            if (1 == ((fspec[1] & 0b00000001)))
            {
                if (1 == ((fspec[2] >> 7) & 0b00000001))
                {
                    Array.Copy(data, 4, data, 0, data.Length - 4);
                }
                if (1 == ((fspec[2] >> 6) & 0b00000001))
                {
                    List<byte> data_item_030 = new List<byte>;
                    int i = 0;
                    while (i != -1)
                    {
                        data_item_030.Add(data[i]);
                        if ((data[i] & 0b00000001) == 1) { i++; }
                        else { i = -1; }
                    }
                    Array.Copy(data, data_item_030.Count, data, 0, data.Length - data_item_030.Count);
                }
                if (1 == ((fspec[2] >> 5) & 0b00000001))
                {
                    Array.Copy(data, 2, data, 0, data.Length - 2);
                }
                if (1 == ((fspec[2] >> 4) & 0b00000001))
                {
                    Array.Copy(data, 4, data, 0, data.Length - 4);
                }
                if (1 == ((fspec[2] >> 3) & 0b00000001))
                {
                    byte[] data_item_110 = new byte[2];
                    Array.Copy(data, 0, data_item_110, 0, 2);
                    var result110 = decoder48.decode110(data_item_110);
                    Array.Copy(data, 2, data, 0, data.Length - 2);
                }
                if (1 == ((fspec[2] >> 2) & 0b00000001))
                {
                    List<byte> data_item_120 = new List<byte>;
                    int i = 0;
                    while (i != -1)
                    {
                        data_item_120.Add(data[i]);
                        if ((data[i] & 0b00000001) == 1) { i++; }
                        else { i = -1; }
                    }
                    Array.Copy(data, data_item_120.Count, data, 0, data.Length - data_item_120.Count);
                }
                if (1 == ((fspec[2] >> 1) & 0b00000001))
                {
                    byte[] data_item_230 = new byte[2];
                    Array.Copy(data, 0, data_item_230, 0, 2);
                    var result230 = decoder48.decode230(data_item_230);
                    Array.Copy(data, 2, data, 0, data.Length - 2);
                }
                if (1 == ((fspec[2] & 0b00000001)))
                {
                    if (1 == ((fspec[3] >> 7) & 0b00000001))
                    {
                        Array.Copy(data, 7, data, 0, data.Length - 7);
                    }
                    if (1 == ((fspec[3] >> 6) & 0b00000001))
                    {
                        Array.Copy(data, 1, data, 0, data.Length - 1);
                    }
                    if (1 == ((fspec[3] >> 5) & 0b00000001))
                    {
                        Array.Copy(data, 2, data, 0, data.Length - 2);
                    }
                    if (1 == ((fspec[3] >> 4) & 0b00000001))
                    {
                        Array.Copy(data, 1, data, 0, data.Length - 1);
                    }
                    if (1 == ((fspec[3] >> 3) & 0b00000001))
                    {
                        Array.Copy(data, 2, data, 0, data.Length - 2);
                    }
                    if (1 == ((fspec[3] >> 2) & 0b00000001))
                    {

                    }
                    if (1 == ((fspec[3] >> 1) & 0b00000001))
                    {

                    }
                }
            }
        }
    }
}
