using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ContactBook
{
    string filePath = "database.json";

    public void database()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
            Console.WriteLine("Database created.");
        }
    }

    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProfileIcon { get; set; }
    }

    public void AddContact(Contact newContact)
    {
        var contacts = LoadContacts();
        contacts.Add(newContact);
        SaveContacts(contacts);
        Console.WriteLine("Contact added successfully.");
    }

    public void EditContact(int id, string newName, string newPhone, string newEmail)
    {
        var contacts = LoadContacts();
        var contact = contacts.FirstOrDefault(c => c.Id == id);
        if (contact != null)
        {
            contact.Name = newName;
            contact.PhoneNumber = newPhone;
            contact.Email = newEmail;
            SaveContacts(contacts);
            Console.WriteLine("Contact updated successfully.");
        }
        else
        {
            Console.WriteLine("Contact not found.");
        }
    }

    public void DeleteContact(int id)
    {
        var contacts = LoadContacts();
        var updatedContacts = contacts.Where(c => c.Id != id).ToList();
        SaveContacts(updatedContacts);
        Console.WriteLine("Contact deleted successfully.");
    }

    public void ViewContacts()
    {
        var contacts = LoadContacts();
        foreach (var contact in contacts)
        {
            Console.WriteLine($"Id: {contact.Id}, Name: {contact.Name}, Phone: {contact.PhoneNumber}, Email: {contact.Email}");
        }
    }

    private List<Contact> LoadContacts()
    {
        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
    }

    private void SaveContacts(List<Contact> contacts)
    {
        string json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }

    public static void Main(string[] args)
    {
        var contactBook = new ContactBook();
        contactBook.database();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("\nContact Manager:");
            Console.WriteLine("1. Add Contact");
            Console.WriteLine("2. Edit Contact");
            Console.WriteLine("3. Delete Contact");
            Console.WriteLine("4. View Contacts");
            Console.WriteLine("5. Exit");

            Console.Write("Choose an option: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Press Enter to try again");
                Console.ReadLine();
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.Clear();
                    var newContact = new Contact();
                    Console.Write("Enter Id: ");
                    newContact.Id = int.Parse(Console.ReadLine());
                    Console.Write("Enter Name: ");
                    newContact.Name = Console.ReadLine();
                    Console.Write("Enter Phone Number: ");
                    newContact.PhoneNumber = Console.ReadLine();
                    Console.Write("Enter Email: ");
                    newContact.Email = Console.ReadLine();
                    contactBook.AddContact(newContact);
                    Console.WriteLine("Press Enter to return to menu.");
                    Console.ReadLine();
                    break;

                case 2:
                    Console.Clear();
                    Console.Write("Enter Id of the contact to edit: ");
                    int editId = int.Parse(Console.ReadLine());
                    Console.Write("Enter new Name: ");
                    string newName = Console.ReadLine();
                    Console.Write("Enter new Phone Number: ");
                    string newPhone = Console.ReadLine();
                    Console.Write("Enter new Email: ");
                    string newEmail = Console.ReadLine();
                    contactBook.EditContact(editId, newName, newPhone, newEmail);
                    Console.WriteLine("Press Enter to return to menu.");
                    Console.ReadLine();
                    break;

                case 3:
                    Console.Clear();
                    Console.Write("Enter Id of the contact to delete: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    contactBook.DeleteContact(deleteId);
                    Console.WriteLine("Press Enter to return to menu.");
                    Console.ReadLine();
                    break;

                case 4:
                    Console.Clear();
                    contactBook.ViewContacts();
                    Console.WriteLine("Press Enter to return to menu.");
                    Console.ReadLine();
                    break;

                case 5:
                    Console.Clear();
                    Console.WriteLine("bye");
                    return;

                default:
                    Console.Clear();
                    Console.WriteLine("Press Enter to try again");
                    Console.ReadLine();
                    break;
            }
        }
    }
}