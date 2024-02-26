using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EmployeeManagement.Service
{
    public class EmployeeService
    {
        public IEnumerable<Employee> GetEmployees()
        {
            string xmlFilePath = Path.GetFullPath(XmlLogger._dbFilePath);
            XDocument xmlDoc = XDocument.Load(xmlFilePath);

            // Parse XML data
            var employees = xmlDoc.Descendants("DatabaseRecord").Select(x => new Employee
            {
                Id = int.Parse(x.Element("Employee").Element("Id").Value),
                Name = x.Element("Employee").Element("Name").Value,
                Email = x.Element("Employee").Element("Email").Value,
                Department = Enum.Parse<Dept>(x.Element("Employee").Element("Department").Value)
            });
            XmlLogger.maxId = xmlDoc.Descendants("DatabaseRecord")
                             .Select(x => int.Parse(x.Element("Employee").Element("Id").Value))
                             .DefaultIfEmpty(0) // Handle case where there are no records
                             .Max();
            return employees.ToList();

        }

        public Employee GetEmployee(int id)
        {
            var model = GetEmployees().FirstOrDefault(emp => emp.Id == id);
            return model;
        }

        public string UpdateEmployee(Employee employee)
        {
            {
                string xmlFilePath = Path.GetFullPath(XmlLogger._dbFilePath);
                XDocument xmlDoc = XDocument.Load(xmlFilePath);

                // Find the corresponding DatabaseRecord for the employee to be updated
                XElement recordToUpdate = xmlDoc.Descendants("DatabaseRecord")
                                               .FirstOrDefault(x => int.Parse(x.Element("Employee").Element("Id").Value) == employee.Id);
                if (recordToUpdate != null)
                {
                    Employee employeeToUpdate = ExtractEmployeeFromXml(recordToUpdate);
                    LogEmployeeAction("Updated From", employeeToUpdate);
                    // Update the values of the existing DatabaseRecord
                    recordToUpdate.Element("Employee").Element("Name").Value = employee.Name;
                    recordToUpdate.Element("Employee").Element("Email").Value = employee.Email;
                    recordToUpdate.Element("Employee").Element("Department").Value = employee.Department.ToString();
                    LogEmployeeAction("Updated To", employee);
                    // Save the updated XML document
                    xmlDoc.Save(xmlFilePath);

                    return "Updated";
                }

                // Return a message indicating that the employee with the specified ID was not found
                return $"Employee with ID {employee.Id} not found";
            }
        }

        public void DeleteEmployee(int id)
        {
            string xmlFilePath = Path.GetFullPath(XmlLogger._dbFilePath);
            XDocument xmlDoc = XDocument.Load(xmlFilePath);

            // Find the corresponding DatabaseRecord for the employee to be deleted
            XElement recordToDelete = xmlDoc.Descendants("DatabaseRecord")
                                           .FirstOrDefault(x => int.Parse(x.Element("Employee").Element("Id").Value) == id);

            if (recordToDelete != null)
            {
                Employee employeeToDeleted = ExtractEmployeeFromXml(recordToDelete);
                LogEmployeeAction("Record Deleted", employeeToDeleted);
                // Remove the DatabaseRecord from the XML document
                recordToDelete.Remove();

                // Save the updated XML document
                xmlDoc.Save(xmlFilePath);
            }
        }

        public string SaveEmployee(Employee employee)
        {
            employee.Id = XmlLogger.maxId + 1;
            string xmlFilePath = Path.GetFullPath(XmlLogger._dbFilePath);
            XDocument xmlDoc = XDocument.Load(xmlFilePath);
            XElement newRecord = new XElement("DatabaseRecord",
                new XElement("Timestamp", DateTime.UtcNow),
                new XElement("Employee",
                    new XElement("Id", employee.Id),
                    new XElement("Name", employee.Name),
                    new XElement("Email", employee.Email),
                    new XElement("Department", employee.Department)
                )
            );
            LogEmployeeAction("Employee Added", employee);
            xmlDoc.Root.Add(newRecord);
            xmlDoc.Save(xmlFilePath);
            return "Updated Successfully";
        }

        public IEnumerable<Logger> GetLogData()
        {
            string xmlFilePath = Path.GetFullPath(XmlLogger._logFilePath);
            XDocument xmlDoc = XDocument.Load(xmlFilePath);

            // Parse XML data
            var logEntries = xmlDoc.Descendants("LogEntry").Select(x => new Logger
            {
                Timestamp = DateTime.Parse(x.Element("Timestamp").Value),
                Action = x.Element("Action").Value,
                Employee = new Employee
                {
                    Id = int.Parse(x.Element("Employee").Element("Id").Value),
                    Name = x.Element("Employee").Element("Name").Value,
                    Email = x.Element("Employee").Element("Email").Value,
                    Department = Enum.Parse<Dept>(x.Element("Employee").Element("Department").Value)
                }
            });
            return logEntries.ToList();
        }

        public static void LogEmployeeAction(string action, Employee employee)
        {
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

            var soapEnvelope = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XElement(soapenv + "Header"),
                new XElement(soapenv + "Body",
                    new XElement("LogEntry",
                        new XElement("Timestamp", DateTime.UtcNow),
                        new XElement("Action", action),
                        new XElement("Employee",
                            new XElement("Id", employee.Id),
                            new XElement("Name", employee.Name),
                            new XElement("Email", employee.Email),
                            new XElement("Department", employee.Department)
                        )
                    )
                )
            );

            var existingXml = XDocument.Load(XmlLogger._logFilePath);
            existingXml.Root.Add(soapEnvelope);
            existingXml.Save(XmlLogger._logFilePath);
        }


        public Employee ExtractEmployeeFromXml(XElement xmlElement)
        {
            if (xmlElement == null)
            {
                return null;
            }

            // Extracts specific properties from XML element and create an Employee instance
            Employee employee = new Employee
            {
                Id = int.Parse(xmlElement.Element("Employee").Element("Id").Value),
                Name = xmlElement.Element("Employee").Element("Name").Value,
                Email = xmlElement.Element("Employee").Element("Email").Value,
                Department = Enum.Parse<Dept>(xmlElement.Element("Employee").Element("Department").Value)
            };
            return employee;
        }
    }
}
