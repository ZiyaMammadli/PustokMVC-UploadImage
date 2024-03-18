using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace PustokMVC.CustomExceptions.BookExceptions
{
    public class BookNotfoundException:Exception
    { 
        public BookNotfoundException()
        {

        }
        public BookNotfoundException(string message):base(message)
        {
            
        }
    }
}
