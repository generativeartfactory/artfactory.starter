# upman-win

Update Manager for Windows (in .NET)


## Usage


~~~
using System;
using System.IO;


public class Test
{
  static void Main(string[] args)
  {
      string root = @"c:\test";

      Merger m = new Merger( root );
        
      // case 1)  check for /versioned folders
      //  -- e.g. downloads/<version>/paket/paket.txt => paket/paket.txt

      m.MergeVersionPack( "paket" );  

      // case 2)  check for /latest
      //  -- e.g. downloads/latest/paket/part.txt => paket/part.txt

      m.MergeLatestPack( "part" );  

      Console.WriteLine("Bye.");
  }
}
~~~


## License

The `upman-win` code is dedicated to the public domain.
Use it as you please with no restrictions whatsoever.
