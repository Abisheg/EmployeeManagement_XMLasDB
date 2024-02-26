using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.IO;
using System.Xml.Linq;

public class XmlLogger
{
    public static string _dbFilePath = "XML_db\\db.xml";
    public static string _logFilePath = "XML_db\\log.xml";
    public static int maxId;
    public void InitializeXmlFiles()
    {
        try
        {
            string[] filePaths = { _dbFilePath, _logFilePath };

            foreach (var filePath in filePaths)
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (!File.Exists(filePath))
                {
                    XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

                    var initialXml = new XElement(soapenv + "Envelope",
                        new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                        new XElement(soapenv + "Header"),
                        new XElement(soapenv + "Body",
                            new XElement("DatabaseRecords")
                        )
                    );
                    initialXml.Save(filePath);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            Console.WriteLine($"Error initializing XML files: {ex.Message}");
        }
    }



    public void LogEmployeeAction(string action, Employee employee)
    {
        var logEntry = new XElement("LogEntry",
            new XElement("Timestamp", DateTime.UtcNow),
            new XElement("Action", action),
            new XElement("Employee",
                new XElement("Id", employee.Id),
                new XElement("Name", employee.Name),
                new XElement("email", employee.Email),
                new XElement("Department", employee.Department)
            )
        );

        var existingXml = XDocument.Load(_logFilePath);
        existingXml.Root.Add(logEntry);
        existingXml.Save(_logFilePath);
    }
    public void SaveEmployeeAction(string action, Employee employee)
    {
        var logEntry = new XElement("DatabaseRecord",
            new XElement("Timestamp", DateTime.UtcNow),
            new XElement("Employee",
                new XElement("Id", employee.Id),
                new XElement("Name", employee.Name),
                new XElement("email", employee.Email),
                new XElement("Department", employee.Department)
            )
        );

        var existingXml = XDocument.Load(_logFilePath);
        existingXml.Root.Add(logEntry);
        existingXml.Save(_logFilePath);
    }
}