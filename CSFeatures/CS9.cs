using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSFeatures;

public class CS9
{
}

#region record types

public class Records
{
    public static void Foo()
    {
        var r1 = new Person1("Jack", "Smith");
        var r3 = new Person1("Jack", "Smith");
        
        Console.WriteLine($"{r1==r3} {r1.ToString()} {r3.ToString()}");
        
        // теперь не обязательно указывать тип 2 раза (доступно с 8)
        Person2 r2 = new("Jack", "Smith");
        
        var e = new Employee("Avanda", "Josh", "Daveson");

        // так нельзя т.к. по умолчанию конструктор только со всеми свойствами
        // var person = new Person1()
        // {
        //     FirstName = "",
        //     LastName = ""
        // }

        
        
        var p = new Person2()
        {
            FirstName = "",
            LastName = "",
        };
        
        // можно очень быстро создать похожую запись с некоторыми отличиями
        var r2WParker = r2 with { LastName = "Parker" };
        // причем с короткой записью тоже
        var r2Adams = r1 with { LastName = "Adams" };
        
        // Это не будет работать т.к. поля readonly
        //r1.FirstName = "1";
        //r2.FirstName = "s";
    }
    
    // В нашем случае не факт что где-то можно будет использовать init (только если без пустого конструктора),
    // т.к. мы обходим конструктор, в котором часто вшита валидация сущности 

    public record Person1(string FirstName, string LastName);

    /// <summary>
    /// Наследование: Свойства в параметрах наследников должны называться тем же именем что и в родителе
    /// </summary>
    /// <param name="CompanyName"></param>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    public record Employee(string CompanyName, string FirstName, string LastName) : Person1(FirstName, LastName);

    // тоже самое что и выше
    public record Person2
    {
        public Person2()
        {
            
        }
        public Person2(string fn, string ln)
        {
            FirstName = fn;
            LastName = ln;
        }

        // можно указать set вместо Init
        public string FirstName { get; init; }
        public string LastName { get; init; }
    };
}

#endregion

#region more pattern matching 

/// <summary>
/// Обогатили синтаксис PM, добавили больше конструкций (скоро от ООП останется только ФП) ))))
/// </summary>

public static class MPM
{
    public static bool IsLetter(this char c) =>
        c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
    
    public static bool IsLetterOrSeparator(this char c) =>
        c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '.' or ',';

    public static void Foo<T>(T r)
    {
        // Можно теперь писать так, если надоело has value / == null
        if (r is null)
            return;
        
        if (r is (decimal or float or double))
            return;
    }
}

#endregion

#region Fit and finish features

// Расширение new() без типа для параметров методов
public class FF
{
    public void Foo(Records.Person1 p1, Records.Person2 p2)
    {
        Console.WriteLine(p1.ToString() + p2);
    }

    /// <summary>
    /// Учитывая что постоянно борятся за явные отображения типов - неюзабельно у нас))
    /// </summary>
    public void CallFoo() => Foo(new("Amanda", "Conor"), new("Michael", "Myers"));
}

#endregion

#region Support for code generators

// Если в кратце - прикольно, конечно)) возможно найти применение, пока в preview
// код будет генерироваться в процессе компиляции
[Generator]
public class HelloWorldGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        // begin creating the source we'll inject into the users compilation
        var sourceBuilder = new StringBuilder(@"
using System;
namespace HelloWorldGenerated
{
    public static class HelloWorld
    {
        public static void SayHello() 
        {
            Console.WriteLine(""Hello from generated code!"");
            Console.WriteLine(""The following syntax trees existed in the compilation that created this program:"");
");

        // using the context, get a list of syntax trees in the users compilation
        var syntaxTrees = context.Compilation.SyntaxTrees;

        // add the filepath of each tree to the class we're building
        foreach (SyntaxTree tree in syntaxTrees)
        {
            sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {tree.FilePath}"");");
        }

        // finish creating the source to inject
        sourceBuilder.Append(@"
        }
    }
}");

        // inject the created source into the users compilation
        context.AddSource("helloWorldGenerator", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this one
    }
}

#endregion