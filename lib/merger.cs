using System;
using System.IO;
using System.Text.RegularExpressions;


//////////
//  core principle
//   - NEVER copy
//   - NEVER delete
//   --  just move files
//
//  minor result =>  allow later restore (should be possible)
//
// NOTE: new option if you use keep=false
//         NEVER copy still holds,
//          but (old) files and dirs get deleted



public class Merger
{
  string _installRoot;  // application root folder
  string _downloadRoot;
  string _atticRoot;    // "old" updates n patches (might get restored)
  string _trashRoot;    // trash - files and folders no longer used (mostly for used for duplicates that get moved from attic)

  string _metaName;  // e.g  meta or pkg or package or paket etc. - where manifest gets stored

  bool _keep;  // e.g. keep (backup) old files and folders -- default is true

  public Merger( string installRoot, string downloadRoot, string atticRoot, string trashRoot, bool keep )
  {
    // todo: can we call main constructor? - use super() or similar?? possible

    _installRoot  = installRoot;   // use appRoot ??
    _downloadRoot = downloadRoot;
    _atticRoot    = atticRoot;
    _trashRoot    = trashRoot;

    _metaName  = "paket";

    _keep = keep;

    Console.WriteLine( "merger folders:" );
    Console.WriteLine( "  installRoot:  >"+ _installRoot  + "<" );
    Console.WriteLine( "  downloadRoot: >"+ _downloadRoot + "<" );
    Console.WriteLine( "  atticRoot:    >"+ _atticRoot    + "<" );
    Console.WriteLine( "  trashRoot:    >"+ _trashRoot    + "<" );    
  }

  public Merger( string installRoot )
   : this( installRoot,
           installRoot + @"\downloads",
           installRoot + @"\downloads\attic",
           installRoot + @"\downloads\trash",
           true )
  {
  }

  public Merger( string installRoot, bool keep )
   : this( installRoot,
           installRoot + @"\downloads",
           installRoot + @"\downloads\attic",
           installRoot + @"\downloads\trash",
           keep )
  {
  }


