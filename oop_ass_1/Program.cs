
// ─────────────────────────────────────────────────────────────
//  PART 01 
// ─────────────────────────────────────────────────────────────

#region Q1

/*
 * CLASS  → Reference type (stored on the Heap).
 *           Assigning one variable to another copies the REFERENCE.
 *
 * STRUCT → Value type (stored on the Stack).
 *           Assigning one variable to another copies the VALUE.
 */

// Example:
/*
class PointClass
{
    public int X, Y;
}

struct PointStruct
{
    public int X, Y;
}

// Usage:
PointClass c1 = new PointClass { X = 1, Y = 2 };
PointClass c2 = c1;          // c2 points to SAME object
c2.X = 99;
Console.WriteLine(c1.X);     // Output: 99  (both changed)

PointStruct s1 = new PointStruct { X = 1, Y = 2 };
PointStruct s2 = s1;         // s2 is an INDEPENDENT copy
s2.X = 99;
Console.WriteLine(s1.X);     // Output: 1   (s1 unchanged)
*/

#endregion

#region Q2 

/*
 * public  → accessible from ANYWHERE (same class, other classes, other projects).
 * private → accessible ONLY inside the class it is declared in.
 */

// Example:
/*
class BankAccount
{
    public  string Owner   = "Ali";   // anyone can read/write
    private double balance = 1000;    // only BankAccount methods can touch it

    public double GetBalance() => balance;          // controlled access
    public void   Deposit(double amount) { balance += amount; }
}

BankAccount acc = new BankAccount();
Console.WriteLine(acc.Owner);          // OK
Console.WriteLine(acc.GetBalance());   // OK  → 1000
// Console.WriteLine(acc.balance);     // ERROR — private
*/

#endregion

#region Q3 

/*
 1. File → New → Project → select "Class Library (.NET)"  → name it (e.g. MyLibrary).
 2. Write your classes inside the library project.
 3. Build the library (Build → Build Solution) → produces MyLibrary.dll.
 4. In your Console/Web project: right-click "Dependencies" → Add Project Reference
    → tick MyLibrary → OK.
 5. Add   using MyLibrary;   at the top of your file, then use the classes normally.
*/

#endregion

#region Q4 

/*
 * A Class Library is a compiled DLL that contains reusable classes, interfaces,
 * and methods — but has NO entry point (no Main method).
 *
 * Why use it?
 *   • Reusability  — share the same code across multiple projects.
 *   • Separation of Concerns — keeps business logic away from UI.
 *   • Maintainability — fix a bug once, all consumers benefit.
 *   • Encapsulation — expose only a clean public API; hide implementation.
*/

#endregion


// ─────────────────────────────────────────────────────────────
//  PART 02 
// ─────────────────────────────────────────────────────────────

using System;

namespace MovieTicketBooking
{
   
    #region Enum TicketType
    enum TicketType
    {
        Standard = 0,
        VIP = 1,
        IMAX = 2
    }
    #endregion

    
    #region Struct Seat
    
    struct Seat
    {
        public char Row;
        public int Number;

        public Seat(char row, int number)
        {
            Row = row;
            Number = number;
        }

        public override string ToString() => $"{Row}{Number}";
    }
    #endregion

    
    #region Class Ticket
    class Ticket
    {
       
        public string MovieName { get; set; }
        public TicketType Type { get; set; }
        public Seat Seat { get; set; }
        private double Price { get; set; }

       
        public Ticket(string movieName, TicketType type, Seat seat, double price)
        {
            MovieName = movieName;
            Type = type;
            Seat = seat;
            Price = price;
        }

        
        public Ticket(string movieName)
            : this(movieName, TicketType.Standard, new Seat('A', 1), 50)
        { }

       
        public double CalcTotal(double taxPercent)
        {
            
            return Price + (Price * taxPercent / 100.0);
        }

       
        public void ApplyDiscount(ref double discountAmount)
        {
            if (discountAmount > 0 && discountAmount <= Price)
            {
                Price -= discountAmount;
                discountAmount = 0;        
            }
            
        }

        
        public void PrintTicket(double taxPercent)
        {
            Console.WriteLine("===== Ticket Info =====");
            Console.WriteLine($"Movie     : {MovieName}");
            Console.WriteLine($"Type      : {Type}");
            Console.WriteLine($"Seat      : {Seat}");
            Console.WriteLine($"Price     : {Price:F2}");
            Console.WriteLine($"Total ({taxPercent}% tax) : {CalcTotal(taxPercent):F2}");
        }
    }
    #endregion

   
    #region Program
    class Program
    {
        static void Main(string[] args)
        {
           
            Console.Write("Enter Movie Name: ");
            string movieName = Console.ReadLine();

            Console.Write("Enter Ticket Type (0 = Standard , 1 = VIP , 2 = IMAX ): ");
            TicketType type = (TicketType)int.Parse(Console.ReadLine());

            Console.Write("Enter Seat Row (A, B, C...): ");
            char row = char.ToUpper(Console.ReadLine()[0]);

            Console.Write("Enter Seat Number: ");
            int seatNumber = int.Parse(Console.ReadLine());

            Console.Write("Enter Price: ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Enter Discount Amount: ");
            double discount = double.Parse(Console.ReadLine());

            double taxPercent = 14;

            
            Ticket ticket = new Ticket(movieName, type, new Seat(row, seatNumber), price);

            Console.WriteLine();

            
            ticket.PrintTicket(taxPercent);

            
            double discountBefore = discount;
            ticket.ApplyDiscount(ref discount);

            Console.WriteLine();
            Console.WriteLine("===== After Discount =====");
            Console.WriteLine($"Discount Before : {discountBefore:F2}");
            Console.WriteLine($"Discount After  : {discount:F2}");

           
            ticket.PrintTicket(taxPercent);
        }
    }
    #endregion
}
