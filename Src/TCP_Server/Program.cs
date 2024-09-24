namespace TCP_Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Server server = new Server("127.0.0.1",1235);
                await server.StartServer();
                
            }catch (Exception ex)
            {
                Console.WriteLine("Error occured : ",ex.Message);   
            }
        }
    }
}
