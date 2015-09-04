using System;

namespace SeleniumHelper
{
    public class MissingConfigurationException : System.Exception
    {
        public MissingConfigurationException(string message): base(message)
        {

        }

        public MissingConfigurationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}