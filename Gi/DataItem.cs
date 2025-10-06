using System.Text;

namespace MySiteApp.Gi
{
  public class DataItem
  {
    public string Name, Url, S = null;
    public byte A, B;

    public DataItem() { }

    public DataItem(string s)
    {
      var i = s.IndexOf("]");
      var m = s.Substring(3, i - 3).Replace(", ", ",").Replace("'", "").Split(',');
      Name = m[0];
      Url = m[1];
      if (m.Length > 2)
      {
        if (!byte.TryParse(m[2], out A))
        {
          S = m[2];
          A = 255;
        }
      }
      else A = 255;
      if (m.Length > 3)
        B = byte.Parse(m[3]);
      else B = 255;
    }

    public override string ToString()
    {
      var sb = new StringBuilder("  ['");
      sb.Append(Name); sb.Append("', '"); sb.Append(Url);
      if (S != null)
      {
        sb.Append("', '"); sb.Append(S); sb.Append("']");
      }
      else if (A < 255)
      {
        sb.Append("', "); sb.Append(A);
        if (B < 255)
        {
          sb.Append(", "); sb.Append(B);
        }
        sb.Append("]");
      }
      else sb.Append("']");
      return sb.ToString();
    }

    public void SetHtml(string s)
    {
      //title="Серебро захода луны" href="/wiki/predmety/dikoviny/dikoviny-nod-kray/serebro-zakhoda-luny
      int i = s.IndexOf("=") + 2;
      Name = s.Substring(i, s.IndexOf("\"", i) - i);
      if (s.Contains("dikoviny"))
      {
        i = s.IndexOf("dikoviny-", i) + 9;
        A = GiStorage.FindLoc(s.Substring(i, s.IndexOf("/", i) - i));
      }
      i = s.LastIndexOf("/") + 1;
      Url = s.Substring(i);
    }
  }
}
