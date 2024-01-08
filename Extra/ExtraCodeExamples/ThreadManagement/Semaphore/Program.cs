int studentNumber = 7;
int maxStudentInLibrary = 3;

// create an instance of Semaphore with a set number of threads(students)
Semaphore semaphore = new Semaphore(maxStudentInLibrary, maxStudentInLibrary);

// students decided to go to the library
for (int i = 1; i <= studentNumber; i++)
{
    Thread student = new Thread(VisitLibrary);
    student.Name = $"Student {i}";
    student.Start();
}

// simulation of visiting the library
void VisitLibrary()
{
    // admitting only students for whom there is space and not allowing others to enter
    semaphore.WaitOne();

    try
    {
        string currentStudent = Thread.CurrentThread.Name!;
        Random random = new Random();

        Console.WriteLine($"{currentStudent} entered the library.");

        Thread.Sleep(100);

        Console.WriteLine($"{currentStudent} is reading a book");

        Thread.Sleep(random.Next(100, 1000));

        Console.WriteLine($"{currentStudent} left the library");
    }
    finally 
    {
        //  student leaves the library, freeing up a spot
        semaphore.Release(); 
    }
}