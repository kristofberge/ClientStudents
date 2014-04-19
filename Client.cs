using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;


public class Client
{

    private const string REQ_SEND_CLASS_LIST = "sndclslt";
    private const string CMD_START_CLASSLIST = "stclassl";
    private const string CMD_START_CLASSDATA = "classdat";
    private const string CMD_END_LIST =        "end_list";

    public static void start()
    {

        try
        {
            TcpClient tcpclnt = new TcpClient();
            Console.WriteLine("Connecting.....");

            tcpclnt.Connect("192.168.0.105", 666);
            // use the ipaddress as in the server program

            Console.WriteLine("Connected");
            Console.Write("Enter the string to be transmitted : ");

            String str = Console.ReadLine();
            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
            Console.WriteLine("Transmitting.....");

            stm.Write(ba, 0, ba.Length);

            byte[] bytesReceived = new byte[8];
            stm.Read(bytesReceived, 0, bytesReceived.Length);

            Console.WriteLine("Received//" + Encoding.ASCII.GetString(bytesReceived, 0, bytesReceived.Length) + "//");
            

            switch (Encoding.ASCII.GetString(bytesReceived, 0,bytesReceived.Length))
            {
                case CMD_START_CLASSLIST:
                    Console.WriteLine("Receiveing class list.");
                    ReceiveClassList(stm);
                    break;
            }
            

            tcpclnt.Close();
        }

        catch (Exception e)
        {
            throw;
        }
    }

    private static void ReceiveClassList(Stream stm)
    {
        string stringMsg = "";

        while(stringMsg!=CMD_END_LIST){
            byte[] bytesReceived = new byte[1];
            stm.Read(bytesReceived, 0, bytesReceived.Length);
            int lengthOfNextLine = Convert.ToInt32(bytesReceived[0]);
            Console.WriteLine("Received length//" + lengthOfNextLine + "//");

            bytesReceived = new byte[lengthOfNextLine];
            stm.Read(bytesReceived, 0, bytesReceived.Length);
            stringMsg = Encoding.ASCII.GetString(bytesReceived, 0, bytesReceived.Length);

            Console.WriteLine("Received//" + stringMsg + "//");

        }
    }

}