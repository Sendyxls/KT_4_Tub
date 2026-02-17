using System;
using System.Collections.Generic;
using System.Linq;

// 1. Интерфейс IPrototype
public interface IPrototype<T>
{
    T Clone(); // Поверхностное копирование
    T DeepClone(); // Глубокое копирование
}

// 2. Вложенный класс - производитель инструмента
public class Manufacturer
{
    public string Name { get; set; }
    public string Country { get; set; }
    public int YearFounded { get; set; }

    public Manufacturer(string name, string country, int yearFounded)
    {
        Name = name;
        Country = country;
        YearFounded = yearFounded;
    }

    // Конструктор копирования для глубокого копирования
    public Manufacturer(Manufacturer other)
    {
        Name = other.Name;
        Country = other.Country;
        YearFounded = other.YearFounded;
    }

    public override string ToString()
    {
        return $"{Name} ({Country}, {YearFounded})";
    }
}

// 3. Базовый класс музыкального инструмента
public abstract class MusicInstrument : IPrototype<MusicInstrument>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public List<string> Features { get; set; }

    public MusicInstrument(string name, decimal price, Manufacturer manufacturer, List<string> features)
    {
        Name = name;
        Price = price;
        Manufacturer = manufacturer;
        Features = features ?? new List<string>();
    }

    // Поверхностное копирование через MemberwiseClone
    public virtual MusicInstrument Clone()
    {
        return (MusicInstrument)this.MemberwiseClone();
    }

    // Глубокое копирование
    public virtual MusicInstrument DeepClone()
    {
        // Клонируем Manufacturer через конструктор копирования
        Manufacturer clonedManufacturer = new Manufacturer(Manufacturer);

        // Клонируем список Features
        List<string> clonedFeatures = new List<string>(Features);

        // Создаем новый экземпляр
        MusicInstrument clone = (MusicInstrument)this.MemberwiseClone();
        clone.Manufacturer = clonedManufacturer;
        clone.Features = clonedFeatures;

        return clone;
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Инструмент: {Name}");
        Console.WriteLine($"Цена: {Price:C}");
        Console.WriteLine($"Производитель: {Manufacturer}");
        Console.WriteLine($"Особенности: {string.Join(", ", Features)}");
    }
}

// 4. Конкретные классы инструментов
public class Guitar : MusicInstrument
{
    public int StringCount { get; set; }
    public bool IsElectric { get; set; }

    public Guitar(string name, decimal price, Manufacturer manufacturer,
                  List<string> features, int stringCount, bool isElectric)
                  : base(name, price, manufacturer, features)
    {
        StringCount = stringCount;
        IsElectric = isElectric;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Количество струн: {StringCount}");
        Console.WriteLine($"Электрогитара: {(IsElectric ? "Да" : "Нет")}");
    }
}

public class Piano : MusicInstrument
{
    public int KeyCount { get; set; }
    public bool IsDigital { get; set; }

    public Piano(string name, decimal price, Manufacturer manufacturer,
                 List<string> features, int keyCount, bool isDigital)
                 : base(name, price, manufacturer, features)
    {
        KeyCount = keyCount;
        IsDigital = isDigital;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Количество клавиш: {KeyCount}");
        Console.WriteLine($"Цифровое пианино: {(IsDigital ? "Да" : "Нет")}");
    }
}

public class DrumSet : MusicInstrument
{
    public int PieceCount { get; set; }
    public bool HasCymbals { get; set; }

    public DrumSet(string name, decimal price, Manufacturer manufacturer,
                   List<string> features, int pieceCount, bool hasCymbals)
                   : base(name, price, manufacturer, features)
    {
        PieceCount = pieceCount;
        HasCymbals = hasCymbals;
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Количество элементов: {PieceCount}");
        Console.WriteLine($"С тарелками: {(HasCymbals ? "Да" : "Нет")}");
    }
}