  public void MergeVersionPack( string manifestName )
  {
    // check if installRoot and donwloadRoot exist? if not do nothing; return
    if( DirUtils.Missing( _installRoot ))
    {
      Console.WriteLine( "installRoot >"+ _installRoot +"< missing; return - nothing to do" );
      return;
    }

    if( DirUtils.Missing( _downloadRoot ))
    {
      Console.WriteLine( "downloadRoot >"+ _downloadRoot +"< missing; return - nothing to do" );
      return;
    }

    string installedVersion = Utils.FindPackVersion( _installRoot+@"\"+_metaName+@"\"+ manifestName + ".txt" );

    string versionRoot  = FindVersionPackDir( manifestName );

    if( versionRoot != null )
    {
       Console.WriteLine( "try merge from w/ versionRoot: " + versionRoot ); 

       string dl_updates = versionRoot+@"\updates";
       string dl_patches = versionRoot+@"\patches";
       string dl_meta    = versionRoot+@"\"+_metaName;   // e.g. <manifestName>-<version>\paket

       string attic_updates = _atticRoot+@"\"+manifestName+"-"+installedVersion+@"\updates";
       string attic_patches = _atticRoot+@"\"+manifestName+"-"+installedVersion+@"\patches";
       string attic_meta    = _atticRoot+@"\"+manifestName+"-"+installedVersion+@"\"+_metaName;  // e.g. <manifestName>-<version>\paket
          
       if( Directory.Exists( dl_updates ))
         HandleUpdates( dl_updates, attic_updates );
       
       if( Directory.Exists( dl_patches ))
         HandlePatches( dl_patches, attic_patches );

       if( Directory.Exists( dl_meta ))  // if all worked - swap manifest - as last step
        HandleManifest( manifestName, dl_meta, attic_meta );

       // we're done - move pack to trash (for clean-up)
       HandlePack( versionRoot );
 
    } // if versionRoot != null
  } // method  MergeVersionPack


  public void MergeLatestPack( string manifestName )
  {
    // check if installRoot and donwloadRoot exist? if not do nothing; return
    if( DirUtils.Missing( _installRoot ))
    {
      Console.WriteLine( "installRoot >"+ _installRoot +"< missing; return - nothing to do" );
      return;
    }

    if( DirUtils.Missing( _downloadRoot ))
    {
      Console.WriteLine( "downloadRoot >"+ _downloadRoot +"< missing; return - nothing to do" );
      return;
    }

    // string installedVersion = Utils.FindPackVersion( _installRoot+@"\"+_metaName+@"\"+ manifestName + ".txt" );

    // todo/fix:
    //  new method!!!!   use FindPackTimestamp()  - assumes no VERSION in manifest
    
 
    string latestRoot = FindLatestPackDir( manifestName );

    if( latestRoot != null )
    {
       Console.WriteLine( "try merge from w/ latestRoot: " + latestRoot ); 

       string dl_updates = latestRoot+@"\updates";
       string dl_patches = latestRoot+@"\patches";
       string dl_meta    = latestRoot+@"\"+_metaName;   // e.g. <manifestName>-<version>\paket

       // note: latest assumes no version; will use timestamp of manifestFile for now
       //   roughly equals the download date ??
       string installedVersion =  Utils.Timestamp();

       // use manifestName instead of latest ?? e.g. kv or similar ?? or just merge everything together anyway!!
       string attic_updates = _atticRoot+@"\"+manifestName+@"-latest\"+installedVersion+@"\updates";
       string attic_patches = _atticRoot+@"\"+manifestName+@"-latest\"+installedVersion+@"\patches";
       string attic_meta    = _atticRoot+@"\"+manifestName+@"-latest\"+installedVersion+@"\"+_metaName;  // e.g. <manifestName>-<version>\paket

       if( Directory.Exists( dl_updates ))
         HandleUpdates( dl_updates, attic_updates );

       if( Directory.Exists( dl_patches ))
         HandlePatches( dl_patches, attic_patches );

       if( Directory.Exists( dl_meta ))  // if all worked - swap manifest - as last step
         HandleManifest( manifestName, dl_meta, attic_meta );

       // we're done - move pack to trash (for clean-up)
       HandlePack( latestRoot );
    }
  }


  string FindLatestPackDir( string manifestName )
  {
    string latestDir = _downloadRoot+@"\"+manifestName+"-latest";
       
    Console.WriteLine( "  check if latest folder exists?" );
    if( Directory.Exists( latestDir ) )
    {
      Console.WriteLine( "  bingo! latest found; checking manifest" );

      string manifestFile = latestDir + @"\" + _metaName+ @"\" + manifestName + ".txt";
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

  string FindVersionPackDir( string manifestName )
  {
    // walk folders and check
       
    string [] dirs = Directory.GetDirectories( _downloadRoot );
    foreach( string dir in dirs )
    {
       Console.WriteLine( "dir: " + dir );

       // note: relativeFile will NOT start w/ slash e.g. weblibs\shared  NOT \weblibs\shared
       // -- cut off downloadRoot
       string relativeDir = dir.Substring( _downloadRoot.Length+1, dir.Length-_downloadRoot.Length-1 );

       Console.WriteLine( "  relativeDir: " + relativeDir );

       // skip tmp, latest, etc e.g. check if it looks like a version
       // note: we will also skip /latest
       if( relativeDir.StartsWith( manifestName ) &&
           Regex.IsMatch( relativeDir, "[0-9]+\\.[0-9]+" ) )  // todo: use anchor? use case insensitivee?
           
       {
          Console.WriteLine( "** bingo - versioned folder found; checking if manifest exists" );

          string manifestFile = dir + @"\" + _metaName + @"\" + manifestName + ".txt";
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


    void HandlePatches( string patchesRoot, string atticRoot )
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



    void HandleUpdates( string updatesRoot, string atticRoot )
    {
       HandleUpdatesWorker( updatesRoot, updatesRoot, atticRoot, 1 );
    }

    void HandleUpdatesWorker( string root, string updatesRoot, string atticRoot, int level )
    {
       string [] dirs = Directory.GetDirectories( root );
       foreach( string dir in dirs )
       {
          if( DirUtils.IsFlat( dir ) || DirUtils.IsFull( dir ) || DirUtils.IsJavaWebArchive( dir ) )
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

    void HandlePack( string dir )
    {
      Console.WriteLine( "process pack dir: " + dir );

      if( _keep == false )
      {
        Console.WriteLine( "delete pack dir - " + dir );
        // Directory.Delete( dir, true );  // note: use true flag for recursive delete!!
        // DirUtils.DeleteRecursivelyPlease( dir );
        MoveDirToTrashAndDeletePlease( dir, _downloadRoot );
      }
      else
      {
        // cleanup pack - (move everything remaining after merge to trash)
        MoveDirToTrash( dir, _downloadRoot );
      }
    }

    void HandleManifest( string manifestName, string metaRoot, string atticRoot )
    {
      // note: for now always keep a history (backup) of manifest
      //   - small file size
      //   if keep==false delete/remove all other files (just keep manifest)
      
      //
      // fix: for keep == false
      //   use "flat" folde structure e.g. no metaRoot ??? just version e.g
      //  <version>/paket.txt instead of <version>/paket/paket.txt
      
       Console.WriteLine( "process manifestName: " + manifestName );

       string versionManifestFile = metaRoot+@"\"+ manifestName + ".txt";
       string atticManifestFile   = atticRoot+@"\"+ manifestName + ".txt";
       string installManifestFile = _installRoot+@"\"+_metaName+@"\"+ manifestName + ".txt"; 
       
       // if file exits in attic; move it to trash
       if( File.Exists( atticManifestFile ))
       {
          MoveFileToTrash( atticManifestFile, _atticRoot ); // note: use base attic root (will include version/latest etc.)
       }

       // note: assume installManifest exists
       //   will crash if missing - it's a feature!! manifest required for now
       // todo: add flag for dry run!!!
       Console.WriteLine( "move to attic - " + installManifestFile + " => " + atticManifestFile );
       FileUtils.MoveAndCreateDirs( installManifestFile, atticManifestFile );

       // todo: add flag for dry run!!!
       Console.WriteLine( "move manifest to install - " + versionManifestFile + " => " + installManifestFile );
       FileUtils.MoveAndCreateDirs( versionManifestFile, installManifestFile );
    }


    void HandlePatch( string file, string patchesRoot, string atticRoot )
    {
       Console.WriteLine( "process file: " + file );

       // note: relativeFile will NOT start with slash e.g. weblibs\private NOT \weblibs\private
       // -- cut off patchesRoot
       string relativeFile = file.Substring( patchesRoot.Length+1, file.Length-patchesRoot.Length-1 );

       Console.WriteLine( "  relative: " + relativeFile );

       string installFile = _installRoot + @"\" + relativeFile;
       Console.WriteLine( "  installFile: " + installFile );

       string atticFile = atticRoot + @"\" + relativeFile;
       Console.WriteLine( "  atticFile: " + atticFile );

       // if file exits already - move it to attic
       // -- if file also exits in attic; move it to trash
       if( File.Exists( installFile ) )
       {
         if( _keep == false )
         {
           /// quick fix/hack: get name from .exe/.dll !!!! do NOT hardcode
           // note: cannot delete ourselfs e.g. usomerged.exe !!! check if move works ??
           if( installFile.Contains( "usomerged" ) )
           {
             Console.WriteLine( "move (usomerged) file to trash - " + installFile );
             MoveFileToTrash( installFile, _installRoot );
           }
           else
           {
             Console.WriteLine( "delete file - " + installFile );
             // File.Delete( installFile );
             MoveFileToTrashAndDeletePlease( installFile, _installRoot );
           }
         }
         else
         {
           if( File.Exists( atticFile ))
           {
             MoveFileToTrash( atticFile, _atticRoot ); // note: use base attic root (will include version/latest etc.)
           }
 
           // todo: add flag for dry run!!!
           Console.WriteLine( "move to attic - " + installFile + " => " + atticFile );
           FileUtils.MoveAndCreateDirs( installFile, atticFile );
         }
        }

        // todo: add flag for dry run!!!
        Console.WriteLine( "move update to install - " + file + " => " + installFile );
        FileUtils.MoveAndCreateDirs( file, installFile );
    }

    void HandleUpdate( string dir, string updatesRoot, string atticRoot )
    {
          Console.WriteLine( "process dir: " + dir );

          // note: relativeDir will NOT start with slash e.g. weblibs\private or \weblibs\private
          //  -- cut off updatesRoot
          string relativeDir = dir.Substring( updatesRoot.Length+1, dir.Length-updatesRoot.Length-1 );
          Console.WriteLine( "  relative: " + relativeDir );

          string installDir = _installRoot + @"\"+ relativeDir;
          Console.WriteLine( "  installDir: " + installDir );

          string atticDir = atticRoot + @"\"+ relativeDir;
          Console.WriteLine( "  atticDir: " + atticDir );

          // if folder exits already - move it to attic
          // -- if folder also exits in attic; move it to trash
          if( Directory.Exists( installDir ) )
          {
            if( _keep == false )
            {
              Console.WriteLine( "delete dir - " + installDir );
              // Directory.Delete( installDir, true );  // note: use true flag for recursive delete!!
              // DirUtils.DeleteRecursivelyPlease( installdir );
              MoveDirToTrashAndDeletePlease( installDir, _installRoot );
            }
            else
            {
              if( Directory.Exists( atticDir ))
              {
                MoveDirToTrash( atticDir, _atticRoot ); // note: use base attic root (will include version/latest etc.)
              }
              // todo: add flag for dry run!!!
              Console.WriteLine( "move to attic - " + installDir + " => " + atticDir );
              DirUtils.MoveAndCreateDirs( installDir, atticDir );
            }
          }

          // todo: add flag for dry run!!!
          Console.WriteLine( "move update to install - " + dir + " => " + installDir );
          DirUtils.MoveAndCreateDirs( dir, installDir );
    }


    void MoveFileToTrashAndDeletePleaseWorker( string file, string root, bool delete )
    {
      // root - used for calculate relative path e.g.   file-root = relative
      //  e.g.
      //   c:\test\downloads\attic\pack-v2014.01.01\pack\pack.txt
      //   c:\test\downloads\attic\pack-v2014.01.01

      // use: better name - move to trash ?? etc.
      // move to trash

      // use a flat folder structure in trash
      // note: relativeFile will NOT start with slash e.g. weblibs\private NOT \weblibs\private
      // -- cut off atticRoot (e.g. <install_dir>/downloads/attic) - results in <manifestName>-<version>/<metaName>/<manifestName>.txt
      string trashRelativeFile = file.Substring( root.Length+1, file.Length-root.Length-1 );   
      Console.WriteLine( "  trashRelativeFile: " + trashRelativeFile );
      
      string ts = Utils.Timestamp();
      string trashRelativeFlatFile = trashRelativeFile.Replace( @"\", "__I__" );
      string trashFile = _trashRoot + @"\xx_" + ts + "__" + trashRelativeFlatFile + ".trash";

       // todo: add flag for dry run!!!
       Console.WriteLine( "move to trash - " + file + " => " + trashFile );
       FileUtils.MoveAndCreateDirs( file, trashFile );
       
       if( delete )
         FileUtils.DeletePlease( trashFile );
    }

    void MoveFileToTrashAndDeletePlease( string file, string root )
    {
      MoveFileToTrashAndDeletePleaseWorker( file, root, true );
    }

    void MoveFileToTrash( string file, string root )
    {
      MoveFileToTrashAndDeletePleaseWorker( file, root, false );
    }

    void MoveDirToTrashAndDeletePleaseWorker( string dir, string root, bool delete )
    {
      // 1) step 1 -- move to trash

      // use a flat folder structure in trash
      // note: relativeDir will NOT start with slash e.g. weblibs\private or \weblibs\private
      // -- cut off updatesRoot
      string trashRelativeDir = dir.Substring( root.Length+1, dir.Length-root.Length-1 );
      Console.WriteLine( "  trashRelativeFile: " + trashRelativeDir );

      string ts = Utils.Timestamp();
      string trashRelativeFlatDir = trashRelativeDir.Replace( @"\", "__I__" );
      string trashDir = _trashRoot + @"\xx_" + ts + "__" + trashRelativeFlatDir + ".trash";

      // todo: add flag for dry run!!!
      Console.WriteLine( "move to trash - " + dir + " => " + trashDir );
      DirUtils.MoveAndCreateDirs( dir, trashDir );

      // 2) step 2 (optional) -- delete
      
      if( delete )
        DirUtils.DeleteRecursivelyPlease( trashDir );
    }

    void MoveDirToTrashAndDeletePlease( string dir, string root )
    {
      MoveDirToTrashAndDeletePleaseWorker( dir, root, true );
    }

    void MoveDirToTrash( string dir, string root )
    {
      MoveDirToTrashAndDeletePleaseWorker( dir, root, false );
    }

}  // class Merger

