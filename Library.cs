using System;
using System.Collections.Generic;

namespace LMS
{
    internal class Library
    {
        public class Details
        {

            public int BookId;
            public string BookName;
            public string BookAuthor;
            public string BookPublisher;
            public int BookCount;

            public Details(int bookId, string bookName, string bookAuthor, string bookPublisher, int bookCount)
            {
                this.BookId = bookId;
                this.BookName = bookName;
                this.BookAuthor = bookAuthor;
                this.BookPublisher = bookPublisher;
                this.BookCount = bookCount;
            }
        }

        public interface ibookrepository
        {
            void AddBook(Details book);
            void RemoveBook(int bookId);
            Details GetBookById(int bookId);
            List<Details> GetAllBooks();
            void UpdateBookCount(int bookId, int newCount);
        }

        public class BookRepository : ibookrepository
        {
            private  List<Details> detailsList = new List<Details>();

            public void AddBook(Details book)
            {
                detailsList.Add(book);
            }

            public void RemoveBook(int bookId)
            {
                var book = detailsList.FirstOrDefault(a => a.BookId == bookId);
                if(book != null)
                {
                    detailsList.Remove(book);
                }
            }

            public Details GetBookById(int bookId)
            {
                 return detailsList.FirstOrDefault(a => a.BookId == bookId);

            }

            public List<Details> GetAllBooks()
            {
                return new List<Details>(detailsList);
            }

            public void UpdateBookCount(int bookId, int newCount)
            {
                var book = GetBookById(bookId);
                if(book != null)
                {
                    book.BookCount = newCount;
                }
            }
        }
        class LibrarySystem
        {
            private static int lastBookId = 0;
            private ibookrepository _ibookrepository;
            public int minLength;
            public int maxLength;

            public LibrarySystem(ibookrepository ibookrepository)
            {
                _ibookrepository = ibookrepository;
            }
            public void AddBook()
            {
                Console.WriteLine();
                Console.WriteLine("********************* ADD BOOK DETAILS BELOW ************************");
                Console.WriteLine();

                int bookId = ++lastBookId;
                Console.WriteLine($"Book ID: {bookId}");

                string bookName;
                minLength = 5;
                maxLength = 50;

                do
                {
                    Console.Write("Enter Book Name: ");
                    bookName = Console.ReadLine();

                    if (string.IsNullOrEmpty(bookName))
                    {
                            Console.WriteLine("Book Name cannot be empty. Please enter a valid name.");
                    }
                    else if (bookName.Length < minLength || bookName.Length > maxLength)
                    {
                        Console.WriteLine($"Book Name must be between {minLength} and {maxLength} characters long.");
                    }

                } while (string.IsNullOrEmpty(bookName) || bookName.Length < minLength || bookName.Length > maxLength);

                string bookAuthor;

                do
                {
                    Console.Write("Enter Book Author: ");
                    bookAuthor = Console.ReadLine();

                    if (string.IsNullOrEmpty(bookAuthor))
                    {
                        Console.WriteLine("Book Author cannot be empty. Please enter a valid author.");
                    }
                    else if (bookAuthor.Length < minLength || bookAuthor.Length > maxLength)
                    {
                        Console.WriteLine($"Book Author must be between {minLength} and {maxLength} characters long.");
                    }
                } while (string.IsNullOrEmpty(bookAuthor) || bookAuthor.Length < minLength || bookAuthor.Length > maxLength);

                string bookPublisher;

                do
                {
                    Console.Write("Enter Book Publisher: ");
                    bookPublisher = Console.ReadLine();

                    if (string.IsNullOrEmpty(bookPublisher))
                    {
                        Console.WriteLine("Book Publisher cannot be empty. Please enter a valid publisher.");
                    }
                    else if (bookPublisher.Length < minLength || bookAuthor.Length > maxLength)
                    {
                        Console.WriteLine($"Book Publisher must be between {minLength} and {maxLength} characters long.");
                    }

                } while (string.IsNullOrEmpty(bookPublisher) || bookPublisher.Length < minLength || bookPublisher.Length > maxLength);

                int bookCount;

                do
                {
                    Console.Write("Enter No of Books: ");
                    string bookCountInput = Console.ReadLine();

                    if (!int.TryParse(bookCountInput, out bookCount) || bookCount <= 0)
                    {
                        Console.WriteLine("Please enter a valid number greater than 0.");
                    }
                } while (bookCount <= 0);

                Details newBook = new Details(bookId, bookName, bookAuthor, bookPublisher, bookCount);
                _ibookrepository.AddBook(newBook);

                Console.WriteLine();
                Console.WriteLine("Added Book Details Successfully");
                Console.WriteLine();
            }


