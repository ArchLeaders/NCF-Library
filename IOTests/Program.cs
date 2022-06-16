using Nintendo.Aamp;
using Nintendo.Bfres;
using Nintendo.Byml;
using Nintendo.Sarc;
using Nintendo.Yaz0;
using System.Diagnostics;

Stopwatch watch = new();
watch.Start();

/// AAMP I/O

Console.Write("Testing AampIO -->");

try
{
    AampFile aamp = new("Data\\IO.aamp");
    Debug.WriteLine(aamp.ToString());

    aamp.WriteBinary("Data\\IO.out.aamp");
    Console.Write("\rTesting AampIO --> Success\n");
}
catch (Exception ex)
{
    File.AppendAllText("Data\\IO.aamp.log", $"[{DateTime.Now}] {ex}\n\n");
    Console.Write("\rTesting AampIO --> Failed\n");
    throw;
}

/// BFRES I/O

Console.Write("\rTesting BfresIO -->");

try
{
    BfresFile bfres = new("Data\\IO.bfres");

    foreach (var file in bfres.Models)
        Debug.WriteLine(file.Value.Name);

    bfres.ToBinary("Data\\IO.out.bfres");
    Console.Write("\rTesting BfresIO --> Success\n");
}
catch (Exception ex)
{
    File.WriteAllText("Data\\IO.bfres.log", $"[{DateTime.Now}] {ex}\n\n");
    Console.Write("\rTesting BfresIO --> Failed\n");
    throw;
}

/// SARC I/O

Console.Write("\rTesting SarcIO -->");

try
{
    SarcFile sarc = new("Data\\IO.sarc");

    foreach (var file in sarc.Files)
        Debug.WriteLine(file.Key);

    sarc.ToBinary("Data\\IO.out.sarc");
    Console.Write("\rTesting SarcIO --> Success\n");
}
catch (Exception ex)
{
    File.WriteAllText("Data\\IO.sarc.log", $"[{DateTime.Now}] {ex}\n\n");
    Console.Write("\rTesting SarcIO --> Failed\n");
    throw;
}

/// BYML I/O

Console.WriteLine("Testing BymlIO & Yaz0 --> ");

double lastTracked = 0;

try
{
    // Decompress
    byte[] decompressed = Yaz0.DecompressFast("Data\\IO.sbyml");
    Console.WriteLine($"Decompression took {(watch.ElapsedMilliseconds - lastTracked) / 1000.0} seconds.");
    lastTracked = watch.ElapsedMilliseconds;

    // Deserialize
    BymlFile byml = new(decompressed);
    Console.WriteLine($"Binary deserialization took {(watch.ElapsedMilliseconds - lastTracked) / 1000.0} seconds.");
    lastTracked = watch.ElapsedMilliseconds;

    // Serialize
    byte[] serialized = byml.ToBinary();
    Console.WriteLine($"Binary -> Binary Serialization took {(watch.ElapsedMilliseconds - lastTracked) / 1000.0} seconds.");
    lastTracked = watch.ElapsedMilliseconds;
    File.WriteAllBytes("Data\\IO.out.byml", serialized);

    // Write YAML
    byml.WriteYaml("Data\\IO.byml.yml");
    Console.WriteLine($"YAML Serialization took {(watch.ElapsedMilliseconds - lastTracked) / 1000.0} seconds.");
    lastTracked = watch.ElapsedMilliseconds;

    // Load YML
    BymlFile fromYml = BymlFile.FromYamlFile("Data\\IO.byml.yml");
    Console.WriteLine($"YAML Deserialization took {(watch.ElapsedMilliseconds - lastTracked) / 1000.0} seconds.");
    lastTracked = watch.ElapsedMilliseconds;

    // Serialize YAML
    byte[] serializedYaml = fromYml.ToBinary();
    Console.WriteLine($"Yaml -> Binary Serialization took {(watch.ElapsedMilliseconds - lastTracked) / 1000.0} seconds.");
    lastTracked = watch.ElapsedMilliseconds;
    File.WriteAllBytes("Data\\IO.out.yml.byml", serializedYaml);

    // Compress (level 7)
    byte[] compressed = Yaz0.CompressFast(serialized);
    Console.WriteLine($"Compression took {(watch.ElapsedMilliseconds - lastTracked) / 1000.0} seconds.");
    lastTracked = watch.ElapsedMilliseconds;

    // Verify it works correctly
    File.WriteAllBytes("Data\\IO.out.sbyml", compressed);

    watch.Stop();
    Console.WriteLine($"Testing BymlIO --> Success (Elapsed seconds: {watch.ElapsedMilliseconds / 1000.0})\n");
}
catch (Exception ex)
{
    File.WriteAllText("Data\\IO.byml.log", $"[{DateTime.Now}] {ex}\n\n");
    Console.WriteLine("\rTesting BymlIO --> Failed\n");
    throw;
}
