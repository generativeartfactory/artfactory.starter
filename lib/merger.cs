using System;
using System.IO;
using System.Text.RegularExpressions;


//////////
//  core principle
//   - NEVER copy
//   - NEVER delete
//   -  just move files
//
//
///  minor result =>  allow later restore (should be possible)



public class Merger
{
  string _installRoot;
  string _downloadRoot;

  public Merger( string installRoot, string downloadRoot )
  {
    _installRoot  = installRoot;   // use appRoot ?? 
    _downloadRoot = downloadRoot;
  }
  
  public Merger( string installRoot )
  {
    // todo: can we call main constructor? - use super() or similar?? possible
    
    _installRoot  = installRoot;   // use appRoot ?? 
    _downloadRoot = _installRoot + @"\downloads";   // default to <installDir>/downloads
  }

  
  public string FindLatestPaketDir( string manifestName )
  {
    string latestDir = _downloadRoot + @"\latest";
       
    Console.WriteLine( "  check if latest folder exists?" );
    if( Directory.Exists( latestDir ) )
    {
      Console.WriteLine( "  bingo! latest found; checking manifest" );

      string manifestFile = latestDir + @"\paket\" + manifestName + ".txt";
      Console.WriteLine( "   check if manifest >"+ manifestFile +"< exists?" );
          
      // check if manifestFile exits ??
      //  if yes
      if( File.Exists( manifestFile ) )
      {
        Console.WriteLine( " *** bingo! found manifest" );
        return latestDir;
      }
      else
      {
        return null;  // nothing found
      }
    }
    else
    {
      return null;  // nothing found
    }
  }

  public string FindVersionPaketDir( string manifestName )
  {
    // walk folders and check
       
    string [] dirs = Directory.GetDirectories( _downloadRoot );
    foreach( string dir in dirs )
    {
       Console.WriteLine( "dir: " + dir );

       // note: relativeFile will start with slash e.g. \weblibs\shared
       string relativeDir = dir.Substring( _downloadRoot.Length, dir.Length-_downloadRoot.Length );   // cut off downloadRoot

       Console.WriteLine( "  relativeDir: " + relativeDir );

       // skip tmp, latest, etc e.g. check if it looks like a version
       // note: we will also skip /latest
       if( Regex.IsMatch( relativeDir, "[0-9]+\\.[0-9]+" ))    // todo: use anchor? use case insensitivee?
       {
          Console.WriteLine( "** bingo - versioned folder found; checking if manifest exists" );

          string manifestFile = dir + @"\paket\" + manifestName + ".txt";
          Console.WriteLine( "   check if manifest >"+ manifestFile +"< exists?" );
          
          // check if manifestFile exits ??
          //  if yes
          if( File.Exists( manifestFile ) )
          {
            Console.WriteLine( " *** bingo! found manifest" );
            return dir;
          }
      }
    }

    return null;   // nothing found; sorry
  }


    public void HandlePatches( string patchesRoot, string atticRoot )
    {
        // note: in theory - patches should only be a handful of files (ideally)
        //   - do NOT abuse for switching complete webapps w/ hundreds of files and subfolders
        //   - NOT idealy - patches do NOT include subfolders (works for now - but may be forbidden in the future??  keep it simple!)

        HandlePatchesWorker( patchesRoot, patchesRoot, atticRoot, 1 );
    }


    void HandlePatchesWorker( string root, string patchesRoot, string atticRoot, int level )
    {
       string [] dirs = Directory.GetDirectories( root );
       foreach( string dir in dirs )
       {
          HandlePatchesWorker( dir, patchesRoot, atticRoot, level+1 );
       }

       string [] files = Directory.GetFiles( root );
       foreach( string file in files )
       {
          HandlePatch( file, patchesRoot, atticRoot );
       }
    }



    public void HandleUpdates( string updatesRoot, string atticRoot )
    {
       HandleUpdatesWorker( updatesRoot, updatesRoot, atticRoot, 1 );
    }

    void HandleUpdatesWorker( string root, string updatesRoot, string atticRoot, int level )
    {
       string [] dirs = Directory.GetDirectories( root );
       foreach( string dir in dirs )
       {
          if( Utils.IsDirFlat( dir ) || Utils.IsDirFull( dir ) || Utils.IsDirJavaWebArchive( dir ) )
          {
            HandleUpdate( dir, updatesRoot, atticRoot );
          }
          else
          {  // assume empty directory (no files) - just used for navigation; continue
            Console.WriteLine( "  skip folder in updates >" + dir + "<; continue walking" );
            HandleUpdatesWorker( dir, updatesRoot, atticRoot, level+1 );
          }
       }
    }