            public void DeleteBook()
            {
                Console.WriteLine();
                Console.WriteLine("************************ DELETE BOOK ******************************");

                Console.WriteLine();

                Console.Write("Enter Book ID: ");
                int bookId = ReadIntegerInput();

                var book = _ibookrepository.GetBookById(bookId);

                if (book != null)
                {
                    _ibookrepository.RemoveBook(bookId);
                    Console.WriteLine($"Deleted Book Id {bookId} Successfully");
                }
                else
                {
                    Console.WriteLine("Book Id not Found");
                }
            }

            public void SearchBook()
            {
                Console.WriteLine();
                Console.WriteLine("********************** Search Book ************************");

                Console.WriteLine();

                Console.Write("Search by BookId : ");
                int searchBookId = ReadIntegerInput();

                var foundBook = _ibookrepository.GetBookById(searchBookId);

                if (foundBook != null)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Book Found. Below are the details for book id {searchBookId} ");
                    DisplayBookDetails(foundBook);
                }
                else
                {
                    Console.WriteLine("Book not Found");
                }
            }


            public void BorrowBooks()
            {
                Console.WriteLine();
                Console.WriteLine("***************************** Borrow Book *******************************");

                Console.WriteLine();

                Console.Write("Enter BookId : ");
                int BorrowBookId = ReadIntegerInput();

                var Borrowbook = _ibookrepository.GetBookById(BorrowBookId);

                if (Borrowbook != null)
                {
                    if (Borrowbook.BookCount > 0)
                    {
                        Borrowbook.BookCount--;
                        Console.WriteLine($"Book '{Borrowbook.BookName}' borrowed successfully! \nRemaining copies: {Borrowbook.BookCount}");
                    }
                    else
                    {
                        Console.WriteLine($"Sorry, all copies of '{Borrowbook.BookName}' are currently borrowed.");
                    }
                }
                else
                {
                    Console.WriteLine("Book ID not found.");
                }
            }

            public void ReturnBook()
            {
                Console.WriteLine();
                Console.WriteLine("************************* Return Book **************************");

                Console.WriteLine();

                Console.Write("Enter Book ID : ");
                int ReturnBookId = ReadIntegerInput();

                var returnBookbyId = _ibookrepository.GetBookById(ReturnBookId);

                if (returnBookbyId != null)
                {
                    Console.Write("Enter How many books you want to Return : ");
                    int ReturnBookCount = ReadIntegerInput();

                    returnBookbyId.BookCount -= ReturnBookCount;
                    Console.WriteLine($"Book {returnBookbyId.BookName} returned successfully! Updated copies: {returnBookbyId.BookCount}");
                }
                else
                {
                    Console.WriteLine("Book ID not found.");
                }
            }

            public void UpdateBookQuantity()
            {
                Console.Write("Enter the BookId : ");
                int bookId = ReadIntegerInput();

                Details BookToUpdate = _ibookrepository.GetBookById(bookId);

                if (BookToUpdate != null)
                {
                    Console.Write("Enter Book Count : ");
                    int newCount = ReadIntegerInput();

                    BookToUpdate.BookCount += newCount;
                    Console.WriteLine($"Updated quantity of book ID {bookId} to {newCount}");
                }
                else
                {
                    Console.WriteLine("Book Quantity not updated");
                }
            }

            public void ViewAllBooks()
            {
                Console.WriteLine("*********************** View All Books ************************");

                Console.WriteLine();

                var allbooks = _ibookrepository.GetAllBooks();

                if (allbooks.Count == 0)
                {
                    Console.WriteLine("No Books Added in Library");
                }
                else
                {
                    foreach (var item in allbooks)
                    {
                        DisplayBookDetails(item);
                    }
                }
                Console.WriteLine();
            }




