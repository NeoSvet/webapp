
namespace MySiteApp.Gi
{
  public class GiStorage
  {
    private List<string> lines = new List<string>();
    public List<DataItem> objects, mobs, talents, bosses;
    public static List<DataItem> locations;
    public List<PersonItem> persons = null;
    public static string NewLoc = null;

    public void Load(string data)
    {
      if (persons != null) return;
      using (var reader = new StringReader(data))
      {
        string? s;
        while ((s = reader.ReadLine()) != null)
        {
          lines.Add(s);
          if (s.Contains("locations"))
            locations = LoadData(reader);
          else if (s.Contains("objects"))
            objects = LoadData(reader);
          else if (s.Contains("mobs"))
            mobs = LoadData(reader);
          else if (s.Contains("talents"))
            talents = LoadData(reader);
          else if (s.Contains("bosses"))
            bosses = LoadData(reader);
          else if (s.Contains("data"))
            LoadPersons(reader);
        }
      }
    }

    public string GetData()
    {
      if (persons == null) return "";
      using (var writer = new StringWriter())
      {
        foreach (var s in lines)
        {
          writer.WriteLine(s);
          if (s.Contains("locations"))
            SaveData(writer, locations);
          else if (s.Contains("objects"))
            SaveData(writer, objects);
          else if (s.Contains("mobs"))
            SaveData(writer, mobs);
          else if (s.Contains("talents"))
            SaveData(writer, talents);
          else if (s.Contains("bosses"))
            SaveData(writer, bosses);
          else if (s.Contains("data"))
            SavePersons(writer);
        }
        return writer.ToString();
      }
    }

    private void SaveData(TextWriter writer, List<DataItem> list)
    {
      for (int i = 0; i < list.Count; i++)
      {
        writer.Write(list[i].ToString());
        if (i < list.Count - 1) writer.Write(", //");
        else writer.Write(" //");
        writer.WriteLine(i);
      }
      writer.WriteLine("];");
    }

    private void SavePersons(TextWriter writer)
    {

      for (int i = 0; i < persons.Count; i++)
      {
        writer.Write("  ["); writer.Write(i);
        writer.Write(persons[i].ToString());
        if (i < persons.Count - 1) writer.WriteLine(",");
        else writer.WriteLine();
      }
      writer.WriteLine("];");
    }

    private List<DataItem> LoadData(TextReader reader)
    {
      var list = new List<DataItem>();
      var s = reader.ReadLine();
      while (s != "];")
      {
        list.Add(new DataItem(s));
        s = reader.ReadLine();
      }
      return list;
    }

    private void LoadPersons(TextReader reader)
    {
      persons = new List<PersonItem>();
      var s = reader.ReadLine();
      while (s != "];")
      {
        persons.Add(new PersonItem(s));
        s = reader.ReadLine();
      }
    }

    public static byte FindLoc(string url)
    {
      for (byte i = 0; i < locations.Count; i++)
      {
        if (locations[i].Url == url) return i;
      }
      NewLoc = url;
      return (byte)locations.Count;
    }

    public bool ContainsPerson(string url)
    {
      if (persons == null) return false;
      foreach (var p in persons)
      {
        if (p.Url == url) return true;
      }
      return false;
    }
  }
}