    void HandlePatch( string file, string patchesRoot, string atticRoot )
    {
       Console.WriteLine( "process file: " + file );

       // note: relativeFile will start with slash e.g. \weblibs\private
       string relativeFile = file.Substring( patchesRoot.Length, file.Length-patchesRoot.Length );   // cut off patchesRoot

       Console.WriteLine( "  relative: " + relativeFile );

       string installFile = _installRoot + relativeFile;
       Console.WriteLine( "  installFile: " + installFile );
      
       string atticFile = atticRoot + relativeFile;
       Console.WriteLine( "  atticFile: " + atticFile );

       if( File.Exists( installFile ) )
       {
         if( File.Exists( atticFile ))
         {
           // check - is it a hack to allow files in attic!!!
           //  for now rename w/ timestamp to make space
           
           // fix: move to trashDir!!!!!!!!!!!!!!!!!
              
           // if file exists in attic - rename with some classifier added
           string ts = DateTime.Now.ToString( "yyyy-MM-dd_HH-mm-ss.fff" );
           File.Move( atticFile, atticFile+"."+ts );
         }

         DirectoryInfo atticDirInfo = new FileInfo( atticFile ).Directory;
         Console.WriteLine( "  atticDirInfo: " + atticDirInfo.Name );
         atticDirInfo.Create(); //  make sure parent dirs exists (create if not)
            
         Console.WriteLine( "move to attic - " + installFile + " => " + atticFile );
            
         // todo: add flag for dry run!!!
         File.Move( installFile, atticFile );
        }
          
        DirectoryInfo installDirInfo = new FileInfo( installFile ).Directory;
        Console.WriteLine( "  installDirInfo: " + installDirInfo.Name );
        installDirInfo.Create(); //  make sure parent dirs exists (create if not)

        Console.WriteLine( "move update to install - " + file + " => " + installFile );

        // todo: add flag for dry run!!!
        File.Move( file, installFile );
    }

    void HandleUpdate( string dir, string updatesRoot, string atticRoot )
    {
          Console.WriteLine( "process dir: " + dir );
          
          // note: relativeDir will start with slash e.g. \weblibs\private
          string relativeFlatDir = dir.Substring( updatesRoot.Length, dir.Length-updatesRoot.Length );   // cut off updatesRoot
          // todo: check if it works w/ multiple replaces (works like gsub not sub) ???
          string relativeDir =  relativeFlatDir.Replace( "__I__", @"\" );     // replace __I__ with proper folder separator, that is, => \

          Console.WriteLine( "  relativeFlat: " + relativeFlatDir );
          Console.WriteLine( "  relative: " + relativeDir );

          string installDir = _installRoot + relativeDir;
          Console.WriteLine( "  installDir: " + installDir );
          
          string atticDir = atticRoot + relativeFlatDir;
          Console.WriteLine( "  atticDir: " + atticDir );

          if( Directory.Exists( installDir ) )
          {
            if( Directory.Exists( atticDir ))
            {
               // check - is it a hack to allow dirs in attic!!!
               //  for now rename w/ timestamp to make space
              
               // if file exists in attic - rename with some classifier added
               string ts = DateTime.Now.ToString( "yyyy-MM-dd_HH-mm-ss.fff" );
               Directory.Move( atticDir, atticDir+"."+ts );
            }

            DirectoryInfo atticParentDirInfo = Directory.GetParent( atticDir );
            atticParentDirInfo.Create(); //  make sure parent dirs exists (create if not)
            
            Console.WriteLine( "move to attic - " + installDir + " => " + atticDir );
            
            // todo: add flag for dry run!!!
            Directory.Move( installDir, atticDir );
          }
          
          DirectoryInfo installParentDirInfo = Directory.GetParent( installDir );
          installParentDirInfo.Create(); //  make sure parent dirs exists (create if not)

          Console.WriteLine( "move update to install - " + dir + " => " + installDir );

          // todo: add flag for dry run!!!
          Directory.Move( dir, installDir );
    }
}  // class Merger