            public void Close()
            {
                Console.WriteLine();
                Console.WriteLine("Thank you for visiting E-library");
            }

            private void DisplayBookDetails(Details book)
            {
                Console.WriteLine();
                Console.WriteLine($"Book ID :{book.BookId}");
                Console.WriteLine($"Book Name :{book.BookName}");
                Console.WriteLine($"Book Author :{book.BookAuthor}");
                Console.WriteLine($"Book Publisher :{book.BookPublisher}");
                Console.WriteLine($"Book Count :{book.BookCount}");
                Console.WriteLine();
            }

            private int ReadIntegerInput()
            {
                int input;

                while (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine();
                    Console.Write("Invalid input. Please enter a valid number :");
                }
                return input;
            }
            public bool AdminLogin()
            {
                Console.WriteLine("********************* ADMIN LOGIN ************************");
                Console.Write("Enter Admin Username: ");
                string adminUsername = Console.ReadLine();

                Console.Write("Enter Admin Password: ");
                string adminPassword = Console.ReadLine();

                if (adminUsername == "Admin@123" && adminPassword == "Admin@123")
                {
                    Console.WriteLine("Admin Login Successful");
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid Admin Credentials");
                    return false;
                }
            }

            public bool UserLogin()
            {
                Console.WriteLine("********************* USER LOGIN ************************");
                Console.Write("Enter Username: ");
                string userUsername = Console.ReadLine();

                Console.Write("Enter Password: ");
                string userPassword = Console.ReadLine();

                if (userUsername == "User@123" && userPassword == "User@123")
                {
                    Console.WriteLine("User Login Successful");
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid User Credentials");
                    return false;
                }
            }

            public void AdminMenu()
            {
                bool select = true;

                while (select)
                {
                    Console.WriteLine();
                    Console.WriteLine("1. Add Book ");
                    Console.WriteLine("2. Delete Book ");
                    Console.WriteLine("3. Update Book Quantity ");
                    Console.WriteLine("4. View All Books ");
                    Console.WriteLine("5. Close ");

                    Console.WriteLine();

                    Console.Write("Select your Option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddBook();
                            break;
                        case "2":
                            DeleteBook();
                            break;
                        case "3":
                            UpdateBookQuantity();
                            break;
                        case "4":
                            ViewAllBooks();
                            break;
                        case "5":
                            Close();
                            select = false;
                            break;
                        default:
                            Console.WriteLine("Select only from 1-5");
                            break;
                    }
                }
            }

            public void UserMenu()
            {
                bool select = true;

                while (select)
                {
                    Console.WriteLine();
                    Console.WriteLine("1. Search Book ");
                    Console.WriteLine("2. Borrow Book ");
                    Console.WriteLine("3. Return Book ");
                    Console.WriteLine("4. View All Books ");
                    Console.WriteLine("5. Close ");

                    Console.WriteLine();

                    Console.Write("Select your Option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            SearchBook();
                            break;
                        case "2":
                            BorrowBooks();
                            break;
                        case "3":
                            ReturnBook();
                            break;
                        case "4":
                            ViewAllBooks();
                            break;
                        case "5":
                            Close();
                            select = false;
                            break;
                        default:
                            Console.WriteLine("Select only from 1-5");
                            break;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            ibookrepository _ibookrepository = new BookRepository();
            LibrarySystem librarySystem = new LibrarySystem(_ibookrepository);

            Console.WriteLine("********************* WELCOME TO E-LIBRARY ************************");
            Console.WriteLine();

            bool isRunning = true;

            while (isRunning)
            {
                Console.WriteLine("1. Admin Login");
                Console.WriteLine("2. User Login");
                Console.WriteLine("3. Exit");

                Console.WriteLine();
                Console.Write("Select your Option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (librarySystem.AdminLogin())
                        {
                            librarySystem.AdminMenu();
                        }
                        break;
                    case "2":
                        if (librarySystem.UserLogin())
                        {
                            librarySystem.UserMenu();
                        }
                        break;
                    case "3":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Select only from 1-3");
                        break;
                }
            }

            Console.WriteLine("Exiting the application...");
            Console.ReadLine();
        }
    }
}
