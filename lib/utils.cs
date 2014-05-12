using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;


public class DirUtils
{
  public static void TryDeleteRecursively( string dir )
  {
    // try to delete directory; if exception ignore delete and try later
    try
    {
       Console.WriteLine( "[DirUtils.TryDeleteRecursively] " + dir );
       Directory.Delete( dir, true );  // NOTE: use recursive=true flag
    }
    catch( Exception )
    {
       Console.WriteLine( "[DirUtils.TryDeleteRecursively] *** Exception thrown; trying to deltete folder: " + dir );
    }
  }

  public static void TryDeleteRecursivelyAndKeep( string dir, string[] keep_files )
  {
     Console.WriteLine( "[DirUtils.TryDeleteRecursivelyAndKeep] " + dir );
     // use depth first walker
     TryDeleteRecursivelyAndKeepWorker( dir, keep_files );
  }

  
  public static bool TryDeleteRecursivelyAndKeepWorker( string path, string[] keep_files )
  {
    string[] dirs = Directory.GetDirectories( path );
    
    int keep_dirs_count = 0;
    foreach( string dir in dirs )
    {
      if( TryDeleteRecursivelyAndKeepWorker( dir, keep_files ))
         keep_dirs_count++;
    }

    // todo: find a more efficent matcher ??? e.g. use shell wildcard match ?? possible??
    string[] files = Directory.GetFiles( path );

    // pass 1: count matching files to keep, if any
    int keep_files_count = 0;

    foreach( string file in files )
    {
      if( FileUtils.IsMatch( file, keep_files ))
      {
        Console.WriteLine( "[DirUtils.TryDeleteWorker] bingo!! found match for file (" + file + ") in folder " + path );
        keep_files_count++;  // NOT delete directory -- includes keep file patthrn matches
      }
    }

    // pass 2: delete all non-matching files; clean out folder
    if( keep_files_count > 0 )
    {
      // NOTE: delete all files not matching
      foreach( string file in files )
      {
        if( FileUtils.IsMatch( file, keep_files ) == false )
          FileUtils.TryDelete( file );
      }

      return true; // NOTE: do NOTE delete directory -- includes keep file pattern matches
    }

    if( keep_dirs_count > 0 )
    {
      Console.WriteLine( "[DirUtils.TryDeleteWorker] keep parent folder " + path );
      return true;  // return; do NOT delete directory -- includes keep files pattern in subfolders!!
    }
    
    // if we get this far, delete folder!!!
    TryDeleteRecursively( path ); // Try delete directory!!!
    return false; // nothing found; mark directory as deleted! 
  }


  public static void DeleteRecursivelyPlease( string dir )
  {
    const int retries = 10;
    for( int i=1; i<retries; i++ )
    {
      try
      {
        Directory.Delete( dir, true );  // NOTE: use recursive=true flag
      }
      catch( DirectoryNotFoundException )
      {
        Console.WriteLine( "[DirUtils.DeleteRecursivelyPlease] DirectoryNotFoundException thrown; stop trying to deltete folder: " + dir );
        return;  // assume already deleted!
      }
      catch( IOException )
      {
        Console.WriteLine( "[DirUtils.DeleteRecursivelyPlease] IOException thrown; retrying to deltete folder in 50ms: " + dir );
        Thread.Sleep( 50 );  // NOTE: 10 x 50 = max. 500 ms
        continue;
      }
      catch( UnauthorizedAccessException )
      {
        Console.WriteLine( "[DirUtils.DeleteRecursivelyPlease] UnauthorizedAccessException thrown; retrying to delete folder in 50ms: " + dir );
        Thread.Sleep( 50 );  // NOTE: 10 x 50 = max. 500 ms
        continue;
      
        // fix: can we use more than one ex at once ???
      }
      // todo: check for read-only files ??? on second/third retry??? why? why not???
      
      // if we get here - assume successfully deleted directory!!!
      return;
    }
  }

  
  public static bool Missing( string dir )
  {
    return Directory.Exists( dir ) == false;
  }

  public static bool IsFlat( string path )
  {
     return path.Contains( "__I__" );
  }

  public static bool IsFull( string path )
  {
     // has some files (e.g. documents, that is, not just folders/directories)
     string [] files = Directory.GetFiles( path );
     return files.Length > 0;
  }

  public static bool IsJavaWebArchive( string path )
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

  public static void MoveAndCreateDirs( string sourceDir, string destDir )
  {
    // make sure parent dirs exists
    DirectoryInfo parentDirInfo = Directory.GetParent( destDir );
    Console.WriteLine( "  dir move - parentDirInfo: " + parentDirInfo.Name );
    parentDirInfo.Create(); //  make sure parent dirs exists (create if not)
               
    Directory.Move( sourceDir, destDir );
  }

} // class DirUtils


public class FileUtils
{

  public static bool IsMatch( string file, string[] keep_files )
  {
    foreach( string keep_file in keep_files )
    {
      if( file.EndsWith( keep_file ))
        return true;
    }
    return false;
  }

  public static void TryDelete( string file )
  {
    // try to delete file; if exception ignore delete and try later
    try
    {
       File.Delete( file );
    }
    catch( Exception )
    {
       Console.WriteLine( "[FileUtils.TryDelete] *** Exception thrown; trying to delete file " + file );
    }
  }


  public static void MoveAndCreateDirs( string sourceFile, string destFile )
  {
    // make sure dest dirs exists
    DirectoryInfo destDirInfo = new FileInfo( destFile ).Directory;
    Console.WriteLine( "  file move - destDirInfo: " + destDirInfo.Name );
    destDirInfo.Create();  //  make sure parent dir(s) exists (create if not) - check if it works for more than one dir/level

    File.Move( sourceFile, destFile );
  }

  public static void DeletePlease( string file )
  {
    const int retries = 10;
    for( int i=1; i<retries; i++ )
    {
      try
      {
        File.Delete( file );
      }
      catch( FileNotFoundException )
      {
        Console.WriteLine( "[FileUtils.DeletePlease] FileNotFoundException thrown; stop trying to deltete file: " + file );
        return;  // assume already deleted!
      }
      catch( IOException )
      {
        Console.WriteLine( "[FileUtils.DeletePlease] IOException thrown; retrying to delete file in 50ms: " + file );
        Thread.Sleep( 50 );  // NOTE: 10 x 50 = max. 500 ms
        continue;
      }
      catch( UnauthorizedAccessException )
      {
        Console.WriteLine( "[FileUtils.DeletePlease] UnauthorizedAccessException thrown; retrying to delete file in 50ms: " + file );
        Thread.Sleep( 50 );  // NOTE: 10 x 50 = max. 500 ms
        continue;
      
        // fix: can we use more than one ex at once ???
      }
      // todo: check for read-only files ??? on second/third retry??? why? why not???
      
      // if we get here - assume successfully deleted file!!!
      return;
    }
  }
} // class FileUtils



public class Utils
{

  public static string Timestamp()
  {
    // note: make sure string is a valid file name/path segement
    return DateTime.Now.ToString( "yyyy-MM-dd_HH-mm-ss.fff" );
  }


  ////////
  // fix: use/move to PackUtils - why? why not???

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

  

} // class Utils

