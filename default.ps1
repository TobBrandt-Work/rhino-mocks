properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\SharedLibs"
  $build_dir = "$base_dir\Build" 
  
  $sln_file = "$base_dir\Rhino.Mocks.sln" 
  $version = "4.0.0.0"
  $humanReadableversion = "4.0"
  $tools_dir = "$base_dir\Tools"
  $release_dir = "$base_dir\Release"
} 

include .\psake_ext.ps1
	
task default -depends Release

task Clean { 
  remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue 
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
} 

task Init -depends Clean { 
	
	Generate-Assembly-Info `
		-file "$base_dir\Rhino.Mocks\Properties\AssemblyInfo.cs" `
		-title "Rhino Mocks $version" `
		-description "Mocking Framework for .NET" `
		-company "" `
		-product "Rhino Mocks $version" `
		-version $version `
		-copyright "Hibernating Rhinos & Ayende Rahien 2004-2009, Mike Meisinger 2013-2014"
		
	Generate-Assembly-Info `
		-file "$base_dir\Rhino.Mocks.Tests\Properties\AssemblyInfo.cs" `
		-title "Rhino Mocks Tests $version" `
		-description "Mocking Framework for .NET" `
		-company "" `
		-product "Rhino Mocks Tests $version" `
		-version $version `
		-clsCompliant "false" `
		-copyright "Hibernating Rhinos & Ayende Rahien 2004-2009, Mike Meisinger 2013-2014"
		
	Generate-Assembly-Info `
		-file "$base_dir\Rhino.Mocks.Tests.Model\Properties\AssemblyInfo.cs" `
		-title "Rhino Mocks Tests Model $version" `
		-description "Mocking Framework for .NET" `
		-company "" `
		-product "Rhino Mocks Tests Model $version" `
		-version $version `
		-clsCompliant "false" `
		-copyright "Hibernating Rhinos & Ayende Rahien 2004-2009, Mike Meisinger 2013-2014"
	
	new-item $release_dir -itemType directory 
	new-item $build_dir -itemType directory 
	cp $tools_dir\xUnit\*.* $build_dir
} 

task Compile -depends Init { 
  & msbuild "$sln_file" "/p:OutDir=$build_dir\\" /p:Configuration=Release
  if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute msbuild"
  }
} 

task Test -depends Compile {
  $old = pwd
  cd $build_dir
  # &.\xunit.console.exe "$build_dir\Rhino.Mocks.Tests.dll"
  # if ($lastExitCode -ne 0) {
  #       throw "Error: Failed to execute tests"
  # }
  cd $old		
}

task Merge {
	$old = pwd
	cd $build_dir
	
	Remove-Item Rhino.Mocks.Partial.dll -ErrorAction SilentlyContinue 
	Rename-Item $build_dir\Rhino.Mocks.dll Rhino.Mocks.Partial.dll
	
	& $tools_dir\ILRepack\ILRepack.exe Rhino.Mocks.Partial.dll `
		Castle.Core.dll `
		/out:Rhino.Mocks.dll `
		/t:library `
		/targetplatform:v4 `
		"/keyfile:$base_dir\meisinger-open-source.snk"
	if ($lastExitCode -ne 0) {
        throw "Error: Failed to merge assemblies!"
    }
	cd $old
}

task Release -depends Test, Merge {
	$buildrev = Get-Git-Commit
	
	& $tools_dir\Zip\zip.exe -9 -A -j `
		$release_dir\Rhino.Mocks-$humanReadableversion-Build-$buildrev.zip `
		$build_dir\Rhino.Mocks.dll `
		$build_dir\Rhino.Mocks.xml `
		license.txt `
		acknowledgements.txt
	if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute ZIP command"
    }
}

