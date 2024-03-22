using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_Server
{
    public class Server
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;

        public Server()
        {
            _ipAddress = IPAddress.Parse("127.0.0.1");
            _port = 1234;
        }
        public void StartServer()
        {
            TcpListener? listener = null;
            TcpClient? client = null;
            try
            {

                listener = new TcpListener(_ipAddress, _port);
                listener.Start();
                Console.WriteLine("Server started.\n------ Listening to Tcp clients at {0}:{1}", _ipAddress, _port);


                while (true)
                {
                    client = listener.AcceptTcpClient(); // Accept the tcp client
                    IPAddress clientIpAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                    Console.WriteLine();
                    Console.Write("============================");
                    Console.Write("clinet connected from {0}", clientIpAddress.ToString());
                    Console.WriteLine("============================");

                    do
                    {
                        GetProcessSendData(client);
                    } while (client.Connected);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client?.Close();
                listener?.Stop();
            }
        }

        private void GetProcessSendData(TcpClient client)
        {
            NetworkStream? stream = null;
            try
            {
                stream = client.GetStream(); // Getting the network stream for reading and wriring
                int noOfBytesRead = 0;

                Console.WriteLine("--------------------------------------\n");
                // Getting the data from network stream
                string messageFromClient = string.Empty;
                byte[] buffer = new byte[10];
                // Get the data
                do
                {
                    noOfBytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (noOfBytesRead > 0)
                    {
                        messageFromClient += Encoding.UTF8.GetString(buffer, 0, noOfBytesRead);
                    }
                    else
                    {
                        break;
                    }
                    noOfBytesRead = 0;
                } while (stream.DataAvailable);
                Console.WriteLine("Message from client : {0}", messageFromClient);

                // Processing the data
                string messageToSendToClient = messageFromClient.ToUpper();
                Byte[] messageBytesToSendtoClient = Encoding.UTF8.GetBytes(messageToSendToClient, 0, messageToSendToClient.Length);
                Console.WriteLine("Processed Data : {0}", messageToSendToClient);


                // Send the processed data to client
                stream.Write(messageBytesToSendtoClient, 0, messageBytesToSendtoClient.Length);
                Console.WriteLine($"Response sent to client : {messageToSendToClient}");
                Console.WriteLine("--------------------------------------\n");
                stream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured : {0} ", ex.Message);
            }
        }
    }
}
