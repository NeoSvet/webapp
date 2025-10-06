using System.Text;

namespace MySiteApp.Gi
{ 
  public class PersonItem
  {
    public string Name, Url;
    public byte Element, Weapon, Stars, Boss, Talent, Object, Mob;

    public PersonItem() { }

    public PersonItem(string s)
    {
      var i = s.IndexOf("]");
      var m = s.Substring(3, i - 3).Replace(", ", ",").Replace("'", "").Split(',');
      //0 - index
      Name = m[1];
      Url = m[2];
      Element = byte.Parse(m[3]);
      Weapon = byte.Parse(m[4]);
      Stars = byte.Parse(m[5]);
      Boss = byte.Parse(m[6]);
      Talent = byte.Parse(m[7]);
      Object = byte.Parse(m[8]);
      Mob = byte.Parse(m[9]);
    }

    public override string ToString()
    {
      var sb = new StringBuilder(", '");
      sb.Append(Name); sb.Append("', '");
      sb.Append(Url); sb.Append("', ");
      sb.Append(Element); sb.Append(", ");
      sb.Append(Weapon); sb.Append(", ");
      sb.Append(Stars); sb.Append(", ");
      sb.Append(Boss); sb.Append(", ");
      sb.Append(Talent); sb.Append(", ");
      sb.Append(Object); sb.Append(", ");
      sb.Append(Mob); sb.Append("]");
      return sb.ToString();
    }
  }
}
