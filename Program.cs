using System.Diagnostics.Metrics;
using System.Security.Principal;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovieTicketBooking
{
    #region Q1

    // a) Problems with this design:

    // 1- Balance can be modified directly from outside the class.
    //    Example: account.Balance = -500;
    // 2- Withdraw() has no validation.
    //    It may allow withdrawing negative amounts or more than the available balance.
    // 3- Auto-properties with public set break encapsulation because any code can change data freely.


    // b) How to fix it:

    // 1- Make fields/properties private or restrict modification using private set.
    //    Example:
    //    public double Balance { get; private set; }
    // 2- Add validation inside Withdraw().
    //    - amount must be > 0
    //    - balance must be sufficient
    // 3- Modify balance only through methods like Deposit() and Withdraw().


    // c) Why exposing public data is bad practice in OOP:

    // 1- Breaks encapsulation.
    // 2- Allows invalid or unsafe data changes.
    // 3- Makes objects inconsistent.
    // 4- Makes maintenance and future updates harder.
    // 5- Prevents adding control and validation logic.


    #endregion

    #region Q2

    // Q02:

    // Difference between Field and Property in C#:

    // Field:
    // - Direct variable used to store data inside a class.
    // - Usually private.
    // - Does not provide control or validation by itself.

    // Property:
    // - Provides controlled access to fields using get and set.
    // - Can contain validation or other logic.
    // - Supports encapsulation.

    // Yes, a property can contain logic.
    // Example: validation inside the set accessor.


    // Example of a read-only calculated property:

    public class Rectangle
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public double Area
        {
            get { return Width * Height; }
        }
    }
    #endregion

    #region Q3
    // a)
    // this[int index] is called an Indexer.
    // It allows objects to be accessed like arrays.

    // b)
    // If someone writes:
    //register[10] = "Ali";
    // An exception will happen because the array size is only 5.

    // To make it safer:
    // Add validation to check the index before accessing the array.

    // c)
    // Yes, a class can have more than one indexer.
    // Useful when we want to access data in different ways.

    // Example:
    //this[int index]
    //this[string name]



    #endregion

    #region Q4
    // a)
    // static means the variable belongs to the class itself, not to each object.

    // TotalOrders:
    // - Shared between all objects.
    // - One copy only.

    // Item:
    // - Non-static field.
    // - Each object has its own separate value.


    // b)
    // No, a static method cannot access Item directly.
    // Static methods can only access static members directly.
    #endregion

    #region Enum TicketType
    enum TicketType
    {
        Standard,
        VIP,
        IMAX
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
        private string movieName;
        public string MovieName {

            get { return movieName; }

            set {
                if (!string.IsNullOrEmpty(value))
                {
                    movieName = value;
                }

            }
        }
        private double price;
        public double Price
        {
            get { return price; }
            set
            {

                if (value > 0)
                    price = value;
            }

        }
        public double PriceAfterTax => price + (price * 14 / 100.0);
        public TicketType Type { get; set; }
        public Seat Seat { get; set; }

        private static int ticketCounter = 0;
        private int ticketId;
        public int TicketId => ticketId;

        public Ticket(string movieName, TicketType type, Seat seat, double price)
        {
            MovieName = movieName;
            Type = type;
            Seat = seat;
            Price = price;

            ticketCounter++;
            ticketId = ticketCounter;
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


        public static int GetTotalTicketsSold()
        {
            return ticketCounter;
        }
    }
    #endregion

    #region Cinema Class
    class Cinema
    {
        private Ticket[] tickets = new Ticket[20];


        public Ticket this[int index]
        {
            get
            {
                if (index >= 0 && index < tickets.Length)
                    return tickets[index];

                return null;
            }

            set
            {
                if (index >= 0 && index < tickets.Length)
                    tickets[index] = value;
            }
        }


        public Ticket this[string movieName]
        {
            get
            {
                foreach (Ticket t in tickets)
                {
                    if (t != null && t.MovieName == movieName)
                        return t;
                }

                return null;
            }
        }

        public bool AddTicket(Ticket t)
        {
            for (int i = 0; i < tickets.Length; i++)
            {
                if (tickets[i] == null)
                {
                    tickets[i] = t;
                    return true;
                }
            }

            return false;
        }
    }

    #endregion

    #region BookingHelper
    static  class BookingHelper
    {
        private static int counter = 0;
        public static double CalcGroupDiscount(int numberOfTickets, double pricePerTicket)
        {
            double total = numberOfTickets * pricePerTicket;

            if (numberOfTickets >= 5)
                return total - (total * 0.10);

            return total;
        }
        public static string GenerateBookingReference()
        {
            counter++;
            return $"BK-{counter}";
        }

    }
    #endregion


    #region Program
    class Program
    {
        static void Main(string[] args)
        {


            Cinema cinema = new Cinema();

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"======= Booking Ticket {i + 1} =======");
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

                Ticket ticket = new Ticket(movieName, type, new Seat(row, seatNumber), price);

                cinema.AddTicket(ticket);

                Console.WriteLine();
            }

            Console.WriteLine("========== All Tickets ==========");

            for (int i = 0; i < 3; i++)
            {
                Ticket ticket = cinema[i];

                Console.WriteLine("===== Ticket Info =====");

                Console.Write($"Ticket #: {ticket.TicketId} ");
                Console.Write($" |Movie Name : {ticket.MovieName} ");
                Console.Write($" |Type : {ticket.Type} ");
                Console.Write($" |Seat : {ticket.Seat} ");
                Console.Write($" |Price : {ticket.Price} ");
                Console.Write($" |Price AfterTax : {ticket.PriceAfterTax} ");
                Console.WriteLine();
            }


            Console.WriteLine();
            Console.WriteLine("========== Search by Movie ==========");
            Console.Write("Enter movie name to search: ");
            string? searchMovie = Console.ReadLine();
            bool found = false;
            for (int i = 0;i < 3;i++) {
                if (cinema[i]?.MovieName == searchMovie) {
                    Ticket ticket = cinema[i];
                    Console.Write("Found: ");
                    Console.Write($"Ticket #: {ticket.TicketId} ");
                    Console.Write($" | Movie Name : {ticket.MovieName} ");
                    Console.Write($" | Type : {ticket.Type} ");
                    Console.Write($" | Seat : {ticket.Seat} ");
                    Console.Write($" | Price : {ticket.Price} ");
                    Console.Write($" | Price AfterTax : {ticket.PriceAfterTax} ");
                    Console.WriteLine();
                    found = true;
                    break;
                }
                
            }
            if (!found) {
                Console.WriteLine("Not Found");
            }



            Console.WriteLine();
            Console.WriteLine("========== Total Tickets ==========");
            Console.WriteLine($"Total Tickets Sold: {Ticket.GetTotalTicketsSold()}");

            Console.WriteLine();
            Console.WriteLine("========== Booking References ==========");
            string ref1 = BookingHelper.GenerateBookingReference();
            string ref2 = BookingHelper.GenerateBookingReference();

            Console.WriteLine(ref1);
            Console.WriteLine(ref2);

            Console.WriteLine();
            Console.WriteLine("========== Group Discount ==========");
            double total = BookingHelper.CalcGroupDiscount(5, 80);
            Console.WriteLine($"Total Price after discount: {total}");
        }







    }
    #endregion
}