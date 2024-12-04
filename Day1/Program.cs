
internal class Program
{
   public static void Main(string[] args)
   {
      var list1 = new List<int>();
      var list2 = new List<int>();


      foreach (string line in File.ReadLines("../../../demo.txt"))
      {
         var parts = line.Split("   ");
         list1.Add( Int32.Parse(parts[0]));
         list2.Add( Int32.Parse(parts[1]));
      }

      list1.Sort();
      list2.Sort();



      var occs = new Dictionary<int, int>();



      for (int i = 0; i < list2.Count; i++)
      {
         var x = list2[i];
   
         if (occs.ContainsKey(x))
         {
            occs[x] += 1;
         }
         else
         {
            occs[x] =  1;
         }
   
      }
      

      var ans = 0;
      for (int j = 0; j < list1.Count(); j++)
      {
         var x = list1[j];
         if (occs.ContainsKey(x))
         {
            ans += occs[x] * x;
         }
      }


      Console.WriteLine(ans);
   }
}