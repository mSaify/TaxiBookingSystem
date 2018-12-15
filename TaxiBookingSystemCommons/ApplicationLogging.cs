using Microsoft.Extensions.Logging;
using System;

namespace TaxiBookingSystemCommons
{
    public static class ApplicationLogging
    {
        static ILoggerFactory _loggerFactory;
        static object _lockObject;

        static ApplicationLogging()
        {
            _lockObject = new object();
        }

        public static ILoggerFactory LoggerFactory {
            get { return _loggerFactory==null? DefaultLoggerFactory() : _loggerFactory; }
            set {

                    if (_loggerFactory == null)
                    {
                        lock (_lockObject)
                        {
                            if (_loggerFactory == null)
                            {

                                _loggerFactory = value;

                            }
                        }
                    }
                }
        }

        private static ILoggerFactory DefaultLoggerFactory()
        {
            return new LoggerFactory();
        }

        public static ILogger CreateLogger<T>() =>
          LoggerFactory.CreateLogger<T>();
    }
}