// 5. Клиентский код
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ПАТТЕРНА ПРОТОТИП ===\n");

        // Создаем производителя
        Manufacturer fender = new Manufacturer("Fender", "USA", 1946);

        // Создаем оригинальную гитару
        Guitar originalGuitar = new Guitar(
            "Stratocaster",
            1500.99m,
            fender,
            new List<string> { "Звукосниматели single-coil", "Тремоло система", "Кленовый гриф" },
            6,
            true
        );

        Console.WriteLine("ОРИГИНАЛЬНЫЙ ОБЪЕКТ:");
        originalGuitar.DisplayInfo();
        Console.WriteLine(new string('-', 50));

        // 1. Демонстрация поверхностного копирования
        Console.WriteLine("\n1. ПОВЕРХНОСТНОЕ КОПИРОВАНИЕ:");
        Guitar shallowCopy = (Guitar)originalGuitar.Clone();

        Console.WriteLine("Копия после создания:");
        shallowCopy.DisplayInfo();

        // Изменяем данные в поверхностной копии
        Console.WriteLine("\nИзменяем данные в поверхностной копии...");
        shallowCopy.Price = 1200.00m;
        shallowCopy.Manufacturer.Name = "Fender Modified"; // Изменяем производителя
        shallowCopy.Features.Add("Новая особенность");

        Console.WriteLine("\nОригинал после изменений в поверхностной копии:");
        originalGuitar.DisplayInfo(); // Изменится Manufacturer и Features

        Console.WriteLine("\nПоверхностная копия после изменений:");
        shallowCopy.DisplayInfo();

        Console.WriteLine(new string('-', 50));

        // 2. Демонстрация глубокого копирования
        Console.WriteLine("\n2. ГЛУБОКОЕ КОПИРОВАНИЕ:");

        // Создаем новый оригинал
        Manufacturer yamaha = new Manufacturer("Yamaha", "Japan", 1887);
        Piano originalPiano = new Piano(
            "Yamaha C7X",
            25000.00m,
            yamaha,
            new List<string> { "Концертный рояль", "Полированная поверхность", "Чугунная рама" },
            88,
            false
        );

        Console.WriteLine("Оригинальное пианино:");
        originalPiano.DisplayInfo();

        // Создаем глубокую копию
        Piano deepCopy = (Piano)originalPiano.DeepClone();

        // Изменяем данные в глубокой копии
        Console.WriteLine("\nИзменяем данные в глубокой копии...");
        deepCopy.Price = 23000.00m;
        deepCopy.Manufacturer.Name = "Yamaha Modified";
        deepCopy.Manufacturer.Country = "China"; // Изменяем страну производителя
        deepCopy.Features[0] = "Измененная особенность";
        deepCopy.Features.Add("Новая особенность");

        Console.WriteLine("\nОригинал после изменений в глубокой копии:");
        originalPiano.DisplayInfo(); // Должен остаться без изменений

        Console.WriteLine("\nГлубокая копия после изменений:");
        deepCopy.DisplayInfo();

        Console.WriteLine(new string('-', 50));

        // 3. Демонстрация копирования с разными типами
        Console.WriteLine("\n3. КОПИРОВАНИЕ РАЗНЫХ ТИПОВ ИНСТРУМЕНТОВ:");

        Manufacturer pearl = new Manufacturer("Pearl", "Japan", 1946);
        DrumSet originalDrums = new DrumSet(
            "Pearl Export",
            1200.00m,
            pearl,
            new List<string> { "Береза/тополь", "Двойной подвес", "Том-холдер" },
            5,
            true
        );

        Console.WriteLine("Оригинальная барабанная установка:");
        originalDrums.DisplayInfo();

        // Создаем копию
        DrumSet drumsCopy = (DrumSet)originalDrums.DeepClone();
        drumsCopy.PieceCount = 7;
        drumsCopy.Features.Add("С дополнительным томом");

        Console.WriteLine("\nМодифицированная копия барабанной установки:");
        drumsCopy.DisplayInfo();

        Console.WriteLine("\n" + new string('=', 50));

    }
}