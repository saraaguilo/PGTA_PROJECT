using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;


class Program
{
    static void Main()
    {

        byte[] fileBytes = ReadFile();

        List<byte[]> Messages = GetCat48Messages(fileBytes);

        for (int i = 0; i < Messages.Count; i++)
        {
            List<byte> fspec = Fspec(Messages[i]);

            if(i == 5001)
            {
                Console.WriteLine(string.Join(",", fspec));
            }
        }

        //Console.WriteLine(string.Join(",",Messages[5001]));
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
}
