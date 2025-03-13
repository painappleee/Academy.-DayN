using Microsoft.EntityFrameworkCore;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
 
public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }

}

public class Enrollment
{
    public int Id { get; set; }

    public int StudentId {  get; set; }

    public int CourseId { get; set; }

    public double Grade { get; set; }

    public Student Student { get; set; }

    public Course Course { get; set; }
}

public class UniversityContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("UniversityDB");
    }
}

class Program
{
    static void Main()
    {
        using (var context = new UniversityContext())
        {
            while (true)
            {
                Console.WriteLine("\nВыберите действие");
                Console.WriteLine("1 - Добавить студента");
                Console.WriteLine("2 - Добавить курс");
                Console.WriteLine("3 - Записать студента на курс");
                Console.WriteLine("4 - Показать список студентов с их курсами");
                Console.WriteLine("5 - Показать список курсов");
                Console.WriteLine("6 - Обновить данные студента");
                Console.WriteLine("7 - Удалить студента");
                Console.WriteLine("8 - Поиск студента по возрасту");
                Console.WriteLine("9 - Поиск курсов у студента");
                Console.WriteLine("10 - Показать всех студентов, отсортированных по возрасту");
                Console.WriteLine("11 - Показать лучших студентов");
                Console.WriteLine("12 - Показать студентов без курсов");

                Console.WriteLine("t - Заполнить БД тестовыми данными");
                Console.WriteLine("q - Выйти");

                string choice = Console.ReadLine();
                Console.Clear();
                switch (choice)
                {
                    case "1":
                        AddStudent(context);
                        break;
                    case "2":
                        AddCourse(context);
                        break;
                    case "3":
                        EnrollStudent(context);
                        break;
                    case "4":
                        QueryStudents(context);
                        break;
                    case "5":
                        ShowAllCourses(context);
                        break;
                    case "6":
                        UpdateStudent(context);
                        break;
                    case "7":
                        DeleteStudent(context);
                        break;
                    case "8":
                        FindStudentByAge(context);
                        break;
                    case "9":
                        FindCoursesByStudent(context);
                        break;
                    case "10":
                        ShowAllStudentsOrderedByAge(context);
                        break;
                    case "11":
                        ShowBestStudents(context);
                        break;
                    case "12":
                        FindStudentsWithoutCourses(context);
                        break;
                    case "t":
                        SeedDatabase(context);
                        break;
                    case "q":
                        break;
                    default:
                        Console.WriteLine("Некорректный ввод");
                        break;
                }

            }
        }
    }

    static void SeedDatabase(UniversityContext context)
    {

        if (!context.Students.Any() && !context.Courses.Any())
        {
            var students = new List<Student>
            {
                new Student { Name="Алиса", Age=20 },
                new Student { Name="Владимир", Age=21 },
                new Student { Name="Максим", Age=20 },
                new Student { Name="Дмитрий", Age=20 }
            };

            var courses = new List<Course>
            {
                new Course { Title = "Высшая математика" },
                new Course { Title = "Нейтронная физика" },
                new Course { Title = "Социология" },

            };


            context.Students.AddRange(students);
            context.Courses.AddRange(courses);
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment
                {
                    StudentId = students[0].Id,
                    CourseId = courses[0].Id,
                    Grade = 4
                },
                new Enrollment
                {
                    StudentId = students[1].Id,
                    CourseId = courses[1].Id,
                    Grade = 3
                },
                new Enrollment
                {
                    StudentId = students[0].Id,
                    CourseId = courses[1].Id,
                    Grade = 5
                },
                new Enrollment
                {
                    StudentId = students[2].Id,
                    CourseId = courses[2].Id,
                    Grade = 5
                },
                new Enrollment
                {
                    StudentId = students[3].Id,
                    CourseId = courses[2].Id,
                    Grade = 3
                }
            };

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
    }

    static void AddStudent(UniversityContext context)
    {
        Console.Write("Введите имя студента: ");
        string name = Console.ReadLine();
        Console.Write("Введите возраст студента: ");
        int age = int.Parse(Console.ReadLine());

        var student = new Student { Name = name, Age = age };
        context.Students.Add(student);
        context.SaveChanges();
        Console.WriteLine($"Студент {name} добавлен");
    }

    static void AddCourse(UniversityContext context)
    {
        Console.Write("Введите название курса: ");
        string title = Console.ReadLine();
        var course = new Course { Title = title };
        context.Courses.Add(course);
        context.SaveChanges();
        Console.WriteLine($"Курс {title} добавлен");
    }

    static void ShowAllStudents(UniversityContext context)
    {
        Console.WriteLine("\nСписок всех студентов: ");
        var students  = context.Students.ToList();
        foreach (var student in students) {
            Console.WriteLine($"ID: {student.Id}, Имя: {student.Name}, Возраст: {student.Age}");
        }
    }

    static void ShowAllCourses(UniversityContext context)
    {
        Console.WriteLine("\nСписок всех курсов: ");
        var courses = context.Courses.ToList();
        foreach (var course in courses)
        {
            Console.WriteLine($"ID: {course.Id}, Название: {course.Title}");
        }
    }

    static void EnrollStudent(UniversityContext context)
    {
        ShowAllStudents(context);
        Console.Write("Введите ID студента");
        int studentId = int.Parse(Console.ReadLine());
        Console.Clear();
        ShowAllCourses(context);
        Console.Write("Введите ID курса: ");
        int courseId = int.Parse(Console.ReadLine());
        Console.Clear();
        Console.Write("Введите оценку");
        double grade = double.Parse(Console.ReadLine());
        Console.Clear();

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            Grade = grade
        };

