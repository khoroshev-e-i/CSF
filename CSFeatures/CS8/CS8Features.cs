namespace CSFeatures.CS8;

public class CS8Features
{
}

#region readonly method in struct

public struct UserStruct
{
    public UserStruct(string name)
    {
        Name = name;
    }

    public string Name { get; }

    /// <summary>
    /// readonly позволяет точно определить что метод структуры не меняет ее состояния
    /// </summary>
    /// <remarks>Не знаю кто использует структуры вообще</remarks>
    public readonly string GetGreetings()
    {
        //Name = "asd";
        return $"Hi {Name}";
    }
}

#endregion

#region default interface member implementation

/// <summary>
/// Как по мне это какой-то рак, просто запретить использование
/// </summary>
public interface ILogger
{
    void Log(string message)
    {
        Console.WriteLine(message);
    }
}

public class WrappedLogger : ILogger
{
    //public void Log(string message) => Console.WriteLine($"Wrapped log message: {message}");
}

#endregion

#region switch expression

public enum Rainbow
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Indigo,
    Violet
}

public class RGBColor
{
    public RGBColor(int r, int g, int b)
    {
        R = r;
        G = g;
        B = b;
    }

    public int R { get; }
    public int G { get; }
    public int B { get; }

    /// <summary>
    /// Ну, тут все прекрасно, очень лаконично и понятно,
    /// синтаксис может быть немного непривычным
    /// </summary>
    public static RGBColor FromRainbow(Rainbow colorBand) =>
        colorBand switch
        {
            Rainbow.Red => new RGBColor(0xFF, 0x00, 0x00),
            Rainbow.Orange => new RGBColor(0xFF, 0x7F, 0x00),
            Rainbow.Yellow => new RGBColor(0xFF, 0xFF, 0x00),
            Rainbow.Green => new RGBColor(0x00, 0xFF, 0x00),
            Rainbow.Blue => new RGBColor(0x00, 0x00, 0xFF),
            Rainbow.Indigo => new RGBColor(0x4B, 0x00, 0x82),
            Rainbow.Violet => new RGBColor(0x94, 0x00, 0xD3),
            _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand)),
        };
}

#endregion

#region property patterns

/// <summary>
/// КНБ за 10 строк, это же восхитительно
/// </summary>
public class PropertyPatterns
{
    public static string RockPaperScissors(string first, string second)
        => (first, second) switch
        {
            ("rock", "paper") => "rock is covered by paper. Paper wins.",
            ("rock", "scissors") => "rock breaks scissors. Rock wins.",
            ("paper", "rock") => "paper covers rock. Paper wins.",
            ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
            ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
            ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
            (_, _) => "tie"
        };
}

#endregion

#region Positional patterns

/// <summary>
/// Работает только если есть метод deconstruct, описывающий как можно разложить объект в switch
/// из этого можно сделать элегантные решения
///
/// Вполне может использоваться чтобы избежать всяких неприятных фабрик в фабриках (если такие есть)
/// например (пример не очень удачный) фабрика по созданию объекта для обработки платежной операции (монета, альфа, ...) в которой фабрика для
/// обработчика коммисионной схемы может вылится в 2 Enum-a в 1 объекте и использоваться в такой кострукции
/// </summary>
public class PositionalPatterns
{
    static Quadrant GetQuadrant(Point point) => point switch
    {
        (0, 0) => Quadrant.Origin,
        var (x, y) when x > 0 && y > 0 => Quadrant.One,
        var (x, y) when x < 0 && y > 0 => Quadrant.Two,
        var (x, y) when x < 0 && y < 0 => Quadrant.Three,
        var (x, y) when x > 0 && y < 0 => Quadrant.Four,
        var (_, _) => Quadrant.OnBorder,
        _ => Quadrant.Unknown
    };
}

public class Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y) => (X, Y) = (x, y);

    public void Deconstruct(out int x, out int y)
    {
        using var ms1 = new MemoryStream();
        
        
        using (var ms = new MemoryStream())
        {
            
        }
        
        (x, y) = (X, Y);
    }
}

public enum Quadrant
{
    Unknown,
    Origin,
    One,
    Two,
    Three,
    Four,
    OnBorder
}

#endregion

#region Using declarations

// это просто using var, использовал, очень удобно
// теже ограничения что и в обычном using, dispose в конце скоупа

#endregion

#region Static local functions

