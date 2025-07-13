using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace EmployeeWebApi.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            
            // Log exception to file
            LogExceptionToFile(exception);
            
            // Set custom error response
            context.Result = new ObjectResult(new
            {
                error = "Internal server error occurred",
                message = exception.Message,
                timestamp = DateTime.UtcNow
            })
            {
                StatusCode = 500
            };
            
            context.ExceptionHandled = true;
        }
        
        private void LogExceptionToFile(Exception exception)
        {
            try
            {
                var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                
                var logFile = Path.Combine(logPath, $"exceptions_{DateTime.Now:yyyyMMdd}.txt");
                var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {exception.GetType().Name}: {exception.Message}\n{exception.StackTrace}\n\n";
                
                File.AppendAllText(logFile, logEntry);
            }
            catch (Exception logEx)
            {
                // Handle logging errors silently or use a different logging mechanism
                Console.WriteLine($"Failed to log exception: {logEx.Message}");
            }
        }
    }
}