        context.Enrollments.Add(enrollment);
        context.SaveChanges();
        Console.WriteLine("Студент записан на курс");

    }

    static void QueryStudents(UniversityContext context)
    {
        var students = context.Students.Include(s=>s.Enrollments).ThenInclude(e=>e.Course).ToList();
        foreach(var student in students)
        {
            Console.WriteLine($"ID: {student.Id}, Имя: {student.Name}, Возраст: {student.Age}");
            foreach(var enrollment in student.Enrollments)
            {
                Console.WriteLine($"       -  Курс: {enrollment.Course.Title}, Оценка: {enrollment.Grade}");
            }
        }
    }
    
    static void UpdateStudent(UniversityContext context)
    {
        ShowAllStudents(context);
        Console.Write("Введите ID студента: ");
        int id  = int.Parse(Console.ReadLine());
        var student = context.Students.Find(id);
        if (student != null)
        {
            Console.Write("Введите новое имя: ");
            student.Name = Console.ReadLine();
            Console.Write("Введите новый возраст: ");
            student.Age = int.Parse(Console.ReadLine());
            context.SaveChanges();
            Console.WriteLine("Данные обновлены");
        }
        else Console.WriteLine("Студент не найден");
    }

    static void DeleteStudent(UniversityContext context)
    {
        ShowAllStudents(context);
        Console.Write("Введите ID студента: ");
        int id = int.Parse(Console.ReadLine());
        var student = context.Students.Find(id);
        if (student != null)
        {
            context.Students.Remove(student);
            context.SaveChanges();
            Console.WriteLine("Студент удалён");
        }
        else Console.WriteLine("Студент не найден");
    }

    static void FindStudentByAge(UniversityContext context)
    {
        Console.WriteLine("Введите возраст для поиска студентов: ");
        int age = int.Parse(Console.ReadLine());
        var students = context.Students.Where(s => s.Age == age).ToList();
        Console.WriteLine($"Студенты с возрастом {age}:");
        foreach(var student in students)
        {
            Console.WriteLine($"ID: {student.Id}, Имя: {student.Name}, Возраст: {student.Age}");
        }

    }

    static void FindCoursesByStudent(UniversityContext context)
    {
        ShowAllStudents(context);
        Console.WriteLine("Введите ID студента для поиска курсов: ");
        int studentId = int.Parse(Console.ReadLine());
        var courses = context.Enrollments.Where(e=>e.StudentId== studentId).Select(e=>e.Course).ToList();
        Console.WriteLine($"Курсы, на которые записан студент ID {studentId}");
        foreach(var course in courses)
        {
            Console.WriteLine($"ID: {course.Id}, Название: {course.Title}");
        }

    }


    // 1. Выведение всех студентов, отсортированных по возрасту(по убыванию и по возрастанию)
    // 1 студент 1, 27 лет
    // 2 студент 2, 22 лет
    // ...

    static void ShowAllStudentsOrderedByAge(UniversityContext context)
    {
        Console.Write("Выберите способ сортировки(1 - по возрастанию, 2 - по убыванию): ");
        string sortBy = Console.ReadLine();
        var students = new List<Student>();
        if (sortBy.Equals("1"))
        {
            students = context.Students.OrderBy(s => s.Age).ToList();
        }
        else if (sortBy.Equals("2"))
        {
            students = context.Students.OrderByDescending(s => s.Age).ToList();
        }
        else
        {
            Console.WriteLine("Некорректный ввод");
            return;
        }

        foreach (var student in students)
        {
            Console.WriteLine($"{student.Name} - {student.Age}");
        }
    }

    // 2. Показать студентов, у которых средний балл выше 4. Определить студента с наивысшем средним баллом и показать его.
    static void ShowBestStudents(UniversityContext context)
    {
        var goodStudents = context.Students.Where(s=>s.Enrollments.Any())
            .Select(s=> new { Student = s, AvgGrade = s.Enrollments.Average(e => e.Grade)})
            .Where(s=>s.AvgGrade>4)
            .OrderByDescending(s=>s.AvgGrade);

        Console.WriteLine("Студенты со средним баллом выше 4:");
        foreach (var student in goodStudents)
        {
            Console.WriteLine($"{student.Student.Name}  {student.AvgGrade}");
        }

        var bestStudent = goodStudents.FirstOrDefault();
        if (bestStudent != null)
            Console.WriteLine($"Лучший студент: {bestStudent.Student.Name}  {bestStudent.AvgGrade}");
        
    }

    // 3. Найти и вывести студентов без курсов. Добавить возможность рекомендовать записаться на курс с наибольшим количеством студентов(первый попавшийся)
    static void FindStudentsWithoutCourses(UniversityContext context)
    {
        var students = context.Students.Where(s => !s.Enrollments.Any());
        Console.WriteLine("Студенты без курсов: ");
        foreach (var student in students) {
            Console.WriteLine(student.Name);
        }


        var popularCourse = context.Courses.Select(c => new { Course = c.Title, StudentsCol = context.Enrollments
                                           .Count(e => e.CourseId == c.Id) })
                                           .OrderByDescending(c => c.StudentsCol)
                                           .FirstOrDefault();
        if (popularCourse != null)
            Console.WriteLine($"Рекомендуем записаться на самый популярный курс: {popularCourse.Course} - {popularCourse.StudentsCol} чел.");


    }

}