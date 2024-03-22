namespace TCP_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Server server = new Server();
                server.StartServer();
            }catch (Exception ex)
            {
                Console.WriteLine("Error occured : ",ex.Message);   
            }
        }
    }
}
