using System.Reflection.Metadata;

namespace PustokMVC.CustomExceptions.Common;

public class SizeOfFileException:Exception
{
    public string Propertyame { get; set; }
    public SizeOfFileException()
    {
        
    }
    public SizeOfFileException(string message):base(message)
    {
        
    }
    public SizeOfFileException(string propertyame,string message):base(message)
    {

        Propertyame = propertyame;

    }
}
