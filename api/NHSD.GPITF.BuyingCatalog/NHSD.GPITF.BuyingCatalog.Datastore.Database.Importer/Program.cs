using CsvHelper.Configuration;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database.Importer
{
  public sealed class Program
  {
    static void Main(string[] args)
    {
      // check data directory exists
      if (args.Length != 1 || !Directory.Exists(args[0]))
      {
        Usage();
        return;
      }

      // check all data files exist
      foreach (var file in GetDataFiles())
      {
        var filePath = Path.Combine(args[0], file);
        if (!File.Exists(filePath))
        {
          Console.WriteLine($"Could not find: {filePath}");
          return;
        }
      }

      new Program(args[0]).Run();
    }

    private static IEnumerable<Type> GetImportTypes()
    {
      // load entities first
      yield return typeof(Organisations);
      yield return typeof(Contacts);
      yield return typeof(Solutions);
      yield return typeof(TechnicalContacts);
      yield return typeof(Capabilities);
      yield return typeof(Frameworks);
      yield return typeof(Standards);

      // load relationships
      yield return typeof(CapabilitiesImplemented);
      yield return typeof(StandardsApplicable);
      yield return typeof(CapabilityFramework);
      yield return typeof(FrameworkSolution);
      yield return typeof(FrameworkStandard);
      yield return typeof(CapabilityStandard);
      yield return typeof(CapabilitiesImplementedEvidence);
      yield return typeof(CapabilitiesImplementedReviews);
      yield return typeof(StandardsApplicableEvidence);
      yield return typeof(StandardsApplicableReviews);
    }

    private static IEnumerable<string> GetDataFiles()
    {
      foreach (var type in GetImportTypes())
      {
        var dataFileName = Path.ChangeExtension(type.Name, ".tsv");
        yield return dataFileName;
      }
    }

    private static IEnumerable<Type> GetClassMapTypes()
    {
      yield return typeof(SolutionsClassMap);
      yield return typeof(CapabilitiesClassMap);
      yield return typeof(FrameworksClassMap);
      yield return typeof(StandardsClassMap);
      yield return typeof(CapabilitiesImplementedClassMap);
      yield return typeof(StandardsApplicableClassMap);
      yield return typeof(CapabilitiesImplementedEvidenceClassMap);
      yield return typeof(CapabilitiesImplementedReviewsClassMap);
      yield return typeof(StandardsApplicableEvidenceClassMap);
      yield return typeof(StandardsApplicableReviewsClassMap);
    }

    private readonly string _dataDirectory;

    public Program(string dataDirectory)
    {
      _dataDirectory = dataDirectory;
    }

    private void Run()
    {
      var config = new ConfigurationBuilder()
        .AddJsonFile("hosting.json")
        .Build();
      var dbConnFact = new DbConnectionFactory(config);
      using (var conn = dbConnFact.Get())
      {
        Console.WriteLine($"Importing from:  {_dataDirectory}");
        Console.WriteLine($"          into:  {conn.ConnectionString}");
        using (var trans = conn.BeginTransaction())
        {
          foreach (var type in GetImportTypes())
          {
            var method = typeof(Program).GetMethod(nameof(Load), BindingFlags.NonPublic | BindingFlags.Instance);
            var generic = method.MakeGenericMethod(type);
            generic.Invoke(this, new object[] { conn, trans });
          }
          trans.Commit();
        }
        Console.WriteLine("Finished!");
      }
    }

    private void Load<T>(IDbConnection conn, IDbTransaction trans)
    {
      var dataFileName = Path.ChangeExtension(typeof(T).Name, ".tsv");
      var tr = File.OpenText(Path.Combine(_dataDirectory, dataFileName));
      var config = new Configuration
      {
        HasHeaderRecord = true,
        Delimiter = "\t"
      };
      foreach (var classMap in GetClassMapTypes())
      {
        config.RegisterClassMap(classMap);
      }

      var csv = new CsvHelper.CsvReader(tr, config);
      var records = csv.GetRecords<T>().ToList();
      Console.WriteLine($"  {dataFileName} ...");

      conn.Insert(records, trans);
    }

    private static void Usage()
    {
      Console.WriteLine("Usage:");
      Console.WriteLine("  NHSD.GPITF.BuyingCatalog.Datastore.Database.Importer.exe [directory-with-data-files]");
      Console.WriteLine();
      Console.WriteLine("Notes:");
      Console.WriteLine("  Database connection is contained in hosting.json");
    }
  }

  public sealed class SolutionsClassMap : ClassMap<Solutions>
  {
    public SolutionsClassMap()
    {
      AutoMap();
      Map(m => m.PreviousId).TypeConverterOption.NullValues(string.Empty);
    }
  }

  public sealed class CapabilitiesClassMap : ClassMap<Capabilities>
  {
    public CapabilitiesClassMap()
    {
      AutoMap();
      Map(m => m.PreviousId).TypeConverterOption.NullValues(string.Empty);
    }
  }

  public sealed class FrameworksClassMap : ClassMap<Frameworks>
  {
    public FrameworksClassMap()
    {
      AutoMap();
      Map(m => m.PreviousId).TypeConverterOption.NullValues(string.Empty);
    }
  }

  public sealed class StandardsClassMap : ClassMap<Standards>
  {
    public StandardsClassMap()
    {
      AutoMap();
      Map(m => m.PreviousId).TypeConverterOption.NullValues(string.Empty);
    }
  }

  public sealed class CapabilitiesImplementedClassMap : ClassMap<CapabilitiesImplemented>
  {
    public CapabilitiesImplementedClassMap()
    {
      AutoMap();
      Map(m => m.Id).Index(0);
      Map(m => m.SolutionId).Index(1);
      Map(m => m.CapabilityId).Index(2);
      Map(m => m.Status).Index(3);
      Map(m => m.OwnerId).TypeConverterOption.NullValues(string.Empty);
    }
  }

  public sealed class StandardsApplicableClassMap : ClassMap<StandardsApplicable>
  {
    public StandardsApplicableClassMap()
    {
      AutoMap();
      Map(m => m.Id).Index(0);
      Map(m => m.SolutionId).Index(1);
      Map(m => m.StandardId).Index(2);
      Map(m => m.Status).Index(3);
      Map(m => m.OwnerId).TypeConverterOption.NullValues(string.Empty);
    }
  }

  public sealed class CapabilitiesImplementedEvidenceClassMap : ClassMap<CapabilitiesImplementedEvidence>
  {
    public CapabilitiesImplementedEvidenceClassMap()
    {
      AutoMap();
      Map(m => m.PreviousId).TypeConverterOption.NullValues(string.Empty);
    }
  }

  public sealed class CapabilitiesImplementedReviewsClassMap : ClassMap<CapabilitiesImplementedReviews>
  {
    public CapabilitiesImplementedReviewsClassMap()
    {
      AutoMap();
      Map(m => m.PreviousId).TypeConverterOption.NullValues(string.Empty);
    }
  }

  public sealed class StandardsApplicableEvidenceClassMap : ClassMap<StandardsApplicableEvidence>
  {
    public StandardsApplicableEvidenceClassMap()
    {
      AutoMap();
      Map(m => m.PreviousId).TypeConverterOption.NullValues(string.Empty);
    }
  }

  public sealed class StandardsApplicableReviewsClassMap : ClassMap<StandardsApplicableReviews>
  {
    public StandardsApplicableReviewsClassMap()
    {
      AutoMap();
      Map(m => m.PreviousId).TypeConverterOption.NullValues(string.Empty);
    }
  }
}
