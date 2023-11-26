using System;

public abstract class Car
{
    public string Name { get; set; }
    public int Speed { get; set; }
    public int Position { get; set; }

    public delegate void FinishHandler(object sender, EventArgs e);
    public event FinishHandler FinishEvent = delegate { };

    public Car(string name, int speed)
    {
        Name = name;
        Speed = speed;
        Position = 0;
    }

    public virtual void Move()
    {
        // Логика перемещения автомобиля
        int distance = new Random().Next(Speed, Speed * 2); // Рандомное расстояние, которое пройдет автомобиль
        Position += distance;

        Console.WriteLine($"{Name} двигается на расстояние {distance}. Текущая позиция: {Position}.");

        // Проверка достижения финиша
        if (Position >= 100)
        {
            OnFinish();
        }
    }

    protected virtual void OnFinish()
    {
        FinishEvent?.Invoke(this, EventArgs.Empty);
    }
}

// Класс "Спортивный автомобиль"
public class SportsCar : Car
{
    public SportsCar(string name, int speed) : base(name, speed)
    {
        // Логика инициализации для спортивного автомобиля
        Console.WriteLine($"{Name} готов к гонкам!");
    }
}

// Класс "Легковой автомобиль"
public class PassengerCar : Car
{
    public PassengerCar(string name, int speed) : base(name, speed)
    {
        // Логика инициализации для легкового автомобиля
        Console.WriteLine($"{Name} выглядит как комфортное средство передвижения.");
    }
}

// Класс "Грузовой автомобиль"
public class Truck : Car
{
    public Truck(string name, int speed) : base(name, speed)
    {
        // Логика инициализации для грузового автомобиля
        Console.WriteLine($"{Name} загружен грузом и готов к гонкам.");
    }
}

// Класс "Автобус"
public class Bus : Car
{
    public Bus(string name, int speed) : base(name, speed)
    {
        // Логика инициализации для автобуса
        Console.WriteLine($"{Name} предназначен для перевозки пассажиров.");
    }
}

// Класс "Игра Гонки"
public class RacingGame
{
    public delegate void StartRaceHandler();
    public delegate void RaceFinishedHandler(string winner);

    public event StartRaceHandler StartRaceEvent = delegate { }; // Инициализация события
    public event RaceFinishedHandler RaceFinishedEvent = delegate { }; // Инициализация события

    public void StartRace()
    {
        // Логика начала гонки
        StartRaceEvent?.Invoke();

        // Создание экземпляров автомобилей
        SportsCar sportsCar = new SportsCar("Спортивный автомобиль", 10);
        PassengerCar passengerCar = new PassengerCar("Легковой автомобиль", 8);
        Truck truck = new Truck("Грузовой автомобиль", 7);
        Bus bus = new Bus("Автобус", 6);

        // Подписка на события финиша для каждого автомобиля
        sportsCar.FinishEvent += (sender, e) => OnFinishRace(sportsCar.Name);
        passengerCar.FinishEvent += (sender, e) => OnFinishRace(passengerCar.Name);
        truck.FinishEvent += (sender, e) => OnFinishRace(truck.Name);
        bus.FinishEvent += (sender, e) => OnFinishRace(bus.Name);

        // Логика процесса гонки
        while (true)
        {
            sportsCar.Move();
            passengerCar.Move();
            truck.Move();
            bus.Move();

            // Пауза между шагами гонки для наглядности
            System.Threading.Thread.Sleep(1000);
        }
    }

    private void OnFinishRace(string winnerName)
    {
        // Логика прибытия на финиш
        Console.WriteLine($"Автомобиль {winnerName} прибыл на финиш!");

        // Уведомление о завершении гонки
        RaceFinishedEvent?.Invoke(winnerName);

        Environment.Exit(0); // Завершаем программу после объявления победителя
    }
}

class Program
{
    static void Main()
    {
        RacingGame racingGame = new RacingGame();

        // Подписка на событие начала гонки
        racingGame.StartRaceEvent += () =>
        {
            Console.WriteLine("Гонка началась!");
            // Дополнительная логика при начале гонки
        };

        // Подписка на событие завершения гонки
        racingGame.RaceFinishedEvent += winner => Console.WriteLine($"Победитель гонки: {winner}");

        // Запуск гонки
        racingGame.StartRace();
    }
}
