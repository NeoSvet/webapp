using MySiteApp.Service;

namespace MySiteApp.Gi
{
  public class GiSource
  {
    private const string URL = "https://genshin-info.ru",
      PERSON = URL + "/wiki/personazhi/",
      VALUE = "characterPromo__propV", END = "</span>";
    private readonly string[] Elements = new string[]
    {
      "Анемо", "Гео", "Элетро", "Дендро", "Гидро", "Пиро", "Крио"
    };
    private readonly string[] Weapons = new string[]
    {
      "Лук", "Катализатор", "Копье", "Меч", "Двуручный меч"
    };
    public DataItem obj, mob, talent, boss;
    private WebService Web;
    private GiStorage Storage = new GiStorage();

    public GiSource(WebService web)
    {
      Web = web;
    }

    public async Task LoadData()
    {
      string content = await Web.LoadAsync("https://neosvet.somee.com/gi/js/data.js");
      Storage.Load(content);
    }

    public async Task<List<ListItem>> LoadPersons()
    {
      string content = await Web.LoadAsync(PERSON);
      int i = content.IndexOf("itemList__item");
      content = content.Substring(i, content.IndexOf("wiad card", i) - i);
      i = content.IndexOf("itemtype");
      var list = new List<ListItem>();
      while (i > 0)
      {
        var item = new ListItem();
        i = content.IndexOf("title", i) + 7;
        item.Title = content.Substring(i, content.IndexOf("\"", i) - i);
        i = content.IndexOf("href", i) + 23;
        item.Url = content.Substring(i, content.IndexOf("/", i) - i);
        if (!item.Url.StartsWith("puteshestvennik") && !item.Url.StartsWith("traveler") &&
          !Storage.ContainsPerson(item.Url))
        {
          i = content.IndexOf("src", i) + 5;
          item.Icon = content.Substring(i, content.IndexOf("\"", i) - i);
          list.Add(item);
        }
        i = content.IndexOf("itemtype", i);
      }
      return list;
    }

    public async Task<PersonItem> LoadPerson(string url)
    {
      string content = await Web.LoadAsync(PERSON + url);
      int i = content.IndexOf("characterPromo__name");
      content = content.Substring(i);
      var item = new PersonItem();
      i = content.IndexOf(">") + 1;
      item.Name = content.Substring(i, content.IndexOf("<", i) - i);
      i = content.IndexOf(VALUE, i);
      if (content.IndexOf(END, i) - i < 250)
        item.Stars = 4;
      else item.Stars = 5;
      i = content.IndexOf(">", content.IndexOf(VALUE, i)) + 1;
      var s = content.Substring(i, content.IndexOf(END, i) - i).Trim();
      byte n;
      for (n = 0; n < Elements.Length; n++)
      {
        if (Elements[n] == s)
        {
          item.Element = n;
          break;
        }
      }
      i = content.IndexOf(">", content.IndexOf(VALUE, i)) + 1;
      s = content.Substring(i, content.IndexOf(END, i) - i).Trim();
      for (n = 0; n < Weapons.Length; n++)
      {
        if (Weapons[n] == s)
        {
          item.Weapon = n;
          break;
        }
      }

      i = content.IndexOf("src", content.IndexOf("itemcard__img", i)) + 5;
      var avatar = content.Substring(i, content.IndexOf("\"", i) - i);

      content = content.Substring(content.IndexOf("leveling", i));
      i = content.IndexOf("title");

      obj = new DataItem();
      obj.SetHtml(content.Substring(i, content.IndexOf("class", i) - i - 3));
      i = content.IndexOf("src", i) + 5;
      obj.S = content.Substring(i, content.IndexOf("\"", i) - i); //icon

      i = content.IndexOf("materialy", i) - 200;
      i = content.IndexOf("href", i) + 6;
      s = content.Substring(i, content.IndexOf("\"", i) - i); //url for mob loot
      mob = await LoadData(s, "enemy");

      i = content.IndexOf("vozvysheniya", i) - 200;
      i = content.IndexOf("href", i) + 6;
      s = content.Substring(i, content.IndexOf("\"", i) - i); //url for boss loot
      boss = await LoadData(s, "bosses");


      i = content.IndexOf("navykov", i) - 200;
      i = content.IndexOf("title", i);
      talent = new DataItem();
      talent.SetHtml(content.Substring(i, content.IndexOf("class", i) - i - 3));
      i = content.IndexOf("src", i) + 5;
      talent.S = content.Substring(i, content.IndexOf("\"", i) - i); //icon

      return item;
    }

    private async Task<DataItem> LoadData(string url, string label)
    {
      var content = await Web.LoadAsync(URL + url);
      int i = content.IndexOf("title", content.IndexOf(label));
      var item = new DataItem();
      item.SetHtml(content.Substring(i, content.IndexOf("class", i) - i - 3));
      i = content.IndexOf("src", i) + 5;
      item.S = content.Substring(i, content.IndexOf("\"", i) - i); //icon
      return item;
    }
  }
}
