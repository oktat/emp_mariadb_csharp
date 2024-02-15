using Microsoft.Data.Sqlite;

var conn = new SqliteConnection ("Data Source=database.db");

menu();

void menu() {
    string? key = "";
    while(key != "vege") {
        showMenu();
        key = Console.ReadLine();
        Console.WriteLine();
        if (key == "1") {
            Console.WriteLine("Dolgozók megjelenítése...");
            readEmployees();
            Console.WriteLine("Folytatáshoz Enter...");
            Console.ReadLine();
        }
        if (key == "2") {
            Console.WriteLine("Új dolgozó felvétele jön...");
            insertEmployee();
            Console.WriteLine("Folytatáshoz Enter...");
            Console.ReadLine();
        }
        if (key == "3") {
            Console.WriteLine("Tábla létrehozása jön...");
            createTable();
            Console.WriteLine("Folytatáshoz Enter...");
            Console.ReadLine();
        }
        if (key == "0") {
            key = "vege";
        }

    }
}

void showMenu() {
    Console.WriteLine("---Menü---");
    Console.WriteLine("1) Olvas");
    Console.WriteLine("2) Új dolgozó");
    Console.WriteLine("3) Tábla létrehozása");
    Console.WriteLine("0) Kilépés");
    Console.WriteLine("----------");
    Console.Write("Válasz: ");
}

void readEmployees() {
    conn.Open();
    SqliteCommand cmd = conn.CreateCommand();
    string sql = "select * from employees";
    cmd.CommandText = sql;
    SqliteDataReader reader = cmd.ExecuteReader();
    while(reader.Read()) {
        Console.WriteLine("{0} {1,15} {2,10} {3} {4}", 
            reader.GetString(0), 
            reader.GetString(1),
            reader.GetString(2),
            reader.GetString(3),
            reader.GetString(4)
            );
    }
    conn.Close();
}

void insertEmployee() {
    conn.Open();
    SqliteCommand cmd = conn.CreateCommand();    
    Console.Write("Név: ");
    string? name = Console.ReadLine();
    Console.Write("Település: ");
    string? city = Console.ReadLine();
    Console.Write("Fizetés: ");
    string? salary = Console.ReadLine();
    Console.Write("Születés: ");
    string? birth = Console.ReadLine();

    string sql = @"
        insert into employees
        (name, city, salary, birth)
        values
        (@name, @city, @salary, @birth)
    ";
    cmd.CommandText = sql;
    cmd.Parameters.AddWithValue("@name", name);
    cmd.Parameters.AddWithValue("@city", city);
    cmd.Parameters.AddWithValue("@salary", salary);
    cmd.Parameters.AddWithValue("@birth", birth);
    cmd.Prepare();
    cmd.ExecuteNonQuery();
    conn.Close();
    Console.WriteLine("Beszúrás vége.");
}

void createTable() {
    conn.Open();
    SqliteCommand cmd = conn.CreateCommand();    
    string sql = @"
        create table if not exists employees(
            id integer not null primary key autoincrement,
            name varchar(50),
            city varchar(50),
            salary double,
            birth date
        )
    ";
    cmd.CommandText = sql;
    cmd.ExecuteNonQuery();
    conn.Close();
}