/// <summary>
/// Ничего особенного, просто избавляет от возможности использовать переменные из скоупа метода
/// учитывая что обычно локальные функции делаются для того чтобы облегчить работу именно с ними - применение сомнительное
/// Можно использовать чтобы явно ограничить скоуп, но в таком случае лучше брать обычный статичный метод
/// </summary>
public class StaticLocalF
{
    int Foo()
    {
        
        Dictionary<int, int> d = new();
        //
        // int y;
        // // Изменит значение y
        // LocalFunction();
        // // Не изменит значение y
        // StaticLocalFunction(y);
        // return y;

        foreach (var dValue in d)
        {
            
        }
        
        void LocalFunction(int @key)
        {
            if(d.TryGetValue(@key, out var @value))
                return;
        }

        return 0;
    }
}

#endregion

#region Disposable ref structs

// Тут должна быть красивая история о том как важно освобождать неуправляемые ресурсы, но кто использует структуры ?
// disposable ref struct? что еще? реализация в интерфейсе? может еще public static void Main уберете? ...
// ладно, если уж сильно надо, то можно будет использовать с using и using var

#endregion

#region Nullable reference types

public class NullableReferenceTypes
{
// таким образом включается для файла



    public int _count;

    /// <summary>
    /// Не может иметь значение null (на самом деле может)
    /// </summary>
    public string _str;

// !! опасная фича, в ссылочные типы, которые `не могут быть null` все еще можно записать null 

// я бы вообще отказался т.к. столько лет адаптироваться под работу с reference type и придти к этому? что вообще у них на уме?
// еще и string Id { get; } теперь не может быть null, но при это туда все равно можно запихнуть null, это жесть какая-то
    public void Foo()
    {
        
        
        string str = GetStr();
        // string str1 = GetStr1();

        // NRE
        var len = str.Length;
        // var len1 = str1.Length;

        // почему это не ошибка ??????
        string GetStr()
        {
            return _count > 0 ? _str : null;
        }

        Records.Person1 per = null;

        // if (per!.FirstName == "")
        
        // а это вообще бред
        // string GetStr1()
        // {
        //     return _count > 0 ? _str : null!;
        // }
    }
}

#endregion

#region Asynchronous streams

public class AsynchronousStreams
{
    public static async IAsyncEnumerable<int> GenerateSequence()
    {
        for (int i = 0; i < 20; i++)
        {
            await Task.Delay(100);
            yield return i;
        }
    }
}

#endregion

#region Asynchronous disposable

// Полезная штука, однако теперь желательно реализовывать оба интерфейса либо только Async с ConfigureAwait(false) внутри
public class ExampleConjunctiveDisposableusing : IDisposable, IAsyncDisposable
{
    IDisposable _disposableResource = new MemoryStream();
    IAsyncDisposable _asyncDisposableResource = new MemoryStream();

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _disposableResource?.Dispose();
            (_asyncDisposableResource as IDisposable)?.Dispose();
            _disposableResource = null;
            _asyncDisposableResource = null;
        }
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (_asyncDisposableResource is not null)
        {
            await _asyncDisposableResource.DisposeAsync().ConfigureAwait(false);
        }

        if (_disposableResource is IAsyncDisposable disposable)
        {
            await disposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            _disposableResource?.Dispose();
        }

        _asyncDisposableResource = null;
        _disposableResource = null;
    }
}

#endregion

#region Indices and ranges

// наверное кого-то сильно бесил Skip.Take/Last/First
// Примечательно что с конца указывается не индекс, а номер элемента т.е. index+1 (нумерация с 1, а не с 0)
public class IndicesAndRanges
{
    private  string[] words = new string[]
    {
        // index from start    index from end
        "The",      // 0                   ^9
        "quick",    // 1                   ^8
        "brown",    // 2                   ^7
        "fox",      // 3                   ^6
        "jumped",   // 4                   ^5
        "over",     // 5                   ^4
        "the",      // 6                   ^3
        "lazy",     // 7                   ^2
        "dog"       // 8                   ^1
    };              // 9 (or words.Length) ^0
}

#endregion

#region Null-coalescing assignment

public class NullCoalescingAssignment
{
// + к лаконичности, но поначалу может немного пугать синтаксис
    
    public void Foo()
    {
        List<int> numbers = null;
        int? i = null;

        
        
        numbers = numbers ?? throw new Exception(); 

        numbers ??= new List<int>();
        
        // тоже самое что numbers = numbers ?? new List<int>();
        
        numbers.Add(i ??= 17); 
        numbers.Add(i ??= 20);

        Console.WriteLine(string.Join(" ", numbers));  // output: 17 17
        Console.WriteLine(i);  // output: 17
    }
}

#endregion