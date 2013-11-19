using System;
using System.IO;
using System.Text.RegularExpressions;


public class Utils
{
  public static string FindPackVersion( string path )
  {
       Console.WriteLine( "read manifest file: " + path );

       // todo: check if file exists? why? why not??

       string[] lines = File.ReadAllLines( path );
       foreach( string line in lines)
       {
          if( Regex.IsMatch( line, "VERSION[:=]" ))    // todo: use anchor? use case insensitivee?
          {
            string[] values = Regex.Split( line, "[:=]" );
            string key   =  values[0].Trim();
            string value =  values[1].Trim(); //  Trim method removes from the current string all leading and trailing white-space chars
            
            // todo: fix: use strip spaces left and right - exists strip
            Console.WriteLine( "bingo! VERSION header found; key=>" + key +"<, value=>" + value + "<" );
            return value;
          }
       }
       
       // return "latest";   // todo: if not found; return nil or "missing" something else? why? why not?
       return null;  // no version found in paket.txt
  }
    
  public static bool IsDirFlat( string path )
  {
     return path.Contains( "__I__" );
  }

  public static bool IsDirFull( string path )
  {
     // has some files (e.g. documents, that is, not just folders/directories)
     string [] files = Directory.GetFiles( path );
     return files.Length > 0;
  }

  public static bool IsDirJavaWebArchive( string path )
  {
    // check for "root/top" folders WEB-INF & META-INF for now
    string [] dirs = Directory.GetDirectories( path );
       
    bool meta_inf_found = false;
    bool web_inf_found  = false;
       
    foreach( string dir in dirs )
    {
       if( dir.EndsWith( @"\META-INF" ))
       {
          // Console.WriteLine( "found \\META-INF");
          meta_inf_found = true;
       }
       if( dir.EndsWith( @"\WEB-INF" ))
       {
         // Console.WriteLine( "found \\WEB-INF" );
         web_inf_found = true;
       }
    }
    
    return meta_inf_found && web_inf_found;
  }

} // class Utils

