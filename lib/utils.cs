using System;
using System.IO;
using System.Text.RegularExpressions;


////
// fix:
//
// split into FileUtils  e.g. FileUtils.MoveAndCreateDirs()
//  and DirUtils   e.g. DirUtils.Missing( ) DirUtils.MoveAndCreateDirs()
//  and PackUtils  e.g. PackUtils.FindVersion(), PackUtils.FindLatest ?? etc.
//  and Utils   e.g. Utils.Timestamp()
//  etc.

public class Utils
{

  public static bool DirMissing( string dir )
  {
    return Directory.Exists( dir ) == false;
  }

  public static void FileMoveAndCreateDirs( string sourceFile, string destFile )
  {
    // make sure dest dirs exists
    DirectoryInfo destDirInfo = new FileInfo( destFile ).Directory;
    Console.WriteLine( "  file move - destDirInfo: " + destDirInfo.Name );
    destDirInfo.Create();  //  make sure parent dir(s) exists (create if not) - check if it works for more than one dir/level

    File.Move( sourceFile, destFile );
  }

  public static void DirMoveAndCreateDirs( string sourceDir, string destDir )
  {
    // make sure parent dirs exists
    DirectoryInfo parentDirInfo = Directory.GetParent( destDir );
    Console.WriteLine( "  dir move - parentDirInfo: " + parentDirInfo.Name );
    parentDirInfo.Create(); //  make sure parent dirs exists (create if not)
               
    Directory.Move( sourceDir, destDir );
  }



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

