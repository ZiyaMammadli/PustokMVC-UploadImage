namespace PustokMVC.CustomExceptions.Common
{
    public class InvalidContentTypeException:Exception
    {
        public string PropertyName { get; set; }
        public InvalidContentTypeException()
        {
            
        }
        public InvalidContentTypeException(string message):base(message) 
        {
            
        }
        public InvalidContentTypeException(string propertyName,string message):base (message)
        {
            PropertyName=propertyName;
        }
    }
}
