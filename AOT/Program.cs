using System.Text;

namespace AOT
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                File.Move("gjiorehgierhrt", "dfgkjrihnthn");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
