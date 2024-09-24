using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        public Server(string ipAddress, int port)
        {
            _ipAddress = IPAddress.Parse(ipAddress);
            _port = port;
        }

        public async Task StartServer()
        {
            TcpListener? listener = null;
            try
            {
                listener = new TcpListener(_ipAddress, _port);
                listener.Start();
                Console.WriteLine($"Server started.\n------ Listening to TCP clients at {_ipAddress}:{_port}");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync(); // Accept the TCP client
                    IPAddress clientIpAddress = ((IPEndPoint)client.Client.RemoteEndPoint!).Address;
                    Console.WriteLine();
                    Console.WriteLine("============================");
                    Console.WriteLine($"Client connected from {clientIpAddress}");
                    Console.WriteLine("============================");

                    // Process the client connection in a separate task
                    _ = ProcessClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                listener?.Stop();
            }
        }

        private async Task ProcessClientAsync(TcpClient client)
        {
            try
            {
                using NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;
                StringBuilder messageFromClient = new StringBuilder();

                // Read data from the client
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    // Convert bytes to string and accumulate
                    messageFromClient.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    string receivedMessage = messageFromClient.ToString();
                    messageFromClient = messageFromClient.Clear();
                    Console.WriteLine("===================================================");
                    Console.WriteLine($"Message from client: {receivedMessage}");

                    // Process the data
                    string messageToSendToClient = receivedMessage.ToUpper();
                    byte[] messageBytesToSend = Encoding.UTF8.GetBytes(messageToSendToClient);
                    Console.WriteLine($"Processed Data: {messageToSendToClient}");

                    // Send the processed data back to the client
                    await stream.WriteAsync(messageBytesToSend, 0, messageBytesToSend.Length);
                    Console.WriteLine($"Response sent to client: {messageToSendToClient}");
                    Console.WriteLine("===================================================");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Occurred: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

    }
}